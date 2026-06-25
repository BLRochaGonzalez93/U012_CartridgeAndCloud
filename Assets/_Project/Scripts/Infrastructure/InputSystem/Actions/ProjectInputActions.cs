using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions
{
    public sealed class ProjectInputActions : IDisposable
    {
        public const string UiMapName = "UI";
        public const string GameplayMapName = "Gameplay";

        public InputActionAsset Asset { get; }

        public InputActionMap UI { get; }
        public InputAction UiPoint { get; }
        public InputAction UiClick { get; }
        public InputAction UiSubmit { get; }
        public InputAction UiCancel { get; }

        public InputActionMap Gameplay { get; }
        public InputAction PointerPosition { get; }
        public InputAction SetDestination { get; }
        public InputAction OrbitDelta { get; }
        public InputAction OrbitHold { get; }
        public InputAction Zoom { get; }
        public InputAction RotatePlacementCounterClockwise { get; }
        public InputAction RotatePlacementClockwise { get; }

        private bool _disposed;

        public ProjectInputActions()
        {
            Asset =
                ScriptableObject.CreateInstance<InputActionAsset>();

            UI = new InputActionMap(UiMapName);

            UiPoint = UI.AddAction(
                "Point",
                InputActionType.PassThrough);
            UiPoint.AddBinding("<Pointer>/position");

            UiClick = UI.AddAction(
                "Click",
                InputActionType.Button);
            UiClick.AddBinding("<Mouse>/leftButton");

            UiSubmit = UI.AddAction(
                "Submit",
                InputActionType.Button);
            UiSubmit.AddBinding("<Keyboard>/enter");
            UiSubmit.AddBinding("<Gamepad>/buttonSouth");

            UiCancel = UI.AddAction(
                "Cancel",
                InputActionType.Button);
            UiCancel.AddBinding("<Keyboard>/escape");
            UiCancel.AddBinding("<Gamepad>/buttonEast");

            Gameplay = new InputActionMap(GameplayMapName);

            PointerPosition = Gameplay.AddAction(
                "PointerPosition",
                InputActionType.PassThrough);
            PointerPosition.AddBinding("<Pointer>/position");

            SetDestination = Gameplay.AddAction(
                "SetDestination",
                InputActionType.Button);
            SetDestination.AddBinding("<Mouse>/leftButton");

            OrbitDelta = Gameplay.AddAction(
                "OrbitDelta",
                InputActionType.PassThrough);
            OrbitDelta.AddBinding("<Pointer>/delta");

            OrbitHold = Gameplay.AddAction(
                "OrbitHold",
                InputActionType.Button);
            OrbitHold.AddBinding("<Mouse>/rightButton");

            Zoom = Gameplay.AddAction(
                "Zoom",
                InputActionType.PassThrough);
            Zoom.AddBinding("<Mouse>/scroll");

            RotatePlacementCounterClockwise =
                Gameplay.AddAction(
                    "RotatePlacementCounterClockwise",
                    InputActionType.Button);
            RotatePlacementCounterClockwise.AddBinding("<Keyboard>/q");

            RotatePlacementClockwise =
                Gameplay.AddAction(
                    "RotatePlacementClockwise",
                    InputActionType.Button);
            RotatePlacementClockwise.AddBinding("<Keyboard>/e");

            Asset.AddActionMap(UI);
            Asset.AddActionMap(Gameplay);
            DisableAll();
        }

        public void DisableAll()
        {
            UI.Disable();
            Gameplay.Disable();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DisableAll();
            _disposed = true;

            if (UnityEngine.Application.isPlaying)
            {
                UnityEngine.Object.Destroy(Asset);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(Asset);
            }
        }
    }
}
