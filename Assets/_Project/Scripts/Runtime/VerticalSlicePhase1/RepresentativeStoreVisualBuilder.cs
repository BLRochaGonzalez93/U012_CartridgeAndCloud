using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public static class RepresentativeStoreVisualBuilder
    {
        private const string VisualRootName =
            "RepresentativeStoreVisuals";

        public static bool TryApply(
            GameObject builtRoot,
            PlacementSurface surface,
            Phase1StoreShellAsset shell,
            Transform entranceAnchor,
            Transform checkoutAnchor,
            Transform receivingAnchor,
            Transform backroomAnchor,
            AutomaticSlidingDoorController door)
        {
            RepresentativePrefabCatalogAsset catalog =
                RepresentativePrefabCatalogAsset
                    .FindLoaded();

            if (builtRoot == null ||
                surface == null ||
                shell == null ||
                catalog == null ||
                catalog.FindArchitecture(
                    "architecture.floor.200") ==
                null)
            {
                return false;
            }

            Transform previous =
                FindDeep(
                    builtRoot.transform,
                    VisualRootName);

            if (previous != null)
            {
                DestroyObject(
                    previous.gameObject);
            }

            GameObject visualRoot =
                new GameObject(
                    VisualRootName);

            visualRoot.transform.SetParent(
                builtRoot.transform,
                false);

            HidePlaceholderRenderers(
                builtRoot.transform);

            float width =
                surface.GridWidth *
                surface.CellSize;
            float depth =
                surface.GridDepth *
                surface.CellSize;
            Vector3 origin =
                surface.GridOrigin;
            Vector3 center =
                new Vector3(
                    origin.x + width * 0.5f,
                    origin.y,
                    origin.z + depth * 0.5f);

            BuildFloor(
                catalog,
                visualRoot.transform,
                origin,
                width,
                depth);
            BuildWalls(
                catalog,
                builtRoot.transform,
                visualRoot.transform,
                width,
                depth);
            BuildFacade(
                catalog,
                builtRoot.transform,
                visualRoot.transform,
                origin,
                center,
                width,
                depth);
            BuildDoor(
                catalog,
                builtRoot.transform,
                shell,
                door);
            BuildBackroomPartition(
                catalog,
                builtRoot.transform,
                visualRoot.transform,
                origin,
                center,
                depth,
                shell);
            BuildSign(
                catalog,
                builtRoot.transform,
                visualRoot.transform);
            BuildZones(
                catalog,
                visualRoot.transform,
                checkoutAnchor,
                receivingAnchor,
                backroomAnchor);
            BuildStaticFurniture(
                catalog,
                builtRoot.transform,
                visualRoot.transform);
            BuildLightingFixtures(
                catalog,
                builtRoot.transform,
                visualRoot.transform);
            BuildThreshold(
                catalog,
                visualRoot.transform,
                entranceAnchor);

            RepresentativePrefabInstance marker =
                visualRoot.AddComponent<
                    RepresentativePrefabInstance>();

            marker.Configure(
                "store-shell",
                "Architecture",
                false);

            return true;
        }

        private static void BuildFloor(
            RepresentativePrefabCatalogAsset catalog,
            Transform parent,
            Vector3 origin,
            float width,
            float depth)
        {
            int fullTwoX =
                Mathf.FloorToInt(width / 2f);
            int fullTwoZ =
                Mathf.FloorToInt(depth / 2f);

            GameObject twoMeter =
                catalog.FindArchitecture(
                    "architecture.floor.200");
            GameObject oneMeter =
                catalog.FindArchitecture(
                    "architecture.floor.100");

            for (int x = 0;
                 x < fullTwoX;
                 x++)
            {
                for (int z = 0;
                     z < fullTwoZ;
                     z++)
                {
                    Instantiate(
                        twoMeter,
                        parent,
                        new Vector3(
                            origin.x +
                            x * 2f +
                            1f,
                            origin.y,
                            origin.z +
                            z * 2f +
                            1f),
                        Quaternion.identity);
                }
            }

            float coveredDepth =
                fullTwoZ * 2f;

            if (depth - coveredDepth >
                0.1f)
            {
                int oneCount =
                    Mathf.RoundToInt(width);

                for (int x = 0;
                     x < oneCount;
                     x++)
                {
                    Instantiate(
                        oneMeter,
                        parent,
                        new Vector3(
                            origin.x +
                            x +
                            0.5f,
                            origin.y,
                            origin.z +
                            coveredDepth +
                            0.5f),
                        Quaternion.identity);
                }
            }
        }

        private static void BuildWalls(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Transform visualRoot,
            float width,
            float depth)
        {
            CoverWall(
                catalog,
                FindDeep(
                    storeRoot,
                    "Wall_Left"),
                visualRoot,
                depth,
                true);
            CoverWall(
                catalog,
                FindDeep(
                    storeRoot,
                    "Wall_Right"),
                visualRoot,
                depth,
                true);
            CoverWall(
                catalog,
                FindDeep(
                    storeRoot,
                    "Wall_Back"),
                visualRoot,
                width,
                false);
        }

        private static void CoverWall(
            RepresentativePrefabCatalogAsset catalog,
            Transform wall,
            Transform visualRoot,
            float length,
            bool alongZ)
        {
            if (wall == null)
            {
                return;
            }

            Renderer renderer =
                wall.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.enabled = false;
            }

            Bounds bounds =
                ResolveBounds(
                    wall.gameObject);

            List<float> segments =
                SegmentLengths(length);
            float cursor =
                -length * 0.5f;

            foreach (float segment in segments)
            {
                string id =
                    segment >= 3.9f
                        ? "architecture.wall.400"
                        : segment >= 1.9f
                            ? "architecture.wall.200"
                            : "architecture.wall.100";

                GameObject prefab =
                    catalog.FindArchitecture(id);

                float centerOffset =
                    cursor +
                    segment * 0.5f;

                Vector3 position =
                    alongZ
                        ? new Vector3(
                            bounds.center.x,
                            bounds.min.y,
                            bounds.center.z +
                            centerOffset)
                        : new Vector3(
                            bounds.center.x +
                            centerOffset,
                            bounds.min.y,
                            bounds.center.z);

                Instantiate(
                    prefab,
                    visualRoot,
                    position,
                    alongZ
                        ? Quaternion.Euler(
                            0f,
                            90f,
                            0f)
                        : Quaternion.identity);

                cursor += segment;
            }
        }

        private static List<float>
            SegmentLengths(
                float length)
        {
            List<float> result =
                new List<float>();

            float remaining =
                Mathf.Round(length);

            while (remaining >= 3.9f)
            {
                result.Add(4f);
                remaining -= 4f;
            }

            if (remaining >= 1.9f)
            {
                result.Add(2f);
                remaining -= 2f;
            }

            if (remaining >= 0.9f)
            {
                result.Add(1f);
            }

            return result;
        }

        private static void BuildFacade(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Transform visualRoot,
            Vector3 origin,
            Vector3 center,
            float width,
            float depth)
        {
            Transform leftWall =
                FindDeep(
                    storeRoot,
                    "Wall_Front_Left");
            Transform rightWall =
                FindDeep(
                    storeRoot,
                    "Wall_Front_Right");
            Transform leftWindow =
                FindDeep(
                    storeRoot,
                    "Window_Left");
            Transform rightWindow =
                FindDeep(
                    storeRoot,
                    "Window_Right");

            DisableRenderer(leftWall);
            DisableRenderer(rightWall);
            DisableRenderer(leftWindow);
            DisableRenderer(rightWindow);

            GameObject facade =
                Instantiate(
                    catalog.FindArchitecture(
                        "architecture.storefront-facade"),
                    visualRoot,
                    new Vector3(
                        center.x,
                        origin.y,
                        center.z -
                        depth * 0.5f),
                    Quaternion.identity);

            if (facade != null)
            {
                facade.transform.SetParent(
                    visualRoot,
                    true);
            }
        }

        private static void BuildDoor(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Phase1StoreShellAsset shell,
            AutomaticSlidingDoorController door)
        {
            Transform entrance =
                FindDeep(
                    storeRoot,
                    "EntranceAndDoor");

            if (entrance == null)
            {
                return;
            }

            DisableRenderer(
                FindDeep(
                    entrance,
                    "Door_Left"));
            DisableRenderer(
                FindDeep(
                    entrance,
                    "Door_Right"));

            GameObject instance =
                InstantiateLocal(
                    catalog.FindArchitecture(
                        "architecture.automatic-door"),
                    entrance);

            if (instance == null ||
                door == null)
            {
                return;
            }

            RepresentativeAutomaticDoorParts
                parts =
                    instance.GetComponent<
                        RepresentativeAutomaticDoorParts>();

            if (parts == null ||
                parts.LeftPanel == null ||
                parts.RightPanel == null)
            {
                return;
            }

            EnsureDoorPanelVertical(
                parts.LeftPanel);
            EnsureDoorPanelVertical(
                parts.RightPanel);

            ResolvePhysicalDoorPanels(
                instance.transform,
                parts.LeftPanel,
                parts.RightPanel,
                out Transform physicalLeft,
                out Transform physicalRight);

            float entranceWidth =
                shell.EntranceWidthCells *
                shell.CellSize;

            door.Configure(
                physicalLeft,
                physicalRight,
                entranceWidth * 0.48f,
                shell.DoorOpenDistance,
                shell.DoorSpeed);
        }

        private static void
            EnsureDoorPanelVertical(
                Transform panel)
        {
            if (panel == null)
            {
                return;
            }

            Quaternion original =
                panel.localRotation;
            Quaternion best =
                original;
            float bestHeight =
                CalculateRendererBounds(
                    panel)
                    .size.y;

            Quaternion[] candidates =
            {
                original *
                Quaternion.Euler(
                    90f,
                    0f,
                    0f),
                original *
                Quaternion.Euler(
                    -90f,
                    0f,
                    0f)
            };

            foreach (Quaternion candidate
                     in candidates)
            {
                panel.localRotation =
                    candidate;

                float candidateHeight =
                    CalculateRendererBounds(
                        panel)
                        .size.y;

                if (candidateHeight >
                    bestHeight + 0.01f)
                {
                    bestHeight =
                        candidateHeight;
                    best = candidate;
                }
            }

            panel.localRotation = best;
        }

        private static Bounds
            CalculateRendererBounds(
                Transform root)
        {
            Renderer[] renderers =
                root.GetComponentsInChildren<
                    Renderer>(true);

            if (renderers == null ||
                renderers.Length == 0)
            {
                return new Bounds(
                    root.position,
                    Vector3.zero);
            }

            Bounds bounds =
                renderers[0].bounds;

            for (int index = 1;
                 index < renderers.Length;
                 index++)
            {
                bounds.Encapsulate(
                    renderers[index]
                        .bounds);
            }

            return bounds;
        }

        private static void
            ResolvePhysicalDoorPanels(
                Transform doorRoot,
                Transform first,
                Transform second,
                out Transform left,
                out Transform right)
        {
            float firstX =
                doorRoot
                    .InverseTransformPoint(
                        first.position)
                    .x;
            float secondX =
                doorRoot
                    .InverseTransformPoint(
                        second.position)
                    .x;

            if (firstX <= secondX)
            {
                left = first;
                right = second;
                return;
            }

            left = second;
            right = first;
        }

        private static void
            BuildBackroomPartition(
                RepresentativePrefabCatalogAsset catalog,
                Transform storeRoot,
                Transform visualRoot,
                Vector3 origin,
                Vector3 center,
                float depth,
                Phase1StoreShellAsset shell)
        {
            DisableRenderer(
                FindDeep(
                    storeRoot,
                    "BackroomPartition_Left"));
            DisableRenderer(
                FindDeep(
                    storeRoot,
                    "BackroomPartition_Right"));

            float z =
                center.z +
                depth * 0.5f -
                shell.BackroomDepthMeters;

            Instantiate(
                catalog.FindArchitecture(
                    "architecture.backroom-partition"),
                visualRoot,
                new Vector3(
                    center.x,
                    origin.y,
                    z),
                Quaternion.identity);
        }

        private static void BuildSign(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Transform visualRoot)
        {
            Transform placeholder =
                FindDeep(
                    storeRoot,
                    "StoreSign");

            if (placeholder == null)
            {
                return;
            }

            Bounds bounds =
                ResolveBounds(
                    placeholder.gameObject);

            DisableRenderer(placeholder);

            Instantiate(
                catalog.FindArchitecture(
                    "architecture.store-sign"),
                visualRoot,
                new Vector3(
                    bounds.center.x,
                    bounds.min.y,
                    bounds.center.z),
                placeholder.rotation);
        }

        private static void BuildZones(
            RepresentativePrefabCatalogAsset catalog,
            Transform parent,
            Transform checkout,
            Transform receiving,
            Transform backroom)
        {
            InstantiateAtAnchor(
                catalog.FindArchitecture(
                    "architecture.zone-checkout"),
                parent,
                checkout);
            InstantiateAtAnchor(
                catalog.FindArchitecture(
                    "architecture.zone-receiving"),
                parent,
                receiving);
            InstantiateAtAnchor(
                catalog.FindArchitecture(
                    "architecture.zone-backroom"),
                parent,
                backroom);
        }

        private static void BuildStaticFurniture(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Transform visualRoot)
        {
            ReplacePlaceholder(
                catalog.FindFurniture(
                    "backroom-storage"),
                FindDeep(
                    storeRoot,
                    "BackroomStorageBlockout"),
                visualRoot);
            ReplacePlaceholder(
                catalog.FindFurniture(
                    "receiving-crate"),
                FindDeep(
                    storeRoot,
                    "ReceivingCrateBlockout"),
                visualRoot);
            ReplacePlaceholder(
                catalog.FindFurniture(
                    "decoration-plant"),
                FindDeep(
                    storeRoot,
                    "DecorationPlant"),
                visualRoot);
        }

        private static void BuildLightingFixtures(
            RepresentativePrefabCatalogAsset catalog,
            Transform storeRoot,
            Transform visualRoot)
        {
            Transform lighting =
                FindDeep(
                    storeRoot,
                    "LightingRig");

            if (lighting == null)
            {
                return;
            }

            GameObject fixture =
                catalog.FindArchitecture(
                    "architecture.linear-panel");

            for (int index = 0;
                 index < lighting.childCount;
                 index++)
            {
                Transform lightTransform =
                    lighting.GetChild(index);

                if (lightTransform.GetComponent<
                        Light>() == null)
                {
                    continue;
                }

                Instantiate(
                    fixture,
                    visualRoot,
                    lightTransform.position +
                    Vector3.up * 0.15f,
                    Quaternion.identity);
            }
        }

        private static void BuildThreshold(
            RepresentativePrefabCatalogAsset catalog,
            Transform parent,
            Transform entranceAnchor)
        {
            if (entranceAnchor == null)
            {
                return;
            }

            Instantiate(
                catalog.FindArchitecture(
                    "architecture.entrance-threshold"),
                parent,
                entranceAnchor.position,
                entranceAnchor.rotation);
        }

        private static void ReplacePlaceholder(
            GameObject prefab,
            Transform placeholder,
            Transform parent)
        {
            if (prefab == null ||
                placeholder == null)
            {
                return;
            }

            Bounds bounds =
                ResolveBounds(
                    placeholder.gameObject);

            DisableRenderer(placeholder);

            Instantiate(
                prefab,
                parent,
                new Vector3(
                    bounds.center.x,
                    bounds.min.y,
                    bounds.center.z),
                placeholder.rotation);
        }

        private static void InstantiateAtAnchor(
            GameObject prefab,
            Transform parent,
            Transform anchor)
        {
            if (anchor == null)
            {
                return;
            }

            Instantiate(
                prefab,
                parent,
                anchor.position,
                anchor.rotation);
        }

        private static GameObject InstantiateLocal(
            GameObject prefab,
            Transform parent)
        {
            if (prefab == null ||
                parent == null)
            {
                return null;
            }

            GameObject instance =
                UnityEngine.Object.Instantiate(
                    prefab,
                    parent,
                    false);

            instance.name =
                prefab.name;
            instance.transform.localPosition =
                Vector3.zero;
            instance.transform.localRotation =
                Quaternion.identity;
            instance.transform.localScale =
                Vector3.one;

            DisableColliders(instance);
            return instance;
        }

        private static GameObject Instantiate(
            GameObject prefab,
            Transform parent,
            Vector3 position,
            Quaternion rotation)
        {
            if (prefab == null ||
                parent == null)
            {
                return null;
            }

            GameObject instance =
                UnityEngine.Object.Instantiate(
                    prefab,
                    position,
                    rotation,
                    parent);

            instance.name =
                prefab.name;

            DisableColliders(instance);
            return instance;
        }

        private static void HidePlaceholderRenderers(
            Transform storeRoot)
        {
            string[] names =
            {
                "FloorOverlay",
                "Window_Left",
                "Window_Right",
                "StoreSign",
                "BackroomStorageBlockout",
                "ReceivingCrateBlockout",
                "DecorationPlant",
                "CheckoutZone",
                "ReceivingZone",
                "BackroomZone"
            };

            foreach (string name in names)
            {
                DisableRenderer(
                    FindDeep(
                        storeRoot,
                        name));
            }
        }

        private static void DisableRenderer(
            Transform target)
        {
            if (target == null)
            {
                return;
            }

            Renderer renderer =
                target.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        private static void DisableColliders(
            GameObject root)
        {
            Collider[] colliders =
                root.GetComponentsInChildren<
                    Collider>(true);

            foreach (Collider collider
                     in colliders)
            {
                collider.enabled = false;
            }
        }

        private static Bounds ResolveBounds(
            GameObject target)
        {
            Collider collider =
                target.GetComponent<Collider>();

            if (collider != null)
            {
                return collider.bounds;
            }

            Renderer renderer =
                target.GetComponent<Renderer>();

            return renderer != null
                ? renderer.bounds
                : new Bounds(
                    target.transform.position,
                    Vector3.zero);
        }

        private static Transform FindDeep(
            Transform root,
            string name)
        {
            if (root == null)
            {
                return null;
            }

            if (string.Equals(
                    root.name,
                    name,
                    StringComparison.Ordinal))
            {
                return root;
            }

            for (int index = 0;
                 index < root.childCount;
                 index++)
            {
                Transform found =
                    FindDeep(
                        root.GetChild(index),
                        name);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
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
                UnityEngine.Object.Destroy(
                    target);
            }
            else
            {
                UnityEngine.Object
                    .DestroyImmediate(
                        target);
            }
        }
    }
}
