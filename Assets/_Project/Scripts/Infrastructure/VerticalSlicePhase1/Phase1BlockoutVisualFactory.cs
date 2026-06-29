using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public static class Phase1BlockoutVisualFactory
    {
        public static void BuildFurniture(
            GameObject root,
            Phase1FurnitureDefinition definition,
            Material material,
            float cellSize)
        {
            if (root == null)
            {
                throw new ArgumentNullException(
                    nameof(root));
            }

            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ClearGeneratedChildren(root.transform);

            Renderer rootRenderer =
                root.GetComponent<Renderer>();

            if (rootRenderer != null)
            {
                rootRenderer.enabled = false;
            }

            float width =
                definition.WidthCells * cellSize;
            float depth =
                definition.DepthCells * cellSize;
            float height =
                definition.HeightMeters;

            switch (definition.Kind)
            {
                case Phase1FurnitureKind
                    .CheckoutCounter:
                    AddCube(
                        root.transform,
                        "CounterBody",
                        new Vector3(
                            width,
                            height * 0.72f,
                            depth),
                        new Vector3(
                            0f,
                            height * 0.36f,
                            0f),
                        material);
                    AddCube(
                        root.transform,
                        "CounterTop",
                        new Vector3(
                            width * 1.05f,
                            height * 0.12f,
                            depth * 1.05f),
                        new Vector3(
                            0f,
                            height * 0.78f,
                            0f),
                        material);
                    AddCube(
                        root.transform,
                        "Register",
                        new Vector3(
                            width * 0.28f,
                            height * 0.22f,
                            depth * 0.35f),
                        new Vector3(
                            width * 0.22f,
                            height * 0.97f,
                            0f),
                        material);
                    break;

                case Phase1FurnitureKind
                    .WallShelf:
                    BuildShelf(
                        root.transform,
                        width,
                        depth,
                        height,
                        shelves: 4,
                        backPanel: true,
                        material);
                    break;

                case Phase1FurnitureKind
                    .CentralShelf:
                    BuildShelf(
                        root.transform,
                        width,
                        depth,
                        height,
                        shelves: 3,
                        backPanel: false,
                        material);
                    break;

                case Phase1FurnitureKind
                    .LowDisplay:
                    BuildShelf(
                        root.transform,
                        width,
                        depth,
                        height,
                        shelves: 2,
                        backPanel: false,
                        material);
                    break;

                case Phase1FurnitureKind
                    .FeaturedDisplay:
                    AddCube(
                        root.transform,
                        "Pedestal",
                        new Vector3(
                            width * 0.82f,
                            height * 0.72f,
                            depth * 0.82f),
                        new Vector3(
                            0f,
                            height * 0.36f,
                            0f),
                        material);
                    AddCube(
                        root.transform,
                        "Top",
                        new Vector3(
                            width,
                            height * 0.12f,
                            depth),
                        new Vector3(
                            0f,
                            height * 0.78f,
                            0f),
                        material);
                    break;

                case Phase1FurnitureKind
                    .BackroomStorage:
                    BuildShelf(
                        root.transform,
                        width,
                        depth,
                        height,
                        shelves: 5,
                        backPanel: true,
                        material);
                    break;

                case Phase1FurnitureKind
                    .ReceivingCrate:
                    AddCube(
                        root.transform,
                        "Crate",
                        new Vector3(
                            width,
                            height,
                            depth),
                        new Vector3(
                            0f,
                            height * 0.5f,
                            0f),
                        material);
                    AddCube(
                        root.transform,
                        "CrateBandA",
                        new Vector3(
                            width * 1.02f,
                            height * 0.1f,
                            depth * 1.03f),
                        new Vector3(
                            0f,
                            height * 0.25f,
                            0f),
                        material);
                    AddCube(
                        root.transform,
                        "CrateBandB",
                        new Vector3(
                            width * 1.02f,
                            height * 0.1f,
                            depth * 1.03f),
                        new Vector3(
                            0f,
                            height * 0.75f,
                            0f),
                        material);
                    break;

                default:
                    AddCube(
                        root.transform,
                        "Decoration",
                        new Vector3(
                            width * 0.75f,
                            height,
                            depth * 0.75f),
                        new Vector3(
                            0f,
                            height * 0.5f,
                            0f),
                        material);
                    break;
            }
        }

        public static GameObject BuildProduct(
            Transform parent,
            Phase1ProductDefinition definition,
            Material material,
            Vector3 localPosition,
            Vector3 localScale)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            PrimitiveType primitive =
                definition.Kind ==
                    Phase1ProductKind.Headset
                    ? PrimitiveType.Cylinder
                    : PrimitiveType.Cube;

            GameObject product =
                GameObject.CreatePrimitive(primitive);

            product.name =
                "Product_" + definition.ProductId;
            product.transform.SetParent(
                parent,
                false);
            product.transform.localPosition =
                localPosition;
            product.transform.localScale =
                localScale;

            Renderer renderer =
                product.GetComponent<Renderer>();

            if (renderer != null &&
                material != null)
            {
                renderer.sharedMaterial = material;
            }

            Phase1ProductVisualMarker marker =
                product.AddComponent<
                    Phase1ProductVisualMarker>();
            marker.Configure(
                definition.ProductId);

            return product;
        }

        public static GameObject BuildCharacter(
            Transform parent,
            string characterId,
            Phase1CharacterRole role,
            Material material,
            Vector3 position)
        {
            GameObject root =
                new GameObject(
                    "Character_" + characterId);
            root.transform.SetParent(
                parent,
                false);
            root.transform.position = position;

            GameObject body =
                GameObject.CreatePrimitive(
                    PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(
                root.transform,
                false);
            body.transform.localPosition =
                new Vector3(0f, 0.9f, 0f);
            body.transform.localScale =
                new Vector3(
                    0.65f,
                    0.9f,
                    0.65f);

            Renderer renderer =
                body.GetComponent<Renderer>();

            if (renderer != null &&
                material != null)
            {
                renderer.sharedMaterial = material;
            }

            Phase1CharacterPresence presence =
                root.AddComponent<
                    Phase1CharacterPresence>();
            presence.Configure(
                characterId,
                role);

            return root;
        }

        public static GameObject AddCube(
            Transform parent,
            string name,
            Vector3 scale,
            Vector3 localPosition,
            Material material)
        {
            GameObject cube =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(
                parent,
                false);
            cube.transform.localPosition =
                localPosition;
            cube.transform.localScale = scale;

            Renderer renderer =
                cube.GetComponent<Renderer>();

            if (renderer != null &&
                material != null)
            {
                renderer.sharedMaterial = material;
            }

            return cube;
        }

        private static void BuildShelf(
            Transform parent,
            float width,
            float depth,
            float height,
            int shelves,
            bool backPanel,
            Material material)
        {
            float postWidth =
                Mathf.Min(0.12f, width * 0.12f);

            AddCube(
                parent,
                "PostLeft",
                new Vector3(
                    postWidth,
                    height,
                    depth),
                new Vector3(
                    -width * 0.5f +
                    postWidth * 0.5f,
                    height * 0.5f,
                    0f),
                material);

            AddCube(
                parent,
                "PostRight",
                new Vector3(
                    postWidth,
                    height,
                    depth),
                new Vector3(
                    width * 0.5f -
                    postWidth * 0.5f,
                    height * 0.5f,
                    0f),
                material);

            for (int index = 0;
                 index < shelves;
                 index++)
            {
                float normalized =
                    shelves == 1
                        ? 0.5f
                        : (float)index /
                          (shelves - 1);

                AddCube(
                    parent,
                    "Shelf_" + index,
                    new Vector3(
                        width,
                        Mathf.Max(
                            0.06f,
                            height * 0.04f),
                        depth),
                    new Vector3(
                        0f,
                        Mathf.Lerp(
                            height * 0.08f,
                            height * 0.92f,
                            normalized),
                        0f),
                    material);
            }

            if (backPanel)
            {
                AddCube(
                    parent,
                    "BackPanel",
                    new Vector3(
                        width,
                        height,
                        Mathf.Max(
                            0.04f,
                            depth * 0.08f)),
                    new Vector3(
                        0f,
                        height * 0.5f,
                        depth * 0.46f),
                    material);
            }
        }

        private static void ClearGeneratedChildren(
            Transform root)
        {
            for (int index =
                     root.childCount - 1;
                 index >= 0;
                 index--)
            {
                Transform child =
                    root.GetChild(index);

                if (UnityEngine.Application
                    .isPlaying)
                {
                    UnityEngine.Object.Destroy(
                        child.gameObject);
                }
                else
                {
                    UnityEngine.Object
                        .DestroyImmediate(
                            child.gameObject);
                }
            }
        }
    }

}
