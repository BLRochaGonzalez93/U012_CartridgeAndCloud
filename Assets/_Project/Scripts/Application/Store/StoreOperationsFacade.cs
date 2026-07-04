using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;

using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public sealed class StoreOperationsFacade
    {
        public const string BackroomContainerId =
            "backroom-inventory";

        private readonly IStoreContentCatalog _catalog;
        private readonly IStoreOperationsStateRepository
            _repository;
        private readonly ActiveGameSessionService
            _activeSession;
        private readonly IUtcClock _clock;

        public StoreOperationsState State {
            get;
            private set;
        }

        public event Action<StoreOperationsState>
            StateChanged;

        public event Action<GameplayFeedbackEvent>
            FeedbackRaised;

        public StoreOperationsFacade(
            IStoreContentCatalog catalog,
            IStoreOperationsStateRepository repository,
            ActiveGameSessionService activeSession,
            IUtcClock clock)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _activeSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
            _clock = clock ??
                throw new ArgumentNullException(
                    nameof(clock));
        }

        public void InitializeForActiveSlot()
        {
            EnsureActiveSession();

            StoreOperationsState loaded =
                _repository.Load(
                    _activeSession.ActiveSlotId);

            string sessionId =
                _activeSession.Snapshot
                    .SessionId.Value;

            State =
                loaded != null &&
                string.Equals(
                    loaded.SessionId,
                    sessionId,
                    StringComparison.Ordinal)
                    ? loaded
                    : StoreOperationsState.Empty(
                        _activeSession.ActiveSlotId,
                        sessionId);

            StateChanged?.Invoke(State);
        }

        public void SaveCheckpoint()
        {
            EnsureInitialized();
            _repository.Save(State);
        }

        public bool DeleteSlotSidecar()
        {
            EnsureActiveSession();
            State = null;

            return _repository.Delete(
                _activeSession.ActiveSlotId);
        }

        public StoreOperationResult OrderFurniture(
            string definitionId,
            int quantity)
        {
            EnsureInitialized();

            if (!_catalog.TryGetFurniture(
                    definitionId,
                    out StoreFixtureDefinition
                        definition))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition was not found.");
            }

            if (!definition.IsPurchasable)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "This blockout fixture is not sold in the shop catalog.");
            }

            return CreateOrder(
                definition.DefinitionId,
                isFurniture: true,
                quantity,
                definition.UnitCostCents);
        }

        public StoreOperationResult OrderProduct(
            string productId,
            int caseQuantity)
        {
            EnsureInitialized();

            if (!_catalog.TryGetProduct(
                    productId,
                    out RetailProductDefinition
                        definition))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Product definition was not found.");
            }

            return CreateOrder(
                definition.ProductId,
                isFurniture: false,
                caseQuantity,
                checked(
                    definition.WholesalePriceCents *
                    definition.UnitsPerCase));
        }

        public StoreOperationResult ReceiveOrder(
            string orderId)
        {
            EnsureInitialized();

            int orderIndex =
                FindOrderIndex(orderId);

            if (orderIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Order was not found.");
            }

            StoreOrderRecord order =
                State.Orders[orderIndex];

            if (order.State !=
                StoreOrderStatus.Ordered)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Only ordered deliveries can be received.");
            }

            long totalCost = checked(
                order.UnitCostCents *
                order.OrderedUnits);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (snapshot.CashCents < totalCost)
            {
                return Failure(
                    StoreOperationStatus
                        .InsufficientCash,
                    "Not enough cash to receive this order.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.Orders[orderIndex] =
                order.ReceiveAll();
            builder.LifetimeExpenseCents =
                checked(
                    builder.LifetimeExpenseCents +
                    totalCost);

            int receivedStock =
                order.OrderedUnits;

            List<InventoryContainerSaveRecord>
                inventories =
                    new List<
                        InventoryContainerSaveRecord>(
                            snapshot.Inventories);

            if (order.IsFurniture)
            {
                builder.AddStock(
                    builder.FurnitureWarehouse,
                    order.ItemId,
                    receivedStock);
            }
            else
            {
                if (!_catalog.TryGetProduct(
                        order.ItemId,
                        out RetailProductDefinition
                            product))
                {
                    return Failure(
                        StoreOperationStatus.NotFound,
                        "Ordered product definition is missing.");
                }

                receivedStock = checked(
                    order.OrderedUnits *
                    product.UnitsPerCase);

                builder.AddStock(
                    builder.ProductWarehouse,
                    order.ItemId,
                    receivedStock);

                inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .AddProductToContainer(
                            snapshot,
                            BackroomContainerId,
                            200,
                            order.ItemId,
                            receivedStock);
            }

            List<SupplierOrderSaveRecord>
                supplierOrders =
                    IntegratedSnapshotStoreOperationsMutator
                        .UpsertSupplierOrder(
                            snapshot,
                            ProjectOrder(
                                order.ReceiveAll()));

            List<EconomyLedgerSaveRecord> ledger =
                IntegratedSnapshotStoreOperationsMutator
                    .AppendLedger(
                        snapshot,
                        "phase1-ledger-cost-" +
                        order.OrderId,
                        "SupplierReceivingCost",
                        order.OrderId,
                        totalCost);

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    cashCents:
                        checked(
                            snapshot.CashCents -
                            totalCost),
                    inventories: inventories,
                    supplierOrders:
                        supplierOrders,
                    ledgerEntries: ledger);

            Commit(
                builder.Build(),
                nextSnapshot);

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.OrderReceived,
                    $"Received {receivedStock} × " +
                    $"{order.ItemId}.",
                    "receiving-zone"));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Expense,
                    "Supplier delivery paid.",
                    "cash-hud",
                    totalCost,
                    snapshot.CurrencyCode));

            return StoreOperationResult.Success(
                "Order received and paid.");
        }

        public StoreOperationResult ConfirmFurniturePlacement(
            string definitionId,
            string instanceId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns)
        {
            EnsureInitialized();

            if (!_catalog.TryGetFurniture(
                    definitionId,
                    out StoreFixtureDefinition
                        definition))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition was not found.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            if (builder.GetStock(
                    builder.FurnitureWarehouse,
                    definitionId) < 1)
            {
                return Failure(
                    StoreOperationStatus
                        .InsufficientStock,
                    "Receive this furniture before placing it.");
            }

            foreach (PlacedStoreFixtureRecord fixture
                     in builder.Fixtures)
            {
                if (string.Equals(
                        fixture.InstanceId,
                        instanceId,
                        StringComparison.Ordinal))
                {
                    return Failure(
                        StoreOperationStatus.Duplicate,
                        "Placement instance already exists.");
                }
            }

            builder.AddStock(
                builder.FurnitureWarehouse,
                definitionId,
                -1);

            PlacedStoreFixtureRecord placed =
                new PlacedStoreFixtureRecord(
                    instanceId,
                    definitionId,
                    anchorX,
                    anchorZ,
                    rotationQuarterTurns,
                    string.Empty,
                    0);

            builder.Fixtures.Add(placed);
            builder.NextFixtureSequence =
                checked(
                    builder.NextFixtureSequence + 1);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            IEnumerable<
                InventoryContainerSaveRecord>
                    inventories =
                        snapshot.Inventories;

            IEnumerable<DisplaySaveRecord> displays =
                snapshot.Displays;

            if (definition.SupportsProducts)
            {
                string containerId =
                    DisplayContainerId(instanceId);

                inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .AddProductToContainer(
                            snapshot,
                            containerId,
                            definition.Capacity,
                            "phase1-empty",
                            0);

                IntegratedGameStateSnapshot interim =
                    IntegratedSnapshotStoreOperationsMutator
                        .Clone(
                            snapshot,
                            snapshot.UpdatedUtc,
                            inventories: inventories);

                displays =
                    IntegratedSnapshotStoreOperationsMutator
                        .UpsertDisplay(
                            interim,
                            instanceId,
                            definitionId,
                            string.Empty,
                            containerId);
            }

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories,
                    displays: displays);

            Commit(
                builder.Build(),
                nextSnapshot);

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.ObjectSelected,
                    $"{definition.DisplayName} placed.",
                    instanceId));

            return StoreOperationResult.Success(
                "Furniture placed.");
        }

        public StoreOperationResult AssignProduct(
            string fixtureInstanceId,
            string productId)
        {
            EnsureInitialized();

            int fixtureIndex =
                FindFixtureIndex(
                    fixtureInstanceId);

            if (fixtureIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out StoreFixtureDefinition
                        furniture) ||
                !furniture.SupportsProducts)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "This fixture cannot display products.");
            }

            if (!_catalog.TryGetProduct(
                    productId,
                    out _))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Product definition was not found.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    productId,
                    0);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<DisplaySaveRecord> displays =
                IntegratedSnapshotStoreOperationsMutator
                    .UpsertDisplay(
                        snapshot,
                        fixture.InstanceId,
                        fixture.DefinitionId,
                        productId,
                        DisplayContainerId(
                            fixture.InstanceId));

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    displays: displays));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.ProductAssigned,
                    $"{productId} assigned.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                "Product assigned.");
        }

        public StoreOperationResult RestockDisplay(
            string fixtureInstanceId,
            int quantity)
        {
            EnsureInitialized();

            if (quantity < 1)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Restock quantity must be positive.");
            }

            int fixtureIndex =
                FindFixtureIndex(
                    fixtureInstanceId);

            if (fixtureIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (string.IsNullOrWhiteSpace(
                    fixture.AssignedProductId))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Assign a product before restocking.");
            }

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out StoreFixtureDefinition
                        furniture))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            if (fixture.ProductQuantity + quantity >
                furniture.Capacity)
            {
                return Failure(
                    StoreOperationStatus
                        .CapacityExceeded,
                    "Display capacity would be exceeded.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            if (builder.GetStock(
                    builder.ProductWarehouse,
                    fixture.AssignedProductId) <
                quantity)
            {
                Raise(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType.OutOfStock,
                        "Not enough backroom stock.",
                        fixture.InstanceId));

                return Failure(
                    StoreOperationStatus
                        .InsufficientStock,
                    "Not enough backroom stock.");
            }

            builder.AddStock(
                builder.ProductWarehouse,
                fixture.AssignedProductId,
                -quantity);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    fixture.AssignedProductId,
                    checked(
                        fixture.ProductQuantity +
                        quantity));

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<InventoryContainerSaveRecord>
                inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .TransferProduct(
                            snapshot,
                            BackroomContainerId,
                            DisplayContainerId(
                                fixture.InstanceId),
                            furniture.Capacity,
                            fixture.AssignedProductId,
                            quantity);

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Restocked,
                    $"Restocked {quantity} units.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                "Display restocked.");
        }

        public StoreOperationResult
            ValidateNextCustomerPurchase()
        {
            EnsureInitialized();

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Open",
                    StringComparison.Ordinal))
            {
                return Failure(
                    StoreOperationStatus
                        .StoreMustBeOpen,
                    "Open the store before serving customers.");
            }

            if (!HasPlacedKind(
                    StoreFixtureKind
                        .CheckoutCounter))
            {
                return Failure(
                    StoreOperationStatus
                        .CheckoutRequired,
                    "Place a checkout counter first.");
            }

            if (snapshot.QueueEntries.Count > 0 ||
                string.Equals(
                    snapshot.CheckoutStation.State,
                    "Busy",
                    StringComparison.Ordinal))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Checkout is already processing another customer.");
            }

            int fixtureIndex =
                FindFirstStockedDisplay();

            if (fixtureIndex < 0)
            {
                return Failure(
                    StoreOperationStatus
                        .InsufficientStock,
                    "No stocked display is available.");
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetProduct(
                    fixture.AssignedProductId,
                    out _))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Assigned product definition is missing.");
            }

            return StoreOperationResult.Success(
                "Customer purchase is ready.");
        }

        public StoreOperationResult
            ProcessNextCustomerPurchase()
        {
            EnsureInitialized();

            StoreOperationResult validation =
                ValidateNextCustomerPurchase();

            if (!validation.Succeeded)
            {
                if (validation.Status ==
                    StoreOperationStatus
                        .InsufficientStock)
                {
                    Raise(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType
                                .CustomerFrustrated,
                            validation.Detail,
                            "store-entrance"));
                }

                return validation;
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            int fixtureIndex =
                FindFirstStockedDisplay();

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetProduct(
                    fixture.AssignedProductId,
                    out RetailProductDefinition
                        product))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Assigned product definition is missing.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    fixture.AssignedProductId,
                    fixture.ProductQuantity - 1);

            builder.CompletedSales =
                checked(
                    builder.CompletedSales + 1);
            builder.LifetimeRevenueCents =
                checked(
                    builder.LifetimeRevenueCents +
                    product.SalePriceCents);
            builder.NextCustomerSequence =
                checked(
                    builder.NextCustomerSequence + 1);

            List<InventoryContainerSaveRecord>
                inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .AddProductToContainer(
                            snapshot,
                            DisplayContainerId(
                                fixture.InstanceId),
                            0,
                            product.ProductId,
                            -1);

            string sequence =
                State.NextCustomerSequence
                    .ToString("0000");

            string customerId =
                "phase1-customer-" + sequence;
            string cartId =
                "phase1-cart-" + sequence;
            string reservationId =
                "phase1-reservation-" +
                sequence;
            string transactionId =
                "phase1-transaction-" +
                snapshot.CurrentDay
                    .ToString("000") +
                "-" +
                sequence;

            List<CustomerSaveRecord> customers =
                new List<CustomerSaveRecord>(
                    snapshot.Customers)
                {
                    new CustomerSaveRecord(
                        customerId,
                        "phase1-customer-profile",
                        "Despawned",
                        0,
                        4)
                };

            List<ShoppingSessionSaveRecord>
                shoppingSessions =
                    new List<
                        ShoppingSessionSaveRecord>(
                            snapshot
                                .ShoppingSessions)
                    {
                        new ShoppingSessionSaveRecord(
                            customerId,
                            "phase1-intent-" +
                            product.ProductId,
                            cartId,
                            "CheckedOut",
                            1)
                    };

            List<ReservationSaveRecord>
                reservations =
                    new List<
                        ReservationSaveRecord>(
                            snapshot.Reservations)
                    {
                        new ReservationSaveRecord(
                            reservationId,
                            customerId,
                            cartId,
                            fixture.InstanceId,
                            product.ProductId,
                            1,
                            "Consumed")
                    };

            List<CheckoutTransactionSaveRecord>
                transactions =
                    new List<
                        CheckoutTransactionSaveRecord>(
                            snapshot.Transactions)
                    {
                        new CheckoutTransactionSaveRecord(
                            transactionId,
                            customerId,
                            cartId,
                            snapshot.CheckoutStation
                                .StationId,
                            "Completed",
                            1,
                            1)
                    };

            List<EconomyLedgerSaveRecord> ledger =
                IntegratedSnapshotStoreOperationsMutator
                    .AppendLedger(
                        snapshot,
                        "phase1-ledger-revenue-" +
                        transactionId,
                        "CheckoutRevenue",
                        transactionId,
                        product.SalePriceCents);

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    cashCents:
                        checked(
                            snapshot.CashCents +
                            product.SalePriceCents),
                    inventories: inventories,
                    customers: customers,
                    shoppingSessions:
                        shoppingSessions,
                    reservations: reservations,
                    queueEntries:
                        new CheckoutQueueEntrySaveRecord[0],
                    checkoutStation:
                        new CheckoutStationSaveRecord(
                            snapshot.CheckoutStation
                                .StationId,
                            "Available",
                            string.Empty),
                    transactions: transactions,
                    ledgerEntries: ledger);

            Commit(
                builder.Build(),
                nextSnapshot);

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.QueueEntered,
                    "Customer joined the checkout queue.",
                    "checkout-zone"));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
                        .CheckoutCompleted,
                    $"{product.DisplayName} sold.",
                    "checkout-zone"));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
                        .CustomerSatisfied,
                    "Customer satisfied.",
                    "customer"));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Revenue,
                    "Sale completed.",
                    "cash-hud",
                    product.SalePriceCents,
                    snapshot.CurrencyCode));

            return StoreOperationResult.Success(
                "Customer purchase completed.");
        }


        public void PublishFeedback(
            GameplayFeedbackEvent feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(
                    nameof(feedback));
            }

            Raise(feedback);
        }

        public StoreOperationResult
            RemoveFurniturePlacement(
                string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex =
                FindFixtureIndex(
                    fixtureInstanceId);

            if (fixtureIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out StoreFixtureDefinition
                        furniture))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.Fixtures.RemoveAt(
                fixtureIndex);

            builder.AddStock(
                builder.FurnitureWarehouse,
                fixture.DefinitionId,
                1);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<InventoryContainerSaveRecord>
                inventories =
                    new List<
                        InventoryContainerSaveRecord>(
                            snapshot.Inventories);

            List<DisplaySaveRecord> displays =
                new List<DisplaySaveRecord>(
                    snapshot.Displays);

            List<ReservationSaveRecord>
                reservations =
                    new List<
                        ReservationSaveRecord>(
                            snapshot.Reservations);

            if (furniture.SupportsProducts)
            {
                if (fixture.ProductQuantity > 0 &&
                    !string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId))
                {
                    builder.AddStock(
                        builder.ProductWarehouse,
                        fixture.AssignedProductId,
                        fixture.ProductQuantity);

                    inventories =
                        IntegratedSnapshotStoreOperationsMutator
                            .AddProductToContainer(
                                snapshot,
                                BackroomContainerId,
                                200,
                                fixture.AssignedProductId,
                                fixture.ProductQuantity);
                }

                string displayContainerId =
                    DisplayContainerId(
                        fixture.InstanceId);

                inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .RemoveContainer(
                            inventories,
                            displayContainerId);

                displays =
                    IntegratedSnapshotStoreOperationsMutator
                        .RemoveDisplay(
                            displays,
                            fixture.InstanceId);

                reservations =
                    IntegratedSnapshotStoreOperationsMutator
                        .RemoveReservationsForDisplay(
                            reservations,
                            fixture.InstanceId);
            }

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories,
                    displays: displays,
                    reservations: reservations));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.ObjectSelected,
                    $"{furniture.DisplayName} returned to warehouse.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                "Furniture removed and returned.");
        }

        public bool HasPlacedKind(
            StoreFixtureKind kind)
        {
            EnsureInitialized();

            foreach (PlacedStoreFixtureRecord fixture
                     in State.Fixtures)
            {
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetFurnitureWarehouseQuantity(
            string definitionId)
        {
            EnsureInitialized();

            return GetStock(
                State.FurnitureWarehouse,
                definitionId);
        }

        public int GetProductWarehouseQuantity(
            string productId)
        {
            EnsureInitialized();

            return GetStock(
                State.ProductWarehouse,
                productId);
        }

        private StoreOperationResult CreateOrder(
            string itemId,
            bool isFurniture,
            int quantity,
            long unitCostCents)
        {
            if (quantity < 1)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Order quantity must be positive.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            string orderId =
                "phase1-order-" +
                builder.NextOrderSequence
                    .ToString("0000");

            StoreOrderRecord order =
                new StoreOrderRecord(
                    orderId,
                    itemId,
                    isFurniture,
                    StoreOrderStatus.Ordered,
                    quantity,
                    0,
                    unitCostCents);

            builder.Orders.Add(order);
            builder.NextOrderSequence =
                checked(
                    builder.NextOrderSequence + 1);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<SupplierOrderSaveRecord>
                supplierOrders =
                    IntegratedSnapshotStoreOperationsMutator
                        .UpsertSupplierOrder(
                            snapshot,
                            ProjectOrder(order));

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    supplierOrders:
                        supplierOrders));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.ObjectSelected,
                    $"Order {orderId} placed.",
                    "supplier-catalog"));

            return StoreOperationResult.Success(
                orderId);
        }

        private StoreOrderProjection ProjectOrder(
            StoreOrderRecord order)
        {
            string state;

            switch (order.State)
            {
                case StoreOrderStatus.Ordered:
                    state = "Placed";
                    break;
                case StoreOrderStatus.Received:
                    state = "Received";
                    break;
                default:
                    state = "Completed";
                    break;
            }

            return new StoreOrderProjection(
                order.OrderId,
                order.ItemId,
                state,
                order.OrderedUnits,
                order.ReceivedUnits,
                order.UnitCostCents);
        }

        private void Commit(
            StoreOperationsState nextState,
            IntegratedGameStateSnapshot nextSnapshot)
        {
            State = nextState;
            _activeSession.Replace(nextSnapshot);
            StateChanged?.Invoke(State);
        }

        private void Raise(
            GameplayFeedbackEvent feedback)
        {
            FeedbackRaised?.Invoke(feedback);
        }

        private StoreOperationResult Failure(
            StoreOperationStatus status,
            string detail)
        {
            return StoreOperationResult.Failure(
                status,
                detail);
        }

        private int FindOrderIndex(
            string orderId)
        {
            for (int index = 0;
                 index < State.Orders.Count;
                 index++)
            {
                if (string.Equals(
                        State.Orders[index].OrderId,
                        orderId,
                        StringComparison.Ordinal))
                {
                    return index;
                }
            }

            return -1;
        }

        private int FindFixtureIndex(
            string fixtureInstanceId)
        {
            for (int index = 0;
                 index < State.Fixtures.Count;
                 index++)
            {
                if (string.Equals(
                        State.Fixtures[index]
                            .InstanceId,
                        fixtureInstanceId,
                        StringComparison.Ordinal))
                {
                    return index;
                }
            }

            return -1;
        }

        private int FindFirstStockedDisplay()
        {
            for (int index = 0;
                 index < State.Fixtures.Count;
                 index++)
            {
                PlacedStoreFixtureRecord fixture =
                    State.Fixtures[index];

                if (fixture.ProductQuantity > 0 &&
                    !string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId))
                {
                    return index;
                }
            }

            return -1;
        }

        private static int GetStock(
            IReadOnlyList<StoreStockRecord>
                stock,
            string itemId)
        {
            foreach (StoreStockRecord item
                     in stock)
            {
                if (string.Equals(
                        item.ItemId,
                        itemId,
                        StringComparison.Ordinal))
                {
                    return item.Quantity;
                }
            }

            return 0;
        }

        private static string DisplayContainerId(
            string fixtureInstanceId)
        {
            return "phase1-display-" +
                fixtureInstanceId;
        }

        private void EnsureActiveSession()
        {
            if (!_activeSession.HasActiveSession)
            {
                throw new InvalidOperationException(
                    "An active save slot is required.");
            }
        }

        private void EnsureInitialized()
        {
            EnsureActiveSession();

            if (State == null ||
                State.SlotId !=
                    _activeSession.ActiveSlotId)
            {
                InitializeForActiveSlot();
            }
        }
    }
}
