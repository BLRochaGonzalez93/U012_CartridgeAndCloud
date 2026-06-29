using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    internal static class Sprint15UiFactory
    {
        private static Font _font;

        public static Canvas CreateCanvas(
            string name,
            int sortingOrder)
        {
            EnsureEventSystem();

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
            scaler.screenMatchMode =
                CanvasScaler.ScreenMatchMode
                    .MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        public static RectTransform CreatePanel(
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

            Image image =
                gameObject.GetComponent<Image>();
            image.color = color;

            return rect;
        }

        public static Text CreateText(
            Transform parent,
            string name,
            string value,
            int fontSize,
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
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow =
                HorizontalWrapMode.Wrap;
            text.verticalOverflow =
                VerticalWrapMode.Overflow;
            text.raycastTarget = false;

            return text;
        }

        public static Button CreateButton(
            Transform parent,
            string name,
            string label,
            Action action,
            int fontSize = 24)
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
                new Color(0.07f, 0.19f, 0.14f, 0.98f);

            Button button =
                gameObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.navigation =
                new Navigation
                {
                    mode = Navigation.Mode.Automatic
                };

            ColorBlock colors =
                button.colors;
            colors.normalColor =
                new Color(0.07f, 0.19f, 0.14f, 1f);
            colors.highlightedColor =
                new Color(0.08f, 0.68f, 0.37f, 1f);
            colors.selectedColor =
                colors.highlightedColor;
            colors.pressedColor =
                new Color(0.02f, 0.42f, 0.20f, 1f);
            colors.disabledColor =
                new Color(0.15f, 0.15f, 0.15f, 0.55f);
            button.colors = colors;

            if (action != null)
            {
                button.onClick.AddListener(
                    () => action());
            }

            LayoutElement layout =
                gameObject.GetComponent<
                    LayoutElement>();
            layout.preferredHeight = 54f;
            layout.minHeight = 44f;

            Text text =
                CreateText(
                    rect,
                    name + "Label",
                    label,
                    fontSize,
                    TextAnchor.MiddleCenter,
                    Color.white);
            Stretch(text.rectTransform, 8f);

            return button;
        }

        public static VerticalLayoutGroup
            AddVerticalLayout(
                GameObject target,
                float spacing,
                RectOffset padding,
                bool childForceExpandHeight = false)
        {
            VerticalLayoutGroup layout =
                target.AddComponent<
                    VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childAlignment =
                TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight =
                childForceExpandHeight;
            return layout;
        }

        public static HorizontalLayoutGroup
            AddHorizontalLayout(
                GameObject target,
                float spacing,
                RectOffset padding)
        {
            HorizontalLayoutGroup layout =
                target.AddComponent<
                    HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childAlignment =
                TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
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

        public static void SetRect(
            RectTransform rect,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 offsetMin,
            Vector2 offsetMax)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }

        public static void Select(Button button)
        {
            if (button == null)
            {
                return;
            }

            if (EventSystem.current != null)
            {
                EventSystem.current
                    .SetSelectedGameObject(
                        button.gameObject);
            }
        }

        private static Font Font
        {
            get
            {
                if (_font == null)
                {
                    _font =
                        Resources
                            .GetBuiltinResource<Font>(
                                "LegacyRuntime.ttf");
                }

                return _font;
            }
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystem =
                new GameObject(
                    "Sprint15EventSystem",
                    typeof(EventSystem));

            UnityEngine.Object
                .DontDestroyOnLoad(eventSystem);
        }
    }
}
