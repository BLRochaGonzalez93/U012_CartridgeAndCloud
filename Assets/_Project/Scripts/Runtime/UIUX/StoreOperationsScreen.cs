using System;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

using VRMGames.CartridgeAndCloud.Application.Audio;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Runtime.Audio;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Composition;
using VRMGames.CartridgeAndCloud.Runtime.Development.Blockout;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    public sealed class StoreOperationsScreen :
        MonoBehaviour
    {
        private enum Tab
        {
            Procedure,
            Shop,
            Deliveries,
            Warehouse,
            Displays,
            Customers,
            Settings
        }

        private StoreOperationsFacade
            _service;
        private IStoreContentCatalog _catalog;
        private StoreOpeningProcedure
            _procedure;
        private StorePlacementCoordinator
            _placement;
        private StoreCharacterLoopController
            _characters;
        private StoreBlockoutBuilder
            _blockout;
        private StoreAudioRouter _audio;

        private Canvas _canvas;
        private GameObject _window;
        private RectTransform _content;
        private Button _openButton;
        private Tab _tab;
        private bool _autosaveCompleted;

        public void Configure(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            StoreOpeningProcedure procedure,
            StorePlacementCoordinator placement,
            StoreCharacterLoopController characters,
            StoreBlockoutBuilder blockout,
            StoreAudioRouter audio)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _procedure = procedure ??
                throw new ArgumentNullException(
                    nameof(procedure));
            _placement = placement ??
                throw new ArgumentNullException(
                    nameof(placement));
            _characters = characters ??
                throw new ArgumentNullException(
                    nameof(characters));
            _blockout = blockout ??
                throw new ArgumentNullException(
                    nameof(blockout));
            _audio = audio ??
                throw new ArgumentNullException(
                    nameof(audio));

            _service.StateChanged +=
                HandleStateChanged;

            BuildCanvas();
        }

        public void SetAutosaveCompleted(
            bool completed)
        {
            _autosaveCompleted = completed;
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

        private void BuildCanvas()
        {
            _canvas =
                ProceduralUiFactory
                    .CreateCanvas(
                        "S16_P1_OperationsCanvas",
                        4550);

            _canvas.transform.SetParent(
                transform,
                false);

            _openButton =
                ProceduralUiFactory.Button(
                    _canvas.transform,
                    "OpenOperations",
                    "Vertical Slice Operations",
                    Open,
                    18);

            RectTransform openRect =
                _openButton.GetComponent<
                    RectTransform>();
            openRect.anchorMin =
                new Vector2(0.76f, 0.02f);
            openRect.anchorMax =
                new Vector2(0.98f, 0.075f);
            openRect.offsetMin = Vector2.zero;
            openRect.offsetMax = Vector2.zero;
        }

        private void Open()
        {
            if (_window != null)
            {
                return;
            }

            UIRuntimeCompositionRoot
                .Instance?.InputGate
                .EnterUiExclusive();

            _window =
                new GameObject(
                    "OperationsWindow",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform rect =
                _window.GetComponent<
                    RectTransform>();
            rect.SetParent(
                _canvas.transform,
                false);
            rect.anchorMin =
                new Vector2(0.54f, 0.08f);
            rect.anchorMax =
                new Vector2(0.985f, 0.90f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            _window.GetComponent<Image>()
                .color =
                    new Color(
                        0.018f,
                        0.055f,
                        0.04f,
                        0.985f);

            ProceduralUiFactory
                .AddVerticalLayout(
                    _window,
                    8f,
                    new RectOffset(
                        14,
                        14,
                        12,
                        12),
                    false);

            Text title =
                ProceduralUiFactory.Text(
                    rect,
                    "Title",
                    "SPRINT 16 · PLAYABLE BLOCKOUT",
                    25,
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            title.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 42f;

            RectTransform tabs =
                ProceduralUiFactory.Panel(
                    rect,
                    "Tabs",
                    Color.clear);
            tabs.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 48f;

            ProceduralUiFactory
                .AddHorizontalLayout(
                    tabs.gameObject,
                    5f,
                    new RectOffset(
                        0,
                        0,
                        2,
                        2));

            AddTab(
                tabs,
                Tab.Procedure,
                "Guide");
            AddTab(tabs, Tab.Shop, "Shop");
            AddTab(
                tabs,
                Tab.Deliveries,
                "Delivery");
            AddTab(
                tabs,
                Tab.Warehouse,
                "Stock");
            AddTab(
                tabs,
                Tab.Displays,
                "Displays");
            AddTab(
                tabs,
                Tab.Customers,
                "Customer");
            AddTab(
                tabs,
                Tab.Settings,
                "Settings");

            RectTransform viewport =
                ProceduralUiFactory.Panel(
                    rect,
                    "Body",
                    Color.clear);
            LayoutElement bodyLayout =
                viewport.gameObject.AddComponent<
                    LayoutElement>();
            bodyLayout.flexibleHeight = 1f;
            bodyLayout.minHeight = 280f;

            _content =
                ProceduralUiFactory
                    .ScrollContent(
                        viewport,
                        "Operations");

            Button close =
                ProceduralUiFactory.Button(
                    rect,
                    "Close",
                    "Close",
                    Close,
                    18);
            close.gameObject
                .GetComponent<LayoutElement>()
                .preferredHeight = 48f;

            RebuildContent();
            ProceduralUiFactory.Select(close);
        }

        private void AddTab(
            Transform parent,
            Tab tab,
            string label)
        {
            ProceduralUiFactory.Button(
                parent,
                "Tab" + tab,
                label,
                () =>
                {
                    _tab = tab;
                    RebuildContent();
                },
                14);
        }

        private void RebuildContent()
        {
            if (_content == null)
            {
                return;
            }

            for (int index =
                     _content.childCount - 1;
                 index >= 0;
                 index--)
            {
                Destroy(
                    _content.GetChild(index)
                        .gameObject);
            }

            switch (_tab)
            {
                case Tab.Procedure:
                    BuildProcedure();
                    break;
                case Tab.Shop:
                    BuildShop();
                    break;
                case Tab.Deliveries:
                    BuildDeliveries();
                    break;
                case Tab.Warehouse:
                    BuildWarehouse();
                    break;
                case Tab.Displays:
                    BuildDisplays();
                    break;
                case Tab.Customers:
                    BuildCustomers();
                    break;
                case Tab.Settings:
                    BuildSettings();
                    break;
            }
        }

        private void BuildProcedure()
        {
            StoreOpeningProcedureStatus status =
                _procedure.Evaluate(
                    _service.State,
                    UIRuntimeCompositionRoot
                        .Instance.ActiveSession
                        .Snapshot,
                    _autosaveCompleted);

            AddHeading(
                status.Title);

            AddParagraph(
                status.Instruction);

            AddParagraph(
                "Current step: " +
                status.Step);

            AddParagraph(
                $"Cash: " +
                StoreUiProjectionService
                    .FormatMoney(
                        UIRuntimeCompositionRoot
                            .Instance.ActiveSession
                            .Snapshot.CashCents,
                        UIRuntimeCompositionRoot
                            .Instance.ActiveSession
                            .Snapshot.CurrencyCode));

            AddParagraph(
                $"Completed sales: " +
                $"{_service.State.CompletedSales}");

            AddParagraph(
                "Phase 2 remains blocked until this guide reaches Completed and the full QA gate passes.");
        }

        private void BuildShop()
        {
            AddHeading(
                "Furniture catalog");

            foreach (StoreFixtureDefinition
                     item in _catalog.Furniture)
            {
                if (!item.IsPurchasable)
                {
                    continue;
                }

                AddAction(
                    $"{item.DisplayName} · " +
                    $"{item.WidthCells}×" +
                    $"{item.DepthCells} · " +
                    StoreUiProjectionService
                        .FormatMoney(
                            item.UnitCostCents,
                            UIRuntimeCompositionRoot
                                .Instance.ActiveSession
                                .Snapshot.CurrencyCode),
                    "Order",
                    () => Execute(
                        _service.OrderFurniture(
                            item.DefinitionId,
                            1)));
            }

            AddHeading(
                "Merchandise catalog");

            foreach (RetailProductDefinition
                     item in _catalog.Products)
            {
                AddAction(
                    $"{item.DisplayName} · case " +
                    $"{item.UnitsPerCase} · " +
                    StoreUiProjectionService
                        .FormatMoney(
                            item.WholesalePriceCents *
                            item.UnitsPerCase,
                            UIRuntimeCompositionRoot
                                .Instance.ActiveSession
                                .Snapshot.CurrencyCode),
                    "Order case",
                    () => Execute(
                        _service.OrderProduct(
                            item.ProductId,
                            1)));
            }
        }

        private void BuildDeliveries()
        {
            AddHeading("Pending deliveries");

            bool any = false;

            foreach (StoreOrderRecord order
                     in _service.State.Orders)
            {
                if (order.State !=
                    StoreOrderStatus.Ordered)
                {
                    continue;
                }

                any = true;

                AddAction(
                    $"{order.OrderId}\n" +
                    $"{order.ItemId} × " +
                    $"{order.OrderedUnits}",
                    "Receive & pay",
                    () =>
                    {
                        StoreOperationResult result =
                            _service.ReceiveOrder(
                                order.OrderId);

                        if (result.Succeeded)
                        {
                            _characters
                                .PresentSupplierDelivery();
                        }

                        Execute(result);
                    });
            }

            if (!any)
            {
                AddParagraph(
                    "No pending deliveries.");
            }
        }

        private void BuildWarehouse()
        {
            AddHeading(
                "Furniture warehouse");

            if (_service.State
                    .FurnitureWarehouse.Count == 0)
            {
                AddParagraph(
                    "No received furniture.");
            }

            foreach (StoreStockRecord stock
                     in _service.State
                         .FurnitureWarehouse)
            {
                AddAction(
                    $"{stock.ItemId} · " +
                    $"{stock.Quantity} available",
                    "Place",
                    () =>
                    {
                        Close();
                        Execute(
                            _placement.BeginPlacement(
                                stock.ItemId));
                    });
            }

            AddHeading(
                "Product warehouse");

            if (_service.State
                    .ProductWarehouse.Count == 0)
            {
                AddParagraph(
                    "No merchandise in backroom.");
            }

            foreach (StoreStockRecord stock
                     in _service.State
                         .ProductWarehouse)
            {
                AddParagraph(
                    $"{stock.ItemId}: " +
                    $"{stock.Quantity} units");
            }
        }

        private void BuildDisplays()
        {
            AddHeading(
                "Placed fixtures");

            bool anyFixture = false;
            bool anyProductDisplay = false;

            foreach (PlacedStoreFixtureRecord
                     fixture in _service.State.Fixtures)
            {
                if (!_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            furniture))
                {
                    continue;
                }

                anyFixture = true;

                if (!furniture.SupportsProducts)
                {
                    AddParagraph(
                        fixture.InstanceId + "\n" +
                        furniture.DisplayName + "\n" +
                        "Operational fixture · " +
                        "no product assignment");
                    continue;
                }

                anyProductDisplay = true;

                string assignedProductName =
                    string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId)
                        ? "unassigned"
                        : fixture.AssignedProductId;

                AddParagraph(
                    fixture.InstanceId + "\n" +
                    furniture.DisplayName + "\n" +
                    "Product: " +
                    assignedProductName + "\n" +
                    "Stock: " +
                    fixture.ProductQuantity + "/" +
                    furniture.Capacity);

                if (string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId))
                {
                    foreach (RetailProductDefinition
                             product in _catalog.Products)
                    {
                        RetailProductDefinition
                            captured = product;

                        int backroomStock =
                            _service
                                .GetProductWarehouseQuantity(
                                    captured.ProductId);

                        AddAction(
                            "Assign " +
                            captured.DisplayName +
                            " · backroom " +
                            backroomStock,
                            "Assign",
                            () => Execute(
                                _service.AssignProduct(
                                    fixture.InstanceId,
                                    captured.ProductId)));
                    }
                }
                else
                {
                    AddAction(
                        "Transfer from backroom",
                        "Restock 1",
                        () => Execute(
                            _service.RestockDisplay(
                                fixture.InstanceId,
                                1)));

                    AddAction(
                        "Transfer from backroom",
                        "Restock 5",
                        () => Execute(
                            _service.RestockDisplay(
                                fixture.InstanceId,
                                5)));
                }
            }

            if (!anyFixture)
            {
                AddParagraph(
                    "Place a fixture first.");
                return;
            }

            if (!anyProductDisplay)
            {
                AddParagraph(
                    "Place a product display to assign merchandise.");
            }
        }

        private void BuildCustomers()
        {
            AddHeading(
                "Playable customer loop");

            AddParagraph(
                "A blockout customer walks through entrance, evaluates a stocked product, picks it, queues, completes checkout and exits.");

            Button button =
                AddAction(
                    "Serve the next available customer",
                    "Spawn customer",
                    () => Execute(
                        _characters
                            .TryServeNextCustomer()));

            button.interactable =
                !_characters
                    .IsCustomerSequenceRunning;
        }

        private void BuildSettings()
        {
            AddHeading(
                "Visibility");

            bool hideWalls =
                _blockout.WallOcclusion != null &&
                _blockout.WallOcclusion
                    .HideOccludingWalls;

            AddAction(
                "Walls between camera and player",
                hideWalls
                    ? "Hide: ON"
                    : "Hide: OFF",
                () =>
                {
                    _blockout.WallOcclusion
                        ?.SetEnabled(
                            !hideWalls);
                    RebuildContent();
                });

            AddHeading(
                "Audio channels");

            foreach (AudioChannel channel
                     in Enum.GetValues(
                         typeof(AudioChannel)))
            {
                AudioChannel captured =
                    channel;

                AddAction(
                    captured + ": " +
                    Mathf.RoundToInt(
                        _audio.GetChannelVolume(
                            captured) * 100f) +
                    "%",
                    "Cycle",
                    () =>
                    {
                        float current =
                            _audio.GetChannelVolume(
                                captured);
                        float next =
                            current >= 0.99f
                                ? 0f
                                : current >= 0.49f
                                    ? 1f
                                    : 0.5f;

                        _audio.SetChannelVolume(
                            captured,
                            next);
                        RebuildContent();
                    });
            }
        }

        private void Execute(
            StoreOperationResult result)
        {
            if (!result.Succeeded)
            {
                UIRuntimeCompositionRoot
                    .Instance
                    ?.SetUserMessage(
                        result.Detail);
            }

            Refresh();
        }

        private Button AddAction(
            string description,
            string buttonLabel,
            Action action)
        {
            RectTransform row =
                ProceduralUiFactory.Panel(
                    _content,
                    "ActionRow",
                    new Color(
                        0.03f,
                        0.09f,
                        0.065f,
                        0.96f));
            row.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 72f;

            ProceduralUiFactory
                .AddHorizontalLayout(
                    row.gameObject,
                    8f,
                    new RectOffset(
                        10,
                        10,
                        6,
                        6));

            Text text =
                ProceduralUiFactory.Text(
                    row,
                    "Description",
                    description,
                    16,
                    TextAnchor.MiddleLeft,
                    Color.white);
            text.gameObject.AddComponent<
                LayoutElement>()
                .flexibleWidth = 2f;

            Button button =
                ProceduralUiFactory.Button(
                    row,
                    "Action",
                    buttonLabel,
                    action,
                    15);
            button.gameObject
                .GetComponent<LayoutElement>()
                .flexibleWidth = 1f;

            return button;
        }

        private void AddHeading(string text)
        {
            Text heading =
                ProceduralUiFactory.Text(
                    _content,
                    "Heading",
                    text,
                    21,
                    TextAnchor.MiddleLeft,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));

            heading.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 38f;
        }

        private void AddParagraph(string text)
        {
            Text paragraph =
                ProceduralUiFactory.Text(
                    _content,
                    "Paragraph",
                    text,
                    16,
                    TextAnchor.UpperLeft,
                    new Color(
                        0.9f,
                        0.95f,
                        0.92f,
                        1f));

            paragraph.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight =
                    Mathf.Max(
                        44f,
                        22f *
                        (1 +
                         text.Length / 50));
        }

        private void Close()
        {
            if (_window != null)
            {
                Destroy(_window);
                _window = null;
                _content = null;
            }

            UIRuntimeCompositionRoot
                .Instance?.InputGate
                .ExitUiExclusive();

            ProceduralUiFactory.Select(
                _openButton);
        }

        private void Refresh()
        {
            if (_window != null)
            {
                RebuildContent();
            }
        }

        private void HandleStateChanged(
            StoreOperationsState state)
        {
            Refresh();
        }
    }
}
