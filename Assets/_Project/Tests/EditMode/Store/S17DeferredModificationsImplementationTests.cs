using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;
using VRMGames.CartridgeAndCloud.Runtime.Characters;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class S17DeferredModificationsImplementationTests
    {
        [Test]
        public void S17_MOD_002_VisitFallbackIsAllowedWithoutVisibleProduct()
        {
            Assert.That(
                CustomerVisitFallbackPolicy.ShouldBrowseWithoutPurchase(
                    hasVisibleProduct: false,
                    admissionAllowed: true),
                Is.True);
            Assert.That(
                CustomerVisitFallbackPolicy.ShouldBrowseWithoutPurchase(
                    hasVisibleProduct: true,
                    admissionAllowed: true),
                Is.False);
        }

        [Test]
        public void S17_MOD_003_004_ProcessAllPublishesOneRunAndDefersEveryMutation()
        {
            string directory = StoreOperationsTestFactory.TempDirectory();
            try
            {
                StoreContentCatalog catalog = StoreOperationsTestFactory.Catalog();
                ActiveGameSessionService active = StoreOperationsTestFactory.ActiveSession();
                StoreOperationsFacade service = new StoreOperationsFacade(
                    catalog,
                    new JsonStoreOperationsStateRepository(directory),
                    active,
                    new StoreOperationsTestFactory.FixedClock(
                        StoreOperationsTestFactory.Utc()));
                service.InitializeForActiveSlot();

                service.OrderFurniture("central-shelf", 1);
                service.OrderProduct("game-neon-drift", 1);
                long cashBefore = active.Snapshot.CashCents;
                List<GameplayFeedbackEvent> feedback =
                    new List<GameplayFeedbackEvent>();
                service.FeedbackRaised += feedback.Add;

                StoreOperationResult dispatch =
                    service.ProcessAllPendingOrders();

                Assert.That(dispatch.Succeeded, Is.True);
                Assert.That(service.State.DeliveryRuns.Count, Is.EqualTo(1));
                Assert.That(service.State.DeliveryRuns[0].Status,
                    Is.EqualTo(StoreDeliveryRunStatus.InTransit));
                Assert.That(service.State.Orders[0].State,
                    Is.EqualTo(StoreOrderStatus.InTransit));
                Assert.That(service.State.Orders[1].State,
                    Is.EqualTo(StoreOrderStatus.InTransit));
                Assert.That(active.Snapshot.CashCents, Is.EqualTo(cashBefore));
                Assert.That(active.Snapshot.LedgerEntries.Count, Is.EqualTo(0));
                Assert.That(service.GetFurnitureWarehouseQuantity("central-shelf"), Is.Zero);
                Assert.That(service.GetProductWarehouseQuantity("game-neon-drift"), Is.Zero);
                Assert.That(
                    feedback.FindAll(item =>
                        item.Kind == GameplayFeedbackType.DeliveryRunStarted).Count,
                    Is.EqualTo(1));

                string runId = service.State.DeliveryRuns[0].DeliveryRunId;
                Assert.That(service.CompleteDeliveryRun(runId).Succeeded, Is.True);
                Assert.That(service.CompleteDeliveryRun(runId).Succeeded, Is.True);
                Assert.That(service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(1));
                Assert.That(service.GetProductWarehouseQuantity("game-neon-drift"), Is.EqualTo(12));
                Assert.That(active.Snapshot.LedgerEntries.Count, Is.EqualTo(2));
                Assert.That(active.Snapshot.CashCents, Is.EqualTo(56000));
            }
            finally
            {
                StoreOperationsTestFactory.DeleteDirectory(directory);
            }
        }

        [Test]
        public void S17_MOD_005_AllAuthoredLodGroupsKeepTheirLastLodVisible()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:Prefab",
                new[] { "Assets/_Project/Prefabs" });
            int validated = 0;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    continue;
                }

                foreach (LODGroup group in prefab.GetComponentsInChildren<LODGroup>(true))
                {
                    LOD[] levels = group.GetLODs();
                    Assert.That(levels.Length, Is.GreaterThan(0), path);
                    Assert.That(
                        levels[levels.Length - 1].screenRelativeTransitionHeight,
                        Is.EqualTo(0f).Within(0.000001f),
                        path + " must keep its final LOD visible at every gameplay zoom.");
                    validated++;
                }
            }

            Assert.That(validated, Is.GreaterThanOrEqualTo(48));

            FieldInfo crateScale = typeof(SupplierDeliveryPresenter).GetField(
                "DeliveryCrateScale",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(crateScale, Is.Not.Null);
            Assert.That((float)crateScale.GetRawConstantValue(), Is.LessThanOrEqualTo(0.4f));
        }

        [TestCase(0, "00:00")]
        [TestCase(100, "08:00")]
        [TestCase(275, "22:00")]
        [TestCase(300, "24:00")]
        public void S17_MOD_006_VirtualClockUsesTwentyFourHourFormat(
            int elapsed,
            string expected)
        {
            Assert.That(
                StoreTradingHoursPolicy.FormatTime(elapsed, 300),
                Is.EqualTo(expected));
        }

        [Test]
        public void S17_MOD_007_008_ClockAndTradingWindowAreIndependentFromStoreState()
        {
            SimulationClock clock = new SimulationClock();
            clock.Synchronize(300, 0, SimulationSpeedPolicy.Normal);
            Assert.That(clock.Tick(1f, false).CurrentElapsedSeconds, Is.EqualTo(1));
            Assert.That(StoreTradingHoursPolicy.CanOpen(99, 300), Is.False);
            Assert.That(StoreTradingHoursPolicy.CanOpen(100, 300), Is.True);
            Assert.That(StoreTradingHoursPolicy.CanOpen(274, 300), Is.True);
            Assert.That(StoreTradingHoursPolicy.CanOpen(275, 300), Is.False);
            Assert.That(StoreTradingHoursPolicy.IsDayComplete(300, 300), Is.True);

            Assert.That(
                StoreTradingHoursPolicy.CanTransition("BeforeOpen", "Open", 100, 300),
                Is.True);
            Assert.That(
                StoreTradingHoursPolicy.CanTransition("Open", "Closing", 140, 300),
                Is.True);
            Assert.That(
                StoreTradingHoursPolicy.CanTransition("Closing", "Closed", 141, 300),
                Is.True);
            Assert.That(
                StoreTradingHoursPolicy.CanTransition("Closed", "Open", 160, 300),
                Is.True);
            Assert.That(
                StoreTradingHoursPolicy.CanTransition("Closed", "Open", 275, 300),
                Is.False);
        }

        [Test]
        public void S17_MOD_009_NestedGroundAnchorUsesWorldSpaceAndRootBasePlane()
        {
            GameObject root = new GameObject("Placed");
            GameObject authored = new GameObject("Authored");
            GameObject anchor = new GameObject("GroundAnchor");
            try
            {
                root.transform.position = new Vector3(0f, 5f, 0f);
                root.transform.localScale = new Vector3(1f, 2f, 1f);
                authored.transform.SetParent(root.transform, false);
                anchor.transform.SetParent(authored.transform, false);
                anchor.transform.localPosition = new Vector3(0f, -0.5f, 0f);

                Assert.That(
                    GroundingUtility.ResolveBasePlaneHeight(root.transform),
                    Is.EqualTo(4f).Within(0.0001f));
                Assert.That(
                    GroundingUtility.AlignRootToGroundAnchor(root.transform, 0f),
                    Is.True);
                Assert.That(anchor.transform.position.y, Is.EqualTo(0f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void S17_MOD_010_CheckoutOwnsStaffPointAndLookTargetAndRotationIsHorizontal()
        {
            GameObject checkout = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/_Project/Prefabs/Furniture/CheckoutCounter.prefab");
            Assert.That(checkout, Is.Not.Null);
            Assert.That(Find(checkout.transform, "StaffPoint"), Is.Not.Null);
            Assert.That(Find(checkout.transform, "StaffLookTarget"), Is.Not.Null);

            Assert.That(
                StaffOrientationUtility.TryResolveHorizontalRotation(
                    Vector3.zero,
                    new Vector3(1f, 4f, 0f),
                    out Quaternion rotation),
                Is.True);
            Vector3 forward = rotation * Vector3.forward;
            Assert.That(forward.x, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(forward.y, Is.EqualTo(0f).Within(0.0001f));
        }

        private static Transform Find(Transform root, string name)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == name)
                {
                    return child;
                }
            }

            return null;
        }
    }
}
