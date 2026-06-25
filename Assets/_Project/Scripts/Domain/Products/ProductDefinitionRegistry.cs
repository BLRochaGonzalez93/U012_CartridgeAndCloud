using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public sealed class ProductDefinitionRegistry
    {
        private readonly Dictionary<ProductDefinitionId, ProductDefinition>
            _definitionsById;

        private readonly ReadOnlyCollection<ProductDefinition> _definitions;

        public IReadOnlyList<ProductDefinition> Definitions =>
            _definitions;

        public int Count => _definitions.Count;

        public ProductDefinitionRegistry(
            IEnumerable<ProductDefinition> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            _definitionsById =
                new Dictionary<ProductDefinitionId, ProductDefinition>();

            List<ProductDefinition> definitionList =
                new List<ProductDefinition>();

            foreach (ProductDefinition definition in definitions)
            {
                if (definition == null)
                {
                    throw new ArgumentException(
                        "Product definitions cannot contain null.",
                        nameof(definitions));
                }

                if (_definitionsById.ContainsKey(definition.Id))
                {
                    throw new ArgumentException(
                        $"Product definition ID {definition.Id} is duplicated.",
                        nameof(definitions));
                }

                _definitionsById.Add(
                    definition.Id,
                    definition);

                definitionList.Add(definition);
            }

            definitionList.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));

            _definitions =
                new ReadOnlyCollection<ProductDefinition>(definitionList);
        }

        public bool Contains(ProductDefinitionId id)
        {
            return _definitionsById.ContainsKey(id);
        }

        public bool TryGet(
            ProductDefinitionId id,
            out ProductDefinition definition)
        {
            return _definitionsById.TryGetValue(
                id,
                out definition);
        }

        public ProductDefinition Get(ProductDefinitionId id)
        {
            if (!_definitionsById.TryGetValue(
                    id,
                    out ProductDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Product definition {id} was not found.");
            }

            return definition;
        }
    }
}
