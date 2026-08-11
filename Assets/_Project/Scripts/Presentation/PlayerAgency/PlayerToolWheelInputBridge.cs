using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.PlayerAgency
{
    /// <summary>
    /// Decouples the Input System assembly from the runtime radial UI. The
    /// bridge carries only input intent and contains no tool-selection rules.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PlayerToolWheelInputBridge : MonoBehaviour
    {
        public bool IsOpen { get; private set; }

        public event Action<Vector2, Vector2> Opened;
        public event Action<Vector2, Vector2> Updated;
        public event Action<Vector2, Vector2> Released;
        public event Action Cancelled;

        public void ApplyInput(
            bool pressed,
            bool held,
            bool released,
            Vector2 pointerPosition,
            Vector2 navigation)
        {
            if (pressed && !IsOpen)
            {
                IsOpen = true;
                Opened?.Invoke(
                    pointerPosition,
                    navigation);
            }

            if (IsOpen && held)
            {
                Updated?.Invoke(
                    pointerPosition,
                    navigation);
            }

            if (released && IsOpen)
            {
                IsOpen = false;
                Released?.Invoke(
                    pointerPosition,
                    navigation);
            }
        }

        private void OnDisable()
        {
            Cancel();
        }

        public void Cancel()
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
            Cancelled?.Invoke();
        }
    }
}
