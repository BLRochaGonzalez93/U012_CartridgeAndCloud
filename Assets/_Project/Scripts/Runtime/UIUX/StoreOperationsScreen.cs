using System;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

using VRMGames.CartridgeAndCloud.Application.Audio;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Runtime.Audio;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Composition;
using VRMGames.CartridgeAndCloud.Runtime.Store;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion;
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
            Employees,
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
        private EmployeeHiringService
            _employeeHiring;
        private EmployeePayrollService
            _employeePayroll;
        private EmployeeScheduleService
            _employeeSchedule;
        private EmployeeStateService
            _employeeState;
        private EmployeePresenceService
            _employeePresence;
        private CandidateId _pendingHireCandidateId;
        private AuthoredStoreRuntimeBinder
            _binder;
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
            EmployeeHiringService employeeHiring,
            EmployeePayrollService employeePayroll,
            EmployeeScheduleService employeeSchedule,
            EmployeeStateService employeeState,
            EmployeePresenceService employeePresence,
            AuthoredStoreRuntimeBinder binder,
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
            _employeeHiring = employeeHiring ??
                throw new ArgumentNullException(
                    nameof(employeeHiring));
            _employeePayroll = employeePayroll ??
                throw new ArgumentNullException(
                    nameof(employeePayroll));
            _employeeSchedule = employeeSchedule ??
                throw new ArgumentNullException(
                    nameof(employeeSchedule));
            _employeeState = employeeState ??
                throw new ArgumentNullException(
                    nameof(employeeState));
            _employeePresence = employeePresence ??
                throw new ArgumentNullException(
                    nameof(employeePresence));
            _binder = binder ??
                throw new ArgumentNullException(
                    nameof(binder));
            _audio = audio ??
                throw new ArgumentNullException(
                    nameof(audio));

            _service.StateChanged +=
                HandleStateChanged;
            _employeeHiring.StateChanged +=
                HandleHiringStateChanged;
            _employeePayroll.StateChanged +=
                HandlePayrollStateChanged;
            _employeeSchedule.StateChanged +=
                HandleScheduleStateChanged;

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

            if (_employeeHiring != null)
            {
                _employeeHiring.StateChanged -=
                    HandleHiringStateChanged;
            }

            if (_employeePayroll != null)
            {
                _employeePayroll.StateChanged -=
                    HandlePayrollStateChanged;
            }

            if (_employeeSchedule != null)
            {
                _employeeSchedule.StateChanged -=
                    HandleScheduleStateChanged;
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
                    "Store Operations",
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
                    "Store Operations",
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
                Tab.Employees,
                "Employees");
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
                case Tab.Employees:
                    BuildEmployees();
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

            string currency =
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession
                    .Snapshot.CurrencyCode;

            AddParagraph(
                "Cash total: " +
                StoreUiProjectionService.FormatMoney(
                    UIRuntimeCompositionRoot
                        .Instance.ActiveSession
                        .Snapshot.CashCents,
                    currency));

            AddParagraph(
                "Reserved for orders: " +
                StoreUiProjectionService.FormatMoney(
                    _service.ReservedFundsCents,
                    currency));

            AddParagraph(
                "Available cash: " +
                StoreUiProjectionService.FormatMoney(
                    _service.AvailableCashCents,
                    currency));

            AddParagraph(
                "Current weekly gross: " +
                StoreUiProjectionService.FormatMoney(
                    _service.State.WeeklyGrossResultCents,
                    currency));

            if (_service.State.LastSettledWeek > 0)
            {
                AddParagraph(
                    "Last weekly settlement · week " +
                    _service.State.LastSettledWeek +
                    "\nGross: " +
                    StoreUiProjectionService.FormatMoney(
                        _service.State.LastWeeklyGrossResultCents,
                        currency) +
                    "\nTax (10% positive gross): " +
                    StoreUiProjectionService.FormatMoney(
                        _service.State.LastWeeklyTaxCents,
                        currency));
            }

            AddParagraph(
                $"Completed sales: " +
                $"{_service.State.CompletedSales}");

        }

        private void BuildShop()
        {
            string currency =
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession
                    .Snapshot.CurrencyCode;

            AddHeading("Purchasing power");
            AddParagraph(
                "Available: " +
                StoreUiProjectionService.FormatMoney(
                    _service.AvailableCashCents,
                    currency) +
                " · Reserved: " +
                StoreUiProjectionService.FormatMoney(
                    _service.ReservedFundsCents,
                    currency));

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

            string currency =
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession
                    .Snapshot.CurrencyCode;

            AddParagraph(
                "Cash total: " +
                StoreUiProjectionService.FormatMoney(
                    UIRuntimeCompositionRoot
                        .Instance.ActiveSession
                        .Snapshot.CashCents,
                    currency) +
                "\nReserved: " +
                StoreUiProjectionService.FormatMoney(
                    _service.ReservedFundsCents,
                    currency) +
                "\nAvailable: " +
                StoreUiProjectionService.FormatMoney(
                    _service.AvailableCashCents,
                    currency));

            bool any = false;

            foreach (StoreOrderRecord order
                     in _service.State.Orders)
            {
                if (order.State == StoreOrderStatus.InTransit)
                {
                    any = true;
                    AddParagraph(
                        order.OrderId + "\n" +
                        order.ItemId + " × " +
                        order.OrderedUnits + "\nIn transit · " +
                        order.DeliveryRunId + " · reserved " +
                        StoreUiProjectionService.FormatMoney(
                            order.ReservedCostCents,
                            currency));
                    continue;
                }

                if (order.State != StoreOrderStatus.Reserved)
                {
                    continue;
                }

                any = true;

                AddAction(
                    order.OrderId + "\n" +
                    order.ItemId + " × " +
                    order.OrderedUnits + "\nReserved " +
                    StoreUiProjectionService.FormatMoney(
                        order.ReservedCostCents,
                        currency),
                    "Dispatch delivery",
                    () => Execute(
                        _service.ReceiveOrder(
                            order.OrderId)));

                AddAction(
                    "Release reservation for " +
                    order.OrderId,
                    "Cancel order",
                    () => Execute(
                        _service.CancelOrder(
                            order.OrderId)));
            }

            if (!any)
            {
                AddParagraph(
                    "No reserved or in-transit deliveries.");
            }
            else if (_service.PendingOrderCount > 0)
            {
                AddAction(
                    _service.PendingOrderCount +
                    " reserved orders · total committed " +
                    StoreUiProjectionService.FormatMoney(
                        _service.ReservedFundsCents,
                        currency),
                    "Dispatch all in one run",
                    () => Execute(
                        _service.ProcessAllPendingOrders()));
            }

            AddHeading("Delivery runs");
            if (_service.State.DeliveryRuns.Count == 0)
            {
                AddParagraph("No delivery runs.");
            }
            else
            {
                foreach (StoreDeliveryRunRecord run
                         in _service.State.DeliveryRuns)
                {
                    AddParagraph(
                        run.DeliveryRunId + " · " +
                        run.OrderIds.Count + " orders · " +
                        StoreUiProjectionService.FormatMoney(
                            run.TotalCostCents,
                            currency) + " · " +
                        run.Status);
                }
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

                int activeReservations =
                    _service.GetActiveReservedQuantity(
                        fixture.InstanceId);

                int unreservedUnits =
                    _service.GetUnreservedDisplayQuantity(
                        fixture.InstanceId);

                int backroomStock =
                    string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId)
                        ? 0
                        : _service
                            .GetProductWarehouseQuantity(
                                fixture.AssignedProductId);

                AddParagraph(
                    fixture.InstanceId + "\n" +
                    furniture.DisplayName + "\n" +
                    "Product: " +
                    assignedProductName + "\n" +
                    "Visible stock: " +
                    fixture.ProductQuantity + "/" +
                    furniture.Capacity + "\n" +
                    "Reserved: " +
                    activeReservations +
                    " · Returnable: " +
                    unreservedUnits + "\n" +
                    "Received backroom stock: " +
                    backroomStock);

                if (string.IsNullOrWhiteSpace(
                        fixture.AssignedProductId))
                {
                    foreach (RetailProductDefinition
                             product in _catalog.Products)
                    {
                        RetailProductDefinition
                            captured = product;

                        int availableStock =
                            _service
                                .GetProductWarehouseQuantity(
                                    captured.ProductId);

                        AddAction(
                            "Assign " +
                            captured.DisplayName +
                            " · received backroom " +
                            availableStock,
                            "Assign",
                            () => Execute(
                                _service.AssignProduct(
                                    fixture.InstanceId,
                                    captured.ProductId)));
                    }

                    continue;
                }

                AddAction(
                    "Transfer one received unit from backroom",
                    "Restock 1",
                    () => Execute(
                        _service.RestockDisplay(
                            fixture.InstanceId,
                            1)));

                AddAction(
                    "Fill with the minimum of received stock and free capacity",
                    "Restock max",
                    () => Execute(
                        _service.RestockDisplay(
                            fixture.InstanceId,
                            furniture.Capacity)));

                if (fixture.ProductQuantity > 0 &&
                    unreservedUnits > 0)
                {
                    AddAction(
                        "Return one unreserved unit to backroom",
                        "Return 1",
                        () => Execute(
                            _service.ReturnDisplayStock(
                                fixture.InstanceId,
                                1)));
                }

                if (fixture.ProductQuantity > 0 &&
                    activeReservations == 0)
                {
                    AddAction(
                        "Return every visible unit and keep the assignment",
                        "Return all",
                        () => Execute(
                            _service.ReturnAllDisplayStock(
                                fixture.InstanceId)));

                    AddAction(
                        "Return every unit and leave the display unassigned",
                        "Return & clear",
                        () => Execute(
                            _service.ReturnAllAndClearDisplay(
                                fixture.InstanceId)));
                }
                else if (fixture.ProductQuantity == 0 &&
                         activeReservations == 0)
                {
                    AddAction(
                        "Remove the product assignment from this empty display",
                        "Clear assignment",
                        () => Execute(
                            _service.ClearDisplayAssignment(
                                fixture.InstanceId)));
                }

                if (activeReservations > 0)
                {
                    AddParagraph(
                        "Stock mutation is limited while active customer reservations exist.");
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
                "A customer walks through entrance, evaluates a stocked product, picks it, queues, completes checkout and exits.");

            AddParagraph(
                "Active customers: " +
                _characters.ActiveCustomerCount + "/" +
                _characters.MaximumCustomerCount);

            if (UIRuntimeCompositionRoot.Instance
                    .StoreOperationalGate != null)
            {
                AddParagraph(
                    UIRuntimeCompositionRoot.Instance
                        .StoreOperationalGate
                        .StatusDetail);
            }

            Button button =
                AddAction(
                    "Serve the next available customer",
                    "Spawn customer",
                    () => Execute(
                        _characters
                            .TryServeNextCustomer()));

            button.interactable =
                _characters.CanServeNextCustomer;
        }

        private void BuildEmployees()
        {
            EmployeeHiringEligibility eligibility =
                _employeeHiring.EvaluateEligibility();
            string currency =
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession
                    .Snapshot.CurrencyCode;

            AddHeading("Hiring requirements");
            AddParagraph(
                "Operating day: " +
                eligibility.CurrentDay +
                " / " +
                eligibility.MinimumOperatingDays +
                (eligibility.MeetsOperatingDayRequirement
                    ? " · ready"
                    : " · locked"));
            AddParagraph(
                "Work area: " +
                (eligibility.HasValidWorkArea
                    ? "valid checkout/work area available"
                    : "place a valid checkout/work area"));
            AddParagraph(
                "Salary obligations: " +
                (eligibility.HasNoOutstandingSalaryObligations
                    ? "clear"
                    : "OUTSTANDING · recruiting is blocked"));
            AddParagraph(
                "Business level " +
                eligibility.MinimumBusinessLevel +
                " and reputation " +
                eligibility.MinimumReputation +
                " are defined progression gates. " +
                "They are not enforced until a progression authority exists.");

            AddHeading("Recruitment channels");
            foreach (RecruitmentChannelDefinition channel
                     in _employeeHiring.Catalog.Channels)
            {
                RecruitmentChannelDefinition captured = channel;
                long effectiveCost =
                    _employeeHiring.GetEffectivePostingCost(captured);
                string delay = captured.MinimumDelayDays ==
                               captured.MaximumDelayDays
                    ? captured.MinimumDelayDays + " day(s)"
                    : captured.MinimumDelayDays + "-" +
                      captured.MaximumDelayDays + " days";

                Button publish = AddAction(
                    captured.DisplayName +
                    " · " + captured.CandidateCount +
                    " candidates · " + delay +
                    " · " +
                    (effectiveCost == 0
                        ? "FREE"
                        : StoreUiProjectionService.FormatMoney(
                            effectiveCost,
                            currency)),
                    "Publish",
                    () => ExecuteHiring(
                        _employeeHiring.Publish(
                            captured.ChannelId)));
                publish.interactable = eligibility.CanRecruit;
            }

            AddHeading("Candidate groups");
            if (_employeeHiring.Postings.Count == 0)
            {
                AddParagraph("No recruitment postings yet.");
            }

            int currentDay = eligibility.CurrentDay;
            foreach (RecruitmentPosting posting
                     in _employeeHiring.Postings)
            {
                RecruitmentPostingState state =
                    posting.GetState(currentDay);

                AddParagraph(
                    posting.Channel.DisplayName +
                    " · " + state +
                    " · published day " +
                    posting.PublishedDay +
                    " · candidates day " +
                    posting.ReadyDay +
                    " · available through day " +
                    (posting.ExpiryDayExclusive - 1));

                if (state == RecruitmentPostingState.Pending)
                {
                    continue;
                }

                if (state == RecruitmentPostingState.Expired)
                {
                    AddParagraph(
                        "This candidate group has expired.");
                    continue;
                }

                bool anyAvailable = false;
                foreach (RecruitmentCandidateEntry entry
                         in posting.Candidates)
                {
                    if (entry.State !=
                        RecruitmentCandidateState.Available)
                    {
                        continue;
                    }

                    anyAvailable = true;
                    EmployeeCandidate candidate = entry.Candidate;
                    string detail =
                        candidate.DisplayName +
                        " · " + FormatProfile(candidate.Profile) +
                        " · " + candidate.Seniority +
                        "\nSkills C/R/P: " +
                        candidate.Skills.Clerk + "/" +
                        candidate.Skills.Restocking + "/" +
                        candidate.Skills.OrderPicking +
                        " · speed " + candidate.WorkSpeed +
                        " · " + candidate.Experience +
                        "\nSalary request: " +
                        StoreUiProjectionService.FormatMoney(
                            candidate.RequestedDailySalaryCents,
                            currency) +
                        "/day · trait: " + candidate.Trait +
                        " · next-day availability";

                    CandidateId candidateId = candidate.CandidateId;
                    bool selected =
                        _pendingHireCandidateId.IsInitialized &&
                        _pendingHireCandidateId == candidateId;

                    AddAction(
                        detail,
                        selected ? "Selected" : "Review",
                        () =>
                        {
                            _pendingHireCandidateId = candidateId;
                            RebuildContent();
                        });

                    if (selected)
                    {
                        AddAction(
                            "Confirm " + candidate.DisplayName +
                            " at " +
                            StoreUiProjectionService.FormatMoney(
                                candidate.RequestedDailySalaryCents,
                                currency) +
                            "/day. Employment starts next day at 08:00.",
                            "Confirm hire",
                            () =>
                            {
                                EmployeeHiringOperationResult result =
                                    _employeeHiring.Hire(candidateId);
                                if (result.Succeeded)
                                {
                                    _pendingHireCandidateId = default;
                                }

                                ExecuteHiring(result);
                            });

                        AddAction(
                            "Return to the candidate list without hiring.",
                            "Cancel",
                            () =>
                            {
                                _pendingHireCandidateId = default;
                                RebuildContent();
                            });
                    }

                    AddAction(
                        "Remove " + candidate.DisplayName +
                        " from this candidate group.",
                        "Discard",
                        () => ExecuteHiring(
                            _employeeHiring.Discard(candidateId)));
                }

                if (!anyAvailable &&
                    state == RecruitmentPostingState.Closed)
                {
                    AddParagraph(
                        "All candidates in this group have been resolved.");
                }
            }

            AddHeading("Payroll");
            EmployeePayrollSummary payroll =
                _employeePayroll.GetSummary();
            AddParagraph(
                "Active employees today: " +
                payroll.ActiveEmployeeCount +
                " · daily payroll " +
                StoreUiProjectionService.FormatMoney(
                    payroll.DailyPayrollCents,
                    currency) +
                " · paid today " +
                StoreUiProjectionService.FormatMoney(
                    payroll.PaidTodayCents,
                    currency));
            AddParagraph(
                "Outstanding: " +
                payroll.OutstandingObligationCount +
                " obligation(s) · " +
                StoreUiProjectionService.FormatMoney(
                    payroll.OutstandingSalaryCents,
                    currency));

            if (payroll.OutstandingObligationCount > 0)
            {
                foreach (EmployeeSalaryObligation obligation
                         in _employeePayroll.OutstandingObligations)
                {
                    AddParagraph(
                        obligation.EmployeeName +
                        " · due day " + obligation.DueDay +
                        " · " +
                        StoreUiProjectionService.FormatMoney(
                            obligation.AmountCents,
                            currency));
                }

                AddAction(
                    "Pay all outstanding salary obligations atomically. " +
                    "Partial payroll is not allowed.",
                    "Pay outstanding",
                    () => ExecutePayroll(
                        _employeePayroll.TrySettleOutstanding()));
            }

            AddHeading("Operational state");
            AddParagraph(
                "Physically present: " +
                _employeePresence.CountPresent() + "/" +
                _employeeHiring.Employees.Count +
                " · task-ready: " +
                _employeeState.CountAvailable() + ". " +
                "State is derived from contractual start, schedule and " +
                "store closing; it is not controlled by UI flags.");

            AddHeading("Hired employees");
            if (_employeeHiring.Employees.Count == 0)
            {
                AddParagraph("No employees hired yet.");
                return;
            }

            foreach (HiredEmployee employee
                     in _employeeHiring.Employees)
            {
                EmployeeId employeeId = employee.EmployeeId;
                EmployeePresenceStatus presenceStatus =
                    _employeePresence.GetStatus(employee);
                EmployeeStateSnapshot operationalState =
                    _employeeState.GetState(employee);
                EmployeeShiftDefinition shift =
                    _employeeSchedule.GetConfiguredShift(employeeId);
                int nextDay = Math.Max(1, _employeeSchedule.CurrentDay + 1);
                EmployeeScheduleExceptionKind nextException =
                    _employeeSchedule.GetException(employeeId, nextDay);

                AddParagraph(
                    employee.DisplayName +
                    " · " + FormatProfile(employee.Profile) +
                    " · " + employee.Seniority +
                    "\nEmployee ID: " + employee.EmployeeId +
                    "\nSalary: " +
                    StoreUiProjectionService.FormatMoney(
                        employee.ContractedDailySalaryCents,
                        currency) +
                    "/scheduled day · starts day " +
                    employee.StartDay + " at 08:00" +
                    "\nState: " +
                    FormatOperationalState(operationalState.State) +
                    " · task-ready " +
                    (operationalState.CanAcceptTasks ? "YES" : "NO") +
                    "\nPresence: " + FormatPresence(presenceStatus) +
                    " · current time " +
                    FormatScheduleMinute(_employeeSchedule.CurrentVirtualMinute));

                AddAction(
                    "Shift: " + shift.DisplayName + " " +
                    FormatScheduleMinute(shift.StartMinute) + "-" +
                    FormatScheduleMinute(shift.EndMinute) +
                    " · paid break " +
                    FormatScheduleMinute(shift.BreakStartMinute) + "-" +
                    FormatScheduleMinute(shift.BreakEndMinute) +
                    ". Schedule edits apply from the next game day.",
                    "Next shift",
                    () => ExecuteSchedule(
                        _employeeSchedule.CycleShiftPreset(employeeId)));

                AddParagraph(
                    "7-day work cycle: " +
                    FormatWorkCycle(employeeId));

                for (int cycleDay = 1; cycleDay <= 7; cycleDay++)
                {
                    int capturedCycleDay = cycleDay;
                    bool working = _employeeSchedule.IsRecurringWorkDay(
                        employeeId,
                        capturedCycleDay);
                    AddAction(
                        "Recurring cycle day " + capturedCycleDay +
                        " is " + (working ? "WORK" : "OFF") +
                        ". Current day remains locked.",
                        working ? "Set OFF" : "Set WORK",
                        () => ExecuteSchedule(
                            _employeeSchedule.ToggleRecurringWorkDay(
                                employeeId,
                                capturedCycleDay)));
                }

                AddAction(
                    "Day " + nextDay + " exception: " + nextException +
                    ". Cycle None → DayOff → ForceWork.",
                    "Next exception",
                    () => ExecuteSchedule(
                        _employeeSchedule.CycleNextDayException(employeeId)));
            }
        }

        private void BuildSettings()
        {
            AddHeading(
                "Visibility");

            WallOcclusionController wallOcclusion =
                _binder.WallOcclusion;

            if (wallOcclusion == null)
            {
                AddParagraph(
                    "Automatic wall occlusion is unavailable.");
            }
            else if (!wallOcclusion.CanChangeVisibility)
            {
                AddParagraph(
                    "Automatic wall occlusion is unavailable.");
            }
            else
            {
                bool hideWalls =
                    wallOcclusion.HideOccludingWalls;

                AddAction(
                    "Walls between camera and player",
                    hideWalls
                        ? "Hide: ON"
                        : "Hide: OFF",
                    () =>
                    {
                        wallOcclusion.SetEnabled(
                            !hideWalls);
                        RebuildContent();
                    });
            }

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

        private void ExecuteHiring(
            EmployeeHiringOperationResult result)
        {
            UIRuntimeCompositionRoot
                .Instance
                ?.SetUserMessage(
                    result.Detail);

            Refresh();
        }

        private void ExecutePayroll(
            EmployeePayrollOperationResult result)
        {
            UIRuntimeCompositionRoot
                .Instance
                ?.SetUserMessage(
                    result.Detail);

            Refresh();
        }

        private void ExecuteSchedule(
            EmployeeScheduleOperationResult result)
        {
            UIRuntimeCompositionRoot
                .Instance
                ?.SetUserMessage(result.Detail);

            Refresh();
        }

        private static string FormatProfile(
            EmployeeProfile profile)
        {
            switch (profile)
            {
                case EmployeeProfile.Clerk:
                    return "Clerk";
                case EmployeeProfile.Restocker:
                    return "Restocker";
                case EmployeeProfile.OrderPicker:
                    return "Order picker";
                case EmployeeProfile.Generalist:
                    return "Generalist";
                default:
                    return profile.ToString();
            }
        }

        private string FormatWorkCycle(EmployeeId employeeId)
        {
            string result = string.Empty;
            for (int day = 1; day <= 7; day++)
            {
                if (day > 1)
                {
                    result += " · ";
                }

                result += "D" + day + " " +
                    (_employeeSchedule.IsRecurringWorkDay(employeeId, day)
                        ? "ON"
                        : "OFF");
            }

            return result;
        }

        private static string FormatOperationalState(
            EmployeeOperationalState state)
        {
            switch (state)
            {
                case EmployeeOperationalState.Available:
                    return "AVAILABLE";
                case EmployeeOperationalState.OnBreak:
                    return "ON BREAK";
                case EmployeeOperationalState.Closing:
                    return "CLOSING";
                case EmployeeOperationalState.OffDuty:
                    return "OFF DUTY";
                default:
                    return "AWAITING START";
            }
        }

        private static string FormatPresence(
            EmployeePresenceStatus status)
        {
            switch (status)
            {
                case EmployeePresenceStatus.Present:
                    return "PRESENT / WORKING";
                case EmployeePresenceStatus.OnBreak:
                    return "PRESENT / ON BREAK";
                case EmployeePresenceStatus.Closing:
                    return "PRESENT / CLOSING";
                case EmployeePresenceStatus.OffDuty:
                    return "OFF DUTY";
                default:
                    return "AWAITING START DAY";
            }
        }

        private static string FormatScheduleMinute(int minute)
        {
            if (minute < 0 || minute > 24 * 60)
            {
                return "--:--";
            }

            if (minute == 24 * 60)
            {
                return "24:00";
            }

            return (minute / 60).ToString("00") + ":" +
                   (minute % 60).ToString("00");
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

        private void HandleHiringStateChanged()
        {
            Refresh();
        }

        private void HandlePayrollStateChanged()
        {
            Refresh();
        }

        private void HandleScheduleStateChanged()
        {
            Refresh();
        }
    }
}
