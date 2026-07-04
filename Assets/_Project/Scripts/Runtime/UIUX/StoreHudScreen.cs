using System;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Runtime.Composition;
namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    public sealed class StoreHudScreen :
        MonoBehaviour
    {
        private UIRuntimeCompositionRoot _root;
        private Canvas _canvas;
        private Text _dayText;
        private Text _stateText;
        private Text _cashText;
        private Text _customersText;
        private Text _queueText;
        private Text _saveText;
        private GameObject _panelOverlay;
        private GameObject _confirmationOverlay;
        private GameObject _pauseOverlay;
        private GameObject _tutorialBubble;
        private Button _firstHudButton;
        private float _nextRefreshTime;
        private string _autosaveAttemptedDay =
            string.Empty;

        public void Initialize(
            UIRuntimeCompositionRoot root)
        {
            _root = root ??
                throw new ArgumentNullException(
                    nameof(root));

            _root.Accessibility.Changed +=
                HandleAccessibilityChanged;
            _root.ActiveSession.SnapshotChanged +=
                HandleSnapshotChanged;
            _root.Autosave.Completed +=
                HandleAutosaveCompleted;
            UiInputSignals.CancelRequested +=
                HandleCancelRequested;

            _root.InputGate.ExitUiExclusive();
            Build();
        }

        private void OnDestroy()
        {
            if (_root == null)
            {
                return;
            }

            _root.Accessibility.Changed -=
                HandleAccessibilityChanged;
            _root.ActiveSession.SnapshotChanged -=
                HandleSnapshotChanged;
            _root.Autosave.Completed -=
                HandleAutosaveCompleted;
            UiInputSignals.CancelRequested -=
                HandleCancelRequested;
            _root.InputGate.ExitUiExclusive();
        }

        private void Update()
        {
            if (Time.unscaledTime >=
                _nextRefreshTime)
            {
                _nextRefreshTime =
                    Time.unscaledTime + 0.25f;
                Refresh();
            }
        }

        private void Build()
        {
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }

            _panelOverlay = null;
            _confirmationOverlay = null;
            _pauseOverlay = null;
            _tutorialBubble = null;
            _firstHudButton = null;

            _canvas =
                ProceduralUiFactory.CreateCanvas(
                    "Sprint15StoreCanvas",
                    4000);
            ApplyCanvasScale();
            _canvas.transform.SetParent(
                transform,
                false);

            BuildHud();
            BuildNavigation();

            if (!_root.ActiveSession
                .HasActiveSession)
            {
                BuildNoSessionOverlay();
                return;
            }

            EnsureTutorial();
            Refresh();
        }

        private void BuildHud()
        {
            RectTransform top =
                ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "HudRoot",
                    new Color(
                        0.015f,
                        0.035f,
                        0.028f,
                        0.96f));

            ProceduralUiFactory.SetRect(
                top,
                new Vector2(0f, 0.91f),
                new Vector2(1f, 1f),
                Vector2.zero,
                Vector2.zero);

            ProceduralUiFactory.AddHorizontalLayout(
                top.gameObject,
                12f,
                new RectOffset(
                    18,
                    18,
                    10,
                    10));

            _dayText = HudValue(
                top,
                "Day",
                "Day --");
            _stateText = HudValue(
                top,
                "State",
                "State --");
            _cashText = HudValue(
                top,
                "Cash",
                "Cash --");
            _customersText = HudValue(
                top,
                "Customers",
                "Customers --");
            _queueText = HudValue(
                top,
                "Queue",
                "Queue --");
            _saveText = HudValue(
                top,
                "SaveStatus",
                "Save --");

            Button pause =
                ProceduralUiFactory.CreateButton(
                    top,
                    "PauseButton",
                    "Menu",
                    OpenPause,
                    ScaleText(18));
            pause.gameObject
                .GetComponent<LayoutElement>()
                .preferredWidth = 110f;
        }

        private void BuildNavigation()
        {
            RectTransform navigation =
                ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "ManagementNavigation",
                    new Color(
                        0.018f,
                        0.05f,
                        0.038f,
                        0.95f));

            ProceduralUiFactory.SetRect(
                navigation,
                new Vector2(0f, 0f),
                new Vector2(0.17f, 0.91f),
                Vector2.zero,
                Vector2.zero);

            ProceduralUiFactory.AddVerticalLayout(
                navigation.gameObject,
                8f,
                new RectOffset(
                    12,
                    12,
                    18,
                    18));

            Text heading =
                ProceduralUiFactory.CreateText(
                    navigation,
                    "Heading",
                    "MANAGEMENT",
                    ScaleText(22),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            heading.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 42f;

            AddPanelButton(
                navigation,
                ManagementPanelId.Inventory,
                "Inventory");
            AddPanelButton(
                navigation,
                ManagementPanelId.Suppliers,
                "Suppliers");
            AddPanelButton(
                navigation,
                ManagementPanelId.Displays,
                "Displays");
            AddPanelButton(
                navigation,
                ManagementPanelId.Customers,
                "Customers");
            AddPanelButton(
                navigation,
                ManagementPanelId.Shopping,
                "Shopping");
            AddPanelButton(
                navigation,
                ManagementPanelId.Checkout,
                "Checkout");
            AddPanelButton(
                navigation,
                ManagementPanelId.DayCycle,
                "Day Cycle");
            AddPanelButton(
                navigation,
                ManagementPanelId.Economy,
                "Economy");
            AddPanelButton(
                navigation,
                ManagementPanelId.Help,
                "Help");
            AddPanelButton(
                navigation,
                ManagementPanelId.Accessibility,
                "Accessibility");
        }

        private void AddPanelButton(
            Transform parent,
            ManagementPanelId panelId,
            string label)
        {
            Button button =
                ProceduralUiFactory.CreateButton(
                    parent,
                    "Panel" + panelId,
                    label,
                    () => OpenPanel(panelId),
                    ScaleText(17));

            if (_firstHudButton == null)
            {
                _firstHudButton = button;
            }
        }

        private void BuildNoSessionOverlay()
        {
            RectTransform overlay =
                ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "NoSession",
                    new Color(
                        0f,
                        0f,
                        0f,
                        0.92f));
            ProceduralUiFactory.Stretch(overlay);

            ProceduralUiFactory.AddVerticalLayout(
                overlay.gameObject,
                18f,
                new RectOffset(
                    420,
                    420,
                    280,
                    280));

            Text text =
                ProceduralUiFactory.CreateText(
                    overlay,
                    "Message",
                    "No active save slot.\n" +
                    "Return to Main Menu and select " +
                    "New Game or Continue.",
                    ScaleText(28),
                    TextAnchor.MiddleCenter,
                    Color.white);
            text.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 160f;

            Button button =
                ProceduralUiFactory.CreateButton(
                    overlay,
                    "Return",
                    "Return to Main Menu",
                    _root.ReturnToMainMenu,
                    ScaleText(22));
            ProceduralUiFactory.Select(button);
            _root.InputGate.EnterUiExclusive();
        }

        private void Refresh()
        {
            if (!_root.ActiveSession
                .HasActiveSession)
            {
                return;
            }

            IntegratedGameStateSnapshot snapshot =
                _root.ActiveSession.Snapshot;

            StoreHudSnapshot hud =
                _root.Projection.BuildHud(
                    snapshot,
                    _root.Autosave.CurrentStatus);

            _dayText.text =
                "Day " + hud.CurrentDay;
            _stateText.text =
                hud.StoreState + " · " +
                hud.ElapsedSeconds + "/" +
                hud.DurationSeconds + "s";
            _cashText.text =
                StoreUiProjectionService
                    .FormatMoney(
                        hud.CashCents,
                        hud.CurrencyCode);
            _customersText.text =
                "Customers " +
                hud.ActiveCustomers;
            _queueText.text =
                "Queue " + hud.QueueLength +
                " · " + hud.CheckoutState;
            _saveText.text =
                "Save " + hud.SaveStatus;

            TryClosedDayAutosave(snapshot);
        }

        private void TryClosedDayAutosave(
            IntegratedGameStateSnapshot snapshot)
        {
            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal) ||
                string.Equals(
                    _autosaveAttemptedDay,
                    snapshot.DayCycle.DayId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _autosaveAttemptedDay =
                snapshot.DayCycle.DayId;

            DailyAutosaveResult result =
                _root.Autosave.TryAutosave();
            _root.SetUserMessage(result.Detail);

            if (_panelOverlay != null)
            {
                RebuildCurrentPanelIfNeeded();
            }
        }

        private void OpenPanel(
            ManagementPanelId panelId)
        {
            ClosePanel();

            _root.InputGate.EnterUiExclusive();

            _panelOverlay =
                new GameObject(
                    "ManagementPanelOverlay",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform overlay =
                _panelOverlay.GetComponent<
                    RectTransform>();
            overlay.SetParent(
                _canvas.transform,
                false);

            ProceduralUiFactory.SetRect(
                overlay,
                new Vector2(0.17f, 0f),
                new Vector2(1f, 0.91f),
                Vector2.zero,
                Vector2.zero);

            _panelOverlay.GetComponent<Image>()
                .color =
                    new Color(
                        0.005f,
                        0.012f,
                        0.010f,
                        0.84f);

            RectTransform panel =
                ProceduralUiFactory.CreatePanel(
                    overlay,
                    "PanelContent",
                    new Color(
                        0.025f,
                        0.065f,
                        0.05f,
                        0.99f));

            ProceduralUiFactory.SetRect(
                panel,
                new Vector2(0.08f, 0.06f),
                new Vector2(0.92f, 0.94f),
                Vector2.zero,
                Vector2.zero);

            ProceduralUiFactory.AddVerticalLayout(
                panel.gameObject,
                10f,
                new RectOffset(
                    26,
                    26,
                    20,
                    20));

            if (panelId ==
                ManagementPanelId.Accessibility)
            {
                BuildAccessibilityPanel(panel);
            }
            else
            {
                BuildProjectedPanel(
                    panel,
                    panelId);
            }

            RectTransform actions =
                ProceduralUiFactory.CreatePanel(
                    panel,
                    "PanelActions",
                    Color.clear);
            actions.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 58f;

            ProceduralUiFactory.AddHorizontalLayout(
                actions.gameObject,
                12f,
                new RectOffset(0, 0, 2, 2));

            if (panelId ==
                ManagementPanelId.DayCycle)
            {
                AddDayCycleAction(actions);
            }

            if (_root.ActiveSession
                    .Snapshot.DayCycle.State ==
                    "Closed" &&
                (panelId ==
                    ManagementPanelId.DayCycle ||
                 panelId ==
                    ManagementPanelId.Economy))
            {
                Button nextDay =
                    ProceduralUiFactory.CreateButton(
                        actions,
                        "NextDay",
                        "Continue to Next Day",
                        ContinueToNextDay,
                        ScaleText(19));

                bool saveReady =
                    _root.Autosave.CurrentStatus ==
                        DailyAutosaveStatus.Saved ||
                    _root.Autosave.CurrentStatus ==
                        DailyAutosaveStatus
                            .AlreadySaved;
                nextDay.interactable = saveReady;
            }

            Button close =
                ProceduralUiFactory.CreateButton(
                    actions,
                    "ClosePanel",
                    "Close",
                    ClosePanel,
                    ScaleText(19));

            ProceduralUiFactory.Select(close);
        }

        private void BuildProjectedPanel(
            RectTransform panel,
            ManagementPanelId panelId)
        {
            ManagementPanelSnapshot snapshot =
                _root.Projection.BuildPanel(
                    _root.ActiveSession.Snapshot,
                    panelId);

            Text title =
                ProceduralUiFactory.CreateText(
                    panel,
                    "PanelTitle",
                    snapshot.Title,
                    ScaleText(32),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            title.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 52f;

            RectTransform viewport =
                ProceduralUiFactory.CreatePanel(
                    panel,
                    "Viewport",
                    new Color(
                        0.015f,
                        0.035f,
                        0.028f,
                        0.96f));
            viewport.gameObject.AddComponent<
                LayoutElement>()
                .flexibleHeight = 1f;

            VerticalLayoutGroup layout =
                ProceduralUiFactory.AddVerticalLayout(
                    viewport.gameObject,
                    6f,
                    new RectOffset(
                        14,
                        14,
                        12,
                        12));

            layout.childForceExpandHeight = false;

            foreach (ManagementPanelRow row
                     in snapshot.Rows)
            {
                RectTransform rowRect =
                    ProceduralUiFactory.CreatePanel(
                        viewport,
                        "Row",
                        new Color(
                            0.035f,
                            0.085f,
                            0.065f,
                            0.95f));
                rowRect.gameObject.AddComponent<
                    LayoutElement>()
                    .preferredHeight = 58f;

                ProceduralUiFactory.AddHorizontalLayout(
                    rowRect.gameObject,
                    12f,
                    new RectOffset(
                        12,
                        12,
                        5,
                        5));

                AddRowText(
                    rowRect,
                    row.Label,
                    TextAnchor.MiddleLeft,
                    2f);
                AddRowText(
                    rowRect,
                    row.Value,
                    TextAnchor.MiddleCenter,
                    1f);
                AddRowText(
                    rowRect,
                    row.StatusText,
                    TextAnchor.MiddleRight,
                    2f);
            }
        }

        private void BuildAccessibilityPanel(
            RectTransform panel)
        {
            Text title =
                ProceduralUiFactory.CreateText(
                    panel,
                    "PanelTitle",
                    "Accessibility",
                    ScaleText(32),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            title.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 52f;

            UiAccessibilitySettings settings =
                _root.Accessibility.Current;

            AddSettingRow(
                panel,
                "UI scale",
                settings.UiScalePercent + "%",
                () => ApplyAccessibility(
                    settings.WithUiScale(
                        Mathf.Clamp(
                            settings.UiScalePercent - 10,
                            80,
                            150))),
                () => ApplyAccessibility(
                    settings.WithUiScale(
                        Mathf.Clamp(
                            settings.UiScalePercent + 10,
                            80,
                            150))));

            AddSettingRow(
                panel,
                "Text scale",
                settings.TextScalePercent + "%",
                () => ApplyAccessibility(
                    settings.WithTextScale(
                        Mathf.Clamp(
                            settings.TextScalePercent - 10,
                            80,
                            150))),
                () => ApplyAccessibility(
                    settings.WithTextScale(
                        Mathf.Clamp(
                            settings.TextScalePercent + 10,
                            80,
                            150))));

            AddToggleRow(
                panel,
                "Reduced motion",
                settings.ReduceMotion,
                () => ApplyAccessibility(
                    settings.WithReduceMotion(
                        !settings.ReduceMotion)));

            AddToggleRow(
                panel,
                "Tutorial enabled",
                settings.TutorialEnabled,
                () => ApplyAccessibility(
                    settings.WithTutorialEnabled(
                        !settings.TutorialEnabled)));

            AddToggleRow(
                panel,
                "Destructive confirmations",
                settings.ConfirmDestructiveActions,
                () => ApplyAccessibility(
                    settings
                        .WithDestructiveConfirmations(
                            !settings
                                .ConfirmDestructiveActions)));

            Button restart =
                ProceduralUiFactory.CreateButton(
                    panel,
                    "RestartTutorial",
                    "Restart Tutorial",
                    RestartTutorial,
                    ScaleText(19));
            restart.gameObject
                .GetComponent<LayoutElement>()
                .preferredHeight = 58f;
        }

        private void AddSettingRow(
            Transform parent,
            string label,
            string value,
            Action decrease,
            Action increase)
        {
            RectTransform row =
                ProceduralUiFactory.CreatePanel(
                    parent,
                    label.Replace(" ", string.Empty),
                    new Color(
                        0.035f,
                        0.085f,
                        0.065f,
                        0.95f));
            row.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;

            ProceduralUiFactory.AddHorizontalLayout(
                row.gameObject,
                10f,
                new RectOffset(12, 12, 4, 4));

            AddRowText(
                row,
                label + ": " + value,
                TextAnchor.MiddleLeft,
                2f);
            ProceduralUiFactory.CreateButton(
                row,
                "Decrease",
                "−",
                decrease,
                ScaleText(22));
            ProceduralUiFactory.CreateButton(
                row,
                "Increase",
                "+",
                increase,
                ScaleText(22));
        }

        private void AddToggleRow(
            Transform parent,
            string label,
            bool enabled,
            Action toggle)
        {
            RectTransform row =
                ProceduralUiFactory.CreatePanel(
                    parent,
                    label.Replace(" ", string.Empty),
                    new Color(
                        0.035f,
                        0.085f,
                        0.065f,
                        0.95f));
            row.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;

            ProceduralUiFactory.AddHorizontalLayout(
                row.gameObject,
                10f,
                new RectOffset(12, 12, 4, 4));

            AddRowText(
                row,
                label,
                TextAnchor.MiddleLeft,
                2f);

            ProceduralUiFactory.CreateButton(
                row,
                "Toggle",
                enabled ? "On" : "Off",
                toggle,
                ScaleText(19));
        }

        private void AddRowText(
            Transform parent,
            string value,
            TextAnchor anchor,
            float flexibleWidth)
        {
            Text text =
                ProceduralUiFactory.CreateText(
                    parent,
                    "Text",
                    value,
                    ScaleText(18),
                    anchor,
                    Color.white);
            LayoutElement layout =
                text.gameObject.AddComponent<
                    LayoutElement>();
            layout.flexibleWidth = flexibleWidth;
            layout.preferredHeight = 46f;
        }

        private Text HudValue(
            Transform parent,
            string name,
            string value)
        {
            Text text =
                ProceduralUiFactory.CreateText(
                    parent,
                    name,
                    value,
                    ScaleText(17),
                    TextAnchor.MiddleCenter,
                    Color.white);
            LayoutElement layout =
                text.gameObject.AddComponent<
                    LayoutElement>();
            layout.flexibleWidth = 1f;
            layout.preferredHeight = 50f;
            return text;
        }


        private void AddDayCycleAction(
            Transform actions)
        {
            string state =
                _root.ActiveSession
                    .Snapshot.DayCycle.State;

            switch (state)
            {
                case "BeforeOpen":
                    ProceduralUiFactory.CreateButton(
                        actions,
                        "OpenStore",
                        "Open Store",
                        () => TransitionDay("Open"),
                        ScaleText(18));
                    break;

                case "Open":
                    ProceduralUiFactory.CreateButton(
                        actions,
                        "BeginClosing",
                        "Begin Closing",
                        () => TransitionDay("Closing"),
                        ScaleText(18));
                    break;

                case "Closing":
                    ProceduralUiFactory.CreateButton(
                        actions,
                        "CompleteClosing",
                        "Complete Close",
                        () => TransitionDay("Closed"),
                        ScaleText(18));
                    break;
            }
        }

        private void TransitionDay(
            string targetState)
        {
            bool succeeded =
                _root.TryTransitionDay(
                    targetState,
                    out string reason);

            _root.SetUserMessage(reason);
            ClosePanel();
            Refresh();

            if (!succeeded)
            {
                ShowConfirmation(
                    "Day transition blocked",
                    reason,
                    CloseConfirmation);
            }
        }

        private void ContinueToNextDay()
        {
            _root.BeginNextDay();
            _autosaveAttemptedDay =
                string.Empty;
            ClosePanel();
            EnsureTutorial();
            Refresh();
        }

        private void RestartTutorial()
        {
            if (!_root.ActiveSession
                .HasActiveSession)
            {
                return;
            }

            _root.Tutorial.Restart(
                _root.ActiveSession.ActiveSlotId);
            ClosePanel();
            EnsureTutorial();
        }

        private void EnsureTutorial()
        {
            if (!_root.ActiveSession
                    .HasActiveSession ||
                !_root.Accessibility.Current
                    .TutorialEnabled)
            {
                DestroyTutorialBubble();
                return;
            }

            TutorialProgress progress =
                _root.Tutorial.GetProgress(
                    _root.ActiveSession
                        .ActiveSlotId);

            if (progress.State ==
                TutorialProgressState.NotStarted)
            {
                progress =
                    _root.Tutorial.Start(
                        _root.ActiveSession
                            .ActiveSlotId);
            }

            if (progress.IsTerminal)
            {
                DestroyTutorialBubble();
                return;
            }

            TutorialBubble bubble =
                _root.Tutorial.GetBubble(
                    progress);

            if (bubble == null)
            {
                DestroyTutorialBubble();
                return;
            }

            BuildTutorialBubble(bubble);
        }

        private void BuildTutorialBubble(
            TutorialBubble bubble)
        {
            DestroyTutorialBubble();

            _tutorialBubble =
                new GameObject(
                    "TutorialBubble",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform rect =
                _tutorialBubble.GetComponent<
                    RectTransform>();
            rect.SetParent(
                _canvas.transform,
                false);

            ProceduralUiFactory.SetRect(
                rect,
                new Vector2(0.60f, 0.04f),
                new Vector2(0.96f, 0.34f),
                Vector2.zero,
                Vector2.zero);

            _tutorialBubble.GetComponent<Image>()
                .color =
                    new Color(
                        0.04f,
                        0.14f,
                        0.10f,
                        0.99f);

            ProceduralUiFactory.AddVerticalLayout(
                _tutorialBubble,
                8f,
                new RectOffset(
                    18,
                    18,
                    14,
                    14));

            Text title =
                ProceduralUiFactory.CreateText(
                    rect,
                    "TutorialTitle",
                    bubble.Title,
                    ScaleText(23),
                    TextAnchor.MiddleLeft,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            title.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 34f;

            Text body =
                ProceduralUiFactory.CreateText(
                    rect,
                    "TutorialBody",
                    bubble.Body + "\n" +
                    bubble.ActionHint,
                    ScaleText(17),
                    TextAnchor.UpperLeft,
                    Color.white);
            body.gameObject.AddComponent<
                LayoutElement>()
                .flexibleHeight = 1f;

            Text anchor =
                ProceduralUiFactory.CreateText(
                    rect,
                    "TutorialAnchor",
                    "Target: " + bubble.AnchorId,
                    ScaleText(14),
                    TextAnchor.MiddleLeft,
                    new Color(
                        0.72f,
                        0.82f,
                        0.76f,
                        1f));
            anchor.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 24f;

            RectTransform buttons =
                ProceduralUiFactory.CreatePanel(
                    rect,
                    "TutorialButtons",
                    Color.clear);
            buttons.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 48f;
            ProceduralUiFactory.AddHorizontalLayout(
                buttons.gameObject,
                8f,
                new RectOffset(0, 0, 0, 0));

            ProceduralUiFactory.CreateButton(
                buttons,
                "Next",
                "Next",
                AdvanceTutorial,
                ScaleText(16));
            ProceduralUiFactory.CreateButton(
                buttons,
                "Skip",
                "Skip Tutorial",
                RequestSkipTutorial,
                ScaleText(16));
        }

        private void AdvanceTutorial()
        {
            _root.Tutorial.Advance(
                _root.ActiveSession.ActiveSlotId);
            EnsureTutorial();
        }

        private void RequestSkipTutorial()
        {
            if (_root.Accessibility.Current
                .ConfirmDestructiveActions)
            {
                ShowConfirmation(
                    "Skip tutorial?",
                    "You can restart it later from " +
                    "Accessibility.",
                    () =>
                    {
                        _root.Tutorial.Skip(
                            _root.ActiveSession
                                .ActiveSlotId);
                        CloseConfirmation();
                        EnsureTutorial();
                    });
                return;
            }

            _root.Tutorial.Skip(
                _root.ActiveSession.ActiveSlotId);
            EnsureTutorial();
        }

        private void DestroyTutorialBubble()
        {
            if (_tutorialBubble != null)
            {
                Destroy(_tutorialBubble);
                _tutorialBubble = null;
            }
        }

        private void OpenPause()
        {
            if (_pauseOverlay != null)
            {
                return;
            }

            ClosePanel();
            _root.InputGate.EnterUiExclusive();

            _pauseOverlay =
                BuildModal(
                    "Pause",
                    "Store management is paused " +
                    "while this menu is open.");

            RectTransform panel =
                _pauseOverlay.transform
                    .Find("ModalPanel")
                    .GetComponent<RectTransform>();

            Button resume =
                ProceduralUiFactory.CreateButton(
                    panel,
                    "Resume",
                    "Resume",
                    ClosePause,
                    ScaleText(20));

            ProceduralUiFactory.CreateButton(
                panel,
                "Accessibility",
                "Accessibility",
                () =>
                {
                    ClosePause();
                    OpenPanel(
                        ManagementPanelId
                            .Accessibility);
                },
                ScaleText(20));

            ProceduralUiFactory.CreateButton(
                panel,
                "Help",
                "Help",
                () =>
                {
                    ClosePause();
                    OpenPanel(
                        ManagementPanelId.Help);
                },
                ScaleText(20));

            ProceduralUiFactory.CreateButton(
                panel,
                "MainMenu",
                "Return to Main Menu",
                RequestReturnToMainMenu,
                ScaleText(20));

            ProceduralUiFactory.Select(resume);
        }

        private void RequestReturnToMainMenu()
        {
            bool closed =
                _root.ActiveSession
                    .Snapshot.DayCycle.State ==
                "Closed";

            if (!closed &&
                _root.Accessibility.Current
                    .ConfirmDestructiveActions)
            {
                ShowConfirmation(
                    "Return to Main Menu?",
                    "Only closed days autosave. " +
                    "Unsaved progress may be lost.",
                    _root.ReturnToMainMenu);
                return;
            }

            _root.ReturnToMainMenu();
        }

        private void ShowConfirmation(
            string title,
            string body,
            Action confirm)
        {
            CloseConfirmation();
            _root.InputGate.EnterUiExclusive();

            _confirmationOverlay =
                BuildModal(title, body);

            RectTransform panel =
                _confirmationOverlay.transform
                    .Find("ModalPanel")
                    .GetComponent<RectTransform>();

            Button yes =
                ProceduralUiFactory.CreateButton(
                    panel,
                    "Confirm",
                    "Confirm",
                    confirm,
                    ScaleText(20));
            ProceduralUiFactory.CreateButton(
                panel,
                "Cancel",
                "Cancel",
                CloseConfirmation,
                ScaleText(20));

            ProceduralUiFactory.Select(yes);
        }

        private GameObject BuildModal(
            string title,
            string body)
        {
            GameObject overlay =
                new GameObject(
                    "ModalOverlay",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform overlayRect =
                overlay.GetComponent<
                    RectTransform>();
            overlayRect.SetParent(
                _canvas.transform,
                false);
            ProceduralUiFactory.Stretch(
                overlayRect);

            overlay.GetComponent<Image>()
                .color =
                    new Color(
                        0f,
                        0f,
                        0f,
                        0.82f);

            RectTransform panel =
                ProceduralUiFactory.CreatePanel(
                    overlayRect,
                    "ModalPanel",
                    new Color(
                        0.035f,
                        0.09f,
                        0.07f,
                        1f));

            ProceduralUiFactory.SetRect(
                panel,
                new Vector2(0.34f, 0.24f),
                new Vector2(0.66f, 0.76f),
                Vector2.zero,
                Vector2.zero);

            ProceduralUiFactory.AddVerticalLayout(
                panel.gameObject,
                14f,
                new RectOffset(
                    28,
                    28,
                    24,
                    24));

            Text titleText =
                ProceduralUiFactory.CreateText(
                    panel,
                    "Title",
                    title,
                    ScaleText(29),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            titleText.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 50f;

            Text bodyText =
                ProceduralUiFactory.CreateText(
                    panel,
                    "Body",
                    body,
                    ScaleText(19),
                    TextAnchor.MiddleCenter,
                    Color.white);
            bodyText.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 110f;

            return overlay;
        }

        private void ClosePanel()
        {
            if (_panelOverlay != null)
            {
                Destroy(_panelOverlay);
                _panelOverlay = null;
            }

            RestoreGameplayIfNoModal();
            ProceduralUiFactory.Select(
                _firstHudButton);
        }

        private void ClosePause()
        {
            if (_pauseOverlay != null)
            {
                Destroy(_pauseOverlay);
                _pauseOverlay = null;
            }

            RestoreGameplayIfNoModal();
            ProceduralUiFactory.Select(
                _firstHudButton);
        }

        private void CloseConfirmation()
        {
            if (_confirmationOverlay != null)
            {
                Destroy(_confirmationOverlay);
                _confirmationOverlay = null;
            }

            RestoreGameplayIfNoModal();
        }

        private void RestoreGameplayIfNoModal()
        {
            if (_panelOverlay == null &&
                _pauseOverlay == null &&
                _confirmationOverlay == null)
            {
                _root.InputGate.ExitUiExclusive();
            }
        }

        private void HandleCancelRequested()
        {
            HandleCancel();
        }

        private void HandleCancel()
        {
            if (_confirmationOverlay != null)
            {
                CloseConfirmation();
                return;
            }

            if (_panelOverlay != null)
            {
                ClosePanel();
                return;
            }

            if (_pauseOverlay != null)
            {
                ClosePause();
                return;
            }

            OpenPause();
        }

        private void ApplyAccessibility(
            UiAccessibilitySettings settings)
        {
            _root.Accessibility.Apply(settings);
        }

        private void HandleAccessibilityChanged(
            UiAccessibilitySettings settings)
        {
            _root.InputGate.ExitUiExclusive();
            Build();
        }

        private void HandleSnapshotChanged(
            IntegratedGameStateSnapshot snapshot)
        {
            Refresh();
        }

        private void HandleAutosaveCompleted(
            DailyAutosaveResult result)
        {
            _root.SetUserMessage(result.Detail);
            Refresh();
        }

        private void RebuildCurrentPanelIfNeeded()
        {
            // The next explicit panel open renders the new status.
        }


        private void ApplyCanvasScale()
        {
            CanvasScaler scaler =
                _canvas.GetComponent<CanvasScaler>();
            float scale =
                _root.Accessibility.Current
                    .UiScaleMultiplier;
            scaler.referenceResolution =
                new Vector2(
                    1920f / scale,
                    1080f / scale);
        }

        private int ScaleText(int value)
        {
            return Mathf.RoundToInt(
                value *
                _root.Accessibility.Current
                    .TextScaleMultiplier);
        }
    }
}
