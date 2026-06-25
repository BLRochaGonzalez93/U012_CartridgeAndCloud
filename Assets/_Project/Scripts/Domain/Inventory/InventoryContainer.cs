using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public sealed class InventoryContainer
    {
        private readonly Dictionary<ProductDefinitionId, Quantity> _quantities;

        public InventoryContainerId Id { get; }

        public InventoryContainerType Type { get; }

        public InventoryCapacity Capacity { get; }

        public int UsedCapacity { get; private set; }

        public int AvailableCapacity => Capacity.Units - UsedCapacity;

        public int StackCount => _quantities.Count;

        public IReadOnlyList<InventoryStack> Stacks =>
            CreateStackSnapshot();

        public InventoryContainer(
            InventoryContainerId id,
            InventoryContainerType type,
            InventoryCapacity capacity)
            : this(
                id,
                type,
                capacity,
                Array.Empty<InventoryStack>())
        {
        }

        public InventoryContainer(
            InventoryContainerId id,
            InventoryContainerType type,
            InventoryCapacity capacity,
            IEnumerable<InventoryStack> initialStacks)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Inventory container ID must be initialized.",
                    nameof(id));
            }

            if (type == InventoryContainerType.Unspecified ||
                !Enum.IsDefined(typeof(InventoryContainerType), type))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(type),
                    "Inventory container type must be specified.");
            }

            if (initialStacks == null)
            {
                throw new ArgumentNullException(nameof(initialStacks));
            }

            Id = id;
            Type = type;
            Capacity = capacity;
            _quantities =
                new Dictionary<ProductDefinitionId, Quantity>();

            foreach (InventoryStack stack in initialStacks)
            {
                if (string.IsNullOrWhiteSpace(stack.ProductId.Value) ||
                    stack.Quantity.IsZero)
                {
                    throw new ArgumentException(
                        "Initial stacks must be fully initialized.",
                        nameof(initialStacks));
                }

                if (_quantities.ContainsKey(stack.ProductId))
                {
                    throw new ArgumentException(
                        $"Initial product stack {stack.ProductId} is duplicated.",
                        nameof(initialStacks));
                }

                int nextUsedCapacity =
                    checked(UsedCapacity + stack.Quantity.Value);

                if (nextUsedCapacity > Capacity.Units)
                {
                    throw new ArgumentException(
                        "Initial stacks exceed container capacity.",
                        nameof(initialStacks));
                }

                _quantities.Add(
                    stack.ProductId,
                    stack.Quantity);

                UsedCapacity = nextUsedCapacity;
            }
        }

        public bool ContainsProduct(ProductDefinitionId productId)
        {
            ValidateProductId(productId);
            return _quantities.ContainsKey(productId);
        }

        public Quantity GetQuantity(ProductDefinitionId productId)
        {
            ValidateProductId(productId);

            return _quantities.TryGetValue(
                    productId,
                    out Quantity quantity)
                ? quantity
                : Quantity.Zero;
        }

        public InventoryMutationResult TryAdd(
            ProductDefinitionId productId,
            Quantity quantity)
        {
            ValidateProductId(productId);

            Quantity previousQuantity =
                GetQuantity(productId);

            int previousUsedCapacity = UsedCapacity;

            if (quantity.IsZero)
            {
                return InventoryMutationResult.Failure(
                    InventoryMutationFailureReason.InvalidQuantity,
                    previousQuantity,
                    previousUsedCapacity);
            }

            if (quantity.Value > AvailableCapacity)
            {
                return InventoryMutationResult.Failure(
                    InventoryMutationFailureReason.CapacityExceeded,
                    previousQuantity,
                    previousUsedCapacity);
            }

            AddValidated(productId, quantity);

            return InventoryMutationResult.Success(
                previousQuantity,
                GetQuantity(productId),
                previousUsedCapacity,
                UsedCapacity);
        }

        public InventoryMutationResult TryRemove(
            ProductDefinitionId productId,
            Quantity quantity)
        {
            ValidateProductId(productId);

            Quantity previousQuantity =
                GetQuantity(productId);

            int previousUsedCapacity = UsedCapacity;

            if (quantity.IsZero)
            {
                return InventoryMutationResult.Failure(
                    InventoryMutationFailureReason.InvalidQuantity,
                    previousQuantity,
                    previousUsedCapacity);
            }

            if (quantity > previousQuantity)
            {
                return InventoryMutationResult.Failure(
                    InventoryMutationFailureReason.InsufficientQuantity,
                    previousQuantity,
                    previousUsedCapacity);
            }

            RemoveValidated(productId, quantity);

            return InventoryMutationResult.Success(
                previousQuantity,
                GetQuantity(productId),
                previousUsedCapacity,
                UsedCapacity);
        }

        internal void AddValidated(
            ProductDefinitionId productId,
            Quantity quantity)
        {
            ValidateProductId(productId);

            if (quantity.IsZero ||
                quantity.Value > AvailableCapacity)
            {
                throw new InvalidOperationException(
                    "Validated inventory addition preconditions were not met.");
            }

            Quantity currentQuantity =
                GetQuantity(productId);

            _quantities[productId] =
                currentQuantity.Add(quantity);

            UsedCapacity =
                checked(UsedCapacity + quantity.Value);
        }

        internal void RemoveValidated(
            ProductDefinitionId productId,
            Quantity quantity)
        {
            ValidateProductId(productId);

            Quantity currentQuantity =
                GetQuantity(productId);

            if (quantity.IsZero || quantity > currentQuantity)
            {
                throw new InvalidOperationException(
                    "Validated inventory removal preconditions were not met.");
            }

            Quantity remaining =
                currentQuantity.Subtract(quantity);

            if (remaining.IsZero)
            {
                _quantities.Remove(productId);
            }
            else
            {
                _quantities[productId] = remaining;
            }

            UsedCapacity -= quantity.Value;
        }

        private IReadOnlyList<InventoryStack> CreateStackSnapshot()
        {
            List<InventoryStack> stacks =
                new List<InventoryStack>(_quantities.Count);

            foreach (KeyValuePair<ProductDefinitionId, Quantity> pair in
                     _quantities)
            {
                stacks.Add(
                    new InventoryStack(
                        pair.Key,
                        pair.Value));
            }

            stacks.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.ProductId.Value,
                        right.ProductId.Value));

            return new ReadOnlyCollection<InventoryStack>(stacks);
        }

        private static void ValidateProductId(
            ProductDefinitionId productId)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }
        }
    }
}
