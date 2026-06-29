using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public interface IAccessibilitySettingsRepository
    {
        UiAccessibilitySettings Load();
        void Save(UiAccessibilitySettings settings);
    }

    public interface ITutorialProgressRepository
    {
        TutorialProgress Load(SaveSlotId slotId);
        void Save(
            SaveSlotId slotId,
            TutorialProgress progress);
        bool Delete(SaveSlotId slotId);
    }


    public interface IAutosaveMarkerRepository
    {
        string LoadLastSavedDay(
            SaveSlotId slotId);

        void SaveLastSavedDay(
            SaveSlotId slotId,
            string dayId);

        bool Delete(SaveSlotId slotId);
    }
}
