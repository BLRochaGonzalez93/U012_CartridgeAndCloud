using System;

namespace VRMGames.CartridgeAndCloud.Application.Localization
{
    public sealed class LocaleOption
    {
        public string Code { get; }
        public string DisplayName { get; }
        public bool IsPseudoLocale { get; }

        public LocaleOption(
            string code,
            string displayName,
            bool isPseudoLocale = false)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(
                    "A locale code is required.",
                    nameof(code));
            }

            Code = code.Trim();
            DisplayName = string.IsNullOrWhiteSpace(displayName)
                ? Code
                : displayName.Trim();
            IsPseudoLocale = isPseudoLocale;
        }
    }
}
