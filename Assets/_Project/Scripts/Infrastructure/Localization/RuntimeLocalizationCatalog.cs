using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Localization
{
    public sealed class RuntimeLocalizationCatalog
    {
        private static readonly Regex MoneyPattern =
            new Regex(
                @"(?<![\w])(-?\d+)\.(\d{2})\s+EUR\b",
                RegexOptions.CultureInvariant);
        private static readonly Regex DatePattern =
            new Regex(
                @"\b(\d{4})-(\d{2})-(\d{2})\s+(\d{2}):(\d{2})\s+UTC\b",
                RegexOptions.CultureInvariant);
        private static readonly Regex TechnicalTokenPattern =
            new Regex(
                @"(?<![\p{L}\p{N}_])[\p{L}\p{N}_]+(?:-[\p{L}\p{N}_]+)+(?![\p{L}\p{N}_])",
                RegexOptions.CultureInvariant);

        private readonly Dictionary<string, CatalogEntry>
            _entriesByIdentity;
        private readonly Dictionary<string, CatalogEntry>
            _entriesByEnglish;
        private readonly List<TemplateEntry> _templates;
        private readonly List<PhraseEntry> _phrases;
        private readonly List<AliasEntry> _aliases;

        private RuntimeLocalizationCatalog(
            IEnumerable<CatalogEntry> entries)
        {
            _entriesByIdentity =
                new Dictionary<string, CatalogEntry>(
                    StringComparer.Ordinal);
            _entriesByEnglish =
                new Dictionary<string, CatalogEntry>(
                    StringComparer.OrdinalIgnoreCase);
            _templates = new List<TemplateEntry>();
            _phrases = new List<PhraseEntry>();
            _aliases = new List<AliasEntry>();

            foreach (CatalogEntry entry in entries)
            {
                if (entry == null ||
                    IsRemoved(entry.status) ||
                    string.IsNullOrWhiteSpace(entry.table) ||
                    string.IsNullOrWhiteSpace(entry.key))
                {
                    continue;
                }

                _entriesByIdentity[Identity(
                    entry.table,
                    entry.key)] = entry;

                if (!string.IsNullOrEmpty(entry.enUS) &&
                    !_entriesByEnglish.ContainsKey(entry.enUS))
                {
                    _entriesByEnglish.Add(entry.enUS, entry);
                }

                if (entry.sourceAliases != null)
                {
                    for (int aliasIndex = 0;
                         aliasIndex < entry.sourceAliases.Length;
                         aliasIndex++)
                    {
                        string alias =
                            entry.sourceAliases[aliasIndex];
                        if (string.IsNullOrWhiteSpace(alias))
                        {
                            continue;
                        }

                        _entriesByEnglish[alias] = entry;
                        AliasEntry aliasEntry =
                            AliasEntry.TryCreate(alias, entry);
                        if (aliasEntry != null)
                        {
                            _aliases.Add(aliasEntry);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entry.enUS) &&
                    entry.enUS.IndexOf('{') >= 0)
                {
                    TemplateEntry template =
                        TemplateEntry.TryCreate(entry);
                    if (template != null)
                    {
                        _templates.Add(template);
                    }
                }
                else if (!string.IsNullOrEmpty(entry.enUS))
                {
                    PhraseEntry phrase =
                        PhraseEntry.TryCreate(entry);
                    if (phrase != null)
                    {
                        _phrases.Add(phrase);
                    }
                }
            }

            _templates.Sort((left, right) =>
                right.LiteralCharacterCount.CompareTo(
                    left.LiteralCharacterCount));
            _phrases.Sort((left, right) =>
                right.Source.Length.CompareTo(
                    left.Source.Length));
            _aliases.Sort((left, right) =>
                right.Source.Length.CompareTo(
                    left.Source.Length));
        }

        public static RuntimeLocalizationCatalog Empty()
        {
            return new RuntimeLocalizationCatalog(
                new CatalogEntry[0]);
        }

        public static RuntimeLocalizationCatalog FromJson(
            string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Empty();
            }

            CatalogDocument document =
                JsonUtility.FromJson<CatalogDocument>(json);
            return new RuntimeLocalizationCatalog(
                document == null ||
                document.entries == null
                    ? new CatalogEntry[0]
                    : document.entries);
        }

        public string Get(
            string table,
            string key,
            string localeCode)
        {
            CatalogEntry entry;
            if (!_entriesByIdentity.TryGetValue(
                    Identity(table, key),
                    out entry))
            {
                return key ?? string.Empty;
            }

            return PostProcess(
                Value(entry, localeCode),
                localeCode);
        }

        public bool TryGetExactEntry(
            string sourceText,
            out string table,
            out string key)
        {
            CatalogEntry entry;
            if (_entriesByEnglish.TryGetValue(
                    sourceText ?? string.Empty,
                    out entry))
            {
                table = entry.table;
                key = entry.key;
                return true;
            }

            table = string.Empty;
            key = string.Empty;
            return false;
        }

        public string Translate(
            string sourceText,
            string localeCode)
        {
            return TranslateInternal(
                sourceText ?? string.Empty,
                localeCode,
                0);
        }

        private string TranslateInternal(
            string sourceText,
            string localeCode,
            int depth)
        {
            if (string.IsNullOrEmpty(sourceText))
            {
                return string.Empty;
            }

            if (IsPseudo(localeCode))
            {
                return PseudoLocalize(sourceText);
            }

            CatalogEntry exact;
            if (_entriesByEnglish.TryGetValue(
                    sourceText,
                    out exact))
            {
                return PostProcess(
                    Value(exact, localeCode),
                    localeCode);
            }

            if (depth < 3 && ContainsLineBreak(sourceText))
            {
                string[] pieces = Regex.Split(
                    sourceText,
                    "(\\r\\n|\\n|\\r)");
                for (int index = 0;
                     index < pieces.Length;
                     index++)
                {
                    if (pieces[index] == "\r\n" ||
                        pieces[index] == "\n" ||
                        pieces[index] == "\r")
                    {
                        continue;
                    }

                    pieces[index] = TranslateInternal(
                        pieces[index],
                        localeCode,
                        depth + 1);
                }

                return string.Concat(pieces);
            }

            if (depth < 3)
            {
                for (int index = 0;
                     index < _templates.Count;
                     index++)
                {
                    TemplateEntry template =
                        _templates[index];
                    Match match =
                        template.Pattern.Match(sourceText);
                    if (!match.Success)
                    {
                        continue;
                    }

                    string target =
                        Value(template.Entry, localeCode);
                    for (int placeholderIndex = 0;
                         placeholderIndex <
                         template.Placeholders.Count;
                         placeholderIndex++)
                    {
                        string placeholder =
                            template.Placeholders[
                                placeholderIndex];
                        string captured =
                            match.Groups[
                                "p" + placeholderIndex]
                                .Value;
                        string localizedCaptured =
                            TranslateInternal(
                                captured,
                                localeCode,
                                depth + 1);
                        target = target.Replace(
                            "{" + placeholder + "}",
                            localizedCaptured);
                    }

                    return PostProcess(
                        target,
                        localeCode);
                }
            }

            string composite = TranslatePhrases(
                sourceText,
                localeCode);
            return PostProcess(
                composite,
                localeCode);
        }

        private string TranslatePhrases(
            string sourceText,
            string localeCode)
        {
            string output = TranslateAliases(
                sourceText,
                localeCode);
            List<string> protectedTokens =
                new List<string>();
            output = TechnicalTokenPattern.Replace(
                output,
                match =>
                {
                    CatalogEntry knownEntry;
                    if (_entriesByEnglish.TryGetValue(
                            match.Value,
                            out knownEntry))
                    {
                        return Value(
                            knownEntry,
                            localeCode);
                    }

                    string localizedIdentifier =
                        LocalizeTechnicalIdentifier(
                            match.Value,
                            localeCode);
                    if (!string.IsNullOrEmpty(
                            localizedIdentifier))
                    {
                        return localizedIdentifier;
                    }

                    int tokenIndex = protectedTokens.Count;
                    protectedTokens.Add(match.Value);
                    return "\uE000" + tokenIndex + "\uE001";
                });

            if (string.Equals(
                    localeCode,
                    "es-ES",
                    StringComparison.OrdinalIgnoreCase))
            {
                for (int index = 0;
                     index < _phrases.Count;
                     index++)
                {
                    PhraseEntry phrase = _phrases[index];
                    output = phrase.Pattern.Replace(
                        output,
                        match => phrase.Target);
                }
            }

            for (int index = 0;
                 index < protectedTokens.Count;
                 index++)
            {
                output = output.Replace(
                    "\uE000" + index + "\uE001",
                    protectedTokens[index]);
            }

            return output;
        }

        private static string LocalizeTechnicalIdentifier(
            string identifier,
            string localeCode)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return string.Empty;
            }

            bool spanish = string.Equals(
                localeCode,
                "es-ES",
                StringComparison.OrdinalIgnoreCase);
            string value = identifier.ToLowerInvariant();

            if (value == "store-inventory")
            {
                return spanish
                    ? "Inventario de la tienda"
                    : "Store Inventory";
            }

            if (value == "backroom-inventory")
            {
                return spanish
                    ? "Inventario del almacén"
                    : "Backroom Inventory";
            }

            if (value == "checkout-station-main" ||
                value == "checkout-station-technical")
            {
                return spanish
                    ? "Caja principal"
                    : "Main Checkout";
            }

            string sequence;
            if (TryGetTechnicalSequence(
                    value,
                    "day-",
                    out sequence))
            {
                return (spanish ? "Día " : "Day ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-order-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "order-",
                    out sequence))
            {
                return (spanish ? "Pedido " : "Order ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "delivery-run-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "delivery-",
                    out sequence))
            {
                return (spanish ? "Entrega " : "Delivery ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-transaction-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "transaction-",
                    out sequence))
            {
                return (spanish
                    ? "Transacción "
                    : "Transaction ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-display-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "display-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "fixture-",
                    out sequence))
            {
                return (spanish
                    ? "Expositor "
                    : "Display ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-customer-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "customer-",
                    out sequence))
            {
                return (spanish ? "Cliente " : "Customer ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-cart-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "cart-",
                    out sequence))
            {
                return (spanish ? "Carrito " : "Cart ") +
                    FormatTechnicalSequence(sequence);
            }

            if (TryGetTechnicalSequence(
                    value,
                    "phase1-reservation-",
                    out sequence) ||
                TryGetTechnicalSequence(
                    value,
                    "reservation-",
                    out sequence))
            {
                return (spanish
                    ? "Reserva "
                    : "Reservation ") +
                    FormatTechnicalSequence(sequence);
            }

            return string.Empty;
        }

        private static bool TryGetTechnicalSequence(
            string identifier,
            string prefix,
            out string sequence)
        {
            if (identifier.StartsWith(
                    prefix,
                    StringComparison.OrdinalIgnoreCase) &&
                identifier.Length > prefix.Length)
            {
                sequence = identifier.Substring(prefix.Length);
                return true;
            }

            sequence = string.Empty;
            return false;
        }

        private static string FormatTechnicalSequence(
            string sequence)
        {
            int numericSequence;
            if (int.TryParse(
                    sequence,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out numericSequence))
            {
                return numericSequence.ToString(
                    CultureInfo.InvariantCulture);
            }

            return sequence.Replace('-', ' ');
        }

        private string TranslateAliases(
            string sourceText,
            string localeCode)
        {
            string output = sourceText;
            for (int index = 0;
                 index < _aliases.Count;
                 index++)
            {
                AliasEntry alias = _aliases[index];
                output = alias.Pattern.Replace(
                    output,
                    match => Value(alias.Entry, localeCode));
            }

            return output;
        }

        private static bool ContainsLineBreak(string value)
        {
            return value.IndexOf('\n') >= 0 ||
                value.IndexOf('\r') >= 0;
        }

        private static string Value(
            CatalogEntry entry,
            string localeCode)
        {
            if (string.Equals(
                    localeCode,
                    "es-ES",
                    StringComparison.OrdinalIgnoreCase))
            {
                return entry.esES ?? entry.enUS ??
                    entry.key ?? string.Empty;
            }

            return entry.enUS ?? entry.key ??
                string.Empty;
        }

        private static string PostProcess(
            string value,
            string localeCode)
        {
            string output = value ?? string.Empty;
            bool spanish = string.Equals(
                localeCode,
                "es-ES",
                StringComparison.OrdinalIgnoreCase);

            output = MoneyPattern.Replace(
                output,
                match =>
                {
                    decimal amount;
                    if (!decimal.TryParse(
                            match.Groups[1].Value + "." +
                            match.Groups[2].Value,
                            NumberStyles.Number |
                            NumberStyles.AllowLeadingSign,
                            CultureInfo.InvariantCulture,
                            out amount))
                    {
                        return match.Value;
                    }

                    CultureInfo culture =
                        CultureInfo.GetCultureInfo(
                            spanish ? "es-ES" : "en-US");
                    return spanish
                        ? amount.ToString("N2", culture) + " €"
                        : "€" + amount.ToString("N2", culture);
                });

            output = DatePattern.Replace(
                output,
                match =>
                {
                    DateTime date;
                    if (!DateTime.TryParseExact(
                            match.Value,
                            "yyyy-MM-dd HH:mm 'UTC'",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal |
                            DateTimeStyles.AdjustToUniversal,
                            out date))
                    {
                        return match.Value;
                    }

                    return spanish
                        ? date.ToString(
                            "dd/MM/yyyy HH:mm 'UTC'",
                            CultureInfo.GetCultureInfo("es-ES"))
                        : date.ToString(
                            "MM/dd/yyyy hh:mm tt 'UTC'",
                            CultureInfo.GetCultureInfo("en-US"));
                });

            if (spanish)
            {
                output = Regex.Replace(
                    output,
                    @"(?<=\d)%(?!\w)",
                    " %");
            }

            return output;
        }

        private static string PseudoLocalize(
            string source)
        {
            const string accents =
                "áƀçđéƒğĥíĵķľḿñóƥʠŕşŧúṽŵẋýž";
            const string plain =
                "abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder =
                new StringBuilder(source.Length + 16);
            builder.Append("[!! ");
            for (int index = 0;
                 index < source.Length;
                 index++)
            {
                char value = source[index];
                int lower = plain.IndexOf(
                    char.ToLowerInvariant(value));
                if (lower >= 0)
                {
                    char replacement = accents[lower];
                    builder.Append(char.IsUpper(value)
                        ? char.ToUpperInvariant(replacement)
                        : replacement);
                }
                else
                {
                    builder.Append(value);
                }
            }

            int expansion = Math.Max(3, source.Length / 3);
            builder.Append(' ', expansion);
            builder.Append(" !!]");
            return builder.ToString();
        }

        private static bool IsRemoved(string status)
        {
            return !string.IsNullOrWhiteSpace(status) &&
                status.StartsWith(
                    "REMOVED",
                    StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPseudo(string localeCode)
        {
            return string.Equals(
                localeCode,
                "qps-ploc",
                StringComparison.OrdinalIgnoreCase);
        }

        private static string Identity(
            string table,
            string key)
        {
            return (table ?? string.Empty) + "\n" +
                (key ?? string.Empty);
        }

        [Serializable]
        private sealed class CatalogDocument
        {
            public int schemaVersion;
            public string documentId;
            public CatalogEntry[] entries;
        }

        [Serializable]
        private sealed class CatalogEntry
        {
            public string table;
            public string key;
            public string enUS;
            public string esES;
            public string status;
            public string[] sourceAliases;
        }

        private sealed class AliasEntry
        {
            public string Source { get; }
            public CatalogEntry Entry { get; }
            public Regex Pattern { get; }

            private AliasEntry(
                string source,
                CatalogEntry entry,
                Regex pattern)
            {
                Source = source;
                Entry = entry;
                Pattern = pattern;
            }

            public static AliasEntry TryCreate(
                string source,
                CatalogEntry entry)
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    return null;
                }

                bool startsWithWord =
                    char.IsLetterOrDigit(source[0]) ||
                    source[0] == '_';
                bool endsWithWord =
                    char.IsLetterOrDigit(
                        source[source.Length - 1]) ||
                    source[source.Length - 1] == '_';
                string pattern =
                    (startsWithWord ? @"(?<![\p{L}\p{N}_])" : string.Empty) +
                    Regex.Escape(source) +
                    (endsWithWord ? @"(?![\p{L}\p{N}_])" : string.Empty);

                try
                {
                    return new AliasEntry(
                        source,
                        entry,
                        new Regex(
                            pattern,
                            RegexOptions.CultureInvariant |
                            RegexOptions.IgnoreCase));
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }

        private sealed class PhraseEntry
        {
            public string Source { get; }
            public string Target { get; }
            public Regex Pattern { get; }

            private PhraseEntry(
                string source,
                string target,
                Regex pattern)
            {
                Source = source;
                Target = target;
                Pattern = pattern;
            }

            public static PhraseEntry TryCreate(
                CatalogEntry entry)
            {
                string source = entry.enUS ?? string.Empty;
                string target = entry.esES ?? source;
                if (source.Length < 3 ||
                    string.Equals(
                        source,
                        target,
                        StringComparison.Ordinal))
                {
                    return null;
                }

                bool startsWithWord =
                    char.IsLetterOrDigit(source[0]) ||
                    source[0] == '_';
                bool endsWithWord =
                    char.IsLetterOrDigit(
                        source[source.Length - 1]) ||
                    source[source.Length - 1] == '_';
                string pattern =
                    (startsWithWord ? @"(?<![\p{L}\p{N}_])" : string.Empty) +
                    Regex.Escape(source) +
                    (endsWithWord ? @"(?![\p{L}\p{N}_])" : string.Empty);

                try
                {
                    return new PhraseEntry(
                        source,
                        target,
                        new Regex(
                            pattern,
                            RegexOptions.CultureInvariant |
                            RegexOptions.IgnoreCase));
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }

        private sealed class TemplateEntry
        {
            private static readonly Regex PlaceholderPattern =
                new Regex(
                    @"\{([A-Za-z][A-Za-z0-9_]*)\}",
                    RegexOptions.CultureInvariant);

            public CatalogEntry Entry { get; }
            public Regex Pattern { get; }
            public IReadOnlyList<string> Placeholders { get; }
            public int LiteralCharacterCount { get; }

            private TemplateEntry(
                CatalogEntry entry,
                Regex pattern,
                IReadOnlyList<string> placeholders,
                int literalCharacterCount)
            {
                Entry = entry;
                Pattern = pattern;
                Placeholders = placeholders;
                LiteralCharacterCount = literalCharacterCount;
            }

            public static TemplateEntry TryCreate(
                CatalogEntry entry)
            {
                MatchCollection matches =
                    PlaceholderPattern.Matches(entry.enUS);
                if (matches.Count == 0)
                {
                    return null;
                }

                List<string> placeholders =
                    new List<string>();
                StringBuilder pattern =
                    new StringBuilder("\\A");
                int cursor = 0;
                int literalCharacters = 0;

                for (int index = 0;
                     index < matches.Count;
                     index++)
                {
                    Match match = matches[index];
                    string literal = entry.enUS.Substring(
                        cursor,
                        match.Index - cursor);
                    pattern.Append(Regex.Escape(literal));
                    literalCharacters += literal.Length;

                    string placeholder =
                        match.Groups[1].Value;
                    placeholders.Add(placeholder);
                    pattern.Append("(?<p");
                    pattern.Append(index);
                    pattern.Append(">.*?)");
                    cursor = match.Index + match.Length;
                }

                string tail = entry.enUS.Substring(cursor);
                pattern.Append(Regex.Escape(tail));
                literalCharacters += tail.Length;
                pattern.Append("\\z");

                int literalLetters = 0;
                for (int index = 0;
                     index < entry.enUS.Length;
                     index++)
                {
                    bool insidePlaceholder = false;
                    for (int placeholderIndex = 0;
                         placeholderIndex < matches.Count;
                         placeholderIndex++)
                    {
                        Match placeholder = matches[placeholderIndex];
                        if (index >= placeholder.Index &&
                            index < placeholder.Index +
                                placeholder.Length)
                        {
                            insidePlaceholder = true;
                            break;
                        }
                    }

                    if (!insidePlaceholder &&
                        char.IsLetter(entry.enUS[index]))
                    {
                        literalLetters++;
                    }
                }

                if (literalLetters < 2)
                {
                    return null;
                }

                try
                {
                    return new TemplateEntry(
                        entry,
                        new Regex(
                            pattern.ToString(),
                            RegexOptions.CultureInvariant |
                            RegexOptions.Singleline),
                        placeholders,
                        literalCharacters);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }
    }
}
