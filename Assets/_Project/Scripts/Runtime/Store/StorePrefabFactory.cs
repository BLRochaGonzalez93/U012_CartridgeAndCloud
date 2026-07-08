using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;
using VRMGames.CartridgeAndCloud.Presentation.Products;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;

namespace VRMGames.CartridgeAndCloud.Runtime.Store
{
    /// <summary>
    /// Instantiates only complete authored prefabs. Missing contracts fail loudly;
    /// no primitive, collider, anchor or renderer-bounds fallback is generated.
    /// </summary>
    public static class StorePrefabFactory
    {
        public static GameObject BuildFurniture(
            GameObject technicalContainer,
            string definitionId)
        {
            if (technicalContainer == null)
            {
                throw new ArgumentNullException(nameof(technicalContainer));
            }

            GameObject prefab = RequireCatalog().FindFurniture(definitionId);
            ValidateFurniturePrefab(prefab, definitionId);

            ClearChildren(technicalContainer.transform);

            Renderer technicalRenderer = technicalContainer.GetComponent<Renderer>();
            float groundY = GroundingUtility.ResolveBasePlaneHeight(
                technicalContainer.transform,
                technicalRenderer);
            if (technicalRenderer != null)
            {
                technicalRenderer.enabled = false;
            }

            foreach (Collider collider in technicalContainer.GetComponents<Collider>())
            {
                DestroyObject(collider);
            }

            GameObject instance = UnityEngine.Object.Instantiate(
                prefab,
                technicalContainer.transform,
                false);
            instance.name = "Authored_" + definitionId;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Reciprocal(
                technicalContainer.transform.lossyScale);

            Vector3 worldPosition = technicalContainer.transform.position;
            worldPosition.y = groundY;
            instance.transform.position = worldPosition;
            instance.transform.rotation = technicalContainer.transform.rotation;
            if (!GroundingUtility.AlignRootToGroundAnchor(
                    instance.transform,
                    groundY))
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{prefab.name}' has no valid GroundAnchor or base-centre pivot.");
            }

            return instance;
        }

        public static GameObject InstantiateStandaloneFurniture(
            string definitionId,
            Transform parent,
            string instanceName,
            Vector3 worldPosition)
        {
            GameObject prefab = RequireCatalog().FindFurniture(definitionId);
            ValidateFurniturePrefab(prefab, definitionId);

            GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
            instance.name = instanceName;
            instance.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
            instance.transform.localScale = Vector3.one;
            if (!GroundingUtility.AlignRootToGroundAnchor(
                    instance.transform,
                    worldPosition.y))
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{prefab.name}' has no valid GroundAnchor or base-centre pivot.");
            }

            return instance;
        }

        public static GameObject BuildProduct(
            Transform parent,
            string productId,
            Vector3 localPosition)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            GameObject prefab = RequireCatalog().FindProduct(productId);
            if (prefab == null)
            {
                throw new InvalidOperationException(
                    $"Product prefab '{productId}' is not configured.");
            }

            if (prefab.GetComponent<ProductVisualMarker>() == null)
            {
                throw new InvalidOperationException(
                    $"Product prefab '{prefab.name}' is missing ProductVisualMarker.");
            }

            GameObject visual = UnityEngine.Object.Instantiate(prefab, parent, false);
            visual.name = "Product_" + productId;
            visual.transform.localPosition = localPosition;
            visual.transform.localRotation = Quaternion.identity;
            visual.transform.localScale = Reciprocal(parent.lossyScale);
            return visual;
        }

        private static StoreVisualPrefabCatalogAsset RequireCatalog()
        {
            StoreVisualPrefabCatalogAsset catalog =
                StoreVisualPrefabCatalogAsset.FindLoaded();
            if (catalog == null)
            {
                throw new InvalidOperationException(
                    "Representative prefab catalog is not loaded.");
            }

            return catalog;
        }

        private static void ValidateFurniturePrefab(
            GameObject prefab,
            string definitionId)
        {
            if (prefab == null)
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{definitionId}' is not configured.");
            }

            StoreFixturePrefabAuthoring authoring =
                prefab.GetComponent<StoreFixturePrefabAuthoring>();

            if (authoring == null)
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{prefab.name}' has an invalid authoring contract: " +
                    "StoreFixturePrefabAuthoring is missing.");
            }

            if (!string.Equals(
                    authoring.DefinitionId,
                    definitionId,
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{prefab.name}' is authored for " +
                    $"'{authoring.DefinitionId}', not '{definitionId}'.");
            }

            if (!authoring.TryValidate(
                    out string validationReport))
            {
                throw new InvalidOperationException(
                    $"Furniture prefab '{prefab.name}' has an invalid authoring contract: " +
                    validationReport);
            }
        }

        private static Vector3 Reciprocal(Vector3 scale)
        {
            return new Vector3(
                Reciprocal(scale.x),
                Reciprocal(scale.y),
                Reciprocal(scale.z));
        }

        private static float Reciprocal(float value)
        {
            return Mathf.Abs(value) <= 0.0001f ? 1f : 1f / value;
        }

        private static void ClearChildren(Transform root)
        {
            for (int index = root.childCount - 1; index >= 0; index--)
            {
                DestroyObject(root.GetChild(index).gameObject);
            }
        }

        private static void DestroyObject(UnityEngine.Object target)
        {
            if (target == null)
            {
                return;
            }

            if (UnityEngine.Application.isPlaying)
            {
                UnityEngine.Object.Destroy(target);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(target);
            }
        }
    }
}
