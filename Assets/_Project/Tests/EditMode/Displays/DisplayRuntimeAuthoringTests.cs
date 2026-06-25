using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Infrastructure.Displays;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayRuntimeAuthoringTests
    {
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
                Array.Empty<string>(),
                "technical-shelf");

            return asset;
        }
    }
}
