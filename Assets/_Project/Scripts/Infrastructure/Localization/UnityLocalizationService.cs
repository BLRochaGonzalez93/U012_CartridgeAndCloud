using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VRMGames.CartridgeAndCloud.Application.Localization;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Localization
{
    public sealed class UnityLocalizationService :
        ILocalizationService
    {
        private const string CatalogResourcePath =
            "Localization/CC_Localization_Runtime";
        private const string SpanishLocaleCode = "es-ES";
        private const string EnglishLocaleCode = "en-US";
        private const string PseudoLocaleCode = "qps-ploc";

        private readonly ILocalePreferenceRepository
            _preferenceRepository;
        private readonly RuntimeLocalizationCatalog _catalog;
        private readonly IReadOnlyList<LocaleOption>
            _availableLocales;

        public string CurrentLocaleCode { get; private set; }

        public IReadOnlyList<LocaleOption> AvailableLocales =>
            _availableLocales;

        public event Action Changed;

        public UnityLocalizationService(
            ILocalePreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository ??
                throw new ArgumentNullException(
                    nameof(preferenceRepository));

            TextAsset catalogAsset =
                Resources.Load<TextAsset>(
                    CatalogResourcePath);
            _catalog = catalogAsset == null
                ? RuntimeLocalizationCatalog.Empty()
                : RuntimeLocalizationCatalog.FromJson(
                    catalogAsset.text);

            _availableLocales =
                new[]
                {
                    new LocaleOption(
                        SpanishLocaleCode,
                        "Español"),
                    new LocaleOption(
                        EnglishLocaleCode,
                        "English")
                };

            LocalizationPreference preference =
                _preferenceRepository.Load() ??
                LocalizationPreference.Empty();

            CurrentLocaleCode = ResolveInitialLocale(
                preference.LocaleCode);
            ApplyUnityLocale(CurrentLocaleCode);
            RuntimeTextLocalizationBridge.Configure(
                Localize);
        }

        public string Localize(string sourceText)
        {
            string source = sourceText ?? string.Empty;
            string table;
            string key;
            if (_catalog.TryGetExactEntry(
                    source,
                    out table,
                    out key))
            {
                string packageValue =
                    TryGetFromUnityTables(table, key);
                if (!string.IsNullOrWhiteSpace(packageValue) &&
                    !string.Equals(
                        packageValue,
                        key,
                        StringComparison.Ordinal))
                {
                    return packageValue;
                }
            }

            return _catalog.Translate(
                source,
                CurrentLocaleCode);
        }

        public string Get(string table, string key)
        {
            string packageValue =
                TryGetFromUnityTables(table, key);
            if (!string.IsNullOrWhiteSpace(packageValue) &&
                !string.Equals(
                    packageValue,
                    key,
                    StringComparison.Ordinal))
            {
                return packageValue;
            }

            return _catalog.Get(
                table,
                key,
                CurrentLocaleCode);
        }

        public bool SelectLocale(string localeCode)
        {
            string normalized = Normalize(localeCode);
            if (!IsSupported(normalized) ||
                string.Equals(
                    CurrentLocaleCode,
                    normalized,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            CurrentLocaleCode = normalized;
            _preferenceRepository.Save(
                new LocalizationPreference(
                    LocalizationPreference
                        .CurrentSchemaVersion,
                    CurrentLocaleCode,
                    true));
            ApplyUnityLocale(CurrentLocaleCode);
            RuntimeTextLocalizationBridge.NotifyChanged();
            Changed?.Invoke();
            return true;
        }

        public string ToggleProductionLocale()
        {
            string next = string.Equals(
                CurrentLocaleCode,
                SpanishLocaleCode,
                StringComparison.OrdinalIgnoreCase)
                ? EnglishLocaleCode
                : SpanishLocaleCode;
            SelectLocale(next);
            return CurrentLocaleCode;
        }

        public string CurrentDisplayName
        {
            get
            {
                LocaleOption locale =
                    _availableLocales.FirstOrDefault(
                        item => string.Equals(
                            item.Code,
                            CurrentLocaleCode,
                            StringComparison.OrdinalIgnoreCase));
                return locale == null
                    ? CurrentLocaleCode
                    : locale.DisplayName;
            }
        }

        private string ResolveInitialLocale(
            string persistedCode)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            try
            {
                Locale selected =
                    LocalizationSettings.SelectedLocale;
                string selectedCode = selected == null
                    ? string.Empty
                    : Normalize(selected.Identifier.Code);
                if (string.Equals(
                        selectedCode,
                        PseudoLocaleCode,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return PseudoLocaleCode;
                }
            }
            catch
            {
                // Unity Localization may not be initialized yet.
            }
#endif

            string persisted = Normalize(persistedCode);
            if (IsSupported(persisted))
            {
                return persisted;
            }

            switch (global::UnityEngine.Application.systemLanguage)
            {
                case SystemLanguage.English:
                    return EnglishLocaleCode;
                case SystemLanguage.Spanish:
                    return SpanishLocaleCode;
                default:
                    return SpanishLocaleCode;
            }
        }

        private static string Normalize(string localeCode)
        {
            if (string.IsNullOrWhiteSpace(localeCode))
            {
                return string.Empty;
            }

            string trimmed = localeCode.Trim();
            if (trimmed.StartsWith(
                    "es",
                    StringComparison.OrdinalIgnoreCase))
            {
                return SpanishLocaleCode;
            }

            if (trimmed.StartsWith(
                    "en",
                    StringComparison.OrdinalIgnoreCase))
            {
                return EnglishLocaleCode;
            }

            if (string.Equals(
                    trimmed,
                    PseudoLocaleCode,
                    StringComparison.OrdinalIgnoreCase))
            {
                return PseudoLocaleCode;
            }

            return trimmed;
        }

        private static bool IsSupported(string localeCode)
        {
            return string.Equals(
                       localeCode,
                       SpanishLocaleCode,
                       StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(
                       localeCode,
                       EnglishLocaleCode,
                       StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(
                       localeCode,
                       PseudoLocaleCode,
                       StringComparison.OrdinalIgnoreCase);
        }

        private static void ApplyUnityLocale(
            string localeCode)
        {
            try
            {
                IReadOnlyList<Locale> locales =
                    LocalizationSettings
                        .AvailableLocales.Locales;
                Locale locale = locales.FirstOrDefault(
                    candidate => string.Equals(
                        candidate.Identifier.Code,
                        localeCode,
                        StringComparison.OrdinalIgnoreCase));
                if (locale != null)
                {
                    LocalizationSettings.SelectedLocale =
                        locale;
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "[Localization] Unity locale could not be " +
                    "selected yet. Runtime catalog fallback remains " +
                    "active. " + exception.Message);
            }
        }

        private static string TryGetFromUnityTables(
            string table,
            string key)
        {
            if (string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            try
            {
                return LocalizationSettings.StringDatabase
                    .GetLocalizedString(table, key);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
