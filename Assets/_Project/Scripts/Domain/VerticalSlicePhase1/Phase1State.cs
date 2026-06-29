using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1
{
    public sealed class Phase1OrderRecord
    {
        public string OrderId { get; }
        public string ItemId { get; }
        public bool IsFurniture { get; }
        public Phase1OrderState State { get; }
        public int OrderedUnits { get; }
        public int ReceivedUnits { get; }
        public long UnitCostCents { get; }

        public Phase1OrderRecord(
            string orderId,
            string itemId,
            bool isFurniture,
            Phase1OrderState state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents)
        {
            OrderId = Required(
                orderId,
                nameof(orderId));
            ItemId = Required(
                itemId,
                nameof(itemId));

            if (orderedUnits < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(orderedUnits));
            }

            if (receivedUnits < 0 ||
                receivedUnits > orderedUnits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(receivedUnits));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            IsFurniture = isFurniture;
            State = state;
            OrderedUnits = orderedUnits;
            ReceivedUnits = receivedUnits;
            UnitCostCents = unitCostCents;
        }

        public Phase1OrderRecord ReceiveAll()
        {
            return new Phase1OrderRecord(
                OrderId,
                ItemId,
                IsFurniture,
                Phase1OrderState.Received,
                OrderedUnits,
                OrderedUnits,
                UnitCostCents);
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class Phase1StockRecord
    {
        public string ItemId { get; }
        public int Quantity { get; }

        public Phase1StockRecord(
            string itemId,
            int quantity)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException(
                    "An item ID is required.",
                    nameof(itemId));
            }

            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity));
            }

            ItemId = itemId;
            Quantity = quantity;
        }
    }

    public sealed class Phase1PlacedFixtureRecord
    {
        public string InstanceId { get; }
        public string DefinitionId { get; }
        public int AnchorX { get; }
        public int AnchorZ { get; }
        public int RotationQuarterTurns { get; }
        public string AssignedProductId { get; }
        public int ProductQuantity { get; }

        public Phase1PlacedFixtureRecord(
            string instanceId,
            string definitionId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns,
            string assignedProductId,
            int productQuantity)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                throw new ArgumentException(
                    "An instance ID is required.",
                    nameof(instanceId));
            }

            if (string.IsNullOrWhiteSpace(definitionId))
            {
                throw new ArgumentException(
                    "A definition ID is required.",
                    nameof(definitionId));
            }

            if (rotationQuarterTurns < 0 ||
                rotationQuarterTurns > 3)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rotationQuarterTurns));
            }

            if (productQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(productQuantity));
            }

            if (productQuantity > 0 &&
                string.IsNullOrWhiteSpace(
                    assignedProductId))
            {
                throw new ArgumentException(
                    "Stocked fixture requires an assigned product.",
                    nameof(assignedProductId));
            }

            InstanceId = instanceId;
            DefinitionId = definitionId;
            AnchorX = anchorX;
            AnchorZ = anchorZ;
            RotationQuarterTurns =
                rotationQuarterTurns;
            AssignedProductId =
                assignedProductId ?? string.Empty;
            ProductQuantity = productQuantity;
        }

        public Phase1PlacedFixtureRecord
            WithAssignedProduct(
                string productId,
                int quantity)
        {
            return new Phase1PlacedFixtureRecord(
                InstanceId,
                DefinitionId,
                AnchorX,
                AnchorZ,
                RotationQuarterTurns,
                productId,
                quantity);
        }
    }

    public sealed class Phase1StoreState
    {
        private readonly ReadOnlyCollection<
            Phase1OrderRecord> _orders;
        private readonly ReadOnlyCollection<
            Phase1StockRecord> _furnitureWarehouse;
        private readonly ReadOnlyCollection<
            Phase1StockRecord> _productWarehouse;
        private readonly ReadOnlyCollection<
            Phase1PlacedFixtureRecord> _fixtures;

        public SaveSlotId SlotId { get; }
        public string SessionId { get; }
        public int Generation { get; }
        public int NextOrderSequence { get; }
        public int NextFixtureSequence { get; }
        public int NextCustomerSequence { get; }
        public int CompletedSales { get; }
        public long LifetimeRevenueCents { get; }
        public long LifetimeExpenseCents { get; }

        public IReadOnlyList<Phase1OrderRecord>
            Orders => _orders;

        public IReadOnlyList<Phase1StockRecord>
            FurnitureWarehouse =>
                _furnitureWarehouse;

        public IReadOnlyList<Phase1StockRecord>
            ProductWarehouse =>
                _productWarehouse;

        public IReadOnlyList<
            Phase1PlacedFixtureRecord> Fixtures =>
                _fixtures;

        public Phase1StoreState(
            SaveSlotId slotId,
            string sessionId,
            int generation,
            int nextOrderSequence,
            int nextFixtureSequence,
            int nextCustomerSequence,
            int completedSales,
            long lifetimeRevenueCents,
            long lifetimeExpenseCents,
            IEnumerable<Phase1OrderRecord> orders,
            IEnumerable<Phase1StockRecord>
                furnitureWarehouse,
            IEnumerable<Phase1StockRecord>
                productWarehouse,
            IEnumerable<Phase1PlacedFixtureRecord>
                fixtures)
        {
            if (generation < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(generation));
            }

            if (nextOrderSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextOrderSequence));
            }

            if (nextFixtureSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextFixtureSequence));
            }

            if (nextCustomerSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextCustomerSequence));
            }

            if (completedSales < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(completedSales));
            }

            if (lifetimeRevenueCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lifetimeRevenueCents));
            }

            if (lifetimeExpenseCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lifetimeExpenseCents));
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentException(
                    "A session ID is required.",
                    nameof(sessionId));
            }

            SlotId = slotId;
            SessionId = sessionId;
            Generation = generation;
            NextOrderSequence =
                nextOrderSequence;
            NextFixtureSequence =
                nextFixtureSequence;
            NextCustomerSequence =
                nextCustomerSequence;
            CompletedSales = completedSales;
            LifetimeRevenueCents =
                lifetimeRevenueCents;
            LifetimeExpenseCents =
                lifetimeExpenseCents;

            _orders = Copy(
                orders,
                nameof(orders));
            _furnitureWarehouse = Copy(
                furnitureWarehouse,
                nameof(furnitureWarehouse));
            _productWarehouse = Copy(
                productWarehouse,
                nameof(productWarehouse));
            _fixtures = Copy(
                fixtures,
                nameof(fixtures));

            ValidateUniqueIds();
        }

        public static Phase1StoreState Empty(
            SaveSlotId slotId,
            string sessionId)
        {
            return new Phase1StoreState(
                slotId,
                sessionId,
                0,
                1,
                1,
                1,
                0,
                0,
                0,
                new Phase1OrderRecord[0],
                new Phase1StockRecord[0],
                new Phase1StockRecord[0],
                new Phase1PlacedFixtureRecord[0]);
        }

        private void ValidateUniqueIds()
        {
            HashSet<string> orderIds =
                new HashSet<string>(
                    StringComparer.Ordinal);
            HashSet<string> fixtureIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (Phase1OrderRecord order
                     in _orders)
            {
                if (!orderIds.Add(order.OrderId))
                {
                    throw new ArgumentException(
                        "Duplicate order ID.");
                }
            }

            foreach (Phase1PlacedFixtureRecord fixture
                     in _fixtures)
            {
                if (!fixtureIds.Add(
                        fixture.InstanceId))
                {
                    throw new ArgumentException(
                        "Duplicate fixture ID.");
                }
            }
        }

        private static ReadOnlyCollection<T> Copy<T>(
            IEnumerable<T> source,
            string parameterName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    parameterName);
            }

            List<T> copy = new List<T>();

            foreach (T item in source)
            {
                if (item == null)
                {
                    throw new ArgumentException(
                        "Collection cannot contain null.",
                        parameterName);
                }

                copy.Add(item);
            }

            return new ReadOnlyCollection<T>(
                copy);
        }
    }
}
