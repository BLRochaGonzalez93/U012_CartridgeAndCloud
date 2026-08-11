using System;
using UnityEngine;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;

namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    /// <summary>
    /// Read-only contextual inspection surface opened from the validated
    /// world-interaction contract. It never mutates gameplay state.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ContextInspectionPanel : MonoBehaviour
    {
        private ContextInspectionProjection _projection;
        private UiInputContextGate _inputGate;

        private Canvas _canvas;
        private GameObject _blocker;
        private Text _title;
        private Text _subtitle;
        private Text _body;
        private bool _isOpen;
        private IWorldInteractionTarget _openTarget;
        private float _openDistance;
        private float _nextRefreshTime;

        public bool IsOpen => _isOpen;

        public void Configure(
            StoreOperationsFacade store,
            IStoreContentCatalog catalog,
            IActiveGameSession activeSession,
            EmployeeHiringService employeeHiring,
            EmployeeScheduleService employeeSchedule,
            EmployeeStateService employeeState,
            UiInputContextGate inputGate)
        {
            _projection = new ContextInspectionProjection(
                store,
                catalog,
                activeSession,
                employeeHiring,
                employeeSchedule,
                employeeState);

            _inputGate = inputGate ??
                throw new ArgumentNullException(nameof(inputGate));

            Build();
        }

        public void Open(
            IWorldInteractionTarget target,
            float distance)
        {
            if (target == null || _projection == null)
            {
                return;
            }

            EnsureBuilt();

            _openTarget = target;
            _openDistance = Mathf.Max(0f, distance);
            RefreshOpenContent();
            _blocker.SetActive(true);

            if (!_isOpen)
            {
                _inputGate.EnterUiExclusive();
            }

            _isOpen = true;
            _nextRefreshTime = Time.unscaledTime + 0.25f;
        }

        public void Close()
        {
            if (!_isOpen)
            {
                return;
            }

            _isOpen = false;
            _openTarget = null;
            _openDistance = 0f;

            if (_blocker != null)
            {
                _blocker.SetActive(false);
            }

            _inputGate?.ExitUiExclusive();
        }

        private void Update()
        {
            if (!_isOpen ||
                Time.unscaledTime < _nextRefreshTime)
            {
                return;
            }

            _nextRefreshTime = Time.unscaledTime + 0.25f;

            if (!IsTargetAlive(_openTarget))
            {
                Close();
                return;
            }

            RefreshOpenContent();
        }

        private void OnDestroy()
        {
            if (_isOpen)
            {
                _inputGate?.ExitUiExclusive();
            }
        }

        private void Build()
        {
            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
            }

            _canvas = ProceduralUiFactory.CreateCanvas(
                "S20A_P02_ContextInspectionCanvas",
                4650);
            _canvas.transform.SetParent(transform, false);

            _blocker = new GameObject(
                "InspectionBlocker",
                typeof(RectTransform),
                typeof(Image));

            RectTransform blockerRect =
                _blocker.GetComponent<RectTransform>();
            blockerRect.SetParent(_canvas.transform, false);
            ProceduralUiFactory.Stretch(blockerRect);

            Image blockerImage = _blocker.GetComponent<Image>();
            blockerImage.color = new Color(0f, 0f, 0f, 0.32f);
            blockerImage.raycastTarget = true;

            RectTransform window =
                ProceduralUiFactory.CreatePanel(
                    blockerRect,
                    "InspectionWindow",
                    new Color(0.018f, 0.055f, 0.04f, 0.985f));

            ProceduralUiFactory.SetRect(
                window,
                new Vector2(0.66f, 0.16f),
                new Vector2(0.965f, 0.84f),
                Vector2.zero,
                Vector2.zero);

            ProceduralUiFactory.AddVerticalLayout(
                window.gameObject,
                10f,
                new RectOffset(18, 18, 16, 16),
                false,
                false);

            _title = ProceduralUiFactory.Text(
                window,
                "InspectionTitle",
                "Inspection",
                27,
                TextAnchor.MiddleLeft,
                new Color(0.35f, 1f, 0.62f, 1f));
            AddHeight(_title.gameObject, 42f);

            _subtitle = ProceduralUiFactory.Text(
                window,
                "InspectionSubtitle",
                string.Empty,
                16,
                TextAnchor.MiddleLeft,
                new Color(0.72f, 0.88f, 0.78f, 1f));
            AddHeight(_subtitle.gameObject, 32f);

            RectTransform separator =
                ProceduralUiFactory.CreatePanel(
                    window,
                    "Separator",
                    new Color(0.20f, 0.58f, 0.36f, 0.8f));
            AddHeight(separator.gameObject, 2f);

            RectTransform bodyPanel =
                ProceduralUiFactory.CreatePanel(
                    window,
                    "InspectionBodyPanel",
                    new Color(0.012f, 0.035f, 0.026f, 0.92f));
            LayoutElement bodyPanelLayout =
                bodyPanel.gameObject.AddComponent<LayoutElement>();
            bodyPanelLayout.flexibleHeight = 1f;
            bodyPanelLayout.minHeight = 280f;

            _body = ProceduralUiFactory.Text(
                bodyPanel,
                "InspectionBody",
                string.Empty,
                18,
                TextAnchor.UpperLeft,
                Color.white);
            ProceduralUiFactory.Stretch(
                _body.GetComponent<RectTransform>(),
                14f);

            Button close = ProceduralUiFactory.Button(
                window,
                "CloseInspection",
                "Close",
                Close,
                18);
            AddHeight(close.gameObject, 46f);

            _blocker.SetActive(false);
        }

        private void RefreshOpenContent()
        {
            if (!IsTargetAlive(_openTarget) ||
                _projection == null)
            {
                return;
            }

            ContextInspectionContent content =
                _projection.Build(
                    _openTarget,
                    _openDistance);

            _title.text = content.Title;
            _subtitle.text = content.Subtitle;
            _body.text = content.Body;
        }

        private void EnsureBuilt()
        {
            if (_canvas == null || _blocker == null)
            {
                Build();
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

        private static void AddHeight(
            GameObject target,
            float preferredHeight)
        {
            LayoutElement layout =
                target.GetComponent<LayoutElement>();
            if (layout == null)
            {
                layout = target.AddComponent<LayoutElement>();
            }

            layout.preferredHeight = preferredHeight;
        }
    }
}
