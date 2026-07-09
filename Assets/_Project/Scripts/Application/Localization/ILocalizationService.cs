using System;
using System.Collections.Generic;

namespace VRMGames.CartridgeAndCloud.Application.Localization
{
    public interface ILocalizationService
    {
        string CurrentLocaleCode { get; }

        IReadOnlyList<LocaleOption> AvailableLocales { get; }

        event Action Changed;

        string Localize(string sourceText);

        string Get(string table, string key);

        bool SelectLocale(string localeCode);
    }
}
