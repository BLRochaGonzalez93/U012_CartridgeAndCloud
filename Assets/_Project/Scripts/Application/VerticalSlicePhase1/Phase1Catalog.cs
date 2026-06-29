using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    public sealed class Phase1Catalog :
        IPhase1Catalog
    {
        private readonly ReadOnlyCollection<
            Phase1FurnitureDefinition> _furniture;
        private readonly ReadOnlyCollection<
            Phase1ProductDefinition> _products;

        private readonly Dictionary<
            string,
            Phase1FurnitureDefinition>
                _furnitureById;

        private readonly Dictionary<
            string,
            Phase1ProductDefinition>
                _productsById;

        public IReadOnlyList<
            Phase1FurnitureDefinition> Furniture =>
                _furniture;

        public IReadOnlyList<
            Phase1ProductDefinition> Products =>
                _products;

        public Phase1Catalog(
            IEnumerable<Phase1FurnitureDefinition>
                furniture,
            IEnumerable<Phase1ProductDefinition>
                products)
        {
            if (furniture == null)
            {
                throw new ArgumentNullException(
                    nameof(furniture));
            }

            if (products == null)
            {
                throw new ArgumentNullException(
                    nameof(products));
            }

            List<Phase1FurnitureDefinition>
                furnitureCopy =
                    new List<
                        Phase1FurnitureDefinition>();

            List<Phase1ProductDefinition>
                productCopy =
                    new List<
                        Phase1ProductDefinition>();

            _furnitureById =
                new Dictionary<
                    string,
                    Phase1FurnitureDefinition>(
                        StringComparer.Ordinal);

            _productsById =
                new Dictionary<
                    string,
                    Phase1ProductDefinition>(
                        StringComparer.Ordinal);

            foreach (Phase1FurnitureDefinition
                     item in furniture)
            {
                if (item == null)
                {
                    throw new ArgumentException(
                        "Furniture cannot contain null.",
                        nameof(furniture));
                }

                if (_furnitureById.ContainsKey(
                        item.DefinitionId))
                {
                    throw new ArgumentException(
                        $"Duplicate furniture definition " +
                        $"{item.DefinitionId}.",
                        nameof(furniture));
                }

                _furnitureById.Add(
                    item.DefinitionId,
                    item);
                furnitureCopy.Add(item);
            }

            foreach (Phase1ProductDefinition
                     item in products)
            {
                if (item == null)
                {
                    throw new ArgumentException(
                        "Products cannot contain null.",
                        nameof(products));
                }

                if (_productsById.ContainsKey(
                        item.ProductId))
                {
                    throw new ArgumentException(
                        $"Duplicate product definition " +
                        $"{item.ProductId}.",
                        nameof(products));
                }

                _productsById.Add(
                    item.ProductId,
                    item);
                productCopy.Add(item);
            }

            _furniture =
                new ReadOnlyCollection<
                    Phase1FurnitureDefinition>(
                        furnitureCopy);

            _products =
                new ReadOnlyCollection<
                    Phase1ProductDefinition>(
                        productCopy);
        }

        public bool TryGetFurniture(
            string definitionId,
            out Phase1FurnitureDefinition definition)
        {
            return _furnitureById.TryGetValue(
                definitionId ?? string.Empty,
                out definition);
        }

        public bool TryGetProduct(
            string productId,
            out Phase1ProductDefinition definition)
        {
            return _productsById.TryGetValue(
                productId ?? string.Empty,
                out definition);
        }
    }
}
