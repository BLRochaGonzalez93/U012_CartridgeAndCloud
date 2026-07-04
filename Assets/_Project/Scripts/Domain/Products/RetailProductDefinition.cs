using System;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public sealed class RetailProductDefinition
    {
        public string ProductId { get; }
        public string DisplayName { get; }
        public RetailProductKind Kind { get; }
        public long WholesalePriceCents { get; }
        public long SalePriceCents { get; }
        public int UnitsPerCase { get; }
        public string MaterialVariantId { get; }
        public string LabelId { get; }
        public string IconResourcePath { get; }
        public string CoverResourcePath { get; }
        public string PrefabResourcePath { get; }

        public RetailProductDefinition(
            string productId,
            string displayName,
            RetailProductKind kind,
            long wholesalePriceCents,
            long salePriceCents,
            int unitsPerCase,
            string materialVariantId,
            string labelId,
            string iconResourcePath,
            string coverResourcePath,
            string prefabResourcePath)
        {
            ProductId = Required(
                productId,
                nameof(productId));
            DisplayName = Required(
                displayName,
                nameof(displayName));

            if (!Enum.IsDefined(
                    typeof(RetailProductKind),
                    kind))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(kind));
            }

            if (wholesalePriceCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(wholesalePriceCents));
            }

            if (salePriceCents <= wholesalePriceCents)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(salePriceCents),
                    "Sale price must exceed wholesale price.");
            }

            if (unitsPerCase < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitsPerCase));
            }

            Kind = kind;
            WholesalePriceCents =
                wholesalePriceCents;
            SalePriceCents = salePriceCents;
            UnitsPerCase = unitsPerCase;
            MaterialVariantId =
                materialVariantId ?? string.Empty;
            LabelId = labelId ?? string.Empty;
            IconResourcePath =
                iconResourcePath ?? string.Empty;
            CoverResourcePath =
                coverResourcePath ?? string.Empty;
            PrefabResourcePath =
                prefabResourcePath ?? string.Empty;
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

    public enum RetailProductKind
    {
        PhysicalGame = 0,
        GameCase = 1,
        Console = 2,
        Controller = 3,
        Headset = 4,
        Accessory = 5
    }
}
