using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace VRMGames.CartridgeAndCloud.Editor.Localization
{
    public static class Phase5LocalizationCatalogImporter
    {
        private const string CatalogPath =
            "Assets/_Project/Resources/Localization/" +
            "CC_Localization_Runtime.json";
        private const string RootPath =
            "Assets/_Project/Content/Localization";
        private const string GeneratedPath =
            RootPath + "/Generated";
        private const string LocalePath =
            GeneratedPath + "/Locales";
        private const string TablePath =
            GeneratedPath + "/StringTables";
        private const string SettingsPath =
            GeneratedPath + "/CC_LocalizationSettings.asset";
        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 5/" +
            "Synchronize Localization Catalog")]
        public static void SynchronizeFromMenu()
        {
            Synchronize(showSuccess: true);
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 5/" +
            "Validate Localization Catalog")]
        public static void ValidateFromMenu()
        {
            CatalogDocument document = LoadCatalog();
            List<string> errors = Validate(document);
            if (errors.Count > 0)
            {
                throw new InvalidOperationException(
                    "Phase 5 localization validation failed:\n" +
                    string.Join("\n", errors));
            }

            Debug.Log(
                "Sprint 17 Phase 5 localization catalog PASS: " +
                document.entries.Length + " entries, ES/EN complete.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 5/" +
            "Audit Hardcoded Player Text")]
        public static void AuditHardcodedPlayerText()
        {
            string[] prohibited =
            {
                "SPRINT 16 · PLAYABLE BLOCKOUT",
                "Phase 2 remains blocked until",
                "Automatic wall occlusion: Disabled for H6.",
                "Scene flow shell only — gameplay begins in later sprints",
                "Sprint 1 — Technical Main Menu",
                "STORE PROTOTYPE"
            };
            string[] roots =
            {
                "Assets/_Project/Scripts/Runtime/UIUX",
                "Assets/_Project/Scenes/Production"
            };
            List<string> findings = new List<string>();

            foreach (string root in roots)
            {
                if (!Directory.Exists(root))
                {
                    continue;
                }

                foreach (string path in Directory.GetFiles(
                             root,
                             "*.*",
                             SearchOption.AllDirectories))
                {
                    string extension = Path.GetExtension(path);
                    if (!string.Equals(extension, ".cs", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(extension, ".unity", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(extension, ".prefab", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string content = File.ReadAllText(path);
                    foreach (string pattern in prohibited)
                    {
                        if (content.IndexOf(pattern, StringComparison.Ordinal) >= 0)
                        {
                            findings.Add(path + " -> " + pattern);
                        }
                    }
                }
            }

            if (findings.Count > 0)
            {
                throw new InvalidOperationException(
                    "Player-facing internal text audit failed:\n" +
                    string.Join("\n", findings));
            }

            Debug.Log(
                "Sprint 17 Phase 5 hardcoded player text audit PASS.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 5/" +
            "Select Pseudolocale")]
        public static void SelectPseudoLocale()
        {
            Locale locale = FindLocale("qps-ploc");
            if (locale == null)
            {
                Synchronize(showSuccess: false);
                locale = FindLocale("qps-ploc");
            }

            if (locale == null)
            {
                throw new InvalidOperationException(
                    "The qps-ploc locale could not be created.");
            }

            LocalizationSettings.SelectedLocale = locale;
            Debug.Log("Phase 5 pseudolocale selected.");
        }

        private static void Synchronize(bool showSuccess)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                throw new InvalidOperationException(
                    "Localization assets cannot be synchronized while " +
                    "entering or running Play Mode.");
            }

            CatalogDocument document = LoadCatalog();
            List<string> errors = Validate(document);
            if (errors.Count > 0)
            {
                throw new InvalidOperationException(
                    string.Join("\n", errors));
            }

            EnsureFolder(GeneratedPath);
            EnsureFolder(LocalePath);
            EnsureFolder(TablePath);
            EnsureLocalizationSettings();

            Locale spanish = EnsureLocale(
                "es-ES",
                "Español",
                LocalePath + "/Locale_es-ES.asset");
            Locale english = EnsureLocale(
                "en-US",
                "English",
                LocalePath + "/Locale_en-US.asset");
            EnsurePseudoLocale();
            LocalizationSettings.ProjectLocale = spanish;

            CatalogEntry[] runtimeEntries =
                document.entries
                    .Where(entry => !IsRemoved(entry.status))
                    .ToArray();

            foreach (IGrouping<string, CatalogEntry> group in
                     runtimeEntries.GroupBy(entry => entry.table))
            {
                StringTableCollection collection =
                    LocalizationEditorSettings
                        .GetStringTableCollection(group.Key);
                if (collection == null)
                {
                    collection = LocalizationEditorSettings
                        .CreateStringTableCollection(
                            group.Key,
                            TablePath,
                            new List<Locale>
                            {
                                spanish,
                                english
                            });
                }

                StringTable spanishTable =
                    EnsureTable(collection, spanish);
                StringTable englishTable =
                    EnsureTable(collection, english);

                foreach (CatalogEntry entry in group)
                {
                    Upsert(englishTable, entry.key, entry.enUS);
                    Upsert(spanishTable, entry.key, entry.esES);
                }

                // Do not mark tables for startup preload. The generated
                // collections are loaded on demand and the runtime JSON
                // catalog remains the deterministic fallback. Startup
                // preloading can race AssetDatabase imports after a catalog
                // synchronization and leave Localization initialization in a
                // failed state during EditMode or domain reloads.
                collection.SetPreloadTableFlag(false);
                EditorUtility.SetDirty(collection.SharedData);
                EditorUtility.SetDirty(spanishTable);
                EditorUtility.SetDirty(englishTable);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(
                ImportAssetOptions.ForceSynchronousImport);

            if (showSuccess)
            {
                Debug.Log(
                    "Sprint 17 Phase 5 localization synchronization " +
                    "PASS: " + runtimeEntries.Length +
                    " runtime entries across " +
                    runtimeEntries.Select(item => item.table)
                        .Distinct().Count() + " String Tables.");
            }
        }

        private static CatalogDocument LoadCatalog()
        {
            TextAsset asset =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    CatalogPath);
            if (asset == null)
            {
                throw new FileNotFoundException(
                    "Localization catalog not found.",
                    CatalogPath);
            }

            CatalogDocument document =
                JsonUtility.FromJson<CatalogDocument>(asset.text);
            if (document == null || document.entries == null)
            {
                throw new InvalidDataException(
                    "Localization catalog JSON is invalid.");
            }

            return document;
        }

        private static List<string> Validate(
            CatalogDocument document)
        {
            List<string> errors = new List<string>();
            if (document == null || document.schemaVersion != 1)
            {
                errors.Add("Unsupported catalog schemaVersion.");
                return errors;
            }

            HashSet<string> identities =
                new HashSet<string>(StringComparer.Ordinal);
            foreach (CatalogEntry entry in document.entries)
            {
                string identity = entry.table + "::" + entry.key;
                if (string.IsNullOrWhiteSpace(entry.table) ||
                    string.IsNullOrWhiteSpace(entry.key))
                {
                    errors.Add("An entry has an empty table or key.");
                }
                else if (!identities.Add(identity))
                {
                    errors.Add("Duplicate key: " + identity);
                }

                if (string.IsNullOrWhiteSpace(entry.enUS))
                {
                    errors.Add("Missing en-US: " + identity);
                }

                if (string.IsNullOrWhiteSpace(entry.esES))
                {
                    errors.Add("Missing es-ES: " + identity);
                }

                ValidatePlaceholders(
                    identity,
                    entry.enUS,
                    entry.esES,
                    errors);
            }

            return errors;
        }

        private static void ValidatePlaceholders(
            string identity,
            string english,
            string spanish,
            ICollection<string> errors)
        {
            string[] englishTokens = Tokens(english);
            string[] spanishTokens = Tokens(spanish);
            if (!englishTokens.SequenceEqual(spanishTokens))
            {
                errors.Add(
                    "Placeholder mismatch: " + identity);
            }
        }

        private static string[] Tokens(string value)
        {
            List<string> tokens = new List<string>();
            if (string.IsNullOrEmpty(value))
            {
                return tokens.ToArray();
            }

            int cursor = 0;
            while (cursor < value.Length)
            {
                int open = value.IndexOf('{', cursor);
                if (open < 0)
                {
                    break;
                }

                int close = value.IndexOf('}', open + 1);
                if (close < 0)
                {
                    break;
                }

                tokens.Add(value.Substring(
                    open,
                    close - open + 1));
                cursor = close + 1;
            }

            tokens.Sort(StringComparer.Ordinal);
            return tokens.ToArray();
        }

        private static void EnsureLocalizationSettings()
        {
            if (LocalizationEditorSettings
                    .ActiveLocalizationSettings != null)
            {
                return;
            }

            LocalizationSettings settings =
                ScriptableObject.CreateInstance<
                    LocalizationSettings>();
            settings.name = "CC Localization Settings";
            settings.SetAvailableLocales(
                new LocalesProvider());
            settings.SetStringDatabase(
                new LocalizedStringDatabase());
            settings.SetAssetDatabase(
                new LocalizedAssetDatabase());
            settings.GetStartupLocaleSelectors().Clear();
            settings.GetStartupLocaleSelectors().Add(
                new SpecificLocaleSelector
                {
                    LocaleId =
                        new LocaleIdentifier("es-ES")
                });
            AssetDatabase.CreateAsset(settings, SettingsPath);
            LocalizationEditorSettings
                .ActiveLocalizationSettings = settings;
        }

        private static Locale EnsureLocale(
            string code,
            string displayName,
            string path)
        {
            Locale locale =
                LocalizationEditorSettings.GetLocale(code);
            if (locale != null)
            {
                return locale;
            }

            locale = Locale.CreateLocale(code);
            locale.LocaleName = displayName;
            locale.name = "Locale_" + code;
            AssetDatabase.CreateAsset(locale, path);
            LocalizationEditorSettings.AddLocale(locale);
            return locale;
        }

        private static void EnsurePseudoLocale()
        {
            if (FindLocale("qps-ploc") != null)
            {
                return;
            }

            PseudoLocale pseudo =
                PseudoLocale.CreatePseudoLocale();
            pseudo.Identifier =
                new LocaleIdentifier("qps-ploc");
            pseudo.LocaleName = "Pseudo (QA)";
            pseudo.name = "Locale_qps-ploc";
            AssetDatabase.CreateAsset(
                pseudo,
                LocalePath + "/Locale_qps-ploc.asset");
            LocalizationEditorSettings.AddLocale(pseudo);
        }

        private static Locale FindLocale(string code)
        {
            Locale standard =
                LocalizationEditorSettings.GetLocale(code);
            if (standard != null)
            {
                return standard;
            }

            return LocalizationEditorSettings
                .GetPseudoLocales()
                .FirstOrDefault(locale =>
                    string.Equals(
                        locale.Identifier.Code,
                        code,
                        StringComparison.OrdinalIgnoreCase));
        }

        private static StringTable EnsureTable(
            StringTableCollection collection,
            Locale locale)
        {
            StringTable table =
                collection.GetTable(locale.Identifier)
                as StringTable;
            return table ??
                collection.AddNewTable(locale.Identifier)
                as StringTable;
        }

        private static void Upsert(
            StringTable table,
            string key,
            string value)
        {
            StringTableEntry entry = table.GetEntry(key);
            if (entry == null)
            {
                table.AddEntry(key, value ?? string.Empty);
            }
            else
            {
                entry.Value = value ?? string.Empty;
            }
        }

        private static bool IsRemoved(string status)
        {
            return !string.IsNullOrWhiteSpace(status) &&
                status.StartsWith(
                    "REMOVED",
                    StringComparison.OrdinalIgnoreCase);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)
                .Replace('\\', '/');
            string name = Path.GetFileName(path);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        [Serializable]
        private sealed class CatalogDocument
        {
            public int schemaVersion;
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
        }
    }
}
