using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Store Shell",
        fileName =
            "CC_S16_P1_StoreShell")]
    public sealed class Phase1StoreShellAsset :
        ScriptableObject
    {
        [SerializeField, Min(1)]
        private int _widthCells = 20;

        [SerializeField, Min(1)]
        private int _depthCells = 30;

        [SerializeField, Min(1)]
        private int _entranceWidthCells = 4;

        [SerializeField, Min(0.01f)]
        private float _cellSize = 0.5f;

        [SerializeField, Min(0.1f)]
        private float _wallHeight = 3f;

        [SerializeField, Min(0.01f)]
        private float _wallThickness = 0.2f;

        [SerializeField, Min(0.5f)]
        private float _backroomDepthMeters = 3f;

        [SerializeField]
        private Vector3 _checkoutZoneSize =
            new Vector3(3f, 0.05f, 2.5f);

        [SerializeField]
        private Vector3 _checkoutZoneOffset =
            new Vector3(2.8f, 0.03f, -4.8f);

        [SerializeField]
        private Vector3 _receivingZoneSize =
            new Vector3(3f, 0.05f, 2.5f);

        [SerializeField]
        private Vector3 _receivingZoneOffset =
            new Vector3(-2.8f, 0.03f, 5.5f);

        [SerializeField]
        private Vector3 _backroomZoneSize =
            new Vector3(9.5f, 0.05f, 2.8f);

        [SerializeField]
        private Vector3 _backroomZoneOffset =
            new Vector3(0f, 0.03f, 5.8f);

        [SerializeField, Min(0.1f)]
        private float _doorOpenDistance = 1.4f;

        [SerializeField, Min(0.1f)]
        private float _doorSpeed = 3.5f;

        public int WidthCells => _widthCells;
        public int DepthCells => _depthCells;
        public int EntranceWidthCells =>
            _entranceWidthCells;
        public float CellSize => _cellSize;
        public float WallHeight => _wallHeight;
        public float WallThickness =>
            _wallThickness;
        public float BackroomDepthMeters =>
            _backroomDepthMeters;
        public Vector3 CheckoutZoneSize =>
            _checkoutZoneSize;
        public Vector3 CheckoutZoneOffset =>
            _checkoutZoneOffset;
        public Vector3 ReceivingZoneSize =>
            _receivingZoneSize;
        public Vector3 ReceivingZoneOffset =>
            _receivingZoneOffset;
        public Vector3 BackroomZoneSize =>
            _backroomZoneSize;
        public Vector3 BackroomZoneOffset =>
            _backroomZoneOffset;
        public float DoorOpenDistance =>
            _doorOpenDistance;
        public float DoorSpeed => _doorSpeed;
    }
}
