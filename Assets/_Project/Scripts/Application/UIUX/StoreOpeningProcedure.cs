using System;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Inventory;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class StoreOpeningProcedure
    {
        private readonly IStoreContentCatalog _catalog;

        public StoreOpeningProcedure(
            IStoreContentCatalog catalog)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
        }

        public StoreOpeningProcedureStatus Evaluate(
            StoreOperationsState state,
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
                    StoreFixtureKind
                        .CheckoutCounter))
            {
                return Status(
                    StoreOpeningProcedureStep.OrderCheckout,
                    "Buy a checkout counter",
                    "Open Shop Catalog and order one checkout counter.");
            }

            if (!HasReceivedFurniture(
                    state,
                    StoreFixtureKind
                        .CheckoutCounter))
            {
                return Status(
                    StoreOpeningProcedureStep.ReceiveCheckout,
                    "Receive checkout furniture",
                    "Open Deliveries and receive the checkout order.");
            }

            if (!HasPlacedFurniture(
                    state,
                    StoreFixtureKind
                        .CheckoutCounter))
            {
                return Status(
                    StoreOpeningProcedureStep.PlaceCheckout,
                    "Place the checkout counter",
                    "Select the received counter, position it on the grid and confirm.");
            }

            if (!HasAnyDisplayOrder(state))
            {
                return Status(
                    StoreOpeningProcedureStep.OrderDisplay,
                    "Buy a product display",
                    "Order a central shelf, wall shelf, low display or featured display.");
            }

            if (!HasAnyReceivedDisplay(state))
            {
                return Status(
                    StoreOpeningProcedureStep.ReceiveDisplay,
                    "Receive display furniture",
                    "Receive the display order in the backroom.");
            }

            if (!HasAnyPlacedDisplay(state))
            {
                return Status(
                    StoreOpeningProcedureStep.PlaceDisplay,
                    "Place a display",
                    "Place the received display without blocking access routes.");
            }

            if (!HasProductOrder(state))
            {
                return Status(
                    StoreOpeningProcedureStep.OrderProduct,
                    "Order merchandise",
                    "Choose a fictitious product and place a supplier order.");
            }

            if (!HasProductStock(state))
            {
                return Status(
                    StoreOpeningProcedureStep.ReceiveProduct,
                    "Receive merchandise",
                    "Receive and pay the product delivery.");
            }

            if (!HasAssignedDisplay(state))
            {
                return Status(
                    StoreOpeningProcedureStep.AssignProduct,
                    "Assign a product",
                    "Select a placed display and assign the received product.");
            }

            if (!HasStockedDisplay(state))
            {
                return Status(
                    StoreOpeningProcedureStep.RestockDisplay,
                    "Restock the display",
                    "Transfer units from the backroom to the assigned display.");
            }

            if (string.Equals(
                    snapshot.DayCycle.State,
                    "BeforeOpen",
                    StringComparison.Ordinal))
            {
                return Status(
                    StoreOpeningProcedureStep.OpenStore,
                    "Open the store",
                    "Use Day Cycle to open the store.");
            }

            if (state.CompletedSales == 0)
            {
                return Status(
                    StoreOpeningProcedureStep.ServeCustomer,
                    "Serve a customer",
                    "Spawn the next customer and complete one purchase.");
            }

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return Status(
                    StoreOpeningProcedureStep.CloseDay,
                    "Close the day",
                    "Begin Closing, clear operations and complete the close.");
            }

            if (!autosaveCompleted)
            {
                return Status(
                    StoreOpeningProcedureStep.Autosave,
                    "Wait for autosave",
                    "Confirm that the active slot reports Saved.");
            }

            return Status(
                StoreOpeningProcedureStep.Completed,
                "Vertical slice loop complete",
                "The functional Phase 1 gate is complete. Continue to the next day or repeat the loop.");
        }

        private bool HasFurnitureOrder(
            StoreOperationsState state,
            StoreFixtureKind kind)
        {
            foreach (StoreOrderRecord order
                     in state.Orders)
            {
                if (order.IsFurniture &&
                    _catalog.TryGetFurniture(
                        order.ItemId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    return true;
                }
            }

            return false;
        }


        private bool HasAnyDisplayOrder(
            StoreOperationsState state)
        {
            foreach (StoreOrderRecord order
                     in state.Orders)
            {
                if (order.IsFurniture &&
                    _catalog.TryGetFurniture(
                        order.ItemId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasReceivedFurniture(
            StoreOperationsState state,
            StoreFixtureKind kind)
        {
            foreach (StoreStockRecord stock
                     in state.FurnitureWarehouse)
            {
                if (stock.Quantity > 0 &&
                    _catalog.TryGetFurniture(
                        stock.ItemId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.Kind == kind)
                {
                    return true;
                }
            }

            return HasPlacedFurniture(state, kind);
        }

        private bool HasPlacedFurniture(
            StoreOperationsState state,
            StoreFixtureKind kind)
        {
            foreach (PlacedStoreFixtureRecord fixture
                     in state.Fixtures)
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

        private bool HasAnyReceivedDisplay(
            StoreOperationsState state)
        {
            foreach (StoreStockRecord stock
                     in state.FurnitureWarehouse)
            {
                if (stock.Quantity > 0 &&
                    _catalog.TryGetFurniture(
                        stock.ItemId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return HasAnyPlacedDisplay(state);
        }

        private bool HasAnyPlacedDisplay(
            StoreOperationsState state)
        {
            foreach (PlacedStoreFixtureRecord fixture
                     in state.Fixtures)
            {
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            definition) &&
                    definition.SupportsProducts)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasProductOrder(
            StoreOperationsState state)
        {
            foreach (StoreOrderRecord order
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
            StoreOperationsState state)
        {
            foreach (StoreStockRecord stock
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
            StoreOperationsState state)
        {
            foreach (PlacedStoreFixtureRecord fixture
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
            StoreOperationsState state)
        {
            foreach (PlacedStoreFixtureRecord fixture
                     in state.Fixtures)
            {
                if (fixture.ProductQuantity > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static StoreOpeningProcedureStatus Status(
            StoreOpeningProcedureStep step,
            string title,
            string instruction)
        {
            return new StoreOpeningProcedureStatus(
                step,
                title,
                instruction);
        }
    }

    public sealed class StoreOpeningProcedureStatus
    {
        public StoreOpeningProcedureStep Step { get; }
        public string Title { get; }
        public string Instruction { get; }

        public StoreOpeningProcedureStatus(
            StoreOpeningProcedureStep step,
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

    public enum StoreOpeningProcedureStep
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
}
