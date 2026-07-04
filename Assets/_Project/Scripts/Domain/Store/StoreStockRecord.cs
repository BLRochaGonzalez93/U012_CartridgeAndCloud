using System;

using VRMGames.CartridgeAndCloud.Domain.Inventory;
namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreStockRecord
    {
        public string ItemId { get; }
        public int Quantity { get; }

        public StoreStockRecord(
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
}
