using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public static class RepresentativePrefabFactory
    {
        public static bool TryBuildFurniture(
            GameObject target,
            string definitionId)
        {
            if (target == null)
            {
                return false;
            }

            RepresentativePrefabCatalogAsset catalog =
                RepresentativePrefabCatalogAsset
                    .FindLoaded();

            GameObject prefab =
                catalog != null
                    ? catalog.FindFurniture(
                        definitionId)
                    : null;

            if (prefab == null)
            {
                return false;
            }

            LODGroup targetLodGroup =
                target.GetComponent<LODGroup>();

            ClearLodGroup(targetLodGroup);
            ClearChildren(target.transform);

            Renderer targetRenderer =
                target.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                targetRenderer.enabled = false;
            }

            GameObject instance =
                UnityEngine.Object.Instantiate(
                    prefab,
                    target.transform,
                    false);

            instance.name =
                "Representative_" +
                definitionId;

            instance.transform.localPosition =
                Vector3.zero;
            instance.transform.localRotation =
                Quaternion.identity;
            instance.transform.localScale =
                Reciprocal(
                    target.transform.localScale);

            LODGroup sourceLodGroup =
                instance.GetComponent<LODGroup>();

            LOD[] sourceLods =
                sourceLodGroup != null
                    ? sourceLodGroup.GetLODs()
                    : Array.Empty<LOD>();

            ClearLodGroup(sourceLodGroup);

            MoveVisualHierarchy(
                instance.transform,
                target.transform);

            if (sourceLods.Length > 0)
            {
                if (targetLodGroup == null)
                {
                    targetLodGroup =
                        target.AddComponent<
                            LODGroup>();
                }

                targetLodGroup.enabled =
                    true;
                targetLodGroup.SetLODs(
                    sourceLods);
                targetLodGroup
                    .RecalculateBounds();
            }

            RepresentativePrefabInstance marker =
                target.GetComponent<
                    RepresentativePrefabInstance>();

            if (marker == null)
            {
                marker =
                    target.AddComponent<
                        RepresentativePrefabInstance>();
            }

            marker.Configure(
                definitionId,
                "Furniture",
                false);

            DestroyObject(instance);
            return true;
        }

        public static bool TryBuildProduct(
            Transform parent,
            string productId,
            Vector3 localPosition,
            out GameObject visual)
        {
            visual = null;

            if (parent == null)
            {
                return false;
            }

            RepresentativePrefabCatalogAsset catalog =
                RepresentativePrefabCatalogAsset
                    .FindLoaded();

            GameObject prefab =
                catalog != null
                    ? catalog.FindProduct(
                        productId)
                    : null;

            if (prefab == null)
            {
                return false;
            }

            visual =
                UnityEngine.Object.Instantiate(
                    prefab,
                    parent,
                    false);

            visual.name =
                "Product_" +
                productId;
            visual.transform.localPosition =
                localPosition;
            visual.transform.localRotation =
                Quaternion.identity;
            visual.transform.localScale =
                Reciprocal(
                    parent.lossyScale);

            Phase1ProductVisualMarker productMarker =
                visual.GetComponent<
                    Phase1ProductVisualMarker>();

            if (productMarker == null)
            {
                productMarker =
                    visual.AddComponent<
                        Phase1ProductVisualMarker>();
            }

            productMarker.Configure(productId);
            return true;
        }

        private static void MoveVisualHierarchy(
            Transform instance,
            Transform target)
        {
            Transform lodZero =
                FindDirectChild(
                    instance,
                    "LOD0");

            if (lodZero != null)
            {
                while (lodZero.childCount > 0)
                {
                    Transform child =
                        lodZero.GetChild(0);

                    child.SetParent(
                        target,
                        true);

                    DisableColliders(child);
                }

                lodZero.SetParent(
                    target,
                    true);

                DestroyObject(
                    lodZero.gameObject);
            }

            while (instance.childCount > 0)
            {
                Transform child =
                    instance.GetChild(0);

                if (child == lodZero)
                {
                    continue;
                }

                child.SetParent(
                    target,
                    true);

                DisableColliders(child);
            }
        }

        private static Transform FindDirectChild(
            Transform root,
            string name)
        {
            if (root == null)
            {
                return null;
            }

            for (int index = 0;
                 index < root.childCount;
                 index++)
            {
                Transform child =
                    root.GetChild(index);

                if (string.Equals(
                        child.name,
                        name,
                        StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }

        private static void ClearLodGroup(
            LODGroup group)
        {
            if (group == null)
            {
                return;
            }

            group.SetLODs(
                Array.Empty<LOD>());
            group.enabled = false;
        }

        private static void DisableColliders(
            Transform root)
        {
            if (root == null)
            {
                return;
            }

            Collider[] colliders =
                root.GetComponentsInChildren<
                    Collider>(true);

            foreach (Collider collider
                     in colliders)
            {
                collider.enabled = false;
            }
        }

        private static Vector3 Reciprocal(
            Vector3 scale)
        {
            return new Vector3(
                Reciprocal(scale.x),
                Reciprocal(scale.y),
                Reciprocal(scale.z));
        }

        private static float Reciprocal(
            float value)
        {
            return Mathf.Abs(value) <=
                   0.0001f
                ? 1f
                : 1f / value;
        }

        private static void ClearChildren(
            Transform root)
        {
            for (int index =
                     root.childCount - 1;
                 index >= 0;
                 index--)
            {
                DestroyObject(
                    root.GetChild(index)
                        .gameObject);
            }
        }

        private static void DestroyObject(
            UnityEngine.Object target)
        {
            if (target == null)
            {
                return;
            }

            if (UnityEngine.Application
                .isPlaying)
            {
                UnityEngine.Object.Destroy(target);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(
                    target);
            }
        }
    }
}
