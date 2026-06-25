using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public sealed class DisplayRestockResult
    {
        public bool Succeeded { get; }

        public DisplayRestockFailureReason FailureReason { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity RequestedQuantity { get; }

        public Quantity TransferredQuantity { get; }

        public Quantity SourceBefore { get; }

        public Quantity SourceAfter { get; }

        public Quantity DisplayBefore { get; }

        public Quantity DisplayAfter { get; }

        public int VisibleUnitsBefore { get; }

        public int VisibleUnitsAfter { get; }

        private DisplayRestockResult(
            bool succeeded,
            DisplayRestockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity transferredQuantity,
            Quantity sourceBefore,
            Quantity sourceAfter,
            Quantity displayBefore,
            Quantity displayAfter,
            int visibleUnitsBefore,
            int visibleUnitsAfter)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            TransferredQuantity = transferredQuantity;
            SourceBefore = sourceBefore;
            SourceAfter = sourceAfter;
            DisplayBefore = displayBefore;
            DisplayAfter = displayAfter;
            VisibleUnitsBefore = visibleUnitsBefore;
            VisibleUnitsAfter = visibleUnitsAfter;
        }

        public static DisplayRestockResult Success(
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity transferredQuantity,
            Quantity sourceBefore,
            Quantity sourceAfter,
            Quantity displayBefore,
            Quantity displayAfter,
            int visibleUnitsBefore,
            int visibleUnitsAfter)
        {
            return new DisplayRestockResult(
                true,
                DisplayRestockFailureReason.None,
                productId,
                requestedQuantity,
                transferredQuantity,
                sourceBefore,
                sourceAfter,
                displayBefore,
                displayAfter,
                visibleUnitsBefore,
                visibleUnitsAfter);
        }

        public static DisplayRestockResult Failure(
            DisplayRestockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity sourceQuantity,
            Quantity displayQuantity,
            int visibleUnits)
        {
            return new DisplayRestockResult(
                false,
                failureReason,
                productId,
                requestedQuantity,
                Quantity.Zero,
                sourceQuantity,
                sourceQuantity,
                displayQuantity,
                displayQuantity,
                visibleUnits,
                visibleUnits);
        }
    }
}
