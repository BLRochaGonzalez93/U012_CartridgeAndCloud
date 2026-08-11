using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Products;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;

namespace VRMGames.CartridgeAndCloud.Editor.Content
{
    internal static class ContentCatalogAuthoringService
    {
        internal const string RuntimeRegistryPath =
            "Assets/_Project/Resources/RuntimeAssetRegistry.asset";

        internal sealed class CatalogContext
        {
            public StoreRuntimeAssetRegistry Registry { get; }
            public StoreContentCatalogAsset ContentCatalog { get; }
            public RetailProductCatalogAsset ProductCatalog { get; }
            public StoreFixtureCatalogAsset FixtureCatalog { get; }
            public StoreVisualPrefabCatalogAsset VisualCatalog { get; }

            public CatalogContext(
                StoreRuntimeAssetRegistry registry,
                StoreContentCatalogAsset contentCatalog,
                RetailProductCatalogAsset productCatalog,
                StoreFixtureCatalogAsset fixtureCatalog,
                StoreVisualPrefabCatalogAsset visualCatalog)
            {
                Registry = registry;
                ContentCatalog = contentCatalog;
                ProductCatalog = productCatalog;
                FixtureCatalog = fixtureCatalog;
                VisualCatalog = visualCatalog;
            }
        }

        internal sealed class ProductDraft
        {
            public string ProductId = string.Empty;
            public string DisplayName = string.Empty;
            public RetailProductKind Kind = RetailProductKind.PhysicalGame;
            public long WholesalePriceCents = 100;
            public long SalePriceCents = 200;
            public int UnitsPerCase = 1;
            public string MaterialVariantId = string.Empty;
            public string LabelId = string.Empty;
            public string IconResourcePath = string.Empty;
            public string CoverResourcePath = string.Empty;
            public string PrefabResourcePath = string.Empty;
            public GameObject Prefab;
        }

        internal sealed class FixtureDraft
        {
            public string DefinitionId = string.Empty;
            public string DisplayName = string.Empty;
            public StoreFixtureKind Kind = StoreFixtureKind.CentralShelf;
            public int WidthCells = 1;
            public int DepthCells = 1;
            public float HeightMeters = 1f;
            public int Capacity;
            public long UnitCostCents = 100;
            public bool IsInteractive = true;
            public bool IsPurchasable = true;
            public bool SupportsProducts;
            public string MaterialVariantId = string.Empty;
            public string PrefabResourcePath = string.Empty;
            public GameObject Prefab;
        }

        internal static bool TryLoadContext(
            out CatalogContext context,
            out string message)
        {
            context = null;

            StoreRuntimeAssetRegistry registry =
                AssetDatabase.LoadAssetAtPath<StoreRuntimeAssetRegistry>(
                    RuntimeRegistryPath);

            if (registry == null)
            {
                message =
                    $"Runtime asset registry was not found at '{RuntimeRegistryPath}'.";
                return false;
            }

            StoreContentCatalogAsset contentCatalog =
                registry.ContentCatalog;

            if (contentCatalog == null)
            {
                message =
                    "Runtime asset registry has no content catalog assigned.";
                return false;
            }

            RetailProductCatalogAsset productCatalog =
                contentCatalog.ProductCatalog;
            StoreFixtureCatalogAsset fixtureCatalog =
                contentCatalog.FixtureCatalog;
            StoreVisualPrefabCatalogAsset visualCatalog =
                registry.VisualPrefabs;

            if (productCatalog == null)
            {
                message =
                    "Store content catalog has no retail product catalog assigned.";
                return false;
            }

            if (fixtureCatalog == null)
            {
                message =
                    "Store content catalog has no fixture catalog assigned.";
                return false;
            }

            if (visualCatalog == null)
            {
                message =
                    "Runtime asset registry has no visual prefab catalog assigned.";
                return false;
            }

            try
            {
                contentCatalog.BuildCatalog();
            }
            catch (Exception exception)
            {
                message =
                    "Current content catalogs are not buildable: " +
                    exception.Message;
                return false;
            }

            context = new CatalogContext(
                registry,
                contentCatalog,
                productCatalog,
                fixtureCatalog,
                visualCatalog);

            message =
                $"Ready: {productCatalog.Entries.Count} products, " +
                $"{fixtureCatalog.Entries.Count} fixtures.";
            return true;
        }

