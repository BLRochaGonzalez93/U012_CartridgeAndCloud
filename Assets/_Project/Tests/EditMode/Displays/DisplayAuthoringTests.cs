using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Infrastructure.Displays;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayAuthoringTests
    {
        [Test]
        public void DefinitionAsset_BuildDefinition_MapsFields()
        {
            DisplayDefinitionAsset asset = CreateDefinitionAsset();

            try
            {
                DisplayDefinition definition = asset.BuildDefinition();

                Assert.That(
                    definition.Id.Value,
                    Is.EqualTo("shelf"));
                Assert.That(
                    definition.Capacity.Units,
                    Is.EqualTo(12));
                Assert.That(
                    definition.VisibleUnitLimit,
                    Is.EqualTo(6));
                Assert.That(
                    definition.PlacementDefinitionId,
                    Is.EqualTo("technical-shelf"));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void DefinitionAsset_AllowedCategories_AreMapped()
        {
            DisplayDefinitionAsset asset = CreateDefinitionAsset();

            try
            {
                Assert.That(
                    asset.BuildDefinition()
                        .AllowedCategories.Count,
                    Is.EqualTo(2));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void DefinitionAsset_InvalidCapacity_ThrowsOnBuild()
        {
            DisplayDefinitionAsset asset =
                ScriptableObject.CreateInstance<
                    DisplayDefinitionAsset>();

            asset.Configure(
                "invalid",
                "display.invalid",
                0,
                1,
                Array.Empty<string>(),
                "placement");

            try
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => asset.BuildDefinition());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void CatalogAsset_BuildRegistry_ContainsDefinitions()
        {
            DisplayDefinitionAsset definition =
                CreateDefinitionAsset();

            DisplayCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    DisplayCatalogAsset>();

            catalog.Configure(new[] { definition });

            try
            {
                Assert.That(
                    catalog.BuildRegistry().Count,
                    Is.EqualTo(1));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void CatalogAsset_MissingReference_Throws()
        {
            DisplayCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    DisplayCatalogAsset>();

            catalog.Configure(
                new DisplayDefinitionAsset[] { null });

            try
            {
                Assert.Throws<InvalidOperationException>(
                    () => catalog.BuildRegistry());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }
        }

        [Test]
        public void CatalogAsset_DuplicateDefinitionId_Throws()
        {
            DisplayDefinitionAsset left =
                CreateDefinitionAsset();

            DisplayDefinitionAsset right =
                CreateDefinitionAsset();

            DisplayCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    DisplayCatalogAsset>();

            catalog.Configure(new[] { left, right });

            try
            {
                Assert.Throws<ArgumentException>(
                    () => catalog.BuildRegistry());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
                UnityEngine.Object.DestroyImmediate(left);
                UnityEngine.Object.DestroyImmediate(right);
            }
        }

        [Test]
        public void DefinitionAsset_PrefabReference_IsOptional()
        {
            DisplayDefinitionAsset asset = CreateDefinitionAsset();

            try
            {
                Assert.That(asset.DisplayPrefab, Is.Null);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        

        [Test]
        public void BuildInstance_ValidConfiguration_CreatesDomainInstance()
        {
            GameObject gameObject = new GameObject("Display");
            DisplayDefinitionAsset definition =
                CreateDefinitionAsset();

            try
            {
                DisplayRuntimeAuthoring authoring =
                    gameObject.AddComponent<
                        DisplayRuntimeAuthoring>();

                authoring.Configure("display-01", definition);

                DisplayInstance instance =
                    authoring.BuildInstance();

                Assert.That(
                    instance.Id.Value,
                    Is.EqualTo("display-01"));
                Assert.That(
                    instance.Definition.Id.Value,
                    Is.EqualTo("shelf"));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void BuildInstance_MissingDefinition_Throws()
        {
            GameObject gameObject = new GameObject("Display");

            try
            {
                DisplayRuntimeAuthoring authoring =
                    gameObject.AddComponent<
                        DisplayRuntimeAuthoring>();

                authoring.Configure("display-01", null);

                Assert.Throws<InvalidOperationException>(
                    () => authoring.BuildInstance());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
            }
        }

        [Test]
        public void BuildInstance_EmptyInstanceId_Throws()
        {
            GameObject gameObject = new GameObject("Display");
            DisplayDefinitionAsset definition =
                CreateDefinitionAsset();

            try
            {
                DisplayRuntimeAuthoring authoring =
                    gameObject.AddComponent<
                        DisplayRuntimeAuthoring>();

                authoring.Configure("", definition);

                Assert.Throws<ArgumentException>(
                    () => authoring.BuildInstance());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Configure_ExposesInstanceId()
        {
            GameObject gameObject = new GameObject("Display");
            DisplayDefinitionAsset definition =
                CreateDefinitionAsset();

            try
            {
                DisplayRuntimeAuthoring authoring =
                    gameObject.AddComponent<
                        DisplayRuntimeAuthoring>();

                authoring.Configure("display-01", definition);

                Assert.That(
                    authoring.DisplayInstanceId,
                    Is.EqualTo("display-01"));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void BuildInstance_DoesNotCreateProductGameObjects()
        {
            GameObject gameObject = new GameObject("Display");
            DisplayDefinitionAsset definition =
                CreateDefinitionAsset();

            try
            {
                DisplayRuntimeAuthoring authoring =
                    gameObject.AddComponent<
                        DisplayRuntimeAuthoring>();

                authoring.Configure("display-01", definition);
                authoring.BuildInstance();

                Assert.That(
                    gameObject.transform.childCount,
                    Is.Zero);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        private static DisplayDefinitionAsset CreateDefinitionAsset()
        {
            DisplayDefinitionAsset asset =
                ScriptableObject.CreateInstance<
                    DisplayDefinitionAsset>();

            asset.Configure(
                "shelf",
                "display.shelf",
                12,
                6,
                new[] { "video-game", "accessory" },
                "technical-shelf");

            return asset;
        }
    }
}
