using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;

namespace VRMGames.CartridgeAndCloud.Presentation.Store
{
    public sealed class StoreShellDescriptor : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private int _widthCells = 20;

        [SerializeField, Min(1)]
        private int _depthCells = 30;

        [SerializeField, Min(1)]
        private int _entranceWidthCells = 4;

        [SerializeField, Min(0.01f)]
        private float _cellSize = 0.5f;

        [SerializeField, Min(0.01f)]
        private float _wallHeight = 3f;

        [SerializeField, Min(0.01f)]
        private float _wallThickness = 0.2f;

        [SerializeField]
        private Transform _walkableFloor;

        [SerializeField]
        private Transform _entranceThreshold;

        [SerializeField]
        private Transform _playerSpawn;

        [SerializeField]
        private Transform _technicalPlayer;

        public StoreShellSpecification Specification =>
            new StoreShellSpecification(
                _widthCells,
                _depthCells,
                _entranceWidthCells,
                _cellSize,
                _wallHeight,
                _wallThickness);

        public Transform WalkableFloor =>
            _walkableFloor;

        public Transform EntranceThreshold =>
            _entranceThreshold;

        public Transform PlayerSpawn =>
            _playerSpawn;

        public Transform TechnicalPlayer =>
            _technicalPlayer;

        public bool IsConfigured =>
            _walkableFloor != null &&
            _entranceThreshold != null &&
            _playerSpawn != null &&
            _technicalPlayer != null;

        public void Configure(
            StoreShellSpecification specification,
            Transform walkableFloor,
            Transform entranceThreshold,
            Transform playerSpawn,
            Transform technicalPlayer)
        {
            _widthCells =
                specification.WidthCells;

            _depthCells =
                specification.DepthCells;

            _entranceWidthCells =
                specification.EntranceWidthCells;

            _cellSize =
                specification.CellSize;

            _wallHeight =
                specification.WallHeight;

            _wallThickness =
                specification.WallThickness;

            _walkableFloor =
                walkableFloor;

            _entranceThreshold =
                entranceThreshold;

            _playerSpawn =
                playerSpawn;

            _technicalPlayer =
                technicalPlayer;
        }
    }
}
