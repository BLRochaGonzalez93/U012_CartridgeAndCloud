using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Validation
{
    /// <summary>
    /// Read-only release validation and the canonical Windows x64 build entry point
    /// for Sprint 17 Phase 1. The build command includes production scenes only.
    /// </summary>
    public static class W8ReleaseValidationTool
    {
        private static readonly string[] ProductionScenes =
        {
            "Assets/_Project/Scenes/Production/Bootstrap.unity",
            "Assets/_Project/Scenes/Production/MainMenu.unity",
            "Assets/_Project/Scenes/Production/StoreInitial.unity"
        };

        private static readonly CharacterizationFamily[] RequiredFamilies =
        {
            new CharacterizationFamily("TIM", 8),
            new CharacterizationFamily("CUS", 9),
            new CharacterizationFamily("ODR", 10),
            new CharacterizationFamily("DSP", 6),
            new CharacterizationFamily("SAV", 8),
            new CharacterizationFamily("MGT", 8),
            new CharacterizationFamily("ART", 8),
            new CharacterizationFamily("REG", 12)
        };

        public static IReadOnlyList<string> ProductionScenePaths =>
            Array.AsReadOnly(ProductionScenes);

        [MenuItem("Cartridge & Cloud/W8/Validate Release Configuration")]
        public static void ValidateFromMenu()
        {
            IReadOnlyList<string> errors = ValidateProject();
            if (errors.Count == 0)
            {
                Debug.Log(
                    "W8 release configuration PASS. Production scenes, authored assets, " +
                    "characterization coverage and player identity are valid.");
                return;
            }

            Debug.LogError(
                "W8 release configuration FAILED:\n- " +
                string.Join("\n- ", errors));
        }

        [MenuItem("Cartridge & Cloud/W8/Build Windows x64")]
        public static void BuildWindowsX64()
        {
            IReadOnlyList<string> errors = ValidateProject();
            if (errors.Count > 0)
            {
                throw new BuildFailedException(
                    "W8 release validation failed before build:\n- " +
                    string.Join("\n- ", errors));
            }

            string outputDirectory = Path.Combine(
                "Builds",
                "Windows",
                "CartridgeAndCloud");
            Directory.CreateDirectory(outputDirectory);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = ProductionScenes,
                locationPathName = Path.Combine(
                    outputDirectory,
                    "CartridgeAndCloud.exe"),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.CleanBuildCache |
                          BuildOptions.StrictMode
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException(
                    $"W8 Windows x64 build failed with result {summary.result}. " +
                    $"Errors: {summary.totalErrors}; warnings: {summary.totalWarnings}.");
            }

            Debug.Log(
                $"W8 Windows x64 build PASS: {summary.outputPath} " +
                $"({summary.totalSize} bytes, {summary.totalWarnings} warning(s)).");
        }

        [MenuItem("Cartridge & Cloud/W8/Audit Latest Player Log")]
        public static void AuditLatestPlayerLogFromMenu()
        {
            string path = GetLatestPlayerLogPath();
            IReadOnlyList<string> errors = AuditPlayerLog(path);

            if (errors.Count == 0)
            {
                Debug.Log($"W8 Player.log audit PASS: {path}");
                return;
            }

            Debug.LogError(
                $"W8 Player.log audit FAILED ({path}):\n- " +
                string.Join("\n- ", errors));
        }

        public static IReadOnlyList<string> ValidateProject()
        {
            List<string> errors = new List<string>();
            errors.AddRange(StoreStructuralValidationTool.Validate());
            ValidateProductionScenes(errors);
            ValidatePlayerIdentity(errors);
            ValidateCharacterizationCoverage(errors);
            return errors;
        }

        public static IReadOnlyList<string> AuditPlayerLog(string logPath)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(logPath))
            {
                errors.Add("Player.log path is empty.");
                return errors;
            }

            if (!File.Exists(logPath))
            {
                errors.Add("Player.log was not found at: " + logPath);
                return errors;
            }

            string[] fatalMarkers =
            {
                "NullReferenceException",
                "MissingReferenceException",
                "UnassignedReferenceException",
                "UnityException:",
                "InvalidOperationException:",
                "Assertion failed",
                "Crash!!!",
                "[W8 BLOCKER]"
            };

            int lineNumber = 0;
            foreach (string line in File.ReadLines(logPath))
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                foreach (string marker in fatalMarkers)
                {
                    if (line.IndexOf(marker, StringComparison.Ordinal) < 0)
                    {
                        continue;
                    }

                    errors.Add(
                        $"Line {lineNumber}: {line.Trim()}");
                    break;
                }
            }

            return errors;
        }

        public static string GetLatestPlayerLogPath()
        {
            string company = SanitizePathSegment(PlayerSettings.companyName);
            string product = SanitizePathSegment(PlayerSettings.productName);

            switch (UnityEngine.Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "..",
                        "LocalLow",
                        company,
                        product,
                        "Player.log");

                case RuntimePlatform.OSXEditor:
                    return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        "Library",
                        "Logs",
                        company,
                        product,
                        "Player.log");

                default:
                    return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        ".config",
                        "unity3d",
                        company,
                        product,
                        "Player.log");
            }
        }

        private static void ValidateProductionScenes(List<string> errors)
        {
            HashSet<string> enabledScenes = new HashSet<string>(
                EditorBuildSettings.scenes
                    .Where(scene => scene.enabled)
                    .Select(scene => scene.path),
                StringComparer.Ordinal);

            foreach (string path in ProductionScenes)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null)
                {
                    errors.Add("Production scene is missing: " + path);
                    continue;
                }

                if (!enabledScenes.Contains(path))
                {
                    errors.Add("Production scene is not enabled in Build Settings: " + path);
                }
            }

            if (ProductionScenes.Any(path =>
                    path.IndexOf("/Test/", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    path.EndsWith("TestLab.unity", StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add("The canonical W8 production build contains a test scene.");
            }
        }

        private static void ValidatePlayerIdentity(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(PlayerSettings.companyName))
            {
                errors.Add("PlayerSettings.companyName is empty.");
            }

            if (string.IsNullOrWhiteSpace(PlayerSettings.productName))
            {
                errors.Add("PlayerSettings.productName is empty.");
            }

        }

        private static void ValidateCharacterizationCoverage(List<string> errors)
        {
            string testsRoot = Path.Combine(
                UnityEngine.Application.dataPath,
                "_Project",
                "Tests");

            if (!Directory.Exists(testsRoot))
            {
                errors.Add("The project test root is missing.");
                return;
            }

            string combinedSource = string.Join(
                "\n",
                Directory.GetFiles(testsRoot, "*.cs", SearchOption.AllDirectories)
                    .Select(File.ReadAllText));

            foreach (CharacterizationFamily family in RequiredFamilies)
            {
                for (int index = 1; index <= family.RequiredCount; index++)
                {
                    string token = $"CHAR_{family.Code}_{index:000}";
                    if (combinedSource.IndexOf(token, StringComparison.Ordinal) < 0)
                    {
                        errors.Add("Missing characterization test token: " + token);
                    }
                }
            }
        }

        private static string SanitizePathSegment(string value)
        {
            string result = value ?? string.Empty;
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(invalid, '_');
            }

            return result.Trim();
        }

        private readonly struct CharacterizationFamily
        {
            public string Code { get; }
            public int RequiredCount { get; }

            public CharacterizationFamily(string code, int requiredCount)
            {
                Code = code;
                RequiredCount = requiredCount;
            }
        }
    }

    public sealed class W8BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1000;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.StandaloneWindows64)
            {
                return;
            }

            IReadOnlyList<string> errors =
                W8ReleaseValidationTool.ValidateProject();

            if (errors.Count > 0)
            {
                throw new BuildFailedException(
                    "W8 Windows x64 build blocked:\n- " +
                    string.Join("\n- ", errors));
            }
        }
    }
}
