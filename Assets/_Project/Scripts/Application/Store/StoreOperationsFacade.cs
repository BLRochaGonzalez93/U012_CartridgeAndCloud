using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Customers;
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
    public sealed class StoreOperationsFacade :
        IStoreClosingEconomyService,
        IManualSaveCheckpointParticipant,
        IStoreManagementStateProvider
    {
        public const string BackroomContainerId =
            "backroom-inventory";

        private readonly IStoreContentCatalog _catalog;
        private readonly IStoreOperationsStateRepository
            _repository;
        private readonly ActiveGameSessionService
            _activeSession;
        private readonly IUtcClock _clock;
        private readonly ISaveMutationRegistry _mutations;

        public StoreOperationsState State {
            get;
            private set;
        }

        public StoreOperationsState ManagementState => State;

        public bool CanSeedInitialFixtures {
            get;
            private set;
        }

        public long ReservedFundsCents =>
            State == null
                ? 0
                : State.ReservedFundsCents;

        public long AvailableCashCents
        {
            get
            {
                if (!_activeSession.HasActiveSession)
                {
                    return 0;
                }

                return Math.Max(
                    0,
                    _activeSession.Snapshot.CashCents -
                    ReservedFundsCents);
            }
        }

        public int PendingOrderCount
        {
            get
            {
                if (State == null)
                {
                    return 0;
                }

                int count = 0;
                foreach (StoreOrderRecord order in State.Orders)
                {
                    if (order.State == StoreOrderStatus.Reserved)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public event Action<StoreOperationsState>
            StateChanged;

        public event Action<GameplayFeedbackEvent>
            FeedbackRaised;

        public StoreOperationsFacade(
            IStoreContentCatalog catalog,
            IStoreOperationsStateRepository repository,
            ActiveGameSessionService activeSession,
            IUtcClock clock,
            ISaveMutationRegistry mutations = null)
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
            _mutations = mutations ??
                new SaveMutationRegistry();
        }

        public void InitializeForActiveSlot(
            bool preferBackup = false)
        {
            EnsureActiveSession();

            StoreOperationsState loaded =
                _repository.Load(
                    _activeSession.ActiveSlotId,
                    preferBackup);

            string sessionId =
                _activeSession.Snapshot
                    .SessionId.Value;

            bool loadedMatchesSession =
                loaded != null &&
                string.Equals(
                    loaded.SessionId,
                    sessionId,
                    StringComparison.Ordinal);

            State = loadedMatchesSession
                ? loaded
                : StoreOperationsState.Empty(
                    _activeSession.ActiveSlotId,
                    sessionId);

            CanSeedInitialFixtures =
                !loadedMatchesSession ||
                IsPristineState(State);

            SynchronizeManagementDay(false);
            StateChanged?.Invoke(State);
        }

        public void SaveCheckpoint()
        {
            EnsureInitialized();
            SynchronizeManagementDay(false);
            _repository.Save(State);
        }

        public bool CanCheckpoint(out string reason)
        {
            if (!_activeSession.HasActiveSession)
            {
                reason = "No active store session exists.";
                return false;
            }

            if (State == null)
            {
                reason =
                    "Store operations are still initializing.";
                return false;
            }

            string activeSessionId =
                _activeSession.Snapshot.SessionId.ToString();

            if (!string.Equals(
                    State.SessionId,
                    activeSessionId,
                    StringComparison.Ordinal))
            {
                reason =
                    "Store operations do not match the active session.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public IManualSaveCheckpoint BeginCheckpoint()
        {
            if (!CanCheckpoint(out string reason))
            {
                throw new InvalidOperationException(reason);
            }

            SynchronizeManagementDay(false);

            IStoreOperationsCheckpoint checkpoint =
                _repository.BeginCheckpoint(State);

            return new StoreOperationsManualSaveCheckpoint(
                checkpoint);
        }

        public bool DeleteSlotSidecar()
        {
            EnsureActiveSession();
            State = null;
            CanSeedInitialFixtures = false;

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
                    "This authored fixture is not sold in the shop catalog.");
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

            int orderIndex = FindOrderIndex(orderId);
            if (orderIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Order was not found.");
            }

            return ReceiveOrders(
                new[] { orderIndex },
                false);
        }

        public StoreOperationResult ProcessAllPendingOrders()
        {
            EnsureInitialized();

            List<int> indexes = new List<int>();
            for (int index = 0; index < State.Orders.Count; index++)
            {
                if (State.Orders[index].State == StoreOrderStatus.Reserved)
                {
                    indexes.Add(index);
                }
            }

            if (indexes.Count == 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "There are no pending deliveries to process.");
            }

            return ReceiveOrders(indexes, true);
        }

        public StoreOperationResult CancelOrder(string orderId)
        {
            EnsureInitialized();

            int orderIndex = FindOrderIndex(orderId);
            if (orderIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Order was not found.");
            }

            StoreOrderRecord order = State.Orders[orderIndex];
            if (order.State != StoreOrderStatus.Reserved)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Only pending deliveries can be cancelled.");
            }

            StoreOrderRecord cancelled = order.Cancel();
            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            builder.Orders[orderIndex] = cancelled;

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            List<SupplierOrderSaveRecord> supplierOrders =
                IntegratedSnapshotStoreOperationsMutator
                    .UpsertSupplierOrder(
                        snapshot,
                        ProjectOrder(cancelled));

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    supplierOrders: supplierOrders));

            return StoreOperationResult.Success(
                "Order cancelled and reserved funds released.");
        }

        public StoreOperationResult SettleClosingEconomy()
        {
            EnsureInitialized();

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closing",
                    StringComparison.Ordinal) &&
                !string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Daily settlement requires a closing or closed day.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            List<EconomyLedgerSaveRecord> ledger =
                new List<EconomyLedgerSaveRecord>(
                    snapshot.LedgerEntries);

            bool weeklySettlementApplied = false;
            long taxForDay = 0;
            long nextCashCents = snapshot.CashCents;
            string weeklyDetail = string.Empty;

            if (snapshot.CurrentDay % 7 == 0)
            {
                int weekNumber = snapshot.CurrentDay / 7;

                if (State.LastSettledWeek < weekNumber)
                {
                    long gross = State.WeeklyGrossResultCents;
                    taxForDay = new WeeklyTaxPolicy()
                        .CalculateTaxCents(gross);

                    builder.LastSettledWeek = weekNumber;
                    builder.LastWeeklyGrossResultCents = gross;
                    builder.LastWeeklyTaxCents = taxForDay;
                    builder.WeeklyRevenueCents = 0;
                    builder.WeeklySupplierCostCents = 0;

                    if (taxForDay > 0)
                    {
                        builder.LifetimeTaxCents = checked(
                            builder.LifetimeTaxCents + taxForDay);
                        builder.LifetimeExpenseCents = checked(
                            builder.LifetimeExpenseCents + taxForDay);

                        ledger = IntegratedSnapshotStoreOperationsMutator
                            .AppendLedger(
                                snapshot,
                                "weekly-tax-week-" +
                                weekNumber.ToString("000"),
                                "WeeklyTax",
                                "week-" + weekNumber.ToString("000"),
                                taxForDay);
                    }

                    nextCashCents = checked(
                        snapshot.CashCents - taxForDay);
                    weeklySettlementApplied = true;
                    weeklyDetail = taxForDay > 0
                        ? "Weekly tax settled: " + taxForDay +
                          " minor units."
                        : "Weekly result was not positive; tax settled at zero.";
                }
                else if (State.LastSettledWeek == weekNumber)
                {
                    taxForDay = State.LastWeeklyTaxCents;
                }
            }

            StoreManagementHistory previousHistory =
                builder.ManagementHistory;
            builder.ManagementHistory = previousHistory.CloseDay(
                snapshot.CurrentDay,
                snapshot.DayCycle.DayId,
                snapshot.CashCents,
                nextCashCents,
                taxForDay);

            bool historyChanged = !ReferenceEquals(
                previousHistory,
                builder.ManagementHistory);

            if (!weeklySettlementApplied && !historyChanged)
            {
                return StoreOperationResult.Success(
                    "Daily management history was already finalized.");
            }

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    cashCents: nextCashCents,
                    ledgerEntries: ledger);

            Commit(builder.Build(), nextSnapshot);

            if (weeklySettlementApplied)
            {
                return StoreOperationResult.Success(weeklyDetail);
            }

            return StoreOperationResult.Success(
                "Daily management history finalized.");
        }

        private StoreOperationResult ReceiveOrders(
            IReadOnlyList<int> orderIndexes,
            bool combined)
        {
            if (orderIndexes == null || orderIndexes.Count == 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "At least one reserved order is required.");
            }

            long totalCost = 0;
            List<string> orderIds = new List<string>();
            foreach (int index in orderIndexes)
            {
                if (index < 0 || index >= State.Orders.Count)
                {
                    return Failure(
                        StoreOperationStatus.NotFound,
                        "A reserved order was not found.");
                }

                StoreOrderRecord order = State.Orders[index];
                if (order.State != StoreOrderStatus.Reserved ||
                    !order.HasFundsReservation)
                {
                    return Failure(
                        StoreOperationStatus.InvalidState,
                        "Every delivery must be reserved and not already in transit.");
                }

                totalCost = checked(totalCost + order.TotalCostCents);
                orderIds.Add(order.OrderId);
            }

            IntegratedGameStateSnapshot snapshot = _activeSession.Snapshot;
            if (snapshot.CashCents < totalCost)
            {
                return Failure(
                    StoreOperationStatus.InsufficientCash,
                    "Reserved funds are no longer covered by cash.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            string runId = "delivery-run-" +
                builder.NextDeliveryRunSequence.ToString("0000");
            IntegratedGameStateSnapshot working = snapshot;

            foreach (int index in orderIndexes)
            {
                StoreOrderRecord inTransit =
                    builder.Orders[index].BeginTransit(runId);
                builder.Orders[index] = inTransit;

                List<SupplierOrderSaveRecord> supplierOrders =
                    IntegratedSnapshotStoreOperationsMutator.UpsertSupplierOrder(
                        working,
                        ProjectOrder(inTransit));
                working = IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    working.UpdatedUtc,
                    supplierOrders: supplierOrders);
            }

            builder.DeliveryRuns.Add(
                new StoreDeliveryRunRecord(
                    runId,
                    orderIds,
                    totalCost,
                    StoreDeliveryRunStatus.InTransit));
            builder.NextDeliveryRunSequence = checked(
                builder.NextDeliveryRunSequence + 1);

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    _clock.UtcNow));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.DeliveryRunStarted,
                    combined
                        ? "A consolidated delivery run is on its way."
                        : "A delivery run is on its way.",
                    "receiving-zone",
                    correlationId: runId));

            return StoreOperationResult.Success(
                runId + " is in transit with " + orderIds.Count +
                " order(s) and " + totalCost + " reserved minor units.");
        }

        public StoreOperationResult CompleteDeliveryRun(
            string deliveryRunId)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(deliveryRunId))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "A delivery run ID is required.");
            }

            int runIndex = -1;
            for (int index = 0; index < State.DeliveryRuns.Count; index++)
            {
                if (string.Equals(
                        State.DeliveryRuns[index].DeliveryRunId,
                        deliveryRunId,
                        StringComparison.Ordinal))
                {
                    runIndex = index;
                    break;
                }
            }

            if (runIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Delivery run was not found.");
            }

            StoreDeliveryRunRecord run = State.DeliveryRuns[runIndex];
            if (run.Status == StoreDeliveryRunStatus.Received)
            {
                return StoreOperationResult.Success(
                    deliveryRunId + " was already received.");
            }

            IntegratedGameStateSnapshot snapshot = _activeSession.Snapshot;
            if (snapshot.CashCents < run.TotalCostCents)
            {
                return Failure(
                    StoreOperationStatus.InsufficientCash,
                    "Reserved funds are no longer covered by cash.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            builder.ManagementHistory = builder.ManagementHistory.EnsureDay(
                snapshot.CurrentDay,
                snapshot.DayCycle.DayId,
                snapshot.CashCents);

            IntegratedGameStateSnapshot working = snapshot;
            int receivedUnits = 0;

            foreach (string orderId in run.OrderIds)
            {
                int orderIndex = -1;
                for (int index = 0; index < builder.Orders.Count; index++)
                {
                    if (string.Equals(
                            builder.Orders[index].OrderId,
                            orderId,
                            StringComparison.Ordinal))
                    {
                        orderIndex = index;
                        break;
                    }
                }

                if (orderIndex < 0)
                {
                    return Failure(
                        StoreOperationStatus.NotFound,
                        "A delivery-run order was not found.");
                }

                StoreOrderRecord order = builder.Orders[orderIndex];
                if (order.State != StoreOrderStatus.InTransit ||
                    !string.Equals(
                        order.DeliveryRunId,
                        deliveryRunId,
                        StringComparison.Ordinal))
                {
                    return Failure(
                        StoreOperationStatus.InvalidState,
                        "Every run order must remain in transit until physical receipt.");
                }

                StoreOrderRecord received = order.ReceiveAll(deliveryRunId);
                builder.Orders[orderIndex] = received;
                builder.LifetimeExpenseCents = checked(
                    builder.LifetimeExpenseCents + order.TotalCostCents);
                builder.WeeklySupplierCostCents = checked(
                    builder.WeeklySupplierCostCents + order.TotalCostCents);

                int stockUnits = order.OrderedUnits;
                if (order.IsFurniture)
                {
                    builder.AddStock(
                        builder.FurnitureWarehouse,
                        order.ItemId,
                        stockUnits);
                }
                else
                {
                    if (!_catalog.TryGetProduct(
                            order.ItemId,
                            out RetailProductDefinition product))
                    {
                        return Failure(
                            StoreOperationStatus.NotFound,
                            "Ordered product definition is missing.");
                    }

                    stockUnits = checked(
                        order.OrderedUnits * product.UnitsPerCase);
                    builder.AddStock(
                        builder.ProductWarehouse,
                        order.ItemId,
                        stockUnits);

                    List<InventoryContainerSaveRecord> inventories =
                        IntegratedSnapshotStoreOperationsMutator.AddProductToContainer(
                            working,
                            BackroomContainerId,
                            200,
                            order.ItemId,
                            stockUnits);
                    working = IntegratedSnapshotStoreOperationsMutator.Clone(
                        working,
                        working.UpdatedUtc,
                        inventories: inventories);
                }

                builder.ManagementHistory =
                    builder.ManagementHistory.RecordReceipt(
                        snapshot.CurrentDay,
                        snapshot.DayCycle.DayId,
                        snapshot.CashCents,
                        new StoreReceiptDetailRecord(
                            "receipt-" + deliveryRunId + "-" + order.OrderId,
                            deliveryRunId,
                            order.OrderId,
                            order.ItemId,
                            order.IsFurniture,
                            stockUnits,
                            order.TotalCostCents));

                working = IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    working.UpdatedUtc,
                    supplierOrders:
                        IntegratedSnapshotStoreOperationsMutator.UpsertSupplierOrder(
                            working,
                            ProjectOrder(received)));
                working = IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    working.UpdatedUtc,
                    ledgerEntries:
                        IntegratedSnapshotStoreOperationsMutator.AppendLedger(
                            working,
                            "phase1-ledger-cost-" + order.OrderId,
                            "SupplierReceivingCost",
                            order.OrderId,
                            order.TotalCostCents));

                receivedUnits = checked(receivedUnits + stockUnits);
            }

            builder.DeliveryRuns[runIndex] = run.MarkReceived();
            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    _clock.UtcNow,
                    cashCents: checked(
                        snapshot.CashCents - run.TotalCostCents));

            Commit(builder.Build(), nextSnapshot);

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.OrderReceived,
                    deliveryRunId + " was physically received.",
                    "receiving-zone",
                    correlationId: deliveryRunId));
            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Expense,
                    "Received delivery paid from reserved funds.",
                    "cash-hud",
                    run.TotalCostCents,
                    snapshot.CurrencyCode));

            return StoreOperationResult.Success(
                deliveryRunId + " received " + receivedUnits +
                " units for " + run.TotalCostCents + " minor units.");
        }

        public StoreOperationResult SeedInitialFixtures(
            IEnumerable<PlacedStoreFixtureRecord> fixtures)
        {
            EnsureInitialized();

            if (fixtures == null)
            {
                throw new ArgumentNullException(
                    nameof(fixtures));
            }

            if (!CanSeedInitialFixtures)
            {
                return StoreOperationResult.Success(
                    "Initial fixtures were already resolved for this save.");
            }

            List<PlacedStoreFixtureRecord> seed =
                new List<PlacedStoreFixtureRecord>();
            HashSet<string> instanceIds =
                new HashSet<string>(StringComparer.Ordinal);

            foreach (PlacedStoreFixtureRecord fixture in fixtures)
            {
                if (fixture == null)
                {
                    return Failure(
                        StoreOperationStatus.InvalidState,
                        "Initial fixture collection contains null.");
                }

                if (!instanceIds.Add(fixture.InstanceId))
                {
                    return Failure(
                        StoreOperationStatus.Duplicate,
                        "Initial fixture IDs must be unique.");
                }

                if (!_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition definition))
                {
                    return Failure(
                        StoreOperationStatus.NotFound,
                        $"Initial furniture definition '{fixture.DefinitionId}' was not found.");
                }

                if (fixture.ProductQuantity > definition.Capacity)
                {
                    return Failure(
                        StoreOperationStatus.InvalidState,
                        $"Initial stock exceeds {definition.DisplayName} capacity.");
                }

                if (fixture.ProductQuantity > 0 &&
                    !_catalog.TryGetProduct(
                        fixture.AssignedProductId,
                        out _))
                {
                    return Failure(
                        StoreOperationStatus.NotFound,
                        $"Initial product '{fixture.AssignedProductId}' was not found.");
                }

                seed.Add(fixture);
            }

            if (seed.Count == 0)
            {
                CanSeedInitialFixtures = false;
                return StoreOperationResult.Success(
                    "No initial fixtures were supplied.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            IntegratedGameStateSnapshot workingSnapshot =
                _activeSession.Snapshot;

            foreach (PlacedStoreFixtureRecord fixture in seed)
            {
                builder.Fixtures.Add(fixture);

                _catalog.TryGetFurniture(
                    fixture.DefinitionId,
                    out StoreFixtureDefinition definition);

                if (!definition.SupportsProducts)
                {
                    continue;
                }

                string containerId =
                    DisplayContainerId(fixture.InstanceId);

                IEnumerable<InventoryContainerSaveRecord> inventories =
                    IntegratedSnapshotStoreOperationsMutator
                        .AddProductToContainer(
                            workingSnapshot,
                            containerId,
                            definition.Capacity,
                            fixture.ProductQuantity > 0
                                ? fixture.AssignedProductId
                                : "phase1-empty",
                            fixture.ProductQuantity);

                workingSnapshot =
                    IntegratedSnapshotStoreOperationsMutator.Clone(
                        workingSnapshot,
                        workingSnapshot.UpdatedUtc,
                        inventories: inventories);

                IEnumerable<DisplaySaveRecord> displays =
                    IntegratedSnapshotStoreOperationsMutator
                        .UpsertDisplay(
                            workingSnapshot,
                            fixture.InstanceId,
                            fixture.DefinitionId,
                            fixture.ProductQuantity > 0
                                ? fixture.AssignedProductId
                                : string.Empty,
                            containerId);

                workingSnapshot =
                    IntegratedSnapshotStoreOperationsMutator.Clone(
                        workingSnapshot,
                        workingSnapshot.UpdatedUtc,
                        displays: displays);
            }

            builder.NextFixtureSequence =
                checked(
                    builder.NextFixtureSequence +
                    seed.Count);

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    workingSnapshot,
                    _clock.UtcNow));

            CanSeedInitialFixtures = false;

            return StoreOperationResult.Success(
                $"Seeded {seed.Count} authored fixtures.");
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

            if (!string.IsNullOrWhiteSpace(
                    fixture.AssignedProductId))
            {
                if (string.Equals(
                        fixture.AssignedProductId,
                        productId,
                        StringComparison.Ordinal))
                {
                    return Failure(
                        StoreOperationStatus.Duplicate,
                        "This product is already assigned.");
                }

                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Clear the current product assignment before assigning a different product.");
            }

            if (GetActiveReservedQuantity(
                    fixture.InstanceId) > 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Resolve active reservations before assigning a product.");
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
                        furniture) ||
                !furniture.SupportsProducts)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Product display definition is missing.");
            }

            int availableCapacity =
                furniture.Capacity - fixture.ProductQuantity;

            if (availableCapacity < 1)
            {
                return Failure(
                    StoreOperationStatus.CapacityExceeded,
                    "Display is already full.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            int warehouseQuantity =
                builder.GetStock(
                    builder.ProductWarehouse,
                    fixture.AssignedProductId);

            if (warehouseQuantity < 1)
            {
                Raise(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType.OutOfStock,
                        "No received backroom stock is available.",
                        fixture.InstanceId));

                return Failure(
                    StoreOperationStatus.InsufficientStock,
                    "No received backroom stock is available.");
            }

            int transferredQuantity = Math.Min(
                quantity,
                Math.Min(
                    warehouseQuantity,
                    availableCapacity));

            builder.AddStock(
                builder.ProductWarehouse,
                fixture.AssignedProductId,
                -transferredQuantity);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    fixture.AssignedProductId,
                    checked(
                        fixture.ProductQuantity +
                        transferredQuantity));

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
                            transferredQuantity);

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Restocked,
                    $"Restocked {transferredQuantity} units.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                transferredQuantity == quantity
                    ? "Display restocked."
                    : $"Display restocked with {transferredQuantity} of {quantity} requested units.");
        }

        public StoreOperationResult ReturnDisplayStock(
            string fixtureInstanceId,
            int quantity)
        {
            EnsureInitialized();

            if (quantity < 1)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Return quantity must be positive.");
            }

            return ReturnDisplayStockInternal(
                fixtureInstanceId,
                quantity,
                clearAssignment: false,
                requireAllUnits: false);
        }

        public StoreOperationResult ReturnAllDisplayStock(
            string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
            if (fixtureIndex < 0)
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Placed fixture was not found.");
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            if (fixture.ProductQuantity < 1)
            {
                return StoreOperationResult.Success(
                    "Display is already empty and keeps its assignment.");
            }

            return ReturnDisplayStockInternal(
                fixtureInstanceId,
                fixture.ProductQuantity,
                clearAssignment: false,
                requireAllUnits: true);
        }

        public StoreOperationResult ReturnAllAndClearDisplay(
            string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
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
                    "Display has no product assignment.");
            }

            if (fixture.ProductQuantity < 1)
            {
                return ClearDisplayAssignment(
                    fixtureInstanceId);
            }

            return ReturnDisplayStockInternal(
                fixtureInstanceId,
                fixture.ProductQuantity,
                clearAssignment: true,
                requireAllUnits: true);
        }

        public StoreOperationResult ClearDisplayAssignment(
            string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
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
                    "Display has no product assignment.");
            }

            if (fixture.ProductQuantity > 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Return all visible stock before clearing the assignment.");
            }

            if (GetActiveReservedQuantity(
                    fixture.InstanceId) > 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Resolve active reservations before clearing the assignment.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    string.Empty,
                    0);

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            List<DisplaySaveRecord> displays =
                IntegratedSnapshotStoreOperationsMutator
                    .UpsertDisplay(
                        snapshot,
                        fixture.InstanceId,
                        fixture.DefinitionId,
                        string.Empty,
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
                    "Product assignment cleared.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                "Product assignment cleared.");
        }

        private StoreOperationResult ReturnDisplayStockInternal(
            string fixtureInstanceId,
            int quantity,
            bool clearAssignment,
            bool requireAllUnits)
        {
            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
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
                    out StoreFixtureDefinition furniture) ||
                !furniture.SupportsProducts)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "This fixture cannot store display products.");
            }

            if (string.IsNullOrWhiteSpace(
                    fixture.AssignedProductId))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Display has no product assignment.");
            }

            if (quantity > fixture.ProductQuantity)
            {
                return Failure(
                    StoreOperationStatus.InsufficientStock,
                    "Display does not contain the requested quantity.");
            }

            int activeReservedQuantity =
                GetActiveReservedQuantity(
                    fixture.InstanceId,
                    fixture.AssignedProductId);

            int unreservedQuantity =
                fixture.ProductQuantity -
                activeReservedQuantity;

            if (activeReservedQuantity > 0 &&
                (requireAllUnits ||
                 quantity > unreservedQuantity ||
                 clearAssignment))
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Active customer reservations protect this display stock.");
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (quantity > GetAvailableContainerCapacity(
                    snapshot,
                    BackroomContainerId,
                    200))
            {
                return Failure(
                    StoreOperationStatus.CapacityExceeded,
                    "Backroom inventory cannot accept the returned stock.");
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);

            builder.AddStock(
                builder.ProductWarehouse,
                fixture.AssignedProductId,
                quantity);

            int remainingQuantity =
                fixture.ProductQuantity - quantity;

            builder.Fixtures[fixtureIndex] =
                fixture.WithAssignedProduct(
                    clearAssignment
                        ? string.Empty
                        : fixture.AssignedProductId,
                    remainingQuantity);

            List<InventoryContainerSaveRecord> inventories =
                IntegratedSnapshotStoreOperationsMutator
                    .TransferProduct(
                        snapshot,
                        DisplayContainerId(
                            fixture.InstanceId),
                        BackroomContainerId,
                        200,
                        fixture.AssignedProductId,
                        quantity);

            IEnumerable<DisplaySaveRecord> displays =
                snapshot.Displays;

            if (clearAssignment)
            {
                displays =
                    IntegratedSnapshotStoreOperationsMutator
                        .UpsertDisplay(
                            snapshot,
                            fixture.InstanceId,
                            fixture.DefinitionId,
                            string.Empty,
                            DisplayContainerId(
                                fixture.InstanceId));
            }

            Commit(
                builder.Build(),
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    snapshot,
                    _clock.UtcNow,
                    inventories: inventories,
                    displays: displays));

            Raise(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Restocked,
                    clearAssignment
                        ? $"Returned {quantity} units and cleared the assignment."
                        : $"Returned {quantity} units to the backroom.",
                    fixture.InstanceId));

            return StoreOperationResult.Success(
                clearAssignment
                    ? "Display stock returned and assignment cleared."
                    : "Display stock returned to the backroom.");
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
            builder.WeeklyRevenueCents =
                checked(
                    builder.WeeklyRevenueCents +
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

            builder.ManagementHistory =
                builder.ManagementHistory.RecordSale(
                    snapshot.CurrentDay,
                    snapshot.DayCycle.DayId,
                    snapshot.CashCents,
                    new StoreSaleDetailRecord(
                        transactionId,
                        customerId,
                        product.ProductId,
                        1,
                        product.SalePriceCents));

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


        public void SynchronizeManagementDay()
        {
            SynchronizeManagementDay(true);
        }

        public StoreOperationResult RecordCustomerVisit(
            string customerId)
        {
            EnsureInitialized();
            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            StoreManagementHistory next =
                builder.ManagementHistory.RecordVisit(
                    snapshot.CurrentDay,
                    snapshot.DayCycle.DayId,
                    snapshot.CashCents,
                    customerId);

            if (ReferenceEquals(next, builder.ManagementHistory))
            {
                return StoreOperationResult.Success(
                    "Customer visit was already recorded.");
            }

            builder.ManagementHistory = next;
            Commit(builder.Build(), snapshot);
            return StoreOperationResult.Success(
                "Customer visit recorded.");
        }

        public StoreOperationResult RecordCustomerOutcome(
            string customerId,
            StoreCustomerVisitOutcome outcome)
        {
            EnsureInitialized();
            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            StoreManagementHistory next =
                builder.ManagementHistory.RecordVisitOutcome(
                    snapshot.CurrentDay,
                    snapshot.DayCycle.DayId,
                    snapshot.CashCents,
                    customerId,
                    outcome);

            if (ReferenceEquals(next, builder.ManagementHistory))
            {
                return StoreOperationResult.Success(
                    "Customer outcome was already recorded.");
            }

            builder.ManagementHistory = next;
            Commit(builder.Build(), snapshot);
            return StoreOperationResult.Success(
                "Customer outcome recorded.");
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
            CanRemoveFurniturePlacement(
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
                    out StoreFixtureDefinition furniture))
            {
                return Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            string dayState =
                _activeSession.Snapshot.DayCycle.State;

            if (furniture.Kind ==
                StoreFixtureKind.CheckoutCounter)
            {
                StoreCustomerAdmissionDecision decision =
                    new StoreCustomerAdmissionPolicy()
                        .EvaluateCheckoutRemoval(
                            dayState,
                            CountPlacedKind(
                                StoreFixtureKind
                                    .CheckoutCounter));

                if (!decision.Allowed)
                {
                    return Failure(
                        StoreOperationStatus.CheckoutRequired,
                        decision.Detail);
                }
            }

            if (furniture.SupportsProducts &&
                GetActiveReservedQuantity(
                    fixture.InstanceId) > 0)
            {
                return Failure(
                    StoreOperationStatus.InvalidState,
                    "Resolve active customer reservations before removing this display.");
            }

            if (furniture.SupportsProducts &&
                fixture.ProductQuantity >
                GetAvailableContainerCapacity(
                    _activeSession.Snapshot,
                    BackroomContainerId,
                    200))
            {
                return Failure(
                    StoreOperationStatus.CapacityExceeded,
                    "Backroom inventory cannot accept the display stock.");
            }

            return StoreOperationResult.Success(
                "Furniture can be removed.");
        }

        public StoreOperationResult
            RemoveFurniturePlacement(
                string fixtureInstanceId)
        {
            EnsureInitialized();

            StoreOperationResult removalValidation =
                CanRemoveFurniturePlacement(
                    fixtureInstanceId);

            if (!removalValidation.Succeeded)
            {
                return removalValidation;
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
            return CountPlacedKind(kind) > 0;
        }

        public int CountPlacedKind(
            StoreFixtureKind kind)
        {
            EnsureInitialized();

            int count = 0;

            foreach (PlacedStoreFixtureRecord fixture
                     in State.Fixtures)
            {
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    count++;
                }
            }

            return count;
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

        public int GetActiveReservedQuantity(
            string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
            if (fixtureIndex < 0)
            {
                return 0;
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            return GetActiveReservedQuantity(
                fixture.InstanceId,
                fixture.AssignedProductId);
        }

        public int GetUnreservedDisplayQuantity(
            string fixtureInstanceId)
        {
            EnsureInitialized();

            int fixtureIndex = FindFixtureIndex(fixtureInstanceId);
            if (fixtureIndex < 0)
            {
                return 0;
            }

            PlacedStoreFixtureRecord fixture =
                State.Fixtures[fixtureIndex];

            return Math.Max(
                0,
                fixture.ProductQuantity -
                GetActiveReservedQuantity(
                    fixture.InstanceId,
                    fixture.AssignedProductId));
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

            long totalCost = checked(
                unitCostCents * quantity);

            if (AvailableCashCents < totalCost)
            {
                return Failure(
                    StoreOperationStatus.InsufficientCash,
                    "Not enough available cash to reserve this order.");
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
                    StoreOrderStatus.Reserved,
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
                case StoreOrderStatus.Reserved:
                    state = "Reserved";
                    break;
                case StoreOrderStatus.InTransit:
                    state = "InTransit";
                    break;
                case StoreOrderStatus.Received:
                    state = "Received";
                    break;
                case StoreOrderStatus.Cancelled:
                    state = "Cancelled";
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

        private static bool IsPristineState(
            StoreOperationsState state)
        {
            return state != null &&
                   state.Generation == 0 &&
                   state.Orders.Count == 0 &&
                   state.DeliveryRuns.Count == 0 &&
                   state.Fixtures.Count == 0 &&
                   state.FurnitureWarehouse.Count == 0 &&
                   state.ProductWarehouse.Count == 0 &&
                   state.CompletedSales == 0 &&
                   state.LifetimeRevenueCents == 0 &&
                   state.LifetimeExpenseCents == 0 &&
                   state.LifetimeTaxCents == 0 &&
                   state.WeeklyRevenueCents == 0 &&
                   state.WeeklySupplierCostCents == 0 &&
                   state.LastSettledWeek == 0 &&
                   state.LastWeeklyGrossResultCents == 0 &&
                   state.LastWeeklyTaxCents == 0;
        }

        private sealed class StoreOperationsManualSaveCheckpoint :
            IManualSaveCheckpoint
        {
            private IStoreOperationsCheckpoint _checkpoint;

            public StoreOperationsManualSaveCheckpoint(
                IStoreOperationsCheckpoint checkpoint)
            {
                _checkpoint = checkpoint ??
                    throw new ArgumentNullException(
                        nameof(checkpoint));
            }

            public void Commit()
            {
                if (_checkpoint == null)
                {
                    throw new ObjectDisposedException(
                        nameof(StoreOperationsManualSaveCheckpoint));
                }

                _checkpoint.Commit();
            }

            public void Dispose()
            {
                IStoreOperationsCheckpoint checkpoint =
                    _checkpoint;
                if (checkpoint == null)
                {
                    return;
                }

                _checkpoint = null;
                checkpoint.Dispose();
            }
        }

        private void SynchronizeManagementDay(bool notify)
        {
            EnsureInitialized();
            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            StoreManagementHistory nextHistory =
                State.ManagementHistory.EnsureDay(
                    snapshot.CurrentDay,
                    snapshot.DayCycle.DayId,
                    snapshot.CashCents);

            if (ReferenceEquals(
                    nextHistory,
                    State.ManagementHistory))
            {
                return;
            }

            StoreOperationsStateBuilder builder =
                new StoreOperationsStateBuilder(State);
            builder.ManagementHistory = nextHistory;
            State = builder.Build(false);

            if (notify)
            {
                StateChanged?.Invoke(State);
            }
        }

        private void Commit(
            StoreOperationsState nextState,
            IntegratedGameStateSnapshot nextSnapshot)
        {
            using (_mutations.Begin(
                       "StoreOperationsCommit"))
            {
                State = nextState;
                _activeSession.Replace(nextSnapshot);
                StateChanged?.Invoke(State);
            }
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

        private int GetActiveReservedQuantity(
            string displayId,
            string productId)
        {
            if (string.IsNullOrWhiteSpace(displayId))
            {
                return 0;
            }

            int total = 0;

            foreach (ReservationSaveRecord reservation
                     in _activeSession.Snapshot.Reservations)
            {
                if (!string.Equals(
                        reservation.State,
                        "Active",
                        StringComparison.Ordinal) ||
                    !string.Equals(
                        reservation.DisplayId,
                        displayId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(productId) &&
                    !string.Equals(
                        reservation.ProductId,
                        productId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                total = checked(
                    total + reservation.Quantity);
            }

            return total;
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
                        fixture.AssignedProductId) &&
                    fixture.ProductQuantity -
                    GetActiveReservedQuantity(
                        fixture.InstanceId,
                        fixture.AssignedProductId) > 0)
                {
                    return index;
                }
            }

            return -1;
        }

        private static int GetAvailableContainerCapacity(
            IntegratedGameStateSnapshot snapshot,
            string containerId,
            int defaultCapacity)
        {
            foreach (InventoryContainerSaveRecord inventory
                     in snapshot.Inventories)
            {
                if (!string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                int used = 0;
                foreach (ProductQuantitySaveRecord product
                         in inventory.Products)
                {
                    used = checked(used + product.Quantity);
                }

                return Math.Max(
                    0,
                    inventory.Capacity - used);
            }

            return defaultCapacity;
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
