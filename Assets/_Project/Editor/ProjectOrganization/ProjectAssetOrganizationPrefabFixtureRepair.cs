using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization
{
    public static class
        ProjectAssetOrganizationPrefabFixtureRepair
    {
        private const string TestAssetPath =
            "Assets/_Project/Tests/PlayMode/" +
            "VerticalSlicePhase1/" +
            "Sprint16Phase1RuntimePlayModeTests.cs";

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Project Organization/" +
            "Repair Prefab Test Fixture Setup")]
        public static void Repair()
        {
            if (EditorApplication
                .isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "Prefab fixture repair",
                    "Exit Play Mode before running the repair.",
                    "OK");
                return;
            }

            try
            {
                string absolutePath =
                    AbsolutePath(TestAssetPath);
                string source =
                    File.ReadAllText(
                        absolutePath);

                int changes = 0;

                source = InsertRegistrySetup(
                    source,
                    "FurniturePrefab_BuildsBlockout",
                    ref changes);

                source = InsertRegistrySetup(
                    source,
                    "ProductPrefab_BuildsMarker",
                    ref changes);

                Validate(source);

                File.WriteAllText(
                    absolutePath,
                    source);

                Debug.Log(
                    "[Project Organization] " +
                    "Prefab test fixture setup repaired. " +
                    "Changes: " +
                    changes +
                    ".");

                EditorUtility.DisplayDialog(
                    "Prefab fixture repair",
                    "Repair completed.\n\n" +
                    "Unity will recompile the PlayMode test file.\n\n" +
                    "Run first:\n" +
                    "1. FurniturePrefab_BuildsBlockout\n" +
                    "2. ProductPrefab_BuildsMarker\n" +
                    "3. Phase 1 PlayMode (17 tests)",
                    "OK");

                AssetDatabase.Refresh(
                    ImportAssetOptions
                        .ForceSynchronousImport);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                EditorUtility.DisplayDialog(
                    "Prefab fixture repair failed",
                    exception.Message,
                    "OK");
            }
        }

        private static string InsertRegistrySetup(
            string source,
            string methodName,
            ref int changes)
        {
            string setup =
                "            Phase1RuntimeAssetRegistryAsset " +
                "registry =\n" +
                "                RequireRegistry();\n\n" +
                "            Assert.That(\n" +
                "                registry,\n" +
                "                Is.Not.Null);\n\n";

            string alreadyPatchedPattern =
                @"public IEnumerator " +
                Regex.Escape(methodName) +
                @"\(\)\s*\{\s*" +
                @"Phase1RuntimeAssetRegistryAsset\s+" +
                @"registry\s*=\s*RequireRegistry\(\);";

            if (Regex.IsMatch(
                    source,
                    alreadyPatchedPattern,
                    RegexOptions.Singleline))
            {
                return source;
            }

            string methodPattern =
                @"(        public IEnumerator " +
                Regex.Escape(methodName) +
                @"\(\)\s*\{\s*\n)";

            MatchCollection matches =
                Regex.Matches(
                    source,
                    methodPattern,
                    RegexOptions.Singleline);

            if (matches.Count != 1)
            {
                throw new InvalidOperationException(
                    methodName +
                    " setup patch expected one method, " +
                    "but found " +
                    matches.Count +
                    ".");
            }

            changes++;

            return Regex.Replace(
                source,
                methodPattern,
                "$1" + setup,
                RegexOptions.Singleline);
        }

        private static void Validate(
            string source)
        {
            int unityTestCount =
                Regex.Matches(
                    source,
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

            ValidateMethod(
                source,
                "FurniturePrefab_BuildsBlockout");

            ValidateMethod(
                source,
                "ProductPrefab_BuildsMarker");
        }

        private static void ValidateMethod(
            string source,
            string methodName)
        {
            string pattern =
                @"public IEnumerator " +
                Regex.Escape(methodName) +
                @"\(\)\s*\{.*?" +
                @"Phase1RuntimeAssetRegistryAsset\s+" +
                @"registry\s*=\s*RequireRegistry\(\);.*?" +
                @"Object\.Instantiate\(prefab\)";

            if (!Regex.IsMatch(
                    source,
                    pattern,
                    RegexOptions.Singleline))
            {
                throw new InvalidOperationException(
                    methodName +
                    " does not load the runtime registry " +
                    "before prefab instantiation.");
            }
        }

        private static string AbsolutePath(
            string assetPath)
        {
            string projectRoot =
                Directory.GetParent(
                    UnityEngine.Application.dataPath)
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