        internal static bool TryAddProduct(
            CatalogContext context,
            ProductDraft draft,
            out string message)
        {
            if (!TryPreflightProduct(
                    context,
                    draft,
                    out RetailProductCatalogAsset.RetailProductEntry entry,
                    out StoreVisualPrefabCatalogAsset.Entry visualEntry,
                    out message))
            {
                return false;
            }

            RetailProductCatalogAsset.RetailProductEntry[] productEntries =
                Append(context.ProductCatalog.Entries, entry);
            StoreVisualPrefabCatalogAsset.Entry[] visualEntries =
                Append(context.VisualCatalog.Products, visualEntry);

            int undoGroup = BeginUndo(
                "Add Cartridge & Cloud product",
                context.ProductCatalog,
                context.VisualCatalog);

            try
            {
                context.ProductCatalog.Configure(productEntries);
                context.VisualCatalog.Configure(
                    Copy(context.VisualCatalog.Architecture),
                    Copy(context.VisualCatalog.Furniture),
                    visualEntries,
                    Copy(context.VisualCatalog.Expansions));

                EditorUtility.SetDirty(context.ProductCatalog);
                EditorUtility.SetDirty(context.VisualCatalog);
                AssetDatabase.SaveAssets();
                Undo.CollapseUndoOperations(undoGroup);

                message =
                    $"Product '{entry.productId}' added to definition and visual catalogs.";
                return true;
            }
            catch (Exception exception)
            {
                Undo.RevertAllDownToGroup(undoGroup);
                AssetDatabase.SaveAssets();
                message =
                    "Product authoring was reverted: " + exception.Message;
                return false;
            }
        }

        internal static bool TryAddFixture(
            CatalogContext context,
            FixtureDraft draft,
            out string message)
        {
            if (!TryPreflightFixture(
                    context,
                    draft,
                    out StoreFixtureCatalogAsset.StoreFixtureEntry entry,
                    out StoreVisualPrefabCatalogAsset.Entry visualEntry,
                    out message))
            {
                return false;
            }

            StoreFixtureCatalogAsset.StoreFixtureEntry[] fixtureEntries =
                Append(context.FixtureCatalog.Entries, entry);
            StoreVisualPrefabCatalogAsset.Entry[] visualEntries =
                Append(context.VisualCatalog.Furniture, visualEntry);

            int undoGroup = BeginUndo(
                "Add Cartridge & Cloud fixture",
                context.FixtureCatalog,
                context.VisualCatalog);

            try
            {
                context.FixtureCatalog.Configure(fixtureEntries);
                context.VisualCatalog.Configure(
                    Copy(context.VisualCatalog.Architecture),
                    visualEntries,
                    Copy(context.VisualCatalog.Products),
                    Copy(context.VisualCatalog.Expansions));

                EditorUtility.SetDirty(context.FixtureCatalog);
                EditorUtility.SetDirty(context.VisualCatalog);
                AssetDatabase.SaveAssets();
                Undo.CollapseUndoOperations(undoGroup);

                message =
                    $"Fixture '{entry.definitionId}' added to definition and visual catalogs.";
                return true;
            }
            catch (Exception exception)
            {
                Undo.RevertAllDownToGroup(undoGroup);
                AssetDatabase.SaveAssets();
                message =
                    "Fixture authoring was reverted: " + exception.Message;
                return false;
            }
        }

