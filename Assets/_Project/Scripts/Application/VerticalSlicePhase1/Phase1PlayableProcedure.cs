using System;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    public enum Phase1ProcedureStep
    {
        OrderCheckout = 0,
        ReceiveCheckout = 1,
        PlaceCheckout = 2,
        OrderDisplay = 3,
        ReceiveDisplay = 4,
        PlaceDisplay = 5,
        OrderProduct = 6,
        ReceiveProduct = 7,
        AssignProduct = 8,
        RestockDisplay = 9,
        OpenStore = 10,
        ServeCustomer = 11,
        CloseDay = 12,
        Autosave = 13,
        Completed = 14
    }

    public sealed class Phase1ProcedureStatus
    {
        public Phase1ProcedureStep Step { get; }
        public string Title { get; }
        public string Instruction { get; }

        public Phase1ProcedureStatus(
            Phase1ProcedureStep step,
            string title,
            string instruction)
        {
            Step = step;
            Title = title ??
                throw new ArgumentNullException(
                    nameof(title));
            Instruction = instruction ??
                throw new ArgumentNullException(
                    nameof(instruction));
        }
    }

    public sealed class Phase1PlayableProcedure
    {
        private readonly IPhase1Catalog _catalog;

        public Phase1PlayableProcedure(
            IPhase1Catalog catalog)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
        }

        public Phase1ProcedureStatus Evaluate(
            Phase1StoreState state,
            IntegratedGameStateSnapshot snapshot,
            bool autosaveCompleted)
        {
            if (state == null)
            {
                throw new ArgumentNullException(
                    nameof(state));
            }

            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            if (!HasFurnitureOrder(
                    state,
                    Phase1FurnitureKind
                        .CheckoutCounter))
            {
                return Status(
                    Phase1ProcedureStep.OrderCheckout,
                    "Buy a checkout counter",
                    "Open Shop Catalog and order one checkout counter.");
            }

            if (!HasReceivedFurniture(
                    state,
                    Phase1FurnitureKind
                        .CheckoutCounter))
            {
                return Status(
                    Phase1ProcedureStep.ReceiveCheckout,
                    "Receive checkout furniture",
                    "Open Deliveries and receive the checkout order.");
            }

            if (!HasPlacedFurniture(
                    state,
                    Phase1FurnitureKind
                        .CheckoutCounter))
            {
                return Status(
                    Phase1ProcedureStep.PlaceCheckout,
                    "Place the checkout counter",
                    "Select the received counter, position it on the grid and confirm.");
            }

            if (!HasAnyDisplayOrder(state))
            {
                return Status(
                    Phase1ProcedureStep.OrderDisplay,
                    "Buy a product display",
                    "Order a central shelf, wall shelf, low display or featured display.");
            }

            if (!HasAnyReceivedDisplay(state))
            {
                return Status(
                    Phase1ProcedureStep.ReceiveDisplay,
                    "Receive display furniture",
                    "Receive the display order in the backroom.");
            }

            if (!HasAnyPlacedDisplay(state))
            {
                return Status(
                    Phase1ProcedureStep.PlaceDisplay,
                    "Place a display",
                    "Place the received display without blocking access routes.");
            }

            if (!HasProductOrder(state))
            {
                return Status(
                    Phase1ProcedureStep.OrderProduct,
                    "Order merchandise",
                    "Choose a fictitious product and place a supplier order.");
            }

            if (!HasProductStock(state))
            {
                return Status(
                    Phase1ProcedureStep.ReceiveProduct,
                    "Receive merchandise",
                    "Receive and pay the product delivery.");
            }

            if (!HasAssignedDisplay(state))
            {
                return Status(
                    Phase1ProcedureStep.AssignProduct,
                    "Assign a product",
                    "Select a placed display and assign the received product.");
            }

            if (!HasStockedDisplay(state))
            {
                return Status(
                    Phase1ProcedureStep.RestockDisplay,
                    "Restock the display",
                    "Transfer units from the backroom to the assigned display.");
            }

            if (string.Equals(
                    snapshot.DayCycle.State,
                    "BeforeOpen",
                    StringComparison.Ordinal))
            {
                return Status(
                    Phase1ProcedureStep.OpenStore,
                    "Open the store",
                    "Use Day Cycle to open the store.");
            }

            if (state.CompletedSales == 0)
            {
                return Status(
                    Phase1ProcedureStep.ServeCustomer,
                    "Serve a customer",
                    "Spawn the next customer and complete one purchase.");
            }

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return Status(
                    Phase1ProcedureStep.CloseDay,
                    "Close the day",
                    "Begin Closing, clear operations and complete the close.");
            }

            if (!autosaveCompleted)
            {
                return Status(
                    Phase1ProcedureStep.Autosave,
                    "Wait for autosave",
                    "Confirm that the active slot reports Saved.");
            }

            return Status(
                Phase1ProcedureStep.Completed,
                "Vertical slice loop complete",
                "The functional Phase 1 gate is complete. Continue to the next day or repeat the loop.");
        }

        private bool HasFurnitureOrder(
            Phase1StoreState state,
            Phase1FurnitureKind kind)
        {
            foreach (Phase1OrderRecord order
                     in state.Orders)
            {
                if (order.IsFurniture &&
                    _catalog.TryGetFurniture(
                        order.ItemId,
                        out Phase1FurnitureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    return true;
                }
            }

            return false;
        }


        private bool HasAnyDisplayOrder(
            Phase1StoreState state)
        {
            foreach (Phase1OrderRecord order
                     in state.Orders)
            {
                if (order.IsFurniture &&
                    _catalog.TryGetFurniture(
                        order.ItemId,
                        out Phase1FurnitureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasReceivedFurniture(
            Phase1StoreState state,
            Phase1FurnitureKind kind)
        {
            foreach (Phase1StockRecord stock
                     in state.FurnitureWarehouse)
            {
                if (stock.Quantity > 0 &&
                    _catalog.TryGetFurniture(
                        stock.ItemId,
                        out Phase1FurnitureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    return true;
                }
            }

            return HasPlacedFurniture(state, kind);
        }

        private bool HasPlacedFurniture(
            Phase1StoreState state,
            Phase1FurnitureKind kind)
        {
            foreach (Phase1PlacedFixtureRecord fixture
                     in state.Fixtures)
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

        private bool HasAnyReceivedDisplay(
            Phase1StoreState state)
        {
            foreach (Phase1StockRecord stock
                     in state.FurnitureWarehouse)
            {
                if (stock.Quantity > 0 &&
                    _catalog.TryGetFurniture(
                        stock.ItemId,
                        out Phase1FurnitureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return HasAnyPlacedDisplay(state);
        }

        private bool HasAnyPlacedDisplay(
            Phase1StoreState state)
        {
            foreach (Phase1PlacedFixtureRecord fixture
                     in state.Fixtures)
            {
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out Phase1FurnitureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasProductOrder(
            Phase1StoreState state)
        {
            foreach (Phase1OrderRecord order
                     in state.Orders)
            {
                if (!order.IsFurniture)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasProductStock(
            Phase1StoreState state)
        {
            foreach (Phase1StockRecord stock
                     in state.ProductWarehouse)
            {
                if (stock.Quantity > 0)
                {
                    return true;
                }
            }

            return HasStockedDisplay(state);
        }

        private static bool HasAssignedDisplay(
            Phase1StoreState state)
        {
            foreach (Phase1PlacedFixtureRecord fixture
                     in state.Fixtures)
            {
                if (!string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasStockedDisplay(
            Phase1StoreState state)
        {
            foreach (Phase1PlacedFixtureRecord fixture
                     in state.Fixtures)
            {
                if (fixture.ProductQuantity > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static Phase1ProcedureStatus Status(
            Phase1ProcedureStep step,
            string title,
            string instruction)
        {
            return new Phase1ProcedureStatus(
                step,
                title,
                instruction);
        }
    }
}
