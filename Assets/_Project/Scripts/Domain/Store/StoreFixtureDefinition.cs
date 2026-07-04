using System;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreFixtureDefinition
    {
        public string DefinitionId { get; }
        public string DisplayName { get; }
        public StoreFixtureKind Kind { get; }
        public int WidthCells { get; }
        public int DepthCells { get; }
        public float HeightMeters { get; }
        public int Capacity { get; }
        public long UnitCostCents { get; }
        public bool IsInteractive { get; }
        public bool IsPurchasable { get; }
        public bool SupportsProducts { get; }
        public string MaterialVariantId { get; }
        public string PrefabResourcePath { get; }

        public StoreFixtureDefinition(
            string definitionId,
            string displayName,
            StoreFixtureKind kind,
            int widthCells,
            int depthCells,
            float heightMeters,
            int capacity,
            long unitCostCents,
            bool isInteractive,
            bool isPurchasable,
            bool supportsProducts,
            string materialVariantId,
            string prefabResourcePath)
        {
            DefinitionId = Required(
                definitionId,
                nameof(definitionId));
            DisplayName = Required(
                displayName,
                nameof(displayName));

            if (!Enum.IsDefined(
                    typeof(StoreFixtureKind),
                    kind))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(kind));
            }

            if (widthCells < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(widthCells));
            }

            if (depthCells < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depthCells));
            }

            if (heightMeters <= 0f ||
                float.IsNaN(heightMeters) ||
                float.IsInfinity(heightMeters))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(heightMeters));
            }

            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            Kind = kind;
            WidthCells = widthCells;
            DepthCells = depthCells;
            HeightMeters = heightMeters;
            Capacity = capacity;
            UnitCostCents = unitCostCents;
            IsInteractive = isInteractive;
            IsPurchasable = isPurchasable;
            SupportsProducts = supportsProducts;
            MaterialVariantId =
                materialVariantId ?? string.Empty;
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

    public enum StoreFixtureKind
    {
        CheckoutCounter = 0,
        WallShelf = 1,
        CentralShelf = 2,
        LowDisplay = 3,
        FeaturedDisplay = 4,
        BackroomStorage = 5,
        ReceivingCrate = 6,
        Decoration = 7
    }
}