        private static bool TryPreflightProduct(
            CatalogContext context,
            ProductDraft draft,
            out RetailProductCatalogAsset.RetailProductEntry entry,
            out StoreVisualPrefabCatalogAsset.Entry visualEntry,
            out string message)
        {
            entry = null;
            visualEntry = null;

            if (context == null || draft == null)
            {
                message = "Product authoring context is not available.";
                return false;
            }

            string id = NormalizeId(draft.ProductId);
            string displayName = NormalizeText(draft.DisplayName);

            if (string.IsNullOrEmpty(id))
            {
                message = "Product ID is required.";
                return false;
            }

            if (string.IsNullOrEmpty(displayName))
            {
                message = "Product display name is required.";
                return false;
            }

            if (ContainsProductId(context.ProductCatalog.Entries, id))
            {
                message = $"Product ID '{id}' already exists.";
                return false;
            }

            if (ContainsVisualId(context.VisualCatalog, id))
            {
                message = $"Visual prefab ID '{id}' already exists.";
                return false;
            }

            if (!TryValidatePrefabAsset(draft.Prefab, out message))
            {
                return false;
            }

            if (draft.Prefab.GetComponent<ProductVisualMarker>() == null)
            {
                message =
                    $"Prefab '{draft.Prefab.name}' is missing ProductVisualMarker.";
                return false;
            }

            try
            {
                _ = new RetailProductDefinition(
                    id,
                    displayName,
                    draft.Kind,
                    draft.WholesalePriceCents,
                    draft.SalePriceCents,
                    draft.UnitsPerCase,
                    NormalizeText(draft.MaterialVariantId),
                    NormalizeText(draft.LabelId),
                    NormalizeText(draft.IconResourcePath),
                    NormalizeText(draft.CoverResourcePath),
                    NormalizeText(draft.PrefabResourcePath));
            }
            catch (Exception exception)
            {
                message =
                    "Product definition is invalid: " + exception.Message;
                return false;
            }

            entry = new RetailProductCatalogAsset.RetailProductEntry
            {
                productId = id,
                displayName = displayName,
                kind = draft.Kind,
                wholesalePriceCents = draft.WholesalePriceCents,
                salePriceCents = draft.SalePriceCents,
                unitsPerCase = draft.UnitsPerCase,
                materialVariantId = NormalizeText(draft.MaterialVariantId),
                labelId = NormalizeText(draft.LabelId),
                iconResourcePath = NormalizeText(draft.IconResourcePath),
                coverResourcePath = NormalizeText(draft.CoverResourcePath),
                prefabResourcePath = NormalizeText(draft.PrefabResourcePath)
            };

            visualEntry = new StoreVisualPrefabCatalogAsset.Entry
            {
                id = id,
                prefab = draft.Prefab
            };

            message = "Product preflight passed.";
            return true;
        }

        private static bool TryPreflightFixture(
            CatalogContext context,
            FixtureDraft draft,
            out StoreFixtureCatalogAsset.StoreFixtureEntry entry,
            out StoreVisualPrefabCatalogAsset.Entry visualEntry,
            out string message)
        {
            entry = null;
            visualEntry = null;

            if (context == null || draft == null)
            {
                message = "Fixture authoring context is not available.";
                return false;
            }

            string id = NormalizeId(draft.DefinitionId);
            string displayName = NormalizeText(draft.DisplayName);

            if (string.IsNullOrEmpty(id))
            {
                message = "Fixture definition ID is required.";
                return false;
            }

            if (string.IsNullOrEmpty(displayName))
            {
                message = "Fixture display name is required.";
                return false;
            }

            if (ContainsFixtureId(context.FixtureCatalog.Entries, id))
            {
                message = $"Fixture definition ID '{id}' already exists.";
                return false;
            }

            if (ContainsVisualId(context.VisualCatalog, id))
            {
                message = $"Visual prefab ID '{id}' already exists.";
                return false;
            }

            if (!TryValidatePrefabAsset(draft.Prefab, out message))
            {
                return false;
            }

            StoreFixturePrefabAuthoring authoring =
                draft.Prefab.GetComponent<StoreFixturePrefabAuthoring>();

            if (authoring == null)
            {
                message =
                    $"Prefab '{draft.Prefab.name}' is missing StoreFixturePrefabAuthoring.";
                return false;
            }

            if (!string.Equals(
                    authoring.DefinitionId,
                    id,
                    StringComparison.Ordinal))
            {
                message =
                    $"Prefab '{draft.Prefab.name}' is authored for " +
                    $"'{authoring.DefinitionId}', not '{id}'.";
                return false;
            }

            if (!authoring.TryValidate(out string prefabReport))
            {
                message =
                    $"Prefab '{draft.Prefab.name}' is invalid: {prefabReport}";
                return false;
            }

            if (draft.SupportsProducts && draft.Capacity < 1)
            {
                message =
                    "A fixture that supports products must have positive capacity.";
                return false;
            }

            try
            {
                _ = new StoreFixtureDefinition(
                    id,
                    displayName,
                    draft.Kind,
                    draft.WidthCells,
                    draft.DepthCells,
                    draft.HeightMeters,
                    draft.Capacity,
                    draft.UnitCostCents,
                    draft.IsInteractive,
                    draft.IsPurchasable,
                    draft.SupportsProducts,
                    NormalizeText(draft.MaterialVariantId),
                    NormalizeText(draft.PrefabResourcePath));
            }
            catch (Exception exception)
            {
                message =
                    "Fixture definition is invalid: " + exception.Message;
                return false;
            }

            entry = new StoreFixtureCatalogAsset.StoreFixtureEntry
            {
                definitionId = id,
                displayName = displayName,
                kind = draft.Kind,
                widthCells = draft.WidthCells,
                depthCells = draft.DepthCells,
                heightMeters = draft.HeightMeters,
                capacity = draft.Capacity,
                unitCostCents = draft.UnitCostCents,
                isInteractive = draft.IsInteractive,
                isPurchasable = draft.IsPurchasable,
                supportsProducts = draft.SupportsProducts,
                materialVariantId = NormalizeText(draft.MaterialVariantId),
                prefabResourcePath = NormalizeText(draft.PrefabResourcePath)
            };

            visualEntry = new StoreVisualPrefabCatalogAsset.Entry
            {
                id = id,
                prefab = draft.Prefab
            };

            message = "Fixture preflight passed.";
            return true;
        }

