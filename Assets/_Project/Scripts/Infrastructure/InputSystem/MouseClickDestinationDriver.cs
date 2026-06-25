using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.PlayerMovement
{
    [RequireComponent(typeof(ClickDestinationInput))]
    public sealed class MouseClickDestinationDriver : MonoBehaviour
    {
        private ClickDestinationInput _destinationInput;

        private void Awake()
        {
            _destinationInput =
                GetComponent<ClickDestinationInput>();
        }

        public bool TrySetDestinationFromScreenPosition(
            Vector2 screenPosition)
        {
            if (_destinationInput == null)
            {
                _destinationInput =
                    GetComponent<ClickDestinationInput>();
            }

            return _destinationInput != null &&
                   _destinationInput
                       .TrySetDestinationFromScreenPosition(
                           screenPosition);
        }
    }
}
