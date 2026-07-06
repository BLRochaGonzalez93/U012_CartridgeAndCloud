using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
using VRMGames.CartridgeAndCloud.Runtime.Store;
namespace VRMGames.CartridgeAndCloud.Runtime.Inventory
{
    public sealed class InventoryVisualSynchronizer :
        MonoBehaviour
    {
        private StoreOperationsFacade
            _service;
        private IStoreContentCatalog _catalog;
        private Transform _backroomAnchor;
        private Transform _backroomVisualRoot;

        public void Configure(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            Transform backroomAnchor)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _backroomAnchor = backroomAnchor ??
                throw new ArgumentNullException(
                    nameof(backroomAnchor));

            GameObject root =
                new GameObject(
                    "StoredMerchandise");
            root.transform.SetParent(
                _backroomAnchor,
                false);
            root.transform.localPosition =
                new Vector3(
                    1.6f,
                    0.05f,
                    0f);
            _backroomVisualRoot =
                root.transform;

            _service.StateChanged +=
                HandleStateChanged;

            Refresh();
        }

        private void OnDestroy()
        {
            if (_service != null)
            {
                _service.StateChanged -=
                    HandleStateChanged;
            }
        }

        public void Refresh()
        {
            if (_service == null ||
                _service.State == null)
            {
                return;
            }

            RefreshBackroom();
            RefreshDisplays();
        }

        private void HandleStateChanged(
            StoreOperationsState state)
        {
            Refresh();
        }

        private void RefreshBackroom()
        {
            ClearChildren(
                _backroomVisualRoot);

            int visualIndex = 0;

            foreach (StoreStockRecord stock
                     in _service.State
                         .ProductWarehouse)
            {
                if (!_catalog.TryGetProduct(
                        stock.ItemId,
                        out RetailProductDefinition
                            product))
                {
                    continue;
                }

                int visible =
                    Mathf.Min(
                        stock.Quantity,
                        12);

                for (int index = 0;
                     index < visible;
                     index++)
                {
                    int column =
                        visualIndex % 6;
                    int row =
                        visualIndex / 6;

                    GameObject visual =
                        StorePrefabFactory.BuildProduct(
                            _backroomVisualRoot,
                            product.ProductId,
                            new Vector3(
                                column * 0.28f,
                                0.18f + row * 0.34f,
                                0f));

                    DisableColliders(visual);
                    visualIndex++;
                }
            }

        }

        private void RefreshDisplays()
        {
            PlacedFixtureVisual[] visuals =
                UnityEngine.Object
                    .FindObjectsByType<
                        PlacedFixtureVisual>(
                            FindObjectsInactive
                                .Include,
                            FindObjectsSortMode.None);

            foreach (PlacedFixtureVisual visual
                     in visuals)
            {
                Transform productRoot =
                    GetOrCreateProductRoot(
                        visual.transform);

                ConfigureWorldScaleNeutralRoot(
                    productRoot,
                    visual.transform);
                ClearChildren(productRoot);

                PlacedStoreFixtureRecord fixture =
                    FindFixture(
                        visual.InstanceId);

                visual.ConfigureStock(
                    fixture != null
                        ? fixture.AssignedProductId
                        : string.Empty,
                    fixture != null
                        ? fixture.ProductQuantity
                        : 0);

                CustomerBrowseFixtureAuthoring browseAuthoring =
                    visual.GetComponentInChildren<
                        CustomerBrowseFixtureAuthoring>(true);
                if (browseAuthoring != null)
                {
                    browseAuthoring.ConfigureStock(
                        fixture != null
                            ? fixture.AssignedProductId
                            : string.Empty,
                        fixture != null
                            ? fixture.ProductQuantity
                            : 0);
                }

                if (fixture == null ||
                    fixture.ProductQuantity < 1 ||
                    string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId) ||
                    !_catalog.TryGetProduct(
                        fixture.AssignedProductId,
                        out RetailProductDefinition
                            product) ||
                    !_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            furniture) ||
                    !furniture.SupportsProducts)
                {
                    continue;
                }

