using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    public sealed class Phase1VerticalSliceService
    {
        public const string BackroomContainerId =
            "backroom-inventory";

        private readonly IPhase1Catalog _catalog;
        private readonly IPhase1StateRepository
            _repository;
        private readonly ActiveGameSessionService
            _activeSession;
        private readonly IPhase1UtcClock _clock;

        public Phase1StoreState State {
            get;
            private set;
        }

        public event Action<Phase1StoreState>
            StateChanged;

        public event Action<Phase1FeedbackEvent>
            FeedbackRaised;

        public Phase1VerticalSliceService(
            IPhase1Catalog catalog,
            IPhase1StateRepository repository,
            ActiveGameSessionService activeSession,
            IPhase1UtcClock clock)
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

            Phase1StoreState loaded =
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
                    : Phase1StoreState.Empty(
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

        public Phase1OperationResult OrderFurniture(
            string definitionId,
            int quantity)
        {
            EnsureInitialized();

            if (!_catalog.TryGetFurniture(
                    definitionId,
                    out Phase1FurnitureDefinition
                        definition))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Furniture definition was not found.");
            }

            if (!definition.IsPurchasable)
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
                    "This blockout fixture is not sold in the shop catalog.");
            }

            return CreateOrder(
                definition.DefinitionId,
                isFurniture: true,
                quantity,
                definition.UnitCostCents);
        }

        public Phase1OperationResult OrderProduct(
            string productId,
            int caseQuantity)
        {
            EnsureInitialized();

            if (!_catalog.TryGetProduct(
                    productId,
                    out Phase1ProductDefinition
                        definition))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
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

        public Phase1OperationResult ReceiveOrder(
            string orderId)
        {
            EnsureInitialized();

            int orderIndex =
                FindOrderIndex(orderId);

            if (orderIndex < 0)
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Order was not found.");
            }

            Phase1OrderRecord order =
                State.Orders[orderIndex];

            if (order.State !=
                Phase1OrderState.Ordered)
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
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
                    Phase1OperationStatus
                        .InsufficientCash,
                    "Not enough cash to receive this order.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

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
                        out Phase1ProductDefinition
                            product))
                {
                    return Failure(
                        Phase1OperationStatus.NotFound,
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
                    IntegratedSnapshotPhase1Mutator
                        .AddProductToContainer(
                            snapshot,
                            BackroomContainerId,
                            200,
                            order.ItemId,
                            receivedStock);
            }

            List<SupplierOrderSaveRecord>
                supplierOrders =
                    IntegratedSnapshotPhase1Mutator
                        .UpsertSupplierOrder(
                            snapshot,
                            ProjectOrder(
                                order.ReceiveAll()));

            List<EconomyLedgerSaveRecord> ledger =
                IntegratedSnapshotPhase1Mutator
                    .AppendLedger(
                        snapshot,
                        "phase1-ledger-cost-" +
                        order.OrderId,
                        "SupplierReceivingCost",
                        order.OrderId,
                        totalCost);

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotPhase1Mutator.Clone(
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
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.OrderReceived,
                    $"Received {receivedStock} × " +
                    $"{order.ItemId}.",
                    "receiving-zone"));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Expense,
                    "Supplier delivery paid.",
                    "cash-hud",
                    totalCost,
                    snapshot.CurrencyCode));

            return Phase1OperationResult.Success(
                "Order received and paid.");
        }

        public Phase1OperationResult ConfirmFurniturePlacement(
            string definitionId,
            string instanceId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns)
        {
            EnsureInitialized();

            if (!_catalog.TryGetFurniture(
                    definitionId,
                    out Phase1FurnitureDefinition
                        definition))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Furniture definition was not found.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

            if (builder.GetStock(
                    builder.FurnitureWarehouse,
                    definitionId) < 1)
            {
                return Failure(
                    Phase1OperationStatus
                        .InsufficientStock,
                    "Receive this furniture before placing it.");
            }

            foreach (Phase1PlacedFixtureRecord fixture
                     in builder.Fixtures)
            {
                if (string.Equals(
                        fixture.InstanceId,
                        instanceId,
                        StringComparison.Ordinal))
                {
                    return Failure(
                        Phase1OperationStatus.Duplicate,
                        "Placement instance already exists.");
                }
            }

            builder.AddStock(
                builder.FurnitureWarehouse,
                definitionId,
                -1);

            Phase1PlacedFixtureRecord placed =
                new Phase1PlacedFixtureRecord(
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
                    IntegratedSnapshotPhase1Mutator
                        .AddProductToContainer(
                            snapshot,
                            containerId,
                            definition.Capacity,
                            "phase1-empty",
                            0);

                IntegratedGameStateSnapshot interim =
                    IntegratedSnapshotPhase1Mutator
                        .Clone(
                            snapshot,
                            snapshot.UpdatedUtc,
                            inventories: inventories);

                displays =
                    IntegratedSnapshotPhase1Mutator
                        .UpsertDisplay(
                            interim,
                            instanceId,
                            definitionId,
                            string.Empty,
                            containerId);
            }

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotPhase1Mutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories,
                    displays: displays);

            Commit(
                builder.Build(),
                nextSnapshot);

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.ObjectSelected,
                    $"{definition.DisplayName} placed.",
                    instanceId));

            return Phase1OperationResult.Success(
                "Furniture placed.");
        }

        public Phase1OperationResult AssignProduct(
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
                    Phase1OperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            Phase1PlacedFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out Phase1FurnitureDefinition
                        furniture) ||
                !furniture.SupportsProducts)
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
                    "This fixture cannot display products.");
            }

            if (!_catalog.TryGetProduct(
                    productId,
                    out _))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Product definition was not found.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    productId,
                    0);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<DisplaySaveRecord> displays =
                IntegratedSnapshotPhase1Mutator
                    .UpsertDisplay(
                        snapshot,
                        fixture.InstanceId,
                        fixture.DefinitionId,
                        productId,
                        DisplayContainerId(
                            fixture.InstanceId));

            Commit(
                builder.Build(),
                IntegratedSnapshotPhase1Mutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    displays: displays));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.ProductAssigned,
                    $"{productId} assigned.",
                    fixture.InstanceId));

            return Phase1OperationResult.Success(
                "Product assigned.");
        }

        public Phase1OperationResult RestockDisplay(
            string fixtureInstanceId,
            int quantity)
        {
            EnsureInitialized();

            if (quantity < 1)
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
                    "Restock quantity must be positive.");
            }

            int fixtureIndex =
                FindFixtureIndex(
                    fixtureInstanceId);

            if (fixtureIndex < 0)
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            Phase1PlacedFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (string.IsNullOrWhiteSpace(
                    fixture.AssignedProductId))
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
                    "Assign a product before restocking.");
            }

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out Phase1FurnitureDefinition
                        furniture))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            if (fixture.ProductQuantity + quantity >
                furniture.Capacity)
            {
                return Failure(
                    Phase1OperationStatus
                        .CapacityExceeded,
                    "Display capacity would be exceeded.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

            if (builder.GetStock(
                    builder.ProductWarehouse,
                    fixture.AssignedProductId) <
                quantity)
            {
                Raise(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind.OutOfStock,
                        "Not enough backroom stock.",
                        fixture.InstanceId));

                return Failure(
                    Phase1OperationStatus
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
                    IntegratedSnapshotPhase1Mutator
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
                IntegratedSnapshotPhase1Mutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Restocked,
                    $"Restocked {quantity} units.",
                    fixture.InstanceId));

            return Phase1OperationResult.Success(
                "Display restocked.");
        }

        public Phase1OperationResult
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
                    Phase1OperationStatus
                        .StoreMustBeOpen,
                    "Open the store before serving customers.");
            }

            if (!HasPlacedKind(
                    Phase1FurnitureKind
                        .CheckoutCounter))
            {
                return Failure(
                    Phase1OperationStatus
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
                    Phase1OperationStatus.InvalidState,
                    "Checkout is already processing another customer.");
            }

            int fixtureIndex =
                FindFirstStockedDisplay();

            if (fixtureIndex < 0)
            {
                return Failure(
                    Phase1OperationStatus
                        .InsufficientStock,
                    "No stocked display is available.");
            }

            Phase1PlacedFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetProduct(
                    fixture.AssignedProductId,
                    out _))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Assigned product definition is missing.");
            }

            return Phase1OperationResult.Success(
                "Customer purchase is ready.");
        }

        public Phase1OperationResult
            ProcessNextCustomerPurchase()
        {
            EnsureInitialized();

            Phase1OperationResult validation =
                ValidateNextCustomerPurchase();

            if (!validation.Succeeded)
            {
                if (validation.Status ==
                    Phase1OperationStatus
                        .InsufficientStock)
                {
                    Raise(
                        new Phase1FeedbackEvent(
                            Phase1FeedbackKind
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

            Phase1PlacedFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetProduct(
                    fixture.AssignedProductId,
                    out Phase1ProductDefinition
                        product))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Assigned product definition is missing.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

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
                    IntegratedSnapshotPhase1Mutator
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
                IntegratedSnapshotPhase1Mutator
                    .AppendLedger(
                        snapshot,
                        "phase1-ledger-revenue-" +
                        transactionId,
                        "CheckoutRevenue",
                        transactionId,
                        product.SalePriceCents);

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotPhase1Mutator.Clone(
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
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.QueueEntered,
                    "Customer joined the checkout queue.",
                    "checkout-zone"));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind
                        .CheckoutCompleted,
                    $"{product.DisplayName} sold.",
                    "checkout-zone"));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind
                        .CustomerSatisfied,
                    "Customer satisfied.",
                    "customer"));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Revenue,
                    "Sale completed.",
                    "cash-hud",
                    product.SalePriceCents,
                    snapshot.CurrencyCode));

            return Phase1OperationResult.Success(
                "Customer purchase completed.");
        }


        public void PublishFeedback(
            Phase1FeedbackEvent feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(
                    nameof(feedback));
            }

            Raise(feedback);
        }

        public Phase1OperationResult
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
                    Phase1OperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            Phase1PlacedFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (!_catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out Phase1FurnitureDefinition
                        furniture))
            {
                return Failure(
                    Phase1OperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

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
                        IntegratedSnapshotPhase1Mutator
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
                    IntegratedSnapshotPhase1Mutator
                        .RemoveContainer(
                            inventories,
                            displayContainerId);

                displays =
                    IntegratedSnapshotPhase1Mutator
                        .RemoveDisplay(
                            displays,
                            fixture.InstanceId);

                reservations =
                    IntegratedSnapshotPhase1Mutator
                        .RemoveReservationsForDisplay(
                            reservations,
                            fixture.InstanceId);
            }

            Commit(
                builder.Build(),
                IntegratedSnapshotPhase1Mutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories,
                    displays: displays,
                    reservations: reservations));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.ObjectSelected,
                    $"{furniture.DisplayName} returned to warehouse.",
                    fixture.InstanceId));

            return Phase1OperationResult.Success(
                "Furniture removed and returned.");
        }

        public bool HasPlacedKind(
            Phase1FurnitureKind kind)
        {
            EnsureInitialized();

            foreach (Phase1PlacedFixtureRecord fixture
                     in State.Fixtures)
            {
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out Phase1FurnitureDefinition
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

        private Phase1OperationResult CreateOrder(
            string itemId,
            bool isFurniture,
            int quantity,
            long unitCostCents)
        {
            if (quantity < 1)
            {
                return Failure(
                    Phase1OperationStatus.InvalidState,
                    "Order quantity must be positive.");
            }

            Phase1StateBuilder builder =
                new Phase1StateBuilder(State);

            string orderId =
                "phase1-order-" +
                builder.NextOrderSequence
                    .ToString("0000");

            Phase1OrderRecord order =
                new Phase1OrderRecord(
                    orderId,
                    itemId,
                    isFurniture,
                    Phase1OrderState.Ordered,
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
                    IntegratedSnapshotPhase1Mutator
                        .UpsertSupplierOrder(
                            snapshot,
                            ProjectOrder(order));

            Commit(
                builder.Build(),
                IntegratedSnapshotPhase1Mutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    supplierOrders:
                        supplierOrders));

            Raise(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.ObjectSelected,
                    $"Order {orderId} placed.",
                    "supplier-catalog"));

            return Phase1OperationResult.Success(
                orderId);
        }

        private Phase1OrderProjection ProjectOrder(
            Phase1OrderRecord order)
        {
            string state;

            switch (order.State)
            {
                case Phase1OrderState.Ordered:
                    state = "Placed";
                    break;
                case Phase1OrderState.Received:
                    state = "Received";
                    break;
                default:
                    state = "Completed";
                    break;
            }

            return new Phase1OrderProjection(
                order.OrderId,
                order.ItemId,
                state,
                order.OrderedUnits,
                order.ReceivedUnits,
                order.UnitCostCents);
        }

        private void Commit(
            Phase1StoreState nextState,
            IntegratedGameStateSnapshot nextSnapshot)
        {
            State = nextState;
            _activeSession.Replace(nextSnapshot);
            StateChanged?.Invoke(State);
        }

        private void Raise(
            Phase1FeedbackEvent feedback)
        {
            FeedbackRaised?.Invoke(feedback);
        }

        private Phase1OperationResult Failure(
            Phase1OperationStatus status,
            string detail)
        {
            return Phase1OperationResult.Failure(
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
                Phase1PlacedFixtureRecord fixture =
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
            IReadOnlyList<Phase1StockRecord>
                stock,
            string itemId)
        {
            foreach (Phase1StockRecord item
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
                    "An active Sprint 15 slot is required.");
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
