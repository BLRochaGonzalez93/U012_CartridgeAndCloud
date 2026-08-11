using System;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.PlayerAgency;
using VRMGames.CartridgeAndCloud.Domain.PlayerAgency;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Presentation.PlayerAgency;

namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    /// <summary>
    /// Procedural radial selector for player tools. P04 owns selection only;
    /// tool-specific gameplay remains in the phases that implement each tool.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PlayerToolWheelPresenter : MonoBehaviour
    {
        private const float MouseDeadZonePixels = 44f;
        private const float StickDeadZone = 0.35f;
        private const float OptionRadius = 132f;

        private static readonly PlayerToolId[] ToolOrder =
        {
            PlayerToolId.Hands,
            PlayerToolId.Scanner,
            PlayerToolId.Tablet,
            PlayerToolId.Broom
        };

        private PlayerToolSelectionService _selection;
        private PlayerToolWheelInputBridge _input;
        private UiInputContextGate _inputGate;

        private Canvas _canvas;
        private RectTransform _wheelRoot;
        private Text _centerText;
        private RectTransform _statusPanel;
        private Text _statusText;
        private OptionView[] _options;
        private PlayerToolId _highlighted;
        private bool _subscribed;

        public void Configure(
            PlayerToolSelectionService selection,
            PlayerToolWheelInputBridge input,
            UiInputContextGate inputGate)
        {
            Unsubscribe();

            _selection = selection ??
                throw new ArgumentNullException(nameof(selection));
            _input = input ??
                throw new ArgumentNullException(nameof(input));
            _inputGate = inputGate ??
                throw new ArgumentNullException(nameof(inputGate));

            BuildCanvas();
            Subscribe();
            SetHighlighted(_selection.SelectedTool);
            RefreshStatus();
            SetWheelVisible(false);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Update()
        {
            if (_canvas == null)
            {
                return;
            }

            bool uiExclusive =
                _inputGate != null &&
                _inputGate.IsUiExclusive;

            if (uiExclusive)
            {
                if (_input != null && _input.IsOpen)
                {
                    _input.Cancel();
                }

                SetWheelVisible(false);
            }

            if (_statusPanel != null)
            {
                _statusPanel.gameObject.SetActive(
                    !uiExclusive);
            }
        }

        private void BuildCanvas()
        {
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }

            _canvas = ProceduralUiFactory.CreateCanvas(
                "S20A_P04_PlayerToolWheelCanvas",
                4535);
            _canvas.transform.SetParent(transform, false);

            RectTransform shade =
                ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "WheelShade",
                    new Color(0f, 0f, 0f, 0.58f));
            ProceduralUiFactory.Stretch(shade, 0f);
            shade.GetComponent<Image>().raycastTarget = false;
            _wheelRoot = shade;

            RectTransform ring =
                ProceduralUiFactory.CreatePanel(
                    _wheelRoot,
                    "WheelCenter",
                    new Color(0.035f, 0.08f, 0.065f, 0.98f));
            ring.anchorMin = new Vector2(0.5f, 0.5f);
            ring.anchorMax = new Vector2(0.5f, 0.5f);
            ring.pivot = new Vector2(0.5f, 0.5f);
            ring.sizeDelta = new Vector2(138f, 138f);
            ring.anchoredPosition = Vector2.zero;
            ring.GetComponent<Image>().raycastTarget = false;

            _centerText = ProceduralUiFactory.Text(
                ring,
                "SelectedTool",
                string.Empty,
                19,
                TextAnchor.MiddleCenter,
                Color.white);
            ProceduralUiFactory.Stretch(
                _centerText.rectTransform,
                10f);
            _centerText.raycastTarget = false;

            _options = new OptionView[ToolOrder.Length];
            for (int index = 0; index < ToolOrder.Length; index++)
            {
                float angle = 90f - index * 90f;
                float radians = angle * Mathf.Deg2Rad;
                Vector2 position = new Vector2(
                    Mathf.Cos(radians),
                    Mathf.Sin(radians)) * OptionRadius;

                RectTransform panel =
                    ProceduralUiFactory.CreatePanel(
                        _wheelRoot,
                        "Tool_" + ToolOrder[index],
                        new Color(0.04f, 0.16f, 0.12f, 0.98f));
                panel.anchorMin = new Vector2(0.5f, 0.5f);
                panel.anchorMax = new Vector2(0.5f, 0.5f);
                panel.pivot = new Vector2(0.5f, 0.5f);
                panel.sizeDelta = new Vector2(156f, 70f);
                panel.anchoredPosition = position;

                Image background = panel.GetComponent<Image>();
                background.raycastTarget = false;

                Text label = ProceduralUiFactory.Text(
                    panel,
                    "Label",
                    BuildOptionLabel(ToolOrder[index]),
                    16,
                    TextAnchor.MiddleCenter,
                    Color.white);
                ProceduralUiFactory.Stretch(
                    label.rectTransform,
                    8f);
                label.raycastTarget = false;

                _options[index] = new OptionView(
                    ToolOrder[index],
                    background,
                    label);
            }

            _statusPanel =
                ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "SelectedToolStatus",
                    new Color(0.015f, 0.045f, 0.032f, 0.88f));
            _statusPanel.anchorMin = new Vector2(0.79f, 0.035f);
            _statusPanel.anchorMax = new Vector2(0.97f, 0.085f);
            _statusPanel.offsetMin = Vector2.zero;
            _statusPanel.offsetMax = Vector2.zero;
            _statusPanel.GetComponent<Image>().raycastTarget = false;

            _statusText = ProceduralUiFactory.Text(
                _statusPanel,
                "SelectedToolStatusText",
                string.Empty,
                15,
                TextAnchor.MiddleCenter,
                Color.white);
            ProceduralUiFactory.Stretch(
                _statusText.rectTransform,
                6f);
            _statusText.raycastTarget = false;
        }

        private void Subscribe()
        {
            if (_subscribed || _input == null || _selection == null)
            {
                return;
            }

            _input.Opened += HandleOpened;
            _input.Updated += HandleUpdated;
            _input.Released += HandleReleased;
            _input.Cancelled += HandleCancelled;
            _selection.SelectionChanged += HandleSelectionChanged;
            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_subscribed)
            {
                return;
            }

            if (_input != null)
            {
                _input.Opened -= HandleOpened;
                _input.Updated -= HandleUpdated;
                _input.Released -= HandleReleased;
                _input.Cancelled -= HandleCancelled;
            }

            if (_selection != null)
            {
                _selection.SelectionChanged -= HandleSelectionChanged;
            }

            _subscribed = false;
        }

        private void HandleOpened(
            Vector2 pointerPosition,
            Vector2 navigation)
        {
            if (_inputGate != null &&
                _inputGate.IsUiExclusive)
            {
                _input.Cancel();
                return;
            }

            SetHighlighted(_selection.SelectedTool);
            UpdateHighlight(pointerPosition, navigation);
            SetWheelVisible(true);
        }

        private void HandleUpdated(
            Vector2 pointerPosition,
            Vector2 navigation)
        {
            UpdateHighlight(pointerPosition, navigation);
        }

        private void HandleReleased(
            Vector2 pointerPosition,
            Vector2 navigation)
        {
            UpdateHighlight(pointerPosition, navigation);
            _selection.Select(_highlighted);
            SetWheelVisible(false);
            RefreshStatus();
        }

        private void HandleCancelled()
        {
            SetHighlighted(_selection.SelectedTool);
            SetWheelVisible(false);
        }

        private void HandleSelectionChanged(PlayerToolId tool)
        {
            SetHighlighted(tool);
            RefreshStatus();
        }

        private void UpdateHighlight(
            Vector2 pointerPosition,
            Vector2 navigation)
        {
            Vector2 direction = navigation;

            if (direction.sqrMagnitude <
                StickDeadZone * StickDeadZone)
            {
                Vector2 center = new Vector2(
                    Screen.width * 0.5f,
                    Screen.height * 0.5f);
                direction = pointerPosition - center;

                if (direction.sqrMagnitude <
                    MouseDeadZonePixels * MouseDeadZonePixels)
                {
                    SetHighlighted(
                        _selection.SelectedTool);
                    return;
                }
            }

            float angle = Mathf.Atan2(
                direction.y,
                direction.x) * Mathf.Rad2Deg;
            if (angle < 0f)
            {
                angle += 360f;
            }

            PlayerToolId tool;
            if (angle >= 45f && angle < 135f)
            {
                tool = PlayerToolId.Hands;
            }
            else if (angle < 45f || angle >= 315f)
            {
                tool = PlayerToolId.Scanner;
            }
            else if (angle >= 225f && angle < 315f)
            {
                tool = PlayerToolId.Tablet;
            }
            else
            {
                tool = PlayerToolId.Broom;
            }

            SetHighlighted(tool);
        }

        private void SetHighlighted(PlayerToolId tool)
        {
            _highlighted = tool;

            if (_centerText != null)
            {
                _centerText.text = GetDisplayName(tool);
            }

            if (_options == null)
            {
                return;
            }

            foreach (OptionView option in _options)
            {
                bool highlighted = option.Tool == tool;
                bool selected = _selection != null &&
                    _selection.SelectedTool == option.Tool;

                option.Background.color = highlighted
                    ? new Color(0.12f, 0.52f, 0.34f, 0.99f)
                    : selected
                        ? new Color(0.08f, 0.30f, 0.22f, 0.98f)
                        : new Color(0.04f, 0.16f, 0.12f, 0.98f);

                option.Label.text =
                    BuildOptionLabel(option.Tool) +
                    (selected ? "\nSELECTED" : string.Empty);
            }
        }

        private void RefreshStatus()
        {
            if (_statusText == null || _selection == null)
            {
                return;
            }

            _statusText.text =
                "Tool · " +
                GetDisplayName(_selection.SelectedTool);
        }

        private void SetWheelVisible(bool visible)
        {
            if (_wheelRoot != null)
            {
                _wheelRoot.gameObject.SetActive(visible);
            }
        }

        private static string BuildOptionLabel(PlayerToolId tool)
        {
            switch (tool)
            {
                case PlayerToolId.Hands:
                    return "HANDS\nInteraction";
                case PlayerToolId.Scanner:
                    return "SCANNER\nPrepared";
                case PlayerToolId.Tablet:
                    return "TABLET\nPrepared";
                case PlayerToolId.Broom:
                    return "BROOM\nPrepared";
                default:
                    return tool.ToString().ToUpperInvariant();
            }
        }

        private static string GetDisplayName(PlayerToolId tool)
        {
            switch (tool)
            {
                case PlayerToolId.Hands:
                    return "Hands";
                case PlayerToolId.Scanner:
                    return "Scanner";
                case PlayerToolId.Tablet:
                    return "Tablet";
                case PlayerToolId.Broom:
                    return "Broom";
                default:
                    return tool.ToString();
            }
        }

        private sealed class OptionView
        {
            public PlayerToolId Tool { get; }
            public Image Background { get; }
            public Text Label { get; }

            public OptionView(
                PlayerToolId tool,
                Image background,
                Text label)
            {
                Tool = tool;
                Background = background;
                Label = label;
            }
        }
    }
}
