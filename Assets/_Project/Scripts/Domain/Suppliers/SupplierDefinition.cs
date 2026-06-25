using System;

namespace VRMGames.CartridgeAndCloud.Domain.Suppliers
{
    public sealed class SupplierDefinition
    {
        public SupplierId Id { get; }

        public string DisplayNameKey { get; }

        public SupplierDefinition(
            SupplierId id,
            string displayNameKey)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Supplier ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayNameKey))
            {
                throw new ArgumentException(
                    "Supplier display-name key cannot be empty.",
                    nameof(displayNameKey));
            }

            Id = id;
            DisplayNameKey = displayNameKey;
        }
    }
}
