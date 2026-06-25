using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public sealed class DisplayReturnStockResult
    {
        public bool Succeeded { get; }

        public DisplayReturnStockFailureReason FailureReason { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity ReturnedQuantity { get; }

        public Quantity DisplayBefore { get; }

        public Quantity DisplayAfter { get; }

        public Quantity DestinationBefore { get; }

        public Quantity DestinationAfter { get; }

        public bool AssignmentCleared { get; }

        private DisplayReturnStockResult(
            bool succeeded,
            DisplayReturnStockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity returnedQuantity,
            Quantity displayBefore,
            Quantity displayAfter,
            Quantity destinationBefore,
            Quantity destinationAfter,
            bool assignmentCleared)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            ProductId = productId;
            ReturnedQuantity = returnedQuantity;
            DisplayBefore = displayBefore;
            DisplayAfter = displayAfter;
            DestinationBefore = destinationBefore;
            DestinationAfter = destinationAfter;
            AssignmentCleared = assignmentCleared;
        }

        public static DisplayReturnStockResult Success(
            ProductDefinitionId productId,
            Quantity returnedQuantity,
            Quantity displayBefore,
            Quantity displayAfter,
            Quantity destinationBefore,
            Quantity destinationAfter,
            bool assignmentCleared)
        {
            return new DisplayReturnStockResult(
                true,
                DisplayReturnStockFailureReason.None,
                productId,
                returnedQuantity,
                displayBefore,
                displayAfter,
                destinationBefore,
                destinationAfter,
                assignmentCleared);
        }

        public static DisplayReturnStockResult Failure(
            DisplayReturnStockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity displayQuantity,
            Quantity destinationQuantity)
        {
            return new DisplayReturnStockResult(
                false,
                failureReason,
                productId,
                Quantity.Zero,
                displayQuantity,
                displayQuantity,
                destinationQuantity,
                destinationQuantity,
                false);
        }
    }
}
