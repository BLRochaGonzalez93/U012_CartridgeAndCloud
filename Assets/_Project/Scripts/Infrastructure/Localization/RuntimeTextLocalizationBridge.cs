using System;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Localization
{
    public static class RuntimeTextLocalizationBridge
    {
        private static Func<string, string> _resolver =
            value => value ?? string.Empty;
        private static int _version;

        public static int Version => _version;

        public static string Localize(string sourceText)
        {
            try
            {
                return _resolver(sourceText ?? string.Empty) ??
                    string.Empty;
            }
            catch
            {
                return sourceText ?? string.Empty;
            }
        }

        public static void Configure(
            Func<string, string> resolver)
        {
            _resolver = resolver ??
                (value => value ?? string.Empty);
            unchecked
            {
                _version++;
            }
        }

        public static void NotifyChanged()
        {
            unchecked
            {
                _version++;
            }
        }
    }
}
