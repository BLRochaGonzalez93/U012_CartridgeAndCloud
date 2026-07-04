using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Doors
{
    public sealed class AutomaticDoorParts :
        MonoBehaviour
    {
        [SerializeField]
        private Transform _leftPanel;

        [SerializeField]
        private Transform _rightPanel;

        public Transform LeftPanel =>
            _leftPanel;

        public Transform RightPanel =>
            _rightPanel;

        public void Configure(
            Transform leftPanel,
            Transform rightPanel)
        {
            _leftPanel = leftPanel;
            _rightPanel = rightPanel;
        }
    }
}
