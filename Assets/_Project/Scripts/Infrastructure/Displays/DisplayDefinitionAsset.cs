using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Displays
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Displays/Display Definition",
        fileName = "CC_Display_")]
    public sealed class DisplayDefinitionAsset : ScriptableObject
    {
        [SerializeField]
        private string _displayDefinitionId;

        [SerializeField]
        private string _displayNameKey;

        [SerializeField]
        [Min(1)]
        private int _capacityUnits = 1;

        [SerializeField]
        [Min(1)]
        private int _visibleUnitLimit = 1;

        [SerializeField]
        private List<string> _allowedCategoryIds =
            new List<string>();

        [SerializeField]
        private string _placementDefinitionId;

        [SerializeField]
        private GameObject _displayPrefab;

        public string DisplayDefinitionId =>
            _displayDefinitionId;

        public string DisplayNameKey => _displayNameKey;

        public int CapacityUnits => _capacityUnits;

        public int VisibleUnitLimit => _visibleUnitLimit;

        public IReadOnlyList<string> AllowedCategoryIds =>
            _allowedCategoryIds;

        public string PlacementDefinitionId =>
            _placementDefinitionId;

        public GameObject DisplayPrefab => _displayPrefab;

        public DisplayDefinition BuildDefinition()
        {
            List<ProductCategoryId> categories =
                new List<ProductCategoryId>(
                    _allowedCategoryIds.Count);

            foreach (string category in _allowedCategoryIds)
            {
                categories.Add(
                    new ProductCategoryId(category));
            }

            return new DisplayDefinition(
                new DisplayDefinitionId(_displayDefinitionId),
                _displayNameKey,
                new InventoryCapacity(_capacityUnits),
                _visibleUnitLimit,
                categories,
                _placementDefinitionId);
        }

        public void Configure(
            string displayDefinitionId,
            string displayNameKey,
            int capacityUnits,
            int visibleUnitLimit,
            IEnumerable<string> allowedCategoryIds,
            string placementDefinitionId,
            GameObject displayPrefab = null)
        {
            if (allowedCategoryIds == null)
            {
                throw new ArgumentNullException(
                    nameof(allowedCategoryIds));
            }

            _displayDefinitionId = displayDefinitionId;
            _displayNameKey = displayNameKey;
            _capacityUnits = capacityUnits;
            _visibleUnitLimit = visibleUnitLimit;
            _allowedCategoryIds =
                new List<string>(allowedCategoryIds);
            _placementDefinitionId = placementDefinitionId;
            _displayPrefab = displayPrefab;
        }
    }
}
