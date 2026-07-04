using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public sealed class StoreContentCatalog :
        IStoreContentCatalog
    {
        private readonly ReadOnlyCollection<
            StoreFixtureDefinition> _furniture;
        private readonly ReadOnlyCollection<
            RetailProductDefinition> _products;

        private readonly Dictionary<
            string,
            StoreFixtureDefinition>
                _furnitureById;

        private readonly Dictionary<
            string,
            RetailProductDefinition>
                _productsById;

        public IReadOnlyList<
            StoreFixtureDefinition> Furniture =>
                _furniture;

        public IReadOnlyList<
            RetailProductDefinition> Products =>
                _products;

        public StoreContentCatalog(
            IEnumerable<StoreFixtureDefinition>
                furniture,
            IEnumerable<RetailProductDefinition>
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

            List<StoreFixtureDefinition>
                furnitureCopy =
                    new List<
                        StoreFixtureDefinition>();

            List<RetailProductDefinition>
                productCopy =
                    new List<
                        RetailProductDefinition>();

            _furnitureById =
                new Dictionary<
                    string,
                    StoreFixtureDefinition>(
                        StringComparer.Ordinal);

            _productsById =
                new Dictionary<
                    string,
                    RetailProductDefinition>(
                        StringComparer.Ordinal);

            foreach (StoreFixtureDefinition
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

            foreach (RetailProductDefinition
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
                    StoreFixtureDefinition>(
                        furnitureCopy);

            _products =
                new ReadOnlyCollection<
                    RetailProductDefinition>(
                        productCopy);
        }

        public bool TryGetFurniture(
            string definitionId,
            out StoreFixtureDefinition definition)
        {
            return _furnitureById.TryGetValue(
                definitionId ?? string.Empty,
                out definition);
        }

        public bool TryGetProduct(
            string productId,
            out RetailProductDefinition definition)
        {
            return _productsById.TryGetValue(
                productId ?? string.Empty,
                out definition);
        }
    }
}
