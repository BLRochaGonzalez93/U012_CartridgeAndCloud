using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Localization;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Localization
{
    public sealed class JsonLocalePreferenceRepository :
        ILocalePreferenceRepository
    {
        private readonly string _path;

        public JsonLocalePreferenceRepository(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "A locale preference path is required.",
                    nameof(path));
            }

            _path = path;
        }

        public LocalizationPreference Load()
        {
            if (!File.Exists(_path))
            {
                return LocalizationPreference.Empty();
            }

            try
            {
                PreferenceDto dto =
                    JsonUtility.FromJson<PreferenceDto>(
                        File.ReadAllText(_path));

                if (dto == null ||
                    dto.schemaVersion <= 0)
                {
                    return LocalizationPreference.Empty();
                }

                return new LocalizationPreference(
                    dto.schemaVersion,
                    dto.localeCode,
                    dto.wasExplicitlySelected);
            }
            catch
            {
                return LocalizationPreference.Empty();
            }
        }

        public void Save(LocalizationPreference preference)
        {
            if (preference == null)
            {
                throw new ArgumentNullException(
                    nameof(preference));
            }

            AtomicJsonFile.Write(
                _path,
                JsonUtility.ToJson(
                    new PreferenceDto
                    {
                        schemaVersion = preference.SchemaVersion,
                        localeCode = preference.LocaleCode,
                        wasExplicitlySelected =
                            preference.WasExplicitlySelected
                    },
                    true));
        }

        [Serializable]
        private sealed class PreferenceDto
        {
            public int schemaVersion;
            public string localeCode;
            public bool wasExplicitlySelected;
        }
    }
}
