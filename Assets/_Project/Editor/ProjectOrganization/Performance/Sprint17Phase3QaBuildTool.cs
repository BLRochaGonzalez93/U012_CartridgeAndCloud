using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Validation;
using VRMGames.CartridgeAndCloud.Runtime.Performance;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Performance
{
    /// <summary>
    /// Temporary QA build entry point for Sprint 17 Phase 3. It intentionally
    /// does not create the formal Windows_QA or Windows_H6_Candidate profiles,
    /// which remain owned by Sprint 17 Phase 6.
    /// </summary>
    public static class Sprint17Phase3QaBuildTool
    {
        public const string QaBuildDirectory =
            "Builds/Windows_QA_Phase3";
        public const string QaExecutableName =
            "CartridgeAndCloud_QA_Phase3.exe";

        private static readonly string[] QaScenes =
        {
            "Assets/_Project/Scenes/Production/Bootstrap.unity",
            "Assets/_Project/Scenes/Production/MainMenu.unity",
            "Assets/_Project/Scenes/Production/StoreInitial.unity",
            "Assets/_Project/Scenes/Test/TestLab.unity"
        };

        public static IReadOnlyList<string> QaScenePaths =>
            Array.AsReadOnly(QaScenes);

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 3/Validate QA Configuration")]
        public static void ValidateFromMenu()
        {
            IReadOnlyList<string> errors =
                ValidateConfiguration();
            if (errors.Count == 0)
            {
                Debug.Log(
                    "Sprint 17 Phase 3 QA configuration PASS. " +
                    "Windows x64, QA instrumentation, production scenes and TestLab are ready.");
                return;
            }

            Debug.LogError(
                "Sprint 17 Phase 3 QA configuration FAILED:\n- " +
                string.Join("\n- ", errors));
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 3/Build Windows x64 QA")]
        public static void BuildWindowsX64Qa()
        {
            IReadOnlyList<string> errors =
                ValidateConfiguration();
            if (errors.Count > 0)
            {
                throw new BuildFailedException(
                    "Sprint 17 Phase 3 QA validation failed before build:\n- " +
                    string.Join("\n- ", errors));
            }

            Directory.CreateDirectory(QaBuildDirectory);
            string outputPath = Path.Combine(
                QaBuildDirectory,
                QaExecutableName);

            BuildPlayerOptions options =
                new BuildPlayerOptions
                {
                    scenes = QaScenes,
                    locationPathName = outputPath,
                    target = BuildTarget.StandaloneWindows64,
                    options =
                        BuildOptions.Development |
                        BuildOptions.CleanBuildCache |
                        BuildOptions.StrictMode,
                    extraScriptingDefines = new[]
                    {
                        Sprint17Phase3PerformanceCapture
                            .QaScriptingDefine
                    }
                };

            bool previousFrameTimingStats =
                PlayerSettings.enableFrameTimingStats;
            BuildReport report;
            try
            {
                PlayerSettings.enableFrameTimingStats = true;
                report = BuildPipeline.BuildPlayer(options);
            }
            finally
            {
                PlayerSettings.enableFrameTimingStats =
                    previousFrameTimingStats;
            }

            BuildSummary summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException(
                    "Sprint 17 Phase 3 Windows x64 QA build failed with result " +
                    summary.result + ". Errors: " +
                    summary.totalErrors + "; warnings: " +
                    summary.totalWarnings + ".");
            }

            WriteBuildManifest(summary);
            Debug.Log(
                "Sprint 17 Phase 3 Windows x64 QA build PASS: " +
                summary.outputPath + " (" +
                summary.totalSize + " bytes, " +
                summary.totalWarnings + " warning(s)). " +
                "Run the executable and use its QA PERF overlay to export reports.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 3/Open Performance Reports")]
        public static void OpenPerformanceReports()
        {
            string path = Path.Combine(
                global::UnityEngine.Application.persistentDataPath,
                Sprint17Phase3PerformanceCapture
                    .ReportFolderName);
            Directory.CreateDirectory(path);
            EditorUtility.RevealInFinder(path);
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 17/Phase 3/Audit Latest Performance Report")]
        public static void AuditLatestPerformanceReport()
        {
            string reportDirectory =
                FindLatestReportDirectory();
            if (string.IsNullOrWhiteSpace(reportDirectory))
            {
                Debug.LogError(
                    "No Sprint 17 Phase 3 performance report was found in " +
                    Path.Combine(
                        global::UnityEngine.Application.persistentDataPath,
                        Sprint17Phase3PerformanceCapture
                            .ReportFolderName));
                return;
            }

            string summaryPath = Path.Combine(
                reportDirectory,
                "summary.json");
            if (!File.Exists(summaryPath))
            {
                Debug.LogError(
                    "The latest performance report has no summary.json: " +
                    reportDirectory);
                return;
            }

            PerformanceSummary summary =
                JsonUtility.FromJson<PerformanceSummary>(
                    File.ReadAllText(summaryPath));
            List<string> observations =
                EvaluateSummary(summary);

            string heading =
                "Sprint 17 Phase 3 latest performance report: " +
                reportDirectory + "\n" +
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Frames {0}; FPS avg {1:0.0}; frame p95 {2:0.00} ms; p99 {3:0.00} ms; max {4:0.00} ms; max customers {5}; errors {6}.",
                    summary.frameCount,
                    summary.averageFps,
                    summary.p95FrameMilliseconds,
                    summary.p99FrameMilliseconds,
                    summary.maximumFrameMilliseconds,
                    summary.maximumObservedCustomers,
                    summary.errorCount);

            if (observations.Count == 0)
            {
                Debug.Log(
                    heading +
                    "\nNo automatic warning threshold was exceeded. " +
                    "This is supporting evidence, not an automatic Phase 3 approval.");
                return;
            }

            Debug.LogWarning(
                heading +
                "\nObservations:\n- " +
                string.Join("\n- ", observations));
        }

        public static IReadOnlyList<string>
            ValidateConfiguration()
        {
            List<string> errors = new List<string>();
            errors.AddRange(
                W8ReleaseValidationTool.ValidateProject());

            foreach (string path in QaScenes)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null)
                {
                    errors.Add(
                        "QA scene is missing: " + path);
                }
            }

            if (Array.IndexOf(
                    QaScenes,
                    "Assets/_Project/Scenes/Test/TestLab.unity") < 0)
            {
                errors.Add(
                    "The Phase 3 QA build must include TestLab for directed load checks.");
            }

            if (!PlayerSettings.usePlayerLog)
            {
                errors.Add(
                    "PlayerSettings.usePlayerLog must remain enabled for QA.");
            }

            if (string.IsNullOrWhiteSpace(
                    PlayerSettings.companyName) ||
                string.IsNullOrWhiteSpace(
                    PlayerSettings.productName))
            {
                errors.Add(
                    "Company and product names are required to locate Player.log and performance reports.");
            }

            return errors;
        }

        public static List<string> EvaluateSummary(
            PerformanceSummary summary)
        {
            List<string> observations = new List<string>();
            if (summary == null)
            {
                observations.Add(
                    "The summary could not be parsed.");
                return observations;
            }

            if (summary.frameCount < 600L)
            {
                observations.Add(
                    "The capture is too short for representative analysis (fewer than 600 frames).");
            }

            if (summary.maximumObservedCustomers < 8)
            {
                observations.Add(
                    "The documented target load of eight simultaneous customers was not observed.");
            }

            if (summary.p95FrameMilliseconds > 33.3333333d)
            {
                observations.Add(
                    "The p95 frame time exceeds 33.33 ms.");
            }

            if (summary.maximumFrameMilliseconds > 100d)
            {
                observations.Add(
                    "At least one frame exceeded 100 ms; inspect samples and operation markers.");
            }

            if (summary.maximumGcAllocatedBytesPerFrame >
                1024L * 1024L)
            {
                observations.Add(
                    "A frame allocated more than 1 MB of managed memory.");
            }

            if (summary.frameCount > 0L)
            {
                double averageGcBytesPerFrame =
                    summary.totalGcAllocatedBytes /
                    (double)summary.frameCount;
                double allocationFrameRatio =
                    summary.framesWithGcAllocations /
                    (double)summary.frameCount;

                if (averageGcBytesPerFrame > 2048d &&
                    allocationFrameRatio > 0.5d)
                {
                    observations.Add(
                        "Managed allocations are sustained across more than half of measured frames and average more than 2 KB per frame.");
                }
            }

            if (summary.invalidGpuTimingSampleCount > 0L)
            {
                observations.Add(
                    "Some GPU timing samples were rejected as invalid; use frame-time and main-thread metrics as the authoritative evidence for this capture.");
            }

            if (summary.usedMemoryGrowthBytes >
                256L * 1024L * 1024L)
            {
                observations.Add(
                    "Observed used-memory growth exceeds 256 MB.");
            }

            if (summary.errorCount > 0)
            {
                observations.Add(
                    "The captured session contains error, exception or assertion logs.");
            }

            return observations;
        }

        private static void WriteBuildManifest(
            BuildSummary summary)
        {
            BuildManifest manifest = new BuildManifest
            {
                buildId = "S17-P3-QA-" +
                    DateTime.UtcNow.ToString(
                        "yyyyMMdd-HHmmss",
                        CultureInfo.InvariantCulture),
                createdUtc = DateTime.UtcNow.ToString(
                    "O",
                    CultureInfo.InvariantCulture),
                unityVersion = global::UnityEngine.Application.unityVersion,
                applicationVersion =
                    PlayerSettings.bundleVersion,
                target = BuildTarget
                    .StandaloneWindows64.ToString(),
                architecture = "Windows x64",
                developmentBuild = true,
                deepProfiling = false,
                connectProfiler = false,
                frameTimingStats = true,
                qaScriptingDefine =
                    Sprint17Phase3PerformanceCapture
                        .QaScriptingDefine,
                outputPath = summary.outputPath,
                totalSizeBytes = summary.totalSize,
                warningCount = summary.totalWarnings,
                scenes = QaScenes,
                notes =
                    "Temporary Sprint 17 Phase 3 QA build. This is not an H6 candidate and does not replace the formal Phase 6 build profiles."
            };

            File.WriteAllText(
                Path.Combine(
                    QaBuildDirectory,
                    "QA_BUILD_MANIFEST.json"),
                JsonUtility.ToJson(manifest, true));
        }

        private static string FindLatestReportDirectory()
        {
            string root = Path.Combine(
                global::UnityEngine.Application.persistentDataPath,
                Sprint17Phase3PerformanceCapture
                    .ReportFolderName);
            if (!Directory.Exists(root))
            {
                return string.Empty;
            }

            string[] directories =
                Directory.GetDirectories(root);
            Array.Sort(
                directories,
                StringComparer.Ordinal);
            return directories.Length == 0
                ? string.Empty
                : directories[directories.Length - 1];
        }

        [Serializable]
        private sealed class BuildManifest
        {
            public string buildId;
            public string createdUtc;
            public string unityVersion;
            public string applicationVersion;
            public string target;
            public string architecture;
            public bool developmentBuild;
            public bool deepProfiling;
            public bool connectProfiler;
            public bool frameTimingStats;
            public string qaScriptingDefine;
            public string outputPath;
            public ulong totalSizeBytes;
            public int warningCount;
            public string[] scenes;
            public string notes;
        }

        [Serializable]
        public sealed class PerformanceSummary
        {
            public long frameCount;
            public double averageFps;
            public double p95FrameMilliseconds;
            public double p99FrameMilliseconds;
            public double maximumFrameMilliseconds;
            public int maximumObservedCustomers;
            public long totalGcAllocatedBytes;
            public long maximumGcAllocatedBytesPerFrame;
            public long framesWithGcAllocations;
            public long validGpuTimingSampleCount;
            public long invalidGpuTimingSampleCount;
            public bool gpuTimingReliable;
            public long usedMemoryGrowthBytes;
            public int errorCount;
        }
    }
}
