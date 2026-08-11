using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
using VRMGames.CartridgeAndCloud.Presentation.PlayerAgency;
using UnityCamera = UnityEngine.Camera;

namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    /// <summary>
    /// Non-interactive contextual overlay for hover prompts and world-space
    /// status bubbles. It is presentation-only and never mutates
    /// gameplay state.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ContextualWorldFeedbackPresenter : MonoBehaviour
    {
        private const int MaximumMarkers = 24;
        private const float MarkerRefreshSeconds = 0.25f;

        private readonly List<ContextualWorldFeedbackItem> _items =
            new List<ContextualWorldFeedbackItem>(MaximumMarkers);
        private readonly List<MarkerView> _markerViews =
            new List<MarkerView>(MaximumMarkers);

        private ContextualWorldFeedbackProjection _projection;
        private PlayerWorldInteractionController _interaction;
        private UiInputContextGate _inputGate;
        private PlayerToolWheelInputBridge _toolWheelInput;
        private UnityCamera _worldCamera;

        private Canvas _canvas;
        private RectTransform _canvasRect;
        private RectTransform _promptPanel;
        private Text _promptText;
        private float _nextMarkerRefreshTime;

        public void Configure(
            StoreOperationsFacade store,
            IStoreContentCatalog catalog,
            IActiveGameSession activeSession,
            EmployeeStateService employeeState,
            PlayerWorldInteractionController interaction,
            UiInputContextGate inputGate,
            PlayerToolWheelInputBridge toolWheelInput,
            UnityCamera worldCamera)
        {
            _projection = new ContextualWorldFeedbackProjection(
                store,
                catalog,
                activeSession,
                employeeState);

            _interaction = interaction ??
                throw new ArgumentNullException(nameof(interaction));
            _inputGate = inputGate ??
                throw new ArgumentNullException(nameof(inputGate));
            _toolWheelInput = toolWheelInput ??
                throw new ArgumentNullException(nameof(toolWheelInput));
            _worldCamera = worldCamera;

            BuildCanvas();
            RefreshMarkers();
        }

        private void Update()
        {
            if (_projection == null || _interaction == null)
            {
                return;
            }

            if ((_inputGate != null && _inputGate.IsUiExclusive) ||
                (_toolWheelInput != null && _toolWheelInput.IsOpen))
            {
                HidePromptAndFrame();
                HideMarkerViews();
                return;
            }

            UpdateTargetFeedback();

            if (Time.unscaledTime >= _nextMarkerRefreshTime)
            {
                RefreshMarkers();
            }

            UpdateMarkerPositions();
        }

        private void BuildCanvas()
        {
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }

            _canvas = ProceduralUiFactory.CreateCanvas(
                "S20A_P03_ContextualWorldFeedbackCanvas",
                4525);
            _canvas.transform.SetParent(transform, false);
            _canvasRect = _canvas.GetComponent<RectTransform>();

            BuildPrompt();
            BuildMarkerPool();
        }

        private void BuildPrompt()
        {
            _promptPanel = ProceduralUiFactory.CreatePanel(
                _canvas.transform,
                "InteractionPrompt",
                new Color(0.015f, 0.045f, 0.032f, 0.94f));
            _promptPanel.anchorMin = new Vector2(0.33f, 0.055f);
            _promptPanel.anchorMax = new Vector2(0.67f, 0.115f);
            _promptPanel.offsetMin = Vector2.zero;
            _promptPanel.offsetMax = Vector2.zero;

            Image promptImage = _promptPanel.GetComponent<Image>();
            promptImage.raycastTarget = false;

            _promptText = ProceduralUiFactory.Text(
                _promptPanel,
                "InteractionPromptText",
                string.Empty,
                18,
                TextAnchor.MiddleCenter,
                Color.white);
            ProceduralUiFactory.Stretch(_promptText.rectTransform, 10f);
            _promptText.raycastTarget = false;

            _promptPanel.gameObject.SetActive(false);
        }

        private void BuildMarkerPool()
        {
            _markerViews.Clear();

            for (int index = 0; index < MaximumMarkers; index++)
            {
                RectTransform panel = ProceduralUiFactory.CreatePanel(
                    _canvas.transform,
                    "WorldStatusMarker_" + index,
                    new Color(0.04f, 0.18f, 0.11f, 0.94f));
                panel.anchorMin = new Vector2(0.5f, 0.5f);
                panel.anchorMax = new Vector2(0.5f, 0.5f);
                panel.pivot = new Vector2(0.5f, 0.5f);
                panel.sizeDelta = new Vector2(190f, 34f);

                Image panelImage = panel.GetComponent<Image>();
                panelImage.raycastTarget = false;

                Text text = ProceduralUiFactory.Text(
                    panel,
                    "Label",
                    string.Empty,
                    15,
                    TextAnchor.MiddleCenter,
                    Color.white);
                ProceduralUiFactory.Stretch(text.rectTransform, 5f);
                text.raycastTarget = false;

                MarkerView view = new MarkerView(
                    panel,
                    panelImage,
                    text);
                view.SetVisible(false);
                _markerViews.Add(view);
            }
        }

        private void UpdateTargetFeedback()
        {
            IWorldInteractionTarget target = _interaction.CurrentTarget;
            if (!IsTargetAlive(target))
            {
                HidePromptAndFrame();
                return;
            }

            string prompt = _projection.BuildTargetPrompt(
                target,
                _interaction.CurrentDistance);

            if (string.IsNullOrWhiteSpace(prompt))
            {
                HidePromptAndFrame();
                return;
            }

            _promptText.text = prompt;
            _promptPanel.gameObject.SetActive(true);
        }

        private void RefreshMarkers()
        {
            _nextMarkerRefreshTime =
                Time.unscaledTime + MarkerRefreshSeconds;

            _projection?.CollectVisibleItems(_items);

            int visibleCount = Mathf.Min(
                _items.Count,
                _markerViews.Count);

            for (int index = 0; index < _markerViews.Count; index++)
            {
                MarkerView view = _markerViews[index];

                if (index >= visibleCount)
                {
                    view.Clear();
                    continue;
                }

                ContextualWorldFeedbackItem item = _items[index];
                view.Bind(item);
                view.Background.color = ResolveMarkerColor(item.Severity);
                view.Label.text = item.Text;
            }
        }

        private void UpdateMarkerPositions()
        {
            UnityCamera camera = ResolveWorldCamera();
            if (camera == null || _canvasRect == null)
            {
                HideMarkerViews();
                return;
            }

            foreach (MarkerView view in _markerViews)
            {
                if (!view.HasItem || view.Anchor == null)
                {
                    view.SetVisible(false);
                    continue;
                }

                Vector3 worldPoint =
                    view.Anchor.position +
                    Vector3.up * view.VerticalOffset;
                Vector3 screenPoint =
                    camera.WorldToScreenPoint(worldPoint);

                if (screenPoint.z <= 0f ||
                    screenPoint.x < -40f ||
                    screenPoint.x > Screen.width + 40f ||
                    screenPoint.y < -40f ||
                    screenPoint.y > Screen.height + 40f)
                {
                    view.SetVisible(false);
                    continue;
                }

                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvasRect,
                        screenPoint,
                        null,
                        out Vector2 localPoint))
                {
                    view.SetVisible(false);
                    continue;
                }

                view.Rect.anchoredPosition = localPoint;
                view.SetVisible(true);
            }
        }

        private void HidePromptAndFrame()
        {
            if (_promptPanel != null)
            {
                _promptPanel.gameObject.SetActive(false);
            }
        }

        private void HideMarkerViews()
        {
            foreach (MarkerView view in _markerViews)
            {
                view.SetVisible(false);
            }
        }

        private UnityCamera ResolveWorldCamera()
        {
            if (_worldCamera != null)
            {
                return _worldCamera;
            }

            _worldCamera = UnityCamera.main;
            return _worldCamera;
        }

        private static Color ResolveMarkerColor(
            ContextualWorldFeedbackSeverity severity)
        {
            switch (severity)
            {
                case ContextualWorldFeedbackSeverity.Warning:
                    return new Color(0.66f, 0.12f, 0.08f, 0.94f);
                case ContextualWorldFeedbackSeverity.Attention:
                    return new Color(0.62f, 0.37f, 0.05f, 0.94f);
                default:
                    return new Color(0.04f, 0.32f, 0.18f, 0.94f);
            }
        }

        private static bool IsTargetAlive(
            IWorldInteractionTarget target)
        {
            if (target == null)
            {
                return false;
            }

            if (target is Behaviour behaviour &&
                !behaviour.isActiveAndEnabled)
            {
                return false;
            }

            if (target is UnityEngine.Object unityObject)
            {
                return unityObject != null;
            }

            return true;
        }

        private sealed class MarkerView
        {
            public RectTransform Rect { get; }
            public Image Background { get; }
            public Text Label { get; }
            public Transform Anchor { get; private set; }
            public float VerticalOffset { get; private set; }
            public bool HasItem { get; private set; }

            public MarkerView(
                RectTransform rect,
                Image background,
                Text label)
            {
                Rect = rect;
                Background = background;
                Label = label;
            }

            public void Bind(ContextualWorldFeedbackItem item)
            {
                Anchor = item.Anchor;
                VerticalOffset = item.VerticalOffset;
                HasItem = Anchor != null &&
                          !string.IsNullOrWhiteSpace(item.Text);
            }

            public void Clear()
            {
                Anchor = null;
                VerticalOffset = 0f;
                HasItem = false;
                Label.text = string.Empty;
                SetVisible(false);
            }

            public void SetVisible(bool visible)
            {
                if (Rect != null &&
                    Rect.gameObject.activeSelf != visible)
                {
                    Rect.gameObject.SetActive(visible);
                }
            }
        }
    }
}
