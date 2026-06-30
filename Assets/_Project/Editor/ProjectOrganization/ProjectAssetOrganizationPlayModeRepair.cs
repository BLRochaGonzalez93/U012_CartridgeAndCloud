using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization
{
    public static class
        ProjectAssetOrganizationPlayModeRepair
    {
        private const string TestAssetPath =
            "Assets/_Project/Tests/PlayMode/" +
            "VerticalSlicePhase1/" +
            "Sprint16Phase1RuntimePlayModeTests.cs";

        private const string ScenarioAssetPath =
            "Assets/_Project/Scripts/Runtime/" +
            "VerticalSlicePhase1/" +
            "Sprint16Phase1TechnicalScenarioRunner.cs";

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Project Organization/" +
            "Repair PlayMode Asset Loading")]
        public static void Repair()
        {
            if (EditorApplication
                .isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "PlayMode loading repair",
                    "Exit Play Mode before running the repair.",
                    "OK");
                return;
            }

            try
            {
                string testPath =
                    AbsolutePath(TestAssetPath);
                string scenarioPath =
                    AbsolutePath(
                        ScenarioAssetPath);

                string tests =
                    File.ReadAllText(testPath);
                string scenario =
                    File.ReadAllText(
                        scenarioPath);

                int testChanges = 0;
                int scenarioChanges = 0;

                tests = PatchRequiredAssetsTest(
                    tests,
                    ref testChanges);

                tests = Replace(
                    tests,
                    PrefabPattern(
                        "Furniture",
                        "CheckoutCounter"),
                    @"LoadEditorAsset<GameObject>(
                    ""Assets/_Project/Prefabs/" +
                    @"Furniture/CheckoutCounter.prefab"")",
                    1,
                    ref testChanges,
                    "Checkout prefab");

                tests = Replace(
                    tests,
                    PrefabPattern(
                        "Products",
                        "NeonDrift"),
                    @"LoadEditorAsset<GameObject>(
                    ""Assets/_Project/Prefabs/" +
                    @"Products/NeonDrift.prefab"")",
                    1,
                    ref testChanges,
                    "Product prefab");

                tests = Replace(
                    tests,
                    PrefabPattern(
                        "Characters",
                        "Customer"),
                    @"LoadEditorAsset<GameObject>(
                    ""Assets/_Project/Prefabs/" +
                    @"Characters/Customer.prefab"")",
                    1,
                    ref testChanges,
                    "Character prefab");

                tests = Replace(
                    tests,
                    AudioCatalogPattern(),
                    "RequireRegistry().AudioCatalog",
                    2,
                    ref testChanges,
                    "Audio catalog");

                tests = Replace(
                    tests,
                    ContentCatalogPattern(),
                    "RequireRegistry().ContentCatalog",
                    3,
                    ref testChanges,
                    "Content catalog");

                tests = InsertHelpers(
                    tests,
                    ref testChanges);

                scenario = PatchScenarioFallback(
                    scenario,
                    ref scenarioChanges);

                ValidatePatchedSources(
                    tests,
                    scenario);

                File.WriteAllText(
                    testPath,
                    tests);
                File.WriteAllText(
                    scenarioPath,
                    scenario);

                Debug.Log(
                    "[Project Organization] " +
                    "PlayMode asset loading repaired. " +
                    "Test changes: " +
                    testChanges +
                    ", scenario changes: " +
                    scenarioChanges +
                    ".");

                EditorUtility.DisplayDialog(
                    "PlayMode loading repair",
                    "Repair completed.\n\n" +
                    "Unity will recompile the two patched files.\n\n" +
                    "Run next:\n" +
                    "1. RequiredProjectAssets_Load\n" +
                    "2. TechnicalScenario_Completes\n" +
                    "3. PlayMode Phase 1 (17 tests)\n" +
                    "4. Full PlayMode",
                    "OK");

                AssetDatabase.Refresh(
                    ImportAssetOptions
                        .ForceSynchronousImport);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                EditorUtility.DisplayDialog(
                    "PlayMode loading repair failed",
                    exception.Message,
                    "OK");
            }
        }

        private static string
            PatchRequiredAssetsTest(
                string source,
                ref int changes)
        {
            const string pattern =
                @"        \[UnityTest\]\s*" +
                @"public IEnumerator " +
                @"RequiredResources_Load\(\)\s*" +
                @"\{.*?\n        \}\s*" +
                @"\n        \[UnityTest\]\s*" +
                @"public IEnumerator " +
                @"FurniturePrefab_BuildsBlockout";

            const string replacement =
                @"        [UnityTest]
        public IEnumerator RequiredProjectAssets_Load()
        {
            yield return null;

            Phase1RuntimeAssetRegistryAsset registry =
                RequireRegistry();

            Assert.That(
                registry.Settings,
                Is.Not.Null);
            Assert.That(
                registry.ContentCatalog,
                Is.Not.Null);
            Assert.That(
                registry.StoreShell,
                Is.Not.Null);
            Assert.That(
                registry.MaterialPalette,
                Is.Not.Null);
            Assert.That(
                registry.PresentationCatalog,
                Is.Not.Null);
            Assert.That(
                registry.AudioCatalog,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator FurniturePrefab_BuildsBlockout";

            return Replace(
                source,
                pattern,
                replacement,
                1,
                ref changes,
                "Required project assets test");
        }

        private static string PrefabPattern(
            string family,
            string prefabName)
        {
            return
                @"Resources\.Load<GameObject>\(\s*" +
                @"""Sprint16Phase1/""\s*\+\s*" +
                @"""Prefabs/" +
                Regex.Escape(family) +
                @"/""\s*\+\s*" +
                @"""" +
                Regex.Escape(prefabName) +
                @"""\s*\)";
        }

        private static string
            AudioCatalogPattern()
        {
            return
                @"Resources\.Load<\s*" +
                @"Phase1AudioCatalogAsset>\(\s*" +
                @"""Sprint16Phase1/""\s*\+\s*" +
                @"""CC_S16_P1_AudioCatalog""\s*\)";
        }

        private static string
            ContentCatalogPattern()
        {
            return
                @"Resources\.Load<\s*" +
                @"Phase1ContentCatalogAsset>\(\s*" +
                @"""Sprint16Phase1/""\s*\+\s*" +
                @"""CC_S16_P1_ContentCatalog""\s*\)";
        }

        private static string InsertHelpers(
            string source,
            ref int changes)
        {
            if (source.Contains(
                    "private static T " +
                    "LoadEditorAsset<T>("))
            {
                return source;
            }

            const string marker =
                "        private static " +
                "IntegratedGameStateSnapshot\n" +
                "            WithDayState(";

            int index =
                source.IndexOf(
                    marker,
                    StringComparison.Ordinal);

            if (index < 0)
            {
                throw new InvalidOperationException(
                    "Could not locate the PlayMode " +
                    "test helper insertion point.");
            }

            const string helpers =
                @"        private static
            Phase1RuntimeAssetRegistryAsset
            RequireRegistry()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                Phase1RuntimeAssetRegistryAsset
                    .FindLoaded();

            if (registry == null)
            {
                registry =
                    LoadEditorAsset<
                        Phase1RuntimeAssetRegistryAsset>(
                            ""Assets/_Project/Settings/"" +
                            ""Runtime/"" +
                            ""RuntimeAssetRegistry.asset"");
            }

            Assert.That(
                registry,
                Is.Not.Null,
                ""RuntimeAssetRegistry could not be resolved."");

            return registry;
        }

        private static T LoadEditorAsset<T>(
            string assetPath)
            where T : UnityEngine.Object
        {
            System.Type assetDatabaseType = null;

            foreach (Assembly assembly
                     in System.AppDomain.CurrentDomain
                         .GetAssemblies())
            {
                assetDatabaseType =
                    assembly.GetType(
                        ""UnityEditor.AssetDatabase"");

                if (assetDatabaseType != null)
                {
                    break;
                }
            }

            Assert.That(
                assetDatabaseType,
                Is.Not.Null,
                ""UnityEditor.AssetDatabase is unavailable."");

            MethodInfo loadMethod =
                assetDatabaseType.GetMethod(
                    ""LoadAssetAtPath"",
                    BindingFlags.Public |
                    BindingFlags.Static,
                    null,
                    new[]
                    {
                        typeof(string),
                        typeof(System.Type)
                    },
                    null);

            Assert.That(
                loadMethod,
                Is.Not.Null,
                ""AssetDatabase.LoadAssetAtPath was not found."");

            T asset =
                loadMethod.Invoke(
                    null,
                    new object[]
                    {
                        assetPath,
                        typeof(T)
                    }) as T;

            Assert.That(
                asset,
                Is.Not.Null,
                ""Asset not found: "" +
                assetPath);

            return asset;
        }

