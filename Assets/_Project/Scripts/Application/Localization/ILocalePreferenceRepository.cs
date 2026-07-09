namespace VRMGames.CartridgeAndCloud.Application.Localization
{
    public interface ILocalePreferenceRepository
    {
        LocalizationPreference Load();

        void Save(LocalizationPreference preference);
    }
}
