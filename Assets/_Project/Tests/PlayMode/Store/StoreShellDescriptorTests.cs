using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Presentation.Store;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class StoreShellDescriptorTests
    {
        private GameObject _root;
        private GameObject _floor;
        private GameObject _threshold;
        private GameObject _spawn;
        private GameObject _player;

        [SetUp]
        public void SetUp()
        {
            _root =
                new GameObject(
                    "StoreShell");

            _floor =
                new GameObject(
                    "Floor");

            _threshold =
                new GameObject(
                    "Threshold");

            _spawn =
                new GameObject(
                    "Spawn");

            _player =
                new GameObject(
                    "Player");
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(
                _root);

            UnityEngine.Object.DestroyImmediate(
                _floor);

            UnityEngine.Object.DestroyImmediate(
                _threshold);

            UnityEngine.Object.DestroyImmediate(
                _spawn);

            UnityEngine.Object.DestroyImmediate(
                _player);
        }

        [Test]
        public void NewDescriptor_IsNotConfigured()
        {
            StoreShellDescriptor descriptor =
                _root.AddComponent<
                    StoreShellDescriptor>();

            Assert.That(
                descriptor.IsConfigured,
                Is.False);
        }

        [Test]
        public void Configure_StoresRequiredReferences()
        {
            StoreShellDescriptor descriptor =
                _root.AddComponent<
                    StoreShellDescriptor>();

            descriptor.Configure(
                StoreShellSpecification.InitialTier(),
                _floor.transform,
                _threshold.transform,
                _spawn.transform,
                _player.transform);

            Assert.That(
                descriptor.IsConfigured,
                Is.True);

            Assert.That(
                descriptor.WalkableFloor,
                Is.SameAs(_floor.transform));

            Assert.That(
                descriptor.EntranceThreshold,
                Is.SameAs(_threshold.transform));

            Assert.That(
                descriptor.PlayerSpawn,
                Is.SameAs(_spawn.transform));

            Assert.That(
                descriptor.TechnicalPlayer,
                Is.SameAs(_player.transform));
        }

        [Test]
        public void Configure_ExposesInitialTierSpecification()
        {
            StoreShellDescriptor descriptor =
                _root.AddComponent<
                    StoreShellDescriptor>();

            descriptor.Configure(
                StoreShellSpecification.InitialTier(),
                _floor.transform,
                _threshold.transform,
                _spawn.transform,
                _player.transform);

            StoreShellSpecification specification =
                descriptor.Specification;

            Assert.That(
                specification.WidthCells,
                Is.EqualTo(20));

            Assert.That(
                specification.DepthCells,
                Is.EqualTo(30));

            Assert.That(
                specification.EntranceWidthCells,
                Is.EqualTo(4));

            Assert.That(
                specification.CellSize,
                Is.EqualTo(0.5f));
        }
    }
}