";

            changes++;
            return source.Insert(
                index,
                helpers);
        }

        private static string
            PatchScenarioFallback(
                string source,
                ref int changes)
        {
            const string pattern =
                @"            if \(_catalogAsset == null\)\s*" +
                @"\{\s*" +
                @"_catalogAsset =\s*" +
                @"Resources\.Load<\s*" +
                @"Phase1ContentCatalogAsset>\(\s*" +
                @"""Sprint16Phase1/""\s*\+\s*" +
                @"""CC_S16_P1_ContentCatalog""\s*\);\s*" +
                @"\}";

            const string replacement =
                @"            if (_catalogAsset == null)
            {
                Phase1RuntimeAssetRegistryAsset
                    registry =
                        Phase1RuntimeAssetRegistryAsset
                            .FindLoaded();

                _catalogAsset =
                    registry == null
                        ? null
                        : registry.ContentCatalog;
            }";

            return Replace(
                source,
                pattern,
                replacement,
                1,
                ref changes,
                "Technical scenario catalog fallback");
        }

        private static string Replace(
            string source,
            string pattern,
            string replacement,
            int expectedCount,
            ref int changes,
            string label)
        {
            MatchCollection matches =
                Regex.Matches(
                    source,
                    pattern,
                    RegexOptions.Singleline);

            if (matches.Count == 0 &&
                SourceAlreadyPatched(
                    source,
                    label))
            {
                return source;
            }

            if (matches.Count != expectedCount)
            {
                throw new InvalidOperationException(
                    label +
                    " patch expected " +
                    expectedCount +
                    " match(es), but found " +
                    matches.Count +
                    ".");
            }

            changes += matches.Count;

            return Regex.Replace(
                source,
                pattern,
                replacement,
                RegexOptions.Singleline);
        }

        private static bool SourceAlreadyPatched(
            string source,
            string label)
        {
            switch (label)
            {
                case "Required project assets test":
                    return source.Contains(
                        "RequiredProjectAssets_Load");

                case "Checkout prefab":
                    return source.Contains(
                        "Prefabs/Furniture/" +
                        "CheckoutCounter.prefab");

                case "Product prefab":
                    return source.Contains(
                        "Prefabs/Products/" +
                        "NeonDrift.prefab");

                case "Character prefab":
                    return source.Contains(
                        "Prefabs/Characters/" +
                        "Customer.prefab");

                case "Audio catalog":
                    return CountOccurrences(
                        source,
                        "RequireRegistry()" +
                        ".AudioCatalog") >= 2;

                case "Content catalog":
                    return CountOccurrences(
                        source,
                        "RequireRegistry()" +
                        ".ContentCatalog") >= 3;

                case "Technical scenario catalog fallback":
                    return source.Contains(
                        "registry.ContentCatalog");

                default:
                    return false;
            }
        }

        private static int CountOccurrences(
            string source,
            string value)
        {
            int count = 0;
            int index = 0;

            while ((index =
                       source.IndexOf(
                           value,
                           index,
                           StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += value.Length;
            }

            return count;
        }

        private static void ValidatePatchedSources(
            string tests,
            string scenario)
        {
            if (tests.Contains(
                    "Sprint16Phase1/" +
                    "CC_S16_P1_") ||
                tests.Contains(
                    "Sprint16Phase1/" +
                    "Prefabs/"))
            {
                throw new InvalidOperationException(
                    "Legacy Resources paths remain " +
                    "in the PlayMode test source.");
            }

            if (scenario.Contains(
                    "Sprint16Phase1/" +
                    "CC_S16_P1_ContentCatalog"))
            {
                throw new InvalidOperationException(
                    "The technical scenario still " +
                    "contains its legacy catalog path.");
            }

            int unityTestCount =
                Regex.Matches(
                    tests,
                    @"\[UnityTest\]")
                    .Count;

            if (unityTestCount != 17)
            {
                throw new InvalidOperationException(
                    "The PlayMode test count changed. " +
                    "Expected 17, found " +
                    unityTestCount +
                    ".");
            }

            if (!tests.Contains(
                    "RequireRegistry()" +
                    ".AudioCatalog") ||
                !tests.Contains(
                    "LoadEditorAsset<GameObject>") ||
                !scenario.Contains(
                    "registry.ContentCatalog"))
            {
                throw new InvalidOperationException(
                    "The new loading strategy was " +
                    "not fully installed.");
            }
        }

        private static string AbsolutePath(
            string assetPath)
        {
            string projectRoot =
                Directory.GetParent(
                    Application.dataPath)
                    ?.FullName;

            if (string.IsNullOrWhiteSpace(
                    projectRoot))
            {
                throw new InvalidOperationException(
                    "Project root could not be resolved.");
            }

            return Path.Combine(
                projectRoot,
                assetPath.Replace(
                    '/',
                    Path.DirectorySeparatorChar));
        }
    }
}
