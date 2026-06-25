using UnityEngine;
using UnityEngine.InputSystem;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.PlayerMovement
{
    [RequireComponent(typeof(ClickDestinationInput))]
    public sealed class MouseClickDestinationDriver : MonoBehaviour
    {
        private ClickDestinationInput _destinationInput;

        private void Awake()
        {
            _destinationInput = GetComponent<ClickDestinationInput>();
        }

        private void Update()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;
            _destinationInput.TrySetDestinationFromScreenPosition(Mouse.current.position.ReadValue());
        }
    }
}
