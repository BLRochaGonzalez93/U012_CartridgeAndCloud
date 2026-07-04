using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public static class ProceduralUiFactory
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

            Canvas canvas = gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler =
                gameObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode =
                CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution =
                new Vector2(1920f, 1080f);
            scaler.screenMatchMode =
                CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
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
                gameObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            gameObject.GetComponent<Image>().color = color;
            return rect;
        }

        public static RectTransform Panel(
            Transform parent,
            string name,
            Color color)
        {
            return CreatePanel(parent, name, color);
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
                gameObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            Text text = gameObject.GetComponent<Text>();
            text.font = Font;
            text.text = value ?? string.Empty;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        public static Text Text(
            Transform parent,
            string name,
            string value,
            int fontSize,
            TextAnchor alignment,
            Color color)
        {
            return CreateText(
                parent,
                name,
                value,
                fontSize,
                alignment,
                color);
        }

        public static Button CreateButton(
            Transform parent,
            string name,
            string label,
            Action action,
            int fontSize = 24)
        {
            return CreateButtonCore(
                parent,
                name,
                label,
                action,
                fontSize,
                54f,
                44f,
                new Color(0.07f, 0.19f, 0.14f, 0.98f),
                8f);
        }

        public static Button Button(
            Transform parent,
            string name,
            string label,
            Action action,
            int fontSize = 18)
        {
            return CreateButtonCore(
                parent,
                name,
                label,
                action,
                fontSize,
                46f,
                40f,
                new Color(0.04f, 0.24f, 0.14f, 0.98f),
                6f);
        }

        public static VerticalLayoutGroup AddVerticalLayout(
            GameObject target,
            float spacing,
            RectOffset padding,
            bool fitToContent = false,
            bool childForceExpandHeight = false)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            VerticalLayoutGroup layout =
                target.GetComponent<VerticalLayoutGroup>();

            if (layout == null)
            {
                layout = target.AddComponent<VerticalLayoutGroup>();
            }

            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset();
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight =
                childForceExpandHeight;

            if (fitToContent &&
                target.GetComponent<ContentSizeFitter>() == null)
            {
                ContentSizeFitter fitter =
                    target.AddComponent<ContentSizeFitter>();
                fitter.verticalFit =
                    ContentSizeFitter.FitMode.PreferredSize;
            }

            return layout;
        }

        public static HorizontalLayoutGroup AddHorizontalLayout(
            GameObject target,
            float spacing,
            RectOffset padding)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            HorizontalLayoutGroup layout =
                target.GetComponent<HorizontalLayoutGroup>();

            if (layout == null)
            {
                layout = target.AddComponent<HorizontalLayoutGroup>();
            }

            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
        }

        public static RectTransform ScrollContent(
            Transform parent,
            string name)
        {
            RectTransform viewport =
                CreatePanel(
                    parent,
                    name + "Viewport",
                    new Color(
                        0.015f,
                        0.03f,
                        0.025f,
                        0.95f));

            Stretch(viewport);

            ScrollRect scroll =
                viewport.gameObject.AddComponent<ScrollRect>();
            Mask mask =
                viewport.gameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject contentObject =
                new GameObject(
                    name + "Content",
                    typeof(RectTransform));

            RectTransform content =
                contentObject.GetComponent<RectTransform>();
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;

            AddVerticalLayout(
                contentObject,
                8f,
                new RectOffset(10, 10, 10, 10),
                fitToContent: true);

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
            if (rect == null)
            {
                throw new ArgumentNullException(nameof(rect));
            }

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(margin, margin);
            rect.offsetMax = new Vector2(-margin, -margin);
        }

        public static void SetRect(
            RectTransform rect,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 offsetMin,
            Vector2 offsetMax)
        {
            if (rect == null)
            {
                throw new ArgumentNullException(nameof(rect));
            }

            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }

        public static void Select(Selectable selectable)
        {
            if (selectable == null)
            {
                return;
            }

            EnsureEventSystem();

            EventSystem.current.SetSelectedGameObject(
                selectable.gameObject);
        }

        private static Button CreateButtonCore(
            Transform parent,
            string name,
            string label,
            Action action,
            int fontSize,
            float preferredHeight,
            float minimumHeight,
            Color background,
            float labelMargin)
        {
            GameObject gameObject =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(Button),
                    typeof(LayoutElement));

            RectTransform rect =
                gameObject.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            Image image = gameObject.GetComponent<Image>();
            image.color = background;

            Button button = gameObject.GetComponent<Button>();
            button.targetGraphic = image;
            button.navigation =
                new Navigation
                {
                    mode = Navigation.Mode.Automatic
                };

            ColorBlock colors = button.colors;
            colors.normalColor = background;
            colors.highlightedColor =
                new Color(0.08f, 0.68f, 0.37f, 1f);
            colors.selectedColor = colors.highlightedColor;
            colors.pressedColor =
                new Color(0.02f, 0.42f, 0.20f, 1f);
            colors.disabledColor =
                new Color(0.15f, 0.15f, 0.15f, 0.55f);
            button.colors = colors;

            if (action != null)
            {
                button.onClick.AddListener(() => action());
            }

            LayoutElement layout =
                gameObject.GetComponent<LayoutElement>();
            layout.preferredHeight = preferredHeight;
            layout.minHeight = minimumHeight;

            Text labelText =
                CreateText(
                    rect,
                    name + "Label",
                    label,
                    fontSize,
                    TextAnchor.MiddleCenter,
                    Color.white);

            Stretch(labelText.rectTransform, labelMargin);
            return button;
        }

        private static Font Font
        {
            get
            {
                if (_font == null)
                {
                    _font =
                        Resources.GetBuiltinResource<Font>(
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
                    "RuntimeEventSystem",
                    typeof(EventSystem));

            UnityEngine.Object.DontDestroyOnLoad(
                eventSystem);
        }
    }
}
