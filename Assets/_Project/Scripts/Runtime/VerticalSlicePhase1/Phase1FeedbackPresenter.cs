using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1FeedbackPresenter :
        MonoBehaviour
    {
        private readonly Dictionary<
            string,
            Transform> _anchors =
                new Dictionary<
                    string,
                    Transform>(
                        StringComparer.Ordinal);

        private Phase1PresentationCatalogAsset
            _catalog;
        private Phase1AudioRouter _audio;
        private Phase1VfxPool _vfx;
        private Canvas _canvas;
        private Text _message;
        private Image _panel;
        private Coroutine _activeMessage;

        public void Configure(
            Phase1PresentationCatalogAsset catalog,
            Phase1AudioRouter audio,
            int vfxPoolSize)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _audio = audio ??
                throw new ArgumentNullException(
                    nameof(audio));

            _vfx =
                gameObject.AddComponent<
                    Phase1VfxPool>();
            _vfx.Configure(vfxPoolSize);

            BuildCanvas();
        }

        public void RegisterAnchor(
            string id,
            Transform anchor)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                anchor == null)
            {
                return;
            }

            _anchors[id] = anchor;
        }

        public void Present(
            Phase1FeedbackEvent feedback)
        {
            if (feedback == null)
            {
                return;
            }

            Phase1PresentationCatalogAsset
                .FeedbackEntry entry =
                    _catalog.FindFeedback(
                        feedback.Kind);

            string text = feedback.Message;

            if (feedback.HasMoney)
            {
                text += "\n" +
                    (feedback.Kind ==
                        Phase1FeedbackKind.Expense
                        ? "−"
                        : "+") +
                    StoreUiProjectionService
                        .FormatMoney(
                            feedback.MinorUnits,
                            feedback.CurrencyCode);
            }

            if (_activeMessage != null)
            {
                StopCoroutine(_activeMessage);
            }

            _activeMessage =
                StartCoroutine(
                    ShowMessage(
                        text,
                        entry));

            Transform anchor =
                ResolveAnchor(
                    feedback.AnchorId);

            if (anchor != null)
            {
                Color color =
                    ResolveColor(feedback.Kind);

                if (entry == null ||
                    entry.showParticles)
                {
                    _vfx.Play(
                        anchor.position +
                        Vector3.up * 0.8f,
                        color);
                }

            }

            _audio.Play(
                AudioEventFor(
                    feedback.Kind));
        }

        private IEnumerator ShowMessage(
            string text,
            Phase1PresentationCatalogAsset
                .FeedbackEntry entry)
        {
            _message.text = text;
            _message.gameObject.SetActive(true);
            _panel.gameObject.SetActive(true);

            Color color =
                ResolveColorFromEntry(entry);
            _panel.color =
                new Color(
                    color.r,
                    color.g,
                    color.b,
                    0.94f);

            yield return new WaitForSecondsRealtime(
                2.4f);

            _panel.gameObject.SetActive(false);
            _message.gameObject.SetActive(false);
            _activeMessage = null;
        }

        private void BuildCanvas()
        {
            GameObject canvasObject =
                new GameObject(
                    "S16_P1_FeedbackCanvas",
                    typeof(RectTransform),
                    typeof(Canvas),
                    typeof(CanvasScaler),
                    typeof(GraphicRaycaster));

            canvasObject.transform.SetParent(
                transform,
                false);

            _canvas =
                canvasObject.GetComponent<Canvas>();
            _canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 4700;

            CanvasScaler scaler =
                canvasObject.GetComponent<
                    CanvasScaler>();
            scaler.uiScaleMode =
                CanvasScaler.ScaleMode
                    .ScaleWithScreenSize;
            scaler.referenceResolution =
                new Vector2(1920f, 1080f);

            GameObject panelObject =
                new GameObject(
                    "FeedbackPanel",
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform panelRect =
                panelObject.GetComponent<
                    RectTransform>();
            panelRect.SetParent(
                canvasObject.transform,
                false);
            panelRect.anchorMin =
                new Vector2(0.32f, 0.76f);
            panelRect.anchorMax =
                new Vector2(0.68f, 0.89f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            _panel =
                panelObject.GetComponent<Image>();
            _panel.color =
                new Color(
                    0.04f,
                    0.18f,
                    0.11f,
                    0.94f);

            GameObject textObject =
                new GameObject(
                    "FeedbackText",
                    typeof(RectTransform),
                    typeof(Text));

            RectTransform textRect =
                textObject.GetComponent<
                    RectTransform>();
            textRect.SetParent(
                panelRect,
                false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin =
                new Vector2(18f, 10f);
            textRect.offsetMax =
                new Vector2(-18f, -10f);

            _message =
                textObject.GetComponent<Text>();
            _message.font =
                Resources.GetBuiltinResource<
                    Font>(
                        "LegacyRuntime.ttf");
            _message.fontSize = 24;
            _message.alignment =
                TextAnchor.MiddleCenter;
            _message.color = Color.white;
            _message.raycastTarget = false;

            _panel.gameObject.SetActive(false);
            _message.gameObject.SetActive(false);
        }

        private Transform ResolveAnchor(
            string anchorId)
        {
            if (!string.IsNullOrWhiteSpace(
                    anchorId) &&
                _anchors.TryGetValue(
                    anchorId,
                    out Transform registered) &&
                registered != null)
            {
                return registered;
            }

            if (!string.IsNullOrWhiteSpace(
                    anchorId))
            {
                GameObject byName =
                    GameObject.Find(anchorId);

                if (byName != null)
                {
                    return byName.transform;
                }

                Phase1PlacedFixtureVisual[]
                    fixtures =
                        UnityEngine.Object
                            .FindObjectsByType<
                                Phase1PlacedFixtureVisual>(
                                    FindObjectsInactive
                                        .Include,
                                    FindObjectsSortMode
                                        .None);

                foreach (Phase1PlacedFixtureVisual
                         fixture in fixtures)
                {
                    if (string.Equals(
                            fixture.InstanceId,
                            anchorId,
                            StringComparison.Ordinal))
                    {
                        return fixture.transform;
                    }
                }
            }

            return null;
        }

        private static Color ResolveColor(
            Phase1FeedbackKind kind)
        {
            switch (kind)
            {
                case Phase1FeedbackKind
                    .PlacementInvalid:
                case Phase1FeedbackKind
                    .OutOfStock:
                case Phase1FeedbackKind
                    .CustomerFrustrated:
                case Phase1FeedbackKind
                    .AutosaveFailed:
                    return new Color(
                        0.95f,
                        0.24f,
                        0.18f);

                case Phase1FeedbackKind.Expense:
                case Phase1FeedbackKind
                    .ClosingWarning:
                    return new Color(
                        1f,
                        0.68f,
                        0.18f);

                default:
                    return new Color(
                        0.1f,
                        0.85f,
                        0.42f);
            }
        }

        private static Color ResolveColorFromEntry(
            Phase1PresentationCatalogAsset
                .FeedbackEntry entry)
        {
            return entry == null
                ? new Color(
                    0.05f,
                    0.38f,
                    0.22f)
                : ResolveColor(entry.kind);
        }

        private static string AudioEventFor(
            Phase1FeedbackKind kind)
        {
            return "feedback." +
                kind.ToString()
                    .ToLowerInvariant();
        }
    }
}
