using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Suppliers
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Suppliers/Supplier Definition",
        fileName = "CC_Supplier_")]
    public sealed class SupplierDefinitionAsset : ScriptableObject
    {
        [SerializeField]
        private string _supplierId;

        [SerializeField]
        private string _displayNameKey;

        public string SupplierId => _supplierId;

        public string DisplayNameKey => _displayNameKey;

        public SupplierDefinition BuildDefinition()
        {
            return new SupplierDefinition(
                new SupplierId(_supplierId),
                _displayNameKey);
        }

        public void Configure(
            string supplierId,
            string displayNameKey)
        {
            _supplierId = supplierId;
            _displayNameKey = displayNameKey;
        }
    }
}
