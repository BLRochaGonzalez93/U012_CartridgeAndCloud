using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    [CreateAssetMenu(
        fileName = "TechnicalPlaceableDefinition",
        menuName = "Cartridge & Cloud/Placement/Technical Placeable")]
    public sealed class TechnicalPlaceableDefinition : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int _widthCells = 1;

        [SerializeField, Min(1)]
        private int _depthCells = 1;

        [SerializeField, Min(0.01f)]
        private float _previewHeight = 1f;

        public int WidthCells => _widthCells;
        public int DepthCells => _depthCells;
        public float PreviewHeight => _previewHeight;

        public GridSize GridSize => new GridSize(_widthCells, _depthCells);

        public GridFootprint CreateFootprint()
        {
            return new GridFootprint(GridSize);
        }

        public void Configure(
            int widthCells,
            int depthCells,
            float previewHeight)
        {
            if (widthCells <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(widthCells),
                    widthCells,
                    "Width must be greater than zero.");
            }

            if (depthCells <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depthCells),
                    depthCells,
                    "Depth must be greater than zero.");
            }

            if (previewHeight <= 0f ||
                float.IsNaN(previewHeight) ||
                float.IsInfinity(previewHeight))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(previewHeight),
                    previewHeight,
                    "Preview height must be finite and greater than zero.");
            }

            _widthCells = widthCells;
            _depthCells = depthCells;
            _previewHeight = previewHeight;
        }

        private void OnValidate()
        {
            _widthCells = Mathf.Max(1, _widthCells);
            _depthCells = Mathf.Max(1, _depthCells);

            if (_previewHeight <= 0f ||
                float.IsNaN(_previewHeight) ||
                float.IsInfinity(_previewHeight))
            {
                _previewHeight = 1f;
            }
        }
    }
}
