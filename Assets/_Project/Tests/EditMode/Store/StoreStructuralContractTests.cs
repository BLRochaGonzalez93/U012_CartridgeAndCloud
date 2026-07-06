using NUnit.Framework;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Navigation;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class StoreStructuralContractTests
    {
        [Test]
        public void Environment_OwnsSingleAuthoredNavigationSurface()
        {
            GameObject environment = Load<GameObject>(
                "Assets/_Project/Prefabs/Store/StoreInitialEnvironment.prefab");
            Assert.That(environment, Is.Not.Null);

            StoreNavigationAuthoring authoring =
                environment.GetComponent<StoreNavigationAuthoring>();
            Assert.That(authoring, Is.Not.Null);
            Assert.That(authoring.TryValidate(out string report), Is.True, report);

            NavMeshSurface[] surfaces =
                environment.GetComponentsInChildren<NavMeshSurface>(true);
            Assert.That(surfaces, Has.Length.EqualTo(1));
            Assert.That(surfaces[0].collectObjects, Is.EqualTo(CollectObjects.Children));
            Assert.That(
                surfaces[0].useGeometry,
                Is.EqualTo(NavMeshCollectGeometry.PhysicsColliders));

            Assert.That(authoring.BlockingSources, Has.Length.EqualTo(7));
            foreach (BoxCollider blocker in authoring.BlockingSources)
            {
                Assert.That(blocker, Is.Not.Null);
                Assert.That(
                    blocker.transform.IsChildOf(authoring.NavigationRoot),
                    Is.True,
                    blocker.name);
            }

            Transform collision = environment.transform.Find("Collision");
            Assert.That(collision, Is.Not.Null);
            Assert.That(
                collision.GetComponentsInChildren<BoxCollider>(true),
                Has.Length.EqualTo(7));
        }


        [Test]
        public void ActorPrefabs_OwnSingleRootNavigationAgent()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:Prefab",
                new[] { "Assets/_Project/Resources/Characters" });
            Assert.That(guids, Has.Length.EqualTo(7));

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = Load<GameObject>(path);
                NavMeshAgent[] agents =
                    prefab.GetComponentsInChildren<NavMeshAgent>(true);

                Assert.That(agents, Has.Length.EqualTo(1), prefab.name);
                Assert.That(
                    agents[0].gameObject,
                    Is.SameAs(prefab),
                    prefab.name + " must keep navigation on the technical root.");

                if (path.Contains("/Player/"))
                {
                    Assert.That(
                        agents[0].enabled,
                        Is.False,
                        "The visual player prefab must not register an agent before the scene NavMesh exists.");
                }
            }
        }

        [Test]
        public void Furniture_PrefabsOwnCollisionNavigationAndAnchors()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:Prefab",
                new[] { "Assets/_Project/Prefabs/Furniture" });
            Assert.That(guids, Has.Length.EqualTo(8));

            foreach (string guid in guids)
            {
                GameObject prefab = Load<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                Transform collision = prefab.transform.Find("Collision");
                Assert.That(collision, Is.Not.Null, prefab.name);
                Assert.That(prefab.transform.Find("Anchors"), Is.Not.Null, prefab.name);
                Assert.That(collision.GetComponent<BoxCollider>(), Is.Not.Null, prefab.name);
                Assert.That(collision.GetComponent<NavMeshObstacle>(), Is.Not.Null, prefab.name);
                Assert.That(
                    collision.GetComponent<StoreFixtureNavigationObstacle>(),
                    Is.Not.Null,
                    prefab.name);
            }
        }

        [Test]
        public void AutomaticDoor_IsSelfContained()
        {
            GameObject prefab = Load<GameObject>(
                "Assets/_Project/Prefabs/Architecture/Modular/AutomaticDoor.prefab");
            Assert.That(prefab.GetComponent<AutomaticDoorParts>(), Is.Not.Null);
            Assert.That(prefab.GetComponent<AutomaticSlidingDoorController>(), Is.Not.Null);
            Assert.That(
                prefab.GetComponentInChildren<AutomaticDoorSensor>(true),
                Is.Not.Null);
            Assert.That(prefab.transform.Find("Anchors"), Is.Not.Null);
            Assert.That(
                prefab.GetComponentsInChildren<Collider>(true).Length,
                Is.GreaterThanOrEqualTo(3));
        }

        [Test]
        public void ProductAndFutureContent_RolesAreExplicit()
        {
            AssertRoleCount(
                "Assets/_Project/Prefabs/Products",
                PrefabPhysicalContractAuthoring.PhysicalRole.DisplayOnly,
                10);
            AssertRoleCount(
                "Assets/_Project/Prefabs/Expansions",
                PrefabPhysicalContractAuthoring.PhysicalRole.FutureContent,
                19);

            StoreVisualPrefabCatalogAsset catalog = Load<StoreVisualPrefabCatalogAsset>(
                "Assets/_Project/Data/Catalogs/RepresentativePrefabCatalog.asset");
            Assert.That(catalog.Expansions, Is.Empty);
        }

        [Test]
        public void DeliveryRegistry_IsIdempotentPerOrder()
        {
            DeliveryPresentationRegistry registry =
                new DeliveryPresentationRegistry();
            Assert.That(registry.TryBegin("order-001"), Is.True);
            Assert.That(registry.TryBegin("order-001"), Is.False);
            Assert.That(registry.ActiveCount, Is.EqualTo(1));

            registry.Complete("order-001");
            Assert.That(registry.ActiveCount, Is.Zero);
            Assert.That(registry.TryBegin("order-001"), Is.False);
        }

        [Test]
        public void PresentationCatalog_OwnsDeliveryView()
        {
            StorePresentationCatalogAsset catalog = Load<StorePresentationCatalogAsset>(
                "Assets/_Project/Data/Catalogs/PresentationCatalog.asset");
            Assert.That(catalog.SupplierDeliveryViewPrefab, Is.Not.Null);
            Assert.That(
                catalog.SupplierDeliveryViewPrefab.GetComponent<SupplierDeliveryView>(),
                Is.Not.Null);
        }

        private static void AssertRoleCount(
            string folder,
            PrefabPhysicalContractAuthoring.PhysicalRole role,
            int expected)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folder });
            Assert.That(guids, Has.Length.EqualTo(expected));
            foreach (string guid in guids)
            {
                GameObject prefab = Load<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                PrefabPhysicalContractAuthoring contract =
                    prefab.GetComponent<PrefabPhysicalContractAuthoring>();
                Assert.That(contract, Is.Not.Null, prefab.name);
                Assert.That(contract.Role, Is.EqualTo(role), prefab.name);
                Assert.That(contract.TryValidate(out string report), Is.True, report);
            }
        }

        private static T Load<T>(string path)
            where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
