using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1InventoryVisualSynchronizer :
        MonoBehaviour
    {
        private Phase1VerticalSliceService
            _service;
        private IPhase1Catalog _catalog;
        private Phase1MaterialPaletteAsset
            _palette;
        private Transform _backroomAnchor;
        private Transform _backroomVisualRoot;

        public void Configure(
            Phase1VerticalSliceService service,
            IPhase1Catalog catalog,
            Phase1MaterialPaletteAsset palette,
            Transform backroomAnchor)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _palette = palette ??
                throw new ArgumentNullException(
                    nameof(palette));
            _backroomAnchor = backroomAnchor ??
                throw new ArgumentNullException(
                    nameof(backroomAnchor));

            GameObject root =
                new GameObject(
                    "StoredMerchandiseBlockout");
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
            Phase1StoreState state)
        {
            Refresh();
        }

        private void RefreshBackroom()
        {
            ClearChildren(
                _backroomVisualRoot);

            int visualIndex = 0;

            foreach (Phase1StockRecord stock
                     in _service.State
                         .ProductWarehouse)
            {
                if (!_catalog.TryGetProduct(
                        stock.ItemId,
                        out Phase1ProductDefinition
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
                        Phase1BlockoutVisualFactory
                            .BuildProduct(
                                _backroomVisualRoot,
                                product,
                                _palette.Find(
                                    product
                                        .MaterialVariantId),
                                new Vector3(
                                    column * 0.28f,
                                    0.18f +
                                    row * 0.34f,
                                    0f),
                                ProductScale(
                                    product.Kind));

                    DisableColliders(visual);
                    visualIndex++;
                }
            }

            if (visualIndex == 0)
            {
                GameObject emptyCrate =
                    Phase1BlockoutVisualFactory
                        .AddCube(
                            _backroomVisualRoot,
                            "EmptyMerchandiseCrate",
                            new Vector3(
                                0.9f,
                                0.55f,
                                0.65f),
                            new Vector3(
                                0f,
                                0.275f,
                                0f),
                            _palette.Find(
                                "furniture-crate"));

                DisableColliders(emptyCrate);
            }
        }

        private void RefreshDisplays()
        {
            Phase1PlacedFixtureVisual[] visuals =
                UnityEngine.Object
                    .FindObjectsByType<
                        Phase1PlacedFixtureVisual>(
                            FindObjectsInactive
                                .Include,
                            FindObjectsSortMode.None);

            foreach (Phase1PlacedFixtureVisual visual
                     in visuals)
            {
                Transform productRoot =
                    visual.transform.Find(
                        "ProductVisuals");

                if (productRoot == null)
                {
                    GameObject root =
                        new GameObject(
                            "ProductVisuals");
                    root.transform.SetParent(
                        visual.transform,
                        false);
                    productRoot = root.transform;
                }

                ClearChildren(productRoot);

                Phase1PlacedFixtureRecord fixture =
                    FindFixture(
                        visual.InstanceId);

                if (fixture == null ||
                    fixture.ProductQuantity < 1 ||
                    string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId) ||
                    !_catalog.TryGetProduct(
                        fixture.AssignedProductId,
                        out Phase1ProductDefinition
                            product))
                {
                    continue;
                }

                if (!_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out Phase1FurnitureDefinition
                            furniture))
                {
                    continue;
                }

                int visible =
                    Mathf.Min(
                        fixture.ProductQuantity,
                        10);

                float width =
                    furniture.WidthCells *
                    0.5f;
                float depth =
                    furniture.DepthCells *
                    0.5f;

                for (int index = 0;
                     index < visible;
                     index++)
                {
                    int columns =
                        Mathf.Max(
                            1,
                            Mathf.CeilToInt(
                                Mathf.Sqrt(
                                    visible)));

                    int column =
                        index % columns;
                    int row =
                        index / columns;

                    float x =
                        -width * 0.35f +
                        column *
                        Mathf.Min(
                            0.32f,
                            width * 0.7f /
                            Mathf.Max(
                                1,
                                columns - 1));

                    float z =
                        -depth * 0.2f +
                        row * 0.22f;

                    GameObject productVisual =
                        Phase1BlockoutVisualFactory
                            .BuildProduct(
                                productRoot,
                                product,
                                _palette.Find(
                                    product
                                        .MaterialVariantId),
                                new Vector3(
                                    x,
                                    furniture.HeightMeters *
                                    0.78f,
                                    z),
                                ProductScale(
                                    product.Kind));

                    DisableColliders(
                        productVisual);
                }
            }
        }

        private Phase1PlacedFixtureRecord
            FindFixture(
                string instanceId)
        {
            foreach (Phase1PlacedFixtureRecord fixture
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

        private static Vector3 ProductScale(
            Phase1ProductKind kind)
        {
            switch (kind)
            {
                case Phase1ProductKind.Console:
                    return new Vector3(
                        0.34f,
                        0.13f,
                        0.26f);
                case Phase1ProductKind.Controller:
                    return new Vector3(
                        0.22f,
                        0.10f,
                        0.16f);
                case Phase1ProductKind.Headset:
                    return new Vector3(
                        0.16f,
                        0.10f,
                        0.16f);
                case Phase1ProductKind.Accessory:
                    return new Vector3(
                        0.14f,
                        0.18f,
                        0.08f);
                default:
                    return new Vector3(
                        0.16f,
                        0.22f,
                        0.04f);
            }
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
