using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Store;
using VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.VerticalSlicePhase1
{
    public sealed class Sprint16Phase1RuntimePlayModeTests
    {
        [UnityTest]
        public IEnumerator RuntimeRoot_IsInstalled()
        {
            yield return null;

            Assert.That(
                Sprint16Phase1RuntimeRoot.Instance,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RequiredProjectAssets_Load()
        {
            yield return null;

            Phase1RuntimeAssetRegistryAsset registry =
                RequireRegistry();

            Assert.That(
                registry.Settings,
                Is.Not.Null);
            Assert.That(
                registry.ContentCatalog,
                Is.Not.Null);
            Assert.That(
                registry.StoreShell,
                Is.Not.Null);
            Assert.That(
                registry.MaterialPalette,
                Is.Not.Null);
            Assert.That(
                registry.PresentationCatalog,
                Is.Not.Null);
            Assert.That(
                registry.AudioCatalog,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator FurniturePrefab_BuildsBlockout()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                RequireRegistry();

            Assert.That(
                registry,
                Is.Not.Null);

            GameObject prefab =
                LoadEditorAsset<GameObject>(
                    "Assets/_Project/Prefabs/Furniture/CheckoutCounter.prefab");

            GameObject instance =
                Object.Instantiate(prefab);

            yield return null;

            Assert.That(
                instance.transform.childCount,
                Is.GreaterThan(0));

            Object.Destroy(instance);
        }

        [UnityTest]
        public IEnumerator ProductPrefab_BuildsMarker()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                RequireRegistry();

            Assert.That(
                registry,
                Is.Not.Null);

            GameObject prefab =
                LoadEditorAsset<GameObject>(
                    "Assets/_Project/Prefabs/Products/NeonDrift.prefab");

            GameObject instance =
                Object.Instantiate(prefab);

            yield return null;

            Assert.That(
                instance.GetComponentInChildren<
                    Phase1ProductVisualMarker>(),
                Is.Not.Null);

            Object.Destroy(instance);
        }

        [UnityTest]
        public IEnumerator CharacterPrefab_BuildsPresence()
        {
            GameObject prefab =
                LoadEditorAsset<GameObject>(
                    "Assets/_Project/Prefabs/Characters/Customer.prefab");

            GameObject instance =
                Object.Instantiate(prefab);

            yield return null;

            Phase1CharacterPresence presence =
                instance.GetComponent<
                    Phase1CharacterPresence>();

            Assert.That(presence, Is.Not.Null);
            Assert.That(
                presence.Role,
                Is.EqualTo(
                    Phase1CharacterRole.Customer));

            Object.Destroy(instance);
        }

        [UnityTest]
        public IEnumerator AudioRouter_StoresChannelVolume()
        {
            GameObject gameObject =
                new GameObject("AudioRouterTest");

            Phase1AudioRouter router =
                gameObject.AddComponent<
                    Phase1AudioRouter>();

            Phase1AudioCatalogAsset catalog =
                RequireRegistry().AudioCatalog;

            router.Configure(catalog);
            router.SetChannelVolume(
                Phase1AudioChannel.Music,
                0.35f);

            yield return null;

            Assert.That(
                router.GetChannelVolume(
                    Phase1AudioChannel.Music),
                Is.EqualTo(0.35f)
                    .Within(0.001f));

            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator AudioRouter_MissingEventIsSafe()
        {
            GameObject gameObject =
                new GameObject("AudioRouterTest");

            Phase1AudioRouter router =
                gameObject.AddComponent<
                    Phase1AudioRouter>();

            router.Configure(
                RequireRegistry().AudioCatalog);

            Assert.DoesNotThrow(
                () => router.Play(
                    "missing.event"));

            yield return null;
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator VfxPool_CreatesRequestedPool()
        {
            GameObject gameObject =
                new GameObject("VfxPoolTest");

            Phase1VfxPool pool =
                gameObject.AddComponent<
                    Phase1VfxPool>();
            pool.Configure(4);

            yield return null;

            Assert.That(
                gameObject.transform.childCount,
                Is.EqualTo(4));

            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator VfxPool_PlayIsSafe()
        {
            GameObject gameObject =
                new GameObject("VfxPoolTest");

            Phase1VfxPool pool =
                gameObject.AddComponent<
                    Phase1VfxPool>();
            pool.Configure(2);

            Assert.DoesNotThrow(
                () => pool.Play(
                    Vector3.zero,
                    Color.green));

            yield return null;
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator Door_OpensNearCharacter()
        {
            GameObject root =
                new GameObject("DoorTest");
            GameObject left =
                new GameObject("Left");
            GameObject right =
                new GameObject("Right");

            left.transform.SetParent(
                root.transform,
                false);
            right.transform.SetParent(
                root.transform,
                false);

            AutomaticSlidingDoorController door =
                root.AddComponent<
                    AutomaticSlidingDoorController>();

            door.Configure(
                left.transform,
                right.transform,
                1f,
                2f,
                10f);

            GameObject character =
                new GameObject("Character");
            character.AddComponent<
                Phase1CharacterPresence>()
                .Configure(
                    "customer",
                    Phase1CharacterRole.Customer);

            character.transform.position =
                root.transform.position;

            yield return null;
            yield return null;

            Assert.That(door.IsOpen, Is.True);

            Object.Destroy(character);
            Object.Destroy(root);
        }

        [UnityTest]
        public IEnumerator Door_ClosesWithoutCharacter()
        {
            GameObject root =
                new GameObject("DoorTest");
            GameObject left =
                new GameObject("Left");
            GameObject right =
                new GameObject("Right");

            left.transform.SetParent(
                root.transform,
                false);
            right.transform.SetParent(
                root.transform,
                false);

            AutomaticSlidingDoorController door =
                root.AddComponent<
                    AutomaticSlidingDoorController>();

            door.Configure(
                left.transform,
                right.transform,
                1f,
                0.5f,
                10f);

            yield return null;

            Assert.That(door.IsOpen, Is.False);

            Object.Destroy(root);
        }

        [UnityTest]
        public IEnumerator WallSetting_Persists()
        {
            PlayerPrefs.DeleteKey(
                Phase1WallOcclusionController
                    .PlayerPrefsKey);

            GameObject gameObject =
                new GameObject("WallOcclusionTest");
            GameObject target =
                new GameObject("Target");

            Phase1WallOcclusionController controller =
                gameObject.AddComponent<
                    Phase1WallOcclusionController>();

            controller.Configure(
                null,
                target.transform,
                true);
            controller.SetEnabled(false);

            yield return null;

            Assert.That(
                PlayerPrefs.GetInt(
                    Phase1WallOcclusionController
                        .PlayerPrefsKey),
                Is.EqualTo(0));

            Object.Destroy(target);
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator FurnitureVisualFactory_BuildsCheckout()
        {
            GameObject root =
                new GameObject("FurnitureVisual");

            var catalog =
                RequireRegistry().ContentCatalog
                    .BuildCatalog();

            catalog.TryGetFurniture(
                "checkout-counter",
                out var definition);

            Phase1BlockoutVisualFactory
                .BuildFurniture(
                    root,
                    definition,
                    null,
                    0.5f);

            yield return null;

            Assert.That(
                root.transform.childCount,
                Is.GreaterThanOrEqualTo(3));

            Object.Destroy(root);
        }

        [UnityTest]
        public IEnumerator FurnitureVisualFactory_BuildsShelf()
        {
            GameObject root =
                new GameObject("FurnitureVisual");

            var catalog =
                RequireRegistry().ContentCatalog
                    .BuildCatalog();

            catalog.TryGetFurniture(
                "central-shelf",
                out var definition);

            Phase1BlockoutVisualFactory
                .BuildFurniture(
                    root,
                    definition,
                    null,
                    0.5f);

            yield return null;

            Assert.That(
                root.transform.childCount,
                Is.GreaterThanOrEqualTo(5));

            Object.Destroy(root);
        }

        [UnityTest]
        public IEnumerator TechnicalScenario_Completes()
        {
            GameObject gameObject =
                new GameObject(
                    "Sprint16Phase1Scenario");

            Sprint16Phase1TechnicalScenarioRunner
                runner =
                    gameObject.AddComponent<
                        Sprint16Phase1TechnicalScenarioRunner>();

            runner.Configure(
                RequireRegistry().ContentCatalog,
                false);

            runner.RunScenario();

            yield return null;

            Assert.That(
                runner.LastScenarioPassed,
                Is.True);
            Assert.That(
                runner.LastCustomerSaleCompleted,
                Is.True);
            Assert.That(
                runner.LastEconomyIntegrated,
                Is.True);

            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator StoreScene_BuildsPhaseOneBlockout()
        {
            while (Sprint15RuntimeCompositionRoot
                       .Instance == null)
            {
                yield return null;
            }

            Sprint15RuntimeCompositionRoot s15 =
                Sprint15RuntimeCompositionRoot
                    .Instance;

            if (!s15.ActiveSession.HasActiveSession)
            {
                var snapshot =
                    new DefaultIntegratedGameStateFactory(
                        "EUR",
                        100000,
                        300)
                        .Create(
                            new SaveSlotId(0),
                            System.DateTime.UtcNow);

                s15.ActiveSession.Activate(
                    snapshot.SlotId,
                    snapshot);
            }

            AsyncOperation operation =
                SceneManager.LoadSceneAsync(
                    "Store",
                    LoadSceneMode.Single);

            while (!operation.isDone)
            {
                yield return null;
            }

            float timeout =
                Time.realtimeSinceStartup + 8f;

            while (GameObject.Find(
                       Phase1StoreBlockoutBuilder
                           .RootName) == null &&
                   Time.realtimeSinceStartup <
                       timeout)
            {
                yield return null;
            }

            Assert.That(
                GameObject.Find(
                    Phase1StoreBlockoutBuilder
                        .RootName),
                Is.Not.Null);
            Assert.That(
                Object.FindFirstObjectByType<
                    AutomaticSlidingDoorController>(),
                Is.Not.Null);
            Assert.That(
                GameObject.Find(
                    "S16_P1_OperationsCanvas"),
                Is.Not.Null);

            StoreShellDescriptor legacyShell =
                Object.FindFirstObjectByType<
                    StoreShellDescriptor>(
                        FindObjectsInactive.Include);

            Assert.That(
                legacyShell,
                Is.Not.Null);
            Assert.That(
                legacyShell.WalkableFloor
                    .GetComponent<Renderer>()
                    .enabled,
                Is.False,
                "The legacy S5 shell renderer must be hidden while the Sprint 16 shell is active.");

            GameObject openOperations =
                GameObject.Find(
                    "OpenOperations");

            Assert.That(
                openOperations,
                Is.Not.Null);

            openOperations
                .GetComponent<Button>()
                .onClick
                .Invoke();

            yield return null;
            Canvas.ForceUpdateCanvases();
            yield return new WaitForEndOfFrame();

            RectTransform operationsViewport =
                GameObject.Find(
                    "OperationsViewport")
                    ?.GetComponent<
                        RectTransform>();

            Assert.That(
                operationsViewport,
                Is.Not.Null);

            RectTransform body =
                operationsViewport.parent
                    as RectTransform;

            Assert.That(body, Is.Not.Null);
            Assert.That(
                Mathf.Abs(
                    operationsViewport.rect.width -
                    body.rect.width),
                Is.LessThan(2f));
            Assert.That(
                Mathf.Abs(
                    operationsViewport.rect.height -
                    body.rect.height),
                Is.LessThan(2f));
            Assert.That(
                operationsViewport.rect.width,
                Is.GreaterThan(100f));
            Assert.That(
                operationsViewport.rect.height,
                Is.GreaterThan(100f));

            FieldInfo serviceField =
                typeof(
                    Sprint16Phase1RuntimeRoot)
                    .GetField(
                        "_service",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);

            Assert.That(
                serviceField,
                Is.Not.Null);

            Phase1VerticalSliceService service =
                serviceField.GetValue(
                    Sprint16Phase1RuntimeRoot
                        .Instance)
                    as Phase1VerticalSliceService;

            Assert.That(service, Is.Not.Null);

            Phase1OperationResult orderShelf =
                service.OrderFurniture(
                    "central-shelf",
                    1);

            Assert.That(
                orderShelf.Succeeded,
                Is.True);

            string shelfOrderId =
                service.State.Orders[
                    service.State.Orders.Count - 1]
                    .OrderId;

            Assert.That(
                service.ReceiveOrder(
                    shelfOrderId)
                    .Succeeded,
                Is.True);
            Assert.That(
                service.GetFurnitureWarehouseQuantity(
                    "central-shelf"),
                Is.EqualTo(1));

            Phase1PlacementCompatibilityBridge
                bridge =
                    Object.FindFirstObjectByType<
                        Phase1PlacementCompatibilityBridge>();

            PlacementRuntimeController
                placementRuntime =
                    Object.FindFirstObjectByType<
                        PlacementRuntimeController>();

            Assert.That(bridge, Is.Not.Null);
            Assert.That(
                placementRuntime,
                Is.Not.Null);

            MethodInfo createPlacedView =
                typeof(
                    PlacementRuntimeController)
                    .GetMethod(
                        "CreatePlacedView",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);

            Assert.That(
                createPlacedView,
                Is.Not.Null);

            Assert.That(
                bridge.BeginPlacement(
                    "central-shelf")
                    .Succeeded,
                Is.True);

            PlacementInstanceId shelfId =
                new PlacementInstanceId(
                    "phase1-playmode-shelf");

            PlacedObjectRecord shelfRecord =
                new PlacedObjectRecord(
                    shelfId,
                    "central-shelf",
                    new GridCoordinate(8, 10),
                    GridRotation.Degrees0,
                    new GridSize(4, 2));

            Assert.That(
                placementRuntime.Registry
                    .TryPlace(shelfRecord)
                    .IsValid,
                Is.True);

            createPlacedView.Invoke(
                placementRuntime,
                new object[] {
                    shelfRecord
                });

            yield return null;
            yield return null;

            Assert.That(
                service.GetFurnitureWarehouseQuantity(
                    "central-shelf"),
                Is.EqualTo(0));
            Assert.That(
                service.State.Fixtures.Count,
                Is.EqualTo(1));
            Assert.That(
                service.State.Fixtures[0]
                    .DefinitionId,
                Is.EqualTo("central-shelf"));
            Assert.That(
                bridge.BeginPlacement(
                    "central-shelf")
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InsufficientStock));

            Phase1OperationResult orderCheckout =
                service.OrderFurniture(
                    "checkout-counter",
                    1);

            Assert.That(
                orderCheckout.Succeeded,
                Is.True);

            string checkoutOrderId =
                service.State.Orders[
                    service.State.Orders.Count - 1]
                    .OrderId;

            Assert.That(
                service.ReceiveOrder(
                    checkoutOrderId)
                    .Succeeded,
                Is.True);
            Assert.That(
                bridge.BeginPlacement(
                    "checkout-counter")
                    .Succeeded,
                Is.True);

            PlacementInstanceId checkoutId =
                new PlacementInstanceId(
                    "phase1-playmode-checkout");

            PlacedObjectRecord checkoutRecord =
                new PlacedObjectRecord(
                    checkoutId,
                    "checkout-counter",
                    new GridCoordinate(2, 20),
                    GridRotation.Degrees0,
                    new GridSize(4, 2));

            Assert.That(
                placementRuntime.Registry
                    .TryPlace(checkoutRecord)
                    .IsValid,
                Is.True);

            createPlacedView.Invoke(
                placementRuntime,
                new object[] {
                    checkoutRecord
                });

            yield return null;
            yield return null;

            Assert.That(
                service.GetFurnitureWarehouseQuantity(
                    "checkout-counter"),
                Is.EqualTo(0));
            Assert.That(
                service.State.Fixtures.Count,
                Is.EqualTo(2));

            GameObject.Find("TabDisplays")
                .GetComponent<Button>()
                .onClick
                .Invoke();

            yield return null;
            Canvas.ForceUpdateCanvases();

            Text[] displayTexts =
                GameObject.Find(
                    "OperationsWindow")
                    .GetComponentsInChildren<
                        Text>(true);

            string displayBody =
                string.Join(
                    "\n",
                    System.Array.ConvertAll(
                        displayTexts,
                        item => item.text));

            StringAssert.Contains(
                "Central Shelf",
                displayBody);
            StringAssert.Contains(
                "Checkout Counter",
                displayBody);
            StringAssert.Contains(
                "no product assignment",
                displayBody);

            Phase1CharacterLoopController
                characterLoop =
                    Object.FindFirstObjectByType<
                        Phase1CharacterLoopController>();

            Assert.That(
                characterLoop,
                Is.Not.Null);

            Phase1OperationResult closedAttempt =
                characterLoop
                    .TryServeNextCustomer();

            Assert.That(
                closedAttempt.Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .StoreMustBeOpen));
            Assert.That(
                characterLoop
                    .IsCustomerSequenceRunning,
                Is.False);

            Phase1OperationResult productOrder =
                service.OrderProduct(
                    "game-neon-drift",
                    1);

            Assert.That(
                productOrder.Succeeded,
                Is.True);

            string productOrderId =
                service.State.Orders[
                    service.State.Orders.Count - 1]
                    .OrderId;

            Assert.That(
                service.ReceiveOrder(
                    productOrderId)
                    .Succeeded,
                Is.True);
            Assert.That(
                service.AssignProduct(
                    shelfId.Value,
                    "game-neon-drift")
                    .Succeeded,
                Is.True);
            Assert.That(
                service.RestockDisplay(
                    shelfId.Value,
                    5)
                    .Succeeded,
                Is.True);
            Assert.That(
                service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.EqualTo(7));

            IntegratedGameStateSnapshot
                closedSnapshot =
                    s15.ActiveSession.Snapshot;

            s15.ActiveSession.Replace(
                WithDayState(
                    closedSnapshot,
                    "Open",
                    0,
                    System.DateTime.UtcNow));

            Assert.That(
                service.ValidateNextCustomerPurchase()
                    .Succeeded,
                Is.True);

            long cashBeforeSale =
                s15.ActiveSession
                    .Snapshot.CashCents;

            Phase1OperationResult serveResult =
                characterLoop
                    .TryServeNextCustomer();

            Assert.That(
                serveResult.Succeeded,
                Is.True);

            float customerTimeout =
                Time.realtimeSinceStartup + 15f;

            yield return null;

            while (characterLoop
                       .IsCustomerSequenceRunning &&
                   Time.realtimeSinceStartup <
                       customerTimeout)
            {
                yield return null;
            }

            Assert.That(
                characterLoop
                    .IsCustomerSequenceRunning,
                Is.False,
                "Customer sequence exceeded the PlayMode timeout.");
            Assert.That(
                characterLoop
                    .LastCustomerPurchaseResult,
                Is.Not.Null);
            Assert.That(
                characterLoop
                    .LastCustomerPurchaseResult
                    .Succeeded,
                Is.True,
                characterLoop
                    .LastCustomerPurchaseResult
                    .Detail);
            Assert.That(
                s15.ActiveSession
                    .Snapshot.CashCents,
                Is.EqualTo(
                    cashBeforeSale + 2999));
            Assert.That(
                service.State.CompletedSales,
                Is.EqualTo(1));
            Assert.That(
                service.State.Fixtures[0]
                    .ProductQuantity,
                Is.EqualTo(4));
            Assert.That(
                service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.EqualTo(7));

            Phase1FeedbackPresenter
                feedbackPresenter =
                    Object.FindFirstObjectByType<
                        Phase1FeedbackPresenter>();

            Assert.That(
                feedbackPresenter,
                Is.Not.Null);

            GameObject storeRuntime =
                GameObject.Find(
                    "S16_P1_Runtime");

            Assert.That(
                storeRuntime,
                Is.Not.Null);

            GameObject backWall =
                GameObject.Find(
                    "Wall_Back");

            Assert.That(
                backWall,
                Is.Not.Null);

            Phase1PlacedFixtureVisual[]
                fixtureVisuals =
                    Object.FindObjectsByType<
                        Phase1PlacedFixtureVisual>(
                            FindObjectsInactive
                                .Include,
                            FindObjectsSortMode
                                .None);

            Phase1PlacedFixtureVisual
                shelfVisual = null;

            foreach (Phase1PlacedFixtureVisual
                     fixtureVisual
                     in fixtureVisuals)
            {
                if (fixtureVisual.InstanceId ==
                    shelfId.Value)
                {
                    shelfVisual =
                        fixtureVisual;
                    break;
                }
            }

            Assert.That(
                shelfVisual,
                Is.Not.Null);

            Vector3 runtimeScaleBefore =
                storeRuntime.transform
                    .localScale;
            Vector3 wallLossyScaleBefore =
                backWall.transform
                    .lossyScale;
            Vector3 shelfScaleBefore =
                shelfVisual.transform
                    .localScale;

            Collider wallCollider =
                backWall.GetComponent<
                    Collider>();

            Assert.That(
                wallCollider,
                Is.Not.Null);

            Vector3 wallBoundsBefore =
                wallCollider.bounds.size;

            for (int feedbackIndex = 0;
                 feedbackIndex < 24;
                 feedbackIndex++)
            {
                feedbackPresenter.Present(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .Restocked,
                        "Repeated fixture feedback.",
                        shelfId.Value));

                feedbackPresenter.Present(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .Revenue,
                        "Repeated store feedback.",
                        "cash-hud"));
            }

            yield return null;
            yield return new WaitForSecondsRealtime(
                0.35f);

            Assert.That(
                (storeRuntime.transform
                     .localScale -
                 runtimeScaleBefore)
                    .sqrMagnitude,
                Is.LessThan(0.000001f),
                "Feedback must never scale the store runtime root.");

            Assert.That(
                (backWall.transform
                     .lossyScale -
                 wallLossyScaleBefore)
                    .sqrMagnitude,
                Is.LessThan(0.000001f),
                "Feedback must never scale store walls.");

            Assert.That(
                (wallCollider.bounds.size -
                 wallBoundsBefore)
                    .sqrMagnitude,
                Is.LessThan(0.000001f),
                "Feedback must never change wall collider bounds.");

            Assert.That(
                (shelfVisual.transform
                     .localScale -
                 shelfScaleBefore)
                    .sqrMagnitude,
                Is.LessThan(0.000001f),
                "Repeated restock feedback must never scale the fixture.");

            GameObject transientAnchor =
                new GameObject(
                    "Phase1TransientFeedbackAnchor");

            feedbackPresenter.RegisterAnchor(
                "phase1-transient-anchor",
                transientAnchor.transform);

            feedbackPresenter.Present(
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind
                        .ObjectSelected,
                    "Transient pulse.",
                    "phase1-transient-anchor"));

            Object.Destroy(transientAnchor);

            yield return null;
            yield return new WaitForSecondsRealtime(
                0.3f);

            LogAssert.NoUnexpectedReceived();

            AsyncOperation cleanup =
                SceneManager.LoadSceneAsync(
                    "TestLab",
                    LoadSceneMode.Single);

            while (!cleanup.isDone)
            {
                yield return null;
            }
        }

        private static
            Phase1RuntimeAssetRegistryAsset
            RequireRegistry()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                Phase1RuntimeAssetRegistryAsset
                    .FindLoaded();

            if (registry == null)
            {
                registry =
                    LoadEditorAsset<
                        Phase1RuntimeAssetRegistryAsset>(
                            "Assets/_Project/Settings/" +
                            "Runtime/" +
                            "RuntimeAssetRegistry.asset");
            }

            Assert.That(
                registry,
                Is.Not.Null,
                "RuntimeAssetRegistry could not be resolved.");

            return registry;
        }

        private static T LoadEditorAsset<T>(
            string assetPath)
            where T : UnityEngine.Object
        {
            System.Type assetDatabaseType = null;

            foreach (Assembly assembly
                     in System.AppDomain.CurrentDomain
                         .GetAssemblies())
            {
                assetDatabaseType =
                    assembly.GetType(
                        "UnityEditor.AssetDatabase");

                if (assetDatabaseType != null)
                {
                    break;
                }
            }

            Assert.That(
                assetDatabaseType,
                Is.Not.Null,
                "UnityEditor.AssetDatabase is unavailable.");

            MethodInfo loadMethod =
                assetDatabaseType.GetMethod(
                    "LoadAssetAtPath",
                    BindingFlags.Public |
                    BindingFlags.Static,
                    null,
                    new[]
                    {
                        typeof(string),
                        typeof(System.Type)
                    },
                    null);

            Assert.That(
                loadMethod,
                Is.Not.Null,
                "AssetDatabase.LoadAssetAtPath was not found.");

            T asset =
                loadMethod.Invoke(
                    null,
                    new object[]
                    {
                        assetPath,
                        typeof(T)
                    }) as T;

            Assert.That(
                asset,
                Is.Not.Null,
                "Asset not found: " +
                assetPath);

            return asset;
        }

        private static IntegratedGameStateSnapshot
            WithDayState(
                IntegratedGameStateSnapshot source,
                string state,
                int elapsedSeconds,
                System.DateTime updatedUtc)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                updatedUtc,
                source.CurrentDay,
                source.CashCents,
                source.CurrencyCode,
                source.Inventories,
                source.SupplierOrders,
                source.Displays,
                source.Customers,
                source.ShoppingSessions,
                source.Reservations,
                source.QueueEntries,
                new CheckoutStationSaveRecord(
                    source.CheckoutStation
                        .StationId,
                    state == "Closed"
                        ? "Closed"
                        : "Available",
                    string.Empty),
                source.Transactions,
                new DayCycleSaveRecord(
                    source.DayCycle.DayId,
                    state,
                    source.DayCycle
                        .OpenDurationSeconds,
                    elapsedSeconds,
                    source.DayCycle
                        .AutoBeginClosing),
                source.LedgerEntries);
        }

        [UnityTest]
        public IEnumerator StoreScene_HasWarmLightingRig()
        {
            while (Sprint15RuntimeCompositionRoot
                       .Instance == null)
            {
                yield return null;
            }

            Sprint15RuntimeCompositionRoot s15 =
                Sprint15RuntimeCompositionRoot
                    .Instance;

            if (!s15.ActiveSession.HasActiveSession)
            {
                var snapshot =
                    new DefaultIntegratedGameStateFactory(
                        "EUR",
                        100000,
                        300)
                        .Create(
                            new SaveSlotId(0),
                            System.DateTime.UtcNow);

                s15.ActiveSession.Activate(
                    snapshot.SlotId,
                    snapshot);
            }

            yield return SceneManager.LoadSceneAsync(
                "Store",
                LoadSceneMode.Single);

            float timeout =
                Time.realtimeSinceStartup + 8f;

            while (Object.FindObjectsByType<
                       Light>(
                           FindObjectsInactive.Include,
                           FindObjectsSortMode.None)
                       .Length < 5 &&
                   Time.realtimeSinceStartup <
                       timeout)
            {
                yield return null;
            }

            Assert.That(
                Object.FindObjectsByType<
                    Light>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None)
                    .Length,
                Is.GreaterThanOrEqualTo(5));

            yield return SceneManager.LoadSceneAsync(
                "TestLab",
                LoadSceneMode.Single);
        }
    }
}