        private static bool TryValidatePrefabAsset(
            GameObject prefab,
            out string message)
        {
            if (prefab == null)
            {
                message = "A prefab asset is required.";
                return false;
            }

            if (!PrefabUtility.IsPartOfPrefabAsset(prefab))
            {
                message =
                    $"'{prefab.name}' is not a prefab asset from the Project.";
                return false;
            }

            string path = AssetDatabase.GetAssetPath(prefab);
            if (string.IsNullOrWhiteSpace(path) ||
                !path.StartsWith("Assets/", StringComparison.Ordinal))
            {
                message =
                    $"Prefab '{prefab.name}' is not stored inside Assets.";
                return false;
            }

            message = "Prefab asset is valid.";
            return true;
        }

        private static int BeginUndo(
            string label,
            params UnityEngine.Object[] targets)
        {
            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName(label);
            Undo.RecordObjects(targets, label);
            return group;
        }

        private static bool ContainsProductId(
            IReadOnlyList<RetailProductCatalogAsset.RetailProductEntry> entries,
            string id)
        {
            if (entries == null)
            {
                return false;
            }

            for (int index = 0; index < entries.Count; index++)
            {
                RetailProductCatalogAsset.RetailProductEntry entry = entries[index];
                if (entry != null &&
                    string.Equals(entry.productId, id, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsFixtureId(
            IReadOnlyList<StoreFixtureCatalogAsset.StoreFixtureEntry> entries,
            string id)
        {
            if (entries == null)
            {
                return false;
            }

            for (int index = 0; index < entries.Count; index++)
            {
                StoreFixtureCatalogAsset.StoreFixtureEntry entry = entries[index];
                if (entry != null &&
                    string.Equals(entry.definitionId, id, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsVisualId(
            StoreVisualPrefabCatalogAsset catalog,
            string id)
        {
            return ContainsVisualId(catalog.Architecture, id) ||
                   ContainsVisualId(catalog.Furniture, id) ||
                   ContainsVisualId(catalog.Products, id) ||
                   ContainsVisualId(catalog.Expansions, id);
        }

        private static bool ContainsVisualId(
            StoreVisualPrefabCatalogAsset.Entry[] entries,
            string id)
        {
            if (entries == null)
            {
                return false;
            }

            foreach (StoreVisualPrefabCatalogAsset.Entry entry in entries)
            {
                if (entry != null &&
                    string.Equals(entry.id, id, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static T[] Append<T>(
            IReadOnlyList<T> source,
            T value)
        {
            int count = source?.Count ?? 0;
            T[] result = new T[count + 1];

            for (int index = 0; index < count; index++)
            {
                result[index] = source[index];
            }

            result[count] = value;
            return result;
        }

        private static T[] Copy<T>(T[] source)
        {
            if (source == null || source.Length == 0)
            {
                return Array.Empty<T>();
            }

            T[] copy = new T[source.Length];
            Array.Copy(source, copy, source.Length);
            return copy;
        }

        private static string NormalizeId(string value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static string NormalizeText(string value)
        {
            return (value ?? string.Empty).Trim();
        }
    }
}
