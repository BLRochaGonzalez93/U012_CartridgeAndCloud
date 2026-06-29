using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    internal static class Phase1RuntimeUiFactory
    {
        private static Font _font;

        public static Canvas CreateCanvas(
            string name,
            int sortingOrder)
        {
            GameObject gameObject =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(Canvas),
                    typeof(CanvasScaler),
                    typeof(GraphicRaycaster));

            Canvas canvas =
                gameObject.GetComponent<Canvas>();
            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler =
                gameObject.GetComponent<
                    CanvasScaler>();
            scaler.uiScaleMode =
                CanvasScaler.ScaleMode
                    .ScaleWithScreenSize;
            scaler.referenceResolution =
                new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        public static RectTransform Panel(
            Transform parent,
            string name,
            Color color)
        {
            GameObject gameObject =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(Image));

            RectTransform rect =
                gameObject.GetComponent<
                    RectTransform>();
            rect.SetParent(parent, false);

            gameObject.GetComponent<Image>()
                .color = color;

            return rect;
        }

        public static Text Text(
            Transform parent,
            string name,
            string value,
            int size,
            TextAnchor alignment,
            Color color)
        {
            GameObject gameObject =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(Text));

            RectTransform rect =
                gameObject.GetComponent<
                    RectTransform>();
            rect.SetParent(parent, false);

            Text text =
                gameObject.GetComponent<Text>();
            text.font = Font;
            text.text = value ?? string.Empty;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow =
                HorizontalWrapMode.Wrap;
            text.verticalOverflow =
                VerticalWrapMode.Overflow;
            text.raycastTarget = false;

            return text;
        }

        public static Button Button(
            Transform parent,
            string name,
            string label,
            Action action,
            int fontSize = 18)
        {
            GameObject gameObject =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(Button),
                    typeof(LayoutElement));

            RectTransform rect =
                gameObject.GetComponent<
                    RectTransform>();
            rect.SetParent(parent, false);

            Image image =
                gameObject.GetComponent<Image>();
            image.color =
                new Color(
                    0.04f,
                    0.24f,
                    0.14f,
                    0.98f);

            Button button =
                gameObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.navigation =
                new Navigation
                {
                    mode =
                        Navigation.Mode.Automatic
                };

            ColorBlock colors =
                button.colors;
            colors.normalColor =
                new Color(
                    0.04f,
                    0.24f,
                    0.14f,
                    1f);
            colors.highlightedColor =
                new Color(
                    0.05f,
                    0.68f,
                    0.34f,
                    1f);
            colors.selectedColor =
                colors.highlightedColor;
            colors.pressedColor =
                new Color(
                    0.02f,
                    0.42f,
                    0.20f,
                    1f);
            colors.disabledColor =
                new Color(
                    0.16f,
                    0.16f,
                    0.16f,
                    0.55f);
            button.colors = colors;

            if (action != null)
            {
                button.onClick.AddListener(
                    () => action());
            }

            LayoutElement layout =
                gameObject.GetComponent<
                    LayoutElement>();
            layout.preferredHeight = 46f;
            layout.minHeight = 40f;

            Text labelText =
                Text(
                    rect,
                    name + "Label",
                    label,
                    fontSize,
                    TextAnchor.MiddleCenter,
                    Color.white);

            Stretch(
                labelText.rectTransform,
                6f);

            return button;
        }

        public static void AddVerticalLayout(
            GameObject target,
            float spacing,
            RectOffset padding,
            bool fitToContent = true)
        {
            VerticalLayoutGroup layout =
                target.AddComponent<
                    VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childAlignment =
                TextAnchor.UpperCenter;

            if (fitToContent)
            {
                ContentSizeFitter fitter =
                    target.AddComponent<
                        ContentSizeFitter>();
                fitter.verticalFit =
                    ContentSizeFitter.FitMode
                        .PreferredSize;
            }
        }

        public static void AddHorizontalLayout(
            GameObject target,
            float spacing,
            RectOffset padding)
        {
            HorizontalLayoutGroup layout =
                target.AddComponent<
                    HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childAlignment =
                TextAnchor.MiddleCenter;
        }

        public static RectTransform ScrollContent(
            Transform parent,
            string name)
        {
            RectTransform viewport =
                Panel(
                    parent,
                    name + "Viewport",
                    new Color(
                        0.015f,
                        0.03f,
                        0.025f,
                        0.95f));

            Stretch(viewport);

            GameObject scrollObject =
                viewport.gameObject;
            ScrollRect scroll =
                scrollObject.AddComponent<
                    ScrollRect>();

            Mask mask =
                scrollObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject contentObject =
                new GameObject(
                    name + "Content",
                    typeof(RectTransform));

            RectTransform content =
                contentObject.GetComponent<
                    RectTransform>();
            content.SetParent(
                viewport,
                false);
            content.anchorMin =
                new Vector2(0f, 1f);
            content.anchorMax =
                new Vector2(1f, 1f);
            content.pivot =
                new Vector2(0.5f, 1f);
            content.anchoredPosition =
                Vector2.zero;
            content.sizeDelta =
                new Vector2(0f, 0f);

            AddVerticalLayout(
                contentObject,
                8f,
                new RectOffset(
                    10,
                    10,
                    10,
                    10));

            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType =
                ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 24f;

            return content;
        }

        public static void Stretch(
            RectTransform rect,
            float margin = 0f)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin =
                new Vector2(margin, margin);
            rect.offsetMax =
                new Vector2(-margin, -margin);
        }

        public static void Select(
            Selectable selectable)
        {
            if (selectable == null ||
                EventSystem.current == null)
            {
                return;
            }

            EventSystem.current
                .SetSelectedGameObject(
                    selectable.gameObject);
        }

        private static Font Font
        {
            get
            {
                if (_font == null)
                {
                    _font =
                        Resources
                            .GetBuiltinResource<
                                Font>(
                                    "LegacyRuntime.ttf");
                }

                return _font;
            }
        }
    }
}
