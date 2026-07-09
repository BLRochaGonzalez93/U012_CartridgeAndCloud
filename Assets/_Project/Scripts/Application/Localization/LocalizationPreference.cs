using System;

namespace VRMGames.CartridgeAndCloud.Application.Localization
{
    public sealed class LocalizationPreference
    {
        public const int CurrentSchemaVersion = 1;

        public int SchemaVersion { get; }
        public string LocaleCode { get; }
        public bool WasExplicitlySelected { get; }

        public LocalizationPreference(
            int schemaVersion,
            string localeCode,
            bool wasExplicitlySelected)
        {
            if (schemaVersion <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(schemaVersion));
            }

            SchemaVersion = schemaVersion;
            LocaleCode = localeCode == null
                ? string.Empty
                : localeCode.Trim();
            WasExplicitlySelected = wasExplicitlySelected;
        }

        public static LocalizationPreference Empty()
        {
            return new LocalizationPreference(
                CurrentSchemaVersion,
                string.Empty,
                false);
        }
    }
}
