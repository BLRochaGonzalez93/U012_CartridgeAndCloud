using System;

namespace VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1
{
    public enum Phase1FurnitureKind
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

    public enum Phase1ProductKind
    {
        PhysicalGame = 0,
        GameCase = 1,
        Console = 2,
        Controller = 3,
        Headset = 4,
        Accessory = 5
    }

    public enum Phase1CharacterRole
    {
        Employee = 0,
        Customer = 1,
        Supplier = 2,
        Player = 3
    }

    public enum Phase1OrderState
    {
        Ordered = 0,
        Received = 1,
        Completed = 2
    }

    public enum Phase1FeedbackKind
    {
        PlacementValid = 0,
        PlacementInvalid = 1,
        ObjectSelected = 2,
        ObjectHovered = 3,
        ProductAssigned = 4,
        OutOfStock = 5,
        Reserved = 6,
        Restocked = 7,
        OrderReceived = 8,
        CustomerSatisfied = 9,
        CustomerFrustrated = 10,
        QueueEntered = 11,
        CheckoutCompleted = 12,
        Revenue = 13,
        Expense = 14,
        ClosingWarning = 15,
        DayClosed = 16,
        AutosaveSucceeded = 17,
        AutosaveFailed = 18,
        DoorOpened = 19,
        DoorClosed = 20
    }

    public enum Phase1AudioChannel
    {
        Music = 0,
        Ambience = 1,
        Ui = 2,
        Effects = 3
    }

    public sealed class Phase1FurnitureDefinition
    {
        public string DefinitionId { get; }
        public string DisplayName { get; }
        public Phase1FurnitureKind Kind { get; }
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

        public Phase1FurnitureDefinition(
            string definitionId,
            string displayName,
            Phase1FurnitureKind kind,
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
                    typeof(Phase1FurnitureKind),
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

    public sealed class Phase1ProductDefinition
    {
        public string ProductId { get; }
        public string DisplayName { get; }
        public Phase1ProductKind Kind { get; }
        public long WholesalePriceCents { get; }
        public long SalePriceCents { get; }
        public int UnitsPerCase { get; }
        public string MaterialVariantId { get; }
        public string LabelId { get; }
        public string IconResourcePath { get; }
        public string CoverResourcePath { get; }
        public string PrefabResourcePath { get; }

        public Phase1ProductDefinition(
            string productId,
            string displayName,
            Phase1ProductKind kind,
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
                    typeof(Phase1ProductKind),
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
}