                ProductDisplaySlot[] slots =
                    ResolveDisplaySlots(
                        visual.transform,
                        productRoot,
                        furniture.Kind);

                int visible =
                    Mathf.Min(
                        fixture.ProductQuantity,
                        slots.Length);

                for (int index = 0;
                     index < visible;
                     index++)
                {
                    ProductDisplaySlot slot =
                        slots[index];

                    GameObject productVisual =
                        StorePrefabFactory.BuildProduct(
                            productRoot,
                            product.ProductId,
                            slot.LocalPosition);

                    productVisual.transform.localRotation =
                        Quaternion.Euler(
                            slot.LocalEulerAngles);

                    DisableColliders(
                        productVisual);
                }
            }
        }

        private static Transform
            GetOrCreateProductRoot(
                Transform owner)
        {
            Transform root =
                owner.Find(
                    "ProductVisuals");

            if (root != null)
            {
                return root;
            }

            GameObject rootObject =
                new GameObject(
                    "ProductVisuals");
            rootObject.transform.SetParent(
                owner,
                false);
            return rootObject.transform;
        }

        private static ProductDisplaySlot[] ResolveDisplaySlots(
            Transform fixtureRoot,
            Transform productRoot,
            StoreFixtureKind kind)
        {
            ProductDisplaySpotSet set =
                fixtureRoot != null
                    ? fixtureRoot.GetComponentInChildren<ProductDisplaySpotSet>(true)
                    : null;

            if (set != null && set.Spots.Count > 0)
            {
                ProductDisplaySlot[] authored =
                    new ProductDisplaySlot[set.Spots.Count];

                for (int index = 0; index < set.Spots.Count; index++)
                {
                    Transform spot = set.Spots[index];
                    authored[index] = new ProductDisplaySlot(
                        productRoot.InverseTransformPoint(spot.position),
                        productRoot.InverseTransformDirection(spot.forward).sqrMagnitude > 0.0001f
                            ? Quaternion.LookRotation(
                                productRoot.InverseTransformDirection(spot.forward),
                                Vector3.up).eulerAngles
                            : Vector3.zero);
                }

                return authored;
            }

            return BuildDisplaySlots(kind);
        }

        private static ProductDisplaySlot[]
            BuildDisplaySlots(
                StoreFixtureKind kind)
        {
            switch (kind)
            {
                case StoreFixtureKind.WallShelf:
                    return BuildWallShelfSlots();

                case StoreFixtureKind.CentralShelf:
                    return BuildCentralShelfSlots();

                case StoreFixtureKind.LowDisplay:
                    return BuildLowDisplaySlots();

                case StoreFixtureKind.FeaturedDisplay:
                    return BuildFeaturedDisplaySlots();

                default:
                    return Array.Empty<
                        ProductDisplaySlot>();
            }
        }

        private static ProductDisplaySlot[]
            BuildWallShelfSlots()
        {
            float[] levels =
            {
                0.24f,
                0.87f,
                1.50f,
                2.12f
            };

            return BuildShelfFaceSlots(
                levels,
                columns: 6,
                width: 1.72f,
                z: 0.08f,
                yaw: 0f);
        }

        private static ProductDisplaySlot[]
            BuildCentralShelfSlots()
        {
            float[] levels =
            {
                0.24f,
                0.88f,
                1.52f
            };

            ProductDisplaySlot[] front =
                BuildShelfFaceSlots(
                    levels,
                    columns: 5,
                    width: 1.60f,
                    z: 0.22f,
                    yaw: 0f);
            ProductDisplaySlot[] back =
                BuildShelfFaceSlots(
                    levels,
                    columns: 5,
                    width: 1.60f,
                    z: -0.22f,
                    yaw: 180f);

            return CombineSlots(
                front,
                back);
        }

        private static ProductDisplaySlot[]
            BuildLowDisplaySlots()
        {
            float[] levels =
            {
                0.24f,
                0.68f
            };

            ProductDisplaySlot[] front =
                BuildShelfFaceSlots(
                    levels,
                    columns: 3,
                    width: 1.05f,
                    z: 0.22f,
                    yaw: 0f);
            ProductDisplaySlot[] back =
                BuildShelfFaceSlots(
                    levels,
                    columns: 3,
                    width: 1.05f,
                    z: -0.22f,
                    yaw: 180f);

            return CombineSlots(
                front,
                back);
        }

        private static ProductDisplaySlot[]
            BuildFeaturedDisplaySlots()
        {
            ProductDisplaySlot[] slots =
                new ProductDisplaySlot[8];
            int index = 0;

            for (int row = 0;
                 row < 2;
                 row++)
            {
                for (int column = 0;
                     column < 4;
                     column++)
                {
                    slots[index++] =
                        new ProductDisplaySlot(
                            new Vector3(
                                -0.36f +
                                column * 0.24f,
                                0.91f,
                                -0.16f +
                                row * 0.32f),
                            new Vector3(
                                0f,
                                row == 0
                                    ? 180f
                                    : 0f,
                                0f));
                }
            }

            return slots;
        }

        private static ProductDisplaySlot[]
            BuildShelfFaceSlots(
                float[] levels,
                int columns,
                float width,
                float z,
                float yaw)
        {
            ProductDisplaySlot[] slots =
                new ProductDisplaySlot[
                    levels.Length * columns];
            int index = 0;

            for (int levelIndex = 0;
                 levelIndex < levels.Length;
                 levelIndex++)
            {
                for (int column = 0;
                     column < columns;
                     column++)
                {
                    float normalized =
                        columns == 1
                            ? 0.5f
                            : column /
                              (float)(columns - 1);
                    float x =
                        Mathf.Lerp(
                            -width * 0.5f,
                            width * 0.5f,
                            normalized);

                    slots[index++] =
                        new ProductDisplaySlot(
                            new Vector3(
                                x,
                                levels[levelIndex],
                                z),
                            new Vector3(
                                0f,
                                yaw,
                                0f));
                }
            }

            return slots;
        }

        private static ProductDisplaySlot[]
            CombineSlots(
                ProductDisplaySlot[] first,
                ProductDisplaySlot[] second)
        {
            ProductDisplaySlot[] result =
                new ProductDisplaySlot[
                    first.Length + second.Length];

            Array.Copy(
                first,
                0,
                result,
                0,
                first.Length);
            Array.Copy(
                second,
                0,
                result,
                first.Length,
                second.Length);

            return result;
        }

        private readonly struct
            ProductDisplaySlot
        {
            public Vector3 LocalPosition {
                get;
            }

            public Vector3 LocalEulerAngles {
                get;
            }

            public ProductDisplaySlot(
                Vector3 localPosition,
                Vector3 localEulerAngles)
            {
                LocalPosition = localPosition;
                LocalEulerAngles =
                    localEulerAngles;
            }
        }

        private PlacedStoreFixtureRecord
            FindFixture(
                string instanceId)
        {
            foreach (PlacedStoreFixtureRecord fixture
                     in _service.State.Fixtures)
            {
                if (string.Equals(
                        fixture.InstanceId,
                        instanceId,
                        StringComparison.Ordinal))
                {
                    return fixture;
                }
            }

            return null;
        }

        private static void DisableColliders(
            GameObject root)
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

        private static void ConfigureWorldScaleNeutralRoot(
            Transform root,
            Transform owner)
        {
            if (root == null || owner == null)
            {
                return;
            }

            root.localPosition = Vector3.zero;
            root.localRotation = Quaternion.identity;

            Vector3 scale = owner.lossyScale;
            root.localScale = new Vector3(
                SafeReciprocal(scale.x),
                SafeReciprocal(scale.y),
                SafeReciprocal(scale.z));
        }

        private static float SafeReciprocal(float value)
        {
            return Mathf.Abs(value) > 0.0001f
                ? 1f / value
                : 1f;
        }

        private static void ClearChildren(
            Transform root)
        {
            if (root == null)
            {
                return;
            }

            for (int index =
                     root.childCount - 1;
                 index >= 0;
                 index--)
            {
                UnityEngine.Object.Destroy(
                    root.GetChild(index)
                        .gameObject);
            }
        }
    }
}
