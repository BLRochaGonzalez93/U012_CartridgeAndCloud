using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Persistence
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Persistence/Save Integration Settings",
        fileName = "CC_SaveIntegrationSettings_")]
    public sealed class SaveIntegrationSettingsAsset :
        ScriptableObject
    {
        [SerializeField]
        private string _technicalDirectoryName =
            "CC_S14_SaveRecovery";

        [SerializeField, Range(
            SaveSlotId.MinimumValue,
            SaveSlotId.MaximumValue)]
        private int _technicalSlot;

        public string TechnicalDirectoryName =>
            _technicalDirectoryName;

        public SaveSlotId TechnicalSlot =>
            new SaveSlotId(_technicalSlot);

        public void Configure(
            string technicalDirectoryName,
            int technicalSlot)
        {
            _technicalDirectoryName =
                technicalDirectoryName;
            _technicalSlot = technicalSlot;
        }
    }
}
