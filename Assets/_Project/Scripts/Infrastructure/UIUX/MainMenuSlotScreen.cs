using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class MainMenuSlotScreen :
        MonoBehaviour
    {
        private Sprint15RuntimeCompositionRoot _root;
        private Canvas _canvas;
        private GameObject _overlay;
        private Button _firstButton;

        public void Initialize(
            Sprint15RuntimeCompositionRoot root)
        {
            _root = root ??
                throw new ArgumentNullException(
                    nameof(root));

            _root.Accessibility.Changed +=
                HandleAccessibilityChanged;
            Sprint15InputSignals.CancelRequested +=
                HandleCancelRequested;
            _root.InputGate.EnterUiExclusive();
            Build();
        }

        private void OnDestroy()
        {
            if (_root != null)
            {
                _root.Accessibility.Changed -=
                    HandleAccessibilityChanged;
            }

            Sprint15InputSignals.CancelRequested -=
                HandleCancelRequested;
        }

        private void HandleCancelRequested()
        {
            if (_overlay != null)
            {
                CloseOverlay();
            }
        }

        private void Build()
        {
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }

            _canvas =
                Sprint15UiFactory.CreateCanvas(
                    "Sprint15MainMenuCanvas",
                    5000);

            ApplyCanvasScale();

            _canvas.transform.SetParent(
                transform,
                false);

            RectTransform background =
                Sprint15UiFactory.CreatePanel(
                    _canvas.transform,
                    "Background",
                    new Color(
                        0.015f,
                        0.025f,
                        0.022f,
                        0.995f));
            Sprint15UiFactory.Stretch(background);

            RectTransform content =
                Sprint15UiFactory.CreatePanel(
                    background,
                    "Content",
                    new Color(
                        0.025f,
                        0.055f,
                        0.045f,
                        0.98f));

            Sprint15UiFactory.SetRect(
                content,
                new Vector2(0.18f, 0.06f),
                new Vector2(0.82f, 0.94f),
                Vector2.zero,
                Vector2.zero);

            Sprint15UiFactory.AddVerticalLayout(
                content.gameObject,
                14f,
                new RectOffset(
                    36,
                    36,
                    28,
                    28));

            int titleSize = ScaleText(42);
            int bodySize = ScaleText(22);

            Text title =
                Sprint15UiFactory.CreateText(
                    content,
                    "Title",
                    "CARTRIDGE & CLOUD",
                    titleSize,
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            title.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;

            Text subtitle =
                Sprint15UiFactory.CreateText(
                    content,
                    "Subtitle",
                    "Select a save slot",
                    bodySize,
                    TextAnchor.MiddleCenter,
                    Color.white);
            subtitle.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 38f;

            IReadOnlyList<SlotDescriptor>
                descriptors =
                    _root.Slots.InspectAll();

            foreach (SlotDescriptor descriptor
                     in descriptors)
            {
                BuildSlotCard(
                    content,
                    descriptor);
            }

            RectTransform footer =
                Sprint15UiFactory.CreatePanel(
                    content,
                    "Footer",
                    Color.clear);
            footer.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;

            Sprint15UiFactory.AddHorizontalLayout(
                footer.gameObject,
                12f,
                new RectOffset(0, 0, 4, 4));

            Button settings =
                Sprint15UiFactory.CreateButton(
                    footer,
                    "AccessibilityButton",
                    "Accessibility",
                    OpenAccessibility,
                    ScaleText(20));

            Sprint15UiFactory.CreateButton(
                footer,
                "HelpButton",
                "Help",
                OpenHelp,
                ScaleText(20));

            Sprint15UiFactory.CreateButton(
                footer,
                "QuitButton",
                "Quit",
                RequestQuit,
                ScaleText(20));

            if (_firstButton == null)
            {
                _firstButton = settings;
            }

            string message =
                string.IsNullOrWhiteSpace(
                    _root.LastUserMessage)
                    ? "Three independent slots · " +
                      "automatic backup recovery"
                    : _root.LastUserMessage;

            Text status =
                Sprint15UiFactory.CreateText(
                    content,
                    "Status",
                    message,
                    ScaleText(18),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.78f,
                        0.88f,
                        0.82f,
                        1f));
            status.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 42f;

            Sprint15UiFactory.Select(
                _firstButton);
        }

        private void BuildSlotCard(
            Transform parent,
            SlotDescriptor descriptor)
        {
            RectTransform card =
                Sprint15UiFactory.CreatePanel(
                    parent,
                    $"Slot{descriptor.SlotId.Value}",
                    new Color(
                        0.035f,
                        0.10f,
                        0.075f,
                        1f));

            card.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 128f;

            Sprint15UiFactory.AddHorizontalLayout(
                card.gameObject,
                16f,
                new RectOffset(
                    18,
                    18,
                    12,
                    12));

            RectTransform info =
                Sprint15UiFactory.CreatePanel(
                    card,
                    "Info",
                    Color.clear);
            LayoutElement infoLayout =
                info.gameObject.AddComponent<
                    LayoutElement>();
            infoLayout.flexibleWidth = 2f;

            Sprint15UiFactory.AddVerticalLayout(
                info.gameObject,
                4f,
                new RectOffset(0, 0, 0, 0));

            Text heading =
                Sprint15UiFactory.CreateText(
                    info,
                    "Heading",
                    $"SLOT {descriptor.SlotId.Value + 1}",
                    ScaleText(25),
                    TextAnchor.MiddleLeft,
                    Color.white);
            heading.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 34f;

            Text details =
                Sprint15UiFactory.CreateText(
                    info,
                    "Details",
                    Describe(descriptor),
                    ScaleText(18),
                    TextAnchor.MiddleLeft,
                    StateColor(descriptor.State));
            details.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 60f;

            RectTransform actions =
                Sprint15UiFactory.CreatePanel(
                    card,
                    "Actions",
                    Color.clear);
            LayoutElement actionLayout =
                actions.gameObject.AddComponent<
                    LayoutElement>();
            actionLayout.flexibleWidth = 1f;

            Sprint15UiFactory.AddHorizontalLayout(
                actions.gameObject,
                8f,
                new RectOffset(0, 0, 18, 18));

            if (descriptor.CanContinue)
            {
                Button continueButton =
                    Sprint15UiFactory.CreateButton(
                        actions,
                        "Continue",
                        "Continue",
                        () => ContinueSlot(
                            descriptor.SlotId),
                        ScaleText(17));

                if (_firstButton == null)
                {
                    _firstButton =
                        continueButton;
                }
            }

            Button newButton =
                Sprint15UiFactory.CreateButton(
                    actions,
                    "NewGame",
                    descriptor.State ==
                        SlotPresentationState.Empty
                        ? "New Game"
                        : "Replace",
                    () => RequestNewGame(
                        descriptor),
                    ScaleText(17));

            if (_firstButton == null)
            {
                _firstButton = newButton;
            }

            Button deleteButton =
                Sprint15UiFactory.CreateButton(
                    actions,
                    "Delete",
                    "Delete",
                    () => RequestDelete(
                        descriptor),
                    ScaleText(17));
            deleteButton.interactable =
                descriptor.CanDelete;
        }

        private void ContinueSlot(
            SaveSlotId slotId)
        {
            SlotOperationResult result =
                _root.Slots.Continue(slotId);
            _root.SetUserMessage(
                result.RecoveredFromBackup
                    ? "The slot was recovered from " +
                      "its valid backup."
                    : result.Detail);

            if (result.Succeeded)
            {
                _root.EnterStore();
            }
            else
            {
                Build();
            }
        }

        private void RequestNewGame(
            SlotDescriptor descriptor)
        {
            bool requiresConfirmation =
                descriptor.State !=
                    SlotPresentationState.Empty &&
                _root.Accessibility.Current
                    .ConfirmDestructiveActions;

            if (requiresConfirmation)
            {
                ShowConfirmation(
                    "Replace save slot?",
                    "The existing save and tutorial " +
                    "progress will be deleted.",
                    () => CreateNew(
                        descriptor.SlotId,
                        true));
                return;
            }

            CreateNew(
                descriptor.SlotId,
                descriptor.State !=
                    SlotPresentationState.Empty);
        }

        private void CreateNew(
            SaveSlotId slotId,
            bool confirmed)
        {
            SlotOperationResult result =
                _root.Slots.CreateNew(
                    slotId,
                    confirmed);
            _root.SetUserMessage(result.Detail);

            if (result.Succeeded)
            {
                if (_root.Settings
                    .ShowTutorialOnNewGame &&
                    _root.Accessibility.Current
                        .TutorialEnabled)
                {
                    _root.Tutorial.Start(slotId);
                }

                _root.EnterStore();
            }
            else
            {
                CloseOverlay();
                Build();
            }
        }

        private void RequestDelete(
            SlotDescriptor descriptor)
        {
            if (!descriptor.CanDelete)
            {
                return;
            }

            if (_root.Accessibility.Current
                .ConfirmDestructiveActions)
            {
                ShowConfirmation(
                    "Delete save slot?",
                    "This removes the primary save, " +
                    "backup, tutorial progress and " +
                    "autosave marker.",
                    () => DeleteSlot(
                        descriptor.SlotId));
                return;
            }

            DeleteSlot(descriptor.SlotId);
        }

        private void DeleteSlot(
            SaveSlotId slotId)
        {
            SlotOperationResult result =
                _root.Slots.Delete(
                    slotId,
                    confirmed: true);
            _root.SetUserMessage(result.Detail);
            CloseOverlay();
            Build();
        }

        private void OpenAccessibility()
        {
            ShowOverlay(
                "Accessibility",
                BuildAccessibilityContent);
        }

        private void BuildAccessibilityContent(
            RectTransform panel)
        {
            UiAccessibilitySettings settings =
                _root.Accessibility.Current;

            AddSettingRow(
                panel,
                "UI scale",
                settings.UiScalePercent + "%",
                () => Apply(
                    settings.WithUiScale(
                        ClampScale(
                            settings
                                .UiScalePercent -
                            10))),
                () => Apply(
                    settings.WithUiScale(
                        ClampScale(
                            settings
                                .UiScalePercent +
                            10))));

            AddSettingRow(
                panel,
                "Text scale",
                settings.TextScalePercent + "%",
                () => Apply(
                    settings.WithTextScale(
                        ClampScale(
                            settings
                                .TextScalePercent -
                            10))),
                () => Apply(
                    settings.WithTextScale(
                        ClampScale(
                            settings
                                .TextScalePercent +
                            10))));

            AddToggleRow(
                panel,
                "Reduced motion",
                settings.ReduceMotion,
                () => Apply(
                    settings.WithReduceMotion(
                        !settings.ReduceMotion)));

            AddToggleRow(
                panel,
                "Tutorial",
                settings.TutorialEnabled,
                () => Apply(
                    settings
                        .WithTutorialEnabled(
                            !settings
                                .TutorialEnabled)));

            AddToggleRow(
                panel,
                "Destructive confirmations",
                settings
                    .ConfirmDestructiveActions,
                () => Apply(
                    settings
                        .WithDestructiveConfirmations(
                            !settings
                                .ConfirmDestructiveActions)));
        }

        private void OpenHelp()
        {
            ShowOverlay(
                "Help",
                panel =>
                {
                    string help =
                        "Mouse\n" +
                        "• Point, click and scroll\n\n" +
                        "Keyboard\n" +
                        "• Tab / Shift+Tab: move focus\n" +
                        "• Enter: activate\n" +
                        "• Escape: close the top layer\n\n" +
                        "Saving\n" +
                        "• The active slot autosaves once " +
                        "after a valid Closed day.\n" +
                        "• The previous valid generation " +
                        "remains as backup.\n\n" +
                        "Store UI\n" +
                        "• HUD buttons open the panels " +
                        "for all implemented systems.";

                    Text text =
                        Sprint15UiFactory.CreateText(
                            panel,
                            "HelpText",
                            help,
                            ScaleText(20),
                            TextAnchor.UpperLeft,
                            Color.white);
                    text.gameObject.AddComponent<
                        LayoutElement>()
                        .preferredHeight = 440f;
                });
        }

        private void RequestQuit()
        {
            if (_root.Accessibility.Current
                .ConfirmDestructiveActions)
            {
                ShowConfirmation(
                    "Quit Cartridge & Cloud?",
                    "The application will close.",
                    UnityEngine.Application.Quit);
                return;
            }

            UnityEngine.Application.Quit();
        }

        private void ShowConfirmation(
            string title,
            string body,
            Action confirm)
        {
            ShowOverlay(
                title,
                panel =>
                {
                    Text text =
                        Sprint15UiFactory.CreateText(
                            panel,
                            "ConfirmationText",
                            body,
                            ScaleText(21),
                            TextAnchor.MiddleCenter,
                            Color.white);
                    text.gameObject.AddComponent<
                        LayoutElement>()
                        .preferredHeight = 150f;

                    RectTransform buttons =
                        Sprint15UiFactory.CreatePanel(
                            panel,
                            "ConfirmationButtons",
                            Color.clear);
                    buttons.gameObject.AddComponent<
                        LayoutElement>()
                        .preferredHeight = 64f;
                    Sprint15UiFactory
                        .AddHorizontalLayout(
                            buttons.gameObject,
                            12f,
                            new RectOffset(
                                0,
                                0,
                                4,
                                4));

                    Button confirmButton =
                        Sprint15UiFactory
                            .CreateButton(
                                buttons,
                                "Confirm",
                                "Confirm",
                                confirm,
                                ScaleText(20));

                    Sprint15UiFactory
                        .CreateButton(
                            buttons,
                            "Cancel",
                            "Cancel",
                            CloseOverlay,
                            ScaleText(20));

                    Sprint15UiFactory.Select(
                        confirmButton);
                });
        }

        private void ShowOverlay(
            string title,
            Action<RectTransform> buildContent)
        {
            CloseOverlay();

            _overlay =
                new GameObject(
                    "ModalOverlay",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform overlayRect =
                _overlay.GetComponent<
                    RectTransform>();
            overlayRect.SetParent(
                _canvas.transform,
                false);
            Sprint15UiFactory.Stretch(
                overlayRect);

            _overlay.GetComponent<Image>()
                .color =
                    new Color(
                        0f,
                        0f,
                        0f,
                        0.80f);

            RectTransform panel =
                Sprint15UiFactory.CreatePanel(
                    overlayRect,
                    "ModalPanel",
                    new Color(
                        0.035f,
                        0.09f,
                        0.07f,
                        1f));

            Sprint15UiFactory.SetRect(
                panel,
                new Vector2(0.28f, 0.16f),
                new Vector2(0.72f, 0.84f),
                Vector2.zero,
                Vector2.zero);

            Sprint15UiFactory.AddVerticalLayout(
                panel.gameObject,
                16f,
                new RectOffset(
                    30,
                    30,
                    26,
                    26));

            Text heading =
                Sprint15UiFactory.CreateText(
                    panel,
                    "ModalTitle",
                    title,
                    ScaleText(30),
                    TextAnchor.MiddleCenter,
                    new Color(
                        0.35f,
                        1f,
                        0.62f,
                        1f));
            heading.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 54f;

            buildContent(panel);

            Button close =
                Sprint15UiFactory.CreateButton(
                    panel,
                    "Close",
                    "Close",
                    CloseOverlay,
                    ScaleText(20));

            if (EventSystem.current == null ||
                EventSystem.current
                    .currentSelectedGameObject == null)
            {
                Sprint15UiFactory.Select(close);
            }
        }

        private void AddSettingRow(
            Transform parent,
            string label,
            string value,
            Action decrease,
            Action increase)
        {
            RectTransform row =
                Sprint15UiFactory.CreatePanel(
                    parent,
                    label.Replace(" ", string.Empty),
                    Color.clear);
            row.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;
            Sprint15UiFactory.AddHorizontalLayout(
                row.gameObject,
                10f,
                new RectOffset(0, 0, 4, 4));

            Text text =
                Sprint15UiFactory.CreateText(
                    row,
                    "Label",
                    label + ": " + value,
                    ScaleText(19),
                    TextAnchor.MiddleLeft,
                    Color.white);
            text.gameObject.AddComponent<
                LayoutElement>()
                .flexibleWidth = 2f;

            Sprint15UiFactory.CreateButton(
                row,
                "Decrease",
                "−",
                decrease,
                ScaleText(22));
            Sprint15UiFactory.CreateButton(
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
                Sprint15UiFactory.CreatePanel(
                    parent,
                    label.Replace(" ", string.Empty),
                    Color.clear);
            row.gameObject.AddComponent<
                LayoutElement>()
                .preferredHeight = 62f;
            Sprint15UiFactory.AddHorizontalLayout(
                row.gameObject,
                10f,
                new RectOffset(0, 0, 4, 4));

            Text text =
                Sprint15UiFactory.CreateText(
                    row,
                    "Label",
                    label,
                    ScaleText(19),
                    TextAnchor.MiddleLeft,
                    Color.white);
            text.gameObject.AddComponent<
                LayoutElement>()
                .flexibleWidth = 2f;

            Sprint15UiFactory.CreateButton(
                row,
                "Toggle",
                enabled ? "On" : "Off",
                toggle,
                ScaleText(19));
        }

        private void Apply(
            UiAccessibilitySettings settings)
        {
            _root.Accessibility.Apply(settings);
        }

        private void CloseOverlay()
        {
            if (_overlay != null)
            {
                Destroy(_overlay);
                _overlay = null;
            }

            Sprint15UiFactory.Select(
                _firstButton);
        }

        private void HandleAccessibilityChanged(
            UiAccessibilitySettings settings)
        {
            _overlay = null;
            _firstButton = null;
            Build();
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

        private int ScaleText(int size)
        {
            return Mathf.RoundToInt(
                size *
                _root.Accessibility.Current
                    .TextScaleMultiplier);
        }

        private static int ClampScale(int value)
        {
            return Mathf.Clamp(
                value,
                UiAccessibilitySettings
                    .MinimumScalePercent,
                UiAccessibilitySettings
                    .MaximumScalePercent);
        }

        private string Describe(
            SlotDescriptor descriptor)
        {
            switch (descriptor.State)
            {
                case SlotPresentationState.Empty:
                    return "Empty slot";

                case SlotPresentationState.Ready:
                case SlotPresentationState.Recovered:
                    string recovered =
                        descriptor.State ==
                        SlotPresentationState.Recovered
                            ? " · Recovered from backup"
                            : string.Empty;
                    string updated =
                        descriptor.UpdatedUtc.HasValue
                            ? descriptor.UpdatedUtc.Value
                                .ToString(
                                    "yyyy-MM-dd HH:mm 'UTC'")
                            : "Unknown time";

                    return
                        $"Day {descriptor.CurrentDay} · " +
                        StoreUiProjectionService
                            .FormatMoney(
                                descriptor.CashCents,
                                _root.Settings
                                    .CurrencyCode) +
                        $" · {updated}{recovered}";

                case SlotPresentationState
                    .UnsupportedSchema:
                    return "Unsupported save schema";

                case SlotPresentationState.Corrupt:
                    return "Corrupt save without valid backup";

                default:
                    return "Storage unavailable";
            }
        }

        private static Color StateColor(
            SlotPresentationState state)
        {
            switch (state)
            {
                case SlotPresentationState.Ready:
                    return new Color(
                        0.72f,
                        1f,
                        0.82f,
                        1f);
                case SlotPresentationState.Recovered:
                    return new Color(
                        1f,
                        0.88f,
                        0.36f,
                        1f);
                case SlotPresentationState.Empty:
                    return new Color(
                        0.72f,
                        0.76f,
                        0.74f,
                        1f);
                default:
                    return new Color(
                        1f,
                        0.56f,
                        0.48f,
                        1f);
            }
        }
    }
}
