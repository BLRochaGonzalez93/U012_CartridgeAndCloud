using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.BuildProfiles
{
    /// <summary>
    /// Owns the approved Sprint 17 Phase 6 Windows profile matrix, validates
    /// the versioned Build Profile assets and offers reproducible Development
    /// and QA build entry points. The H6 candidate artifact remains Phase 7.
    /// </summary>
    public static class Sprint17Phase6BuildProfileTool
    {
        public const string DevelopmentProfilePath =
            "Assets/_Project/Settings/BuildProfiles/Windows_Development.asset";
        public const string QaProfilePath =
            "Assets/_Project/Settings/BuildProfiles/Windows_QA.asset";
        public const string H6CandidateProfilePath =
            "Assets/_Project/Settings/BuildProfiles/Windows_H6_Candidate.asset";
        public const string MatrixPath =
            "Assets/_Project/Settings/BuildProfiles/BuildProfileMatrix.json";

        public const string DevelopmentDefine = "CC_BUILD_DEVELOPMENT";
        public const string QaDefine = "CC_BUILD_QA";
        public const string H6CandidateDefine = "CC_BUILD_H6_CANDIDATE";
        public const string TemporaryPhase3QaDefine = "CC_SPRINT17_PHASE3_QA";

        public static readonly string[] ProductionScenes =
        {
            "Assets/_Project/Scenes/Production/Bootstrap.unity",
            "Assets/_Project/Scenes/Production/MainMenu.unity",
            "Assets/_Project/Scenes/Production/StoreInitial.unity"
        };

        public static readonly string[] DevelopmentAndQaScenes =
        {
            "Assets/_Project/Scenes/Production/Bootstrap.unity",
            "Assets/_Project/Scenes/Production/MainMenu.unity",
            "Assets/_Project/Scenes/Production/StoreInitial.unity",
            "Assets/_Project/Scenes/Test/TestLab.unity"
        };

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 6/Validate All Build Profiles")]
        public static void ValidateAllFromMenu()
        {
            IReadOnlyList<string> errors = ValidateAllProfiles();
            if (errors.Count == 0)
            {
                Debug.Log(
                    "Sprint 17 Phase 6 build profiles PASS. " +
                    "Development, Windows_QA and Windows_H6_Candidate match the approved matrix.");
                return;
            }

            Debug.LogError(
                "Sprint 17 Phase 6 build profile validation FAILED:\n- " +
                string.Join("\n- ", errors));
        }

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 6/Validate H6 Candidate Profile")]
        public static void ValidateH6FromMenu()
        {
            List<string> errors = new List<string>();
            ValidateGlobalPlayerSettings(errors);
            ValidateProfile(
                H6CandidateProfilePath,
                "Windows_H6_Candidate",
                ProductionScenes,
                new[] { H6CandidateDefine },
                developmentBuild: false,
                errors);

            if (errors.Count == 0)
            {
                Debug.Log(
                    "Sprint 17 Phase 6 H6 candidate profile PASS. " +
                    "Only production scenes are included and development diagnostics are disabled.");
                return;
            }

            Debug.LogError(
                "Sprint 17 Phase 6 H6 candidate profile FAILED:\n- " +
                string.Join("\n- ", errors));
        }

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 6/Build Windows Development")]
        public static void BuildWindowsDevelopment()
        {
            BuildApprovedProfile(
                "Windows_Development",
                DevelopmentAndQaScenes,
                new[] { DevelopmentDefine },
                developmentBuild: true,
                "Builds/Windows_Development/CartridgeAndCloud_Development.exe");
        }

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 6/Build Windows QA")]
        public static void BuildWindowsQa()
        {
            BuildApprovedProfile(
                "Windows_QA",
                DevelopmentAndQaScenes,
                new[] { QaDefine },
                developmentBuild: true,
                "Builds/Windows_QA/CartridgeAndCloud_QA.exe");
        }

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 6/Open Build Profiles Folder")]
        public static void OpenBuildProfilesFolder()
        {
            UnityEngine.Object asset =
                AssetDatabase.LoadMainAssetAtPath(DevelopmentProfilePath);
            if (asset == null)
            {
                Debug.LogError("Build profile folder could not be resolved.");
                return;
            }

            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }

        public static IReadOnlyList<string> ValidateAllProfiles()
        {
            List<string> errors = new List<string>();
            ValidateGlobalPlayerSettings(errors);

            ValidateProfile(
                DevelopmentProfilePath,
                "Windows_Development",
                DevelopmentAndQaScenes,
                new[] { DevelopmentDefine },
                developmentBuild: true,
                errors);

            ValidateProfile(
                QaProfilePath,
                "Windows_QA",
                DevelopmentAndQaScenes,
                new[] { QaDefine },
                developmentBuild: true,
                errors);

            ValidateProfile(
                H6CandidateProfilePath,
                "Windows_H6_Candidate",
                ProductionScenes,
                new[] { H6CandidateDefine },
                developmentBuild: false,
                errors);

            if (AssetDatabase.LoadAssetAtPath<TextAsset>(MatrixPath) == null)
            {
                errors.Add("Build profile matrix is missing: " + MatrixPath);
            }

            return errors;
        }

        public static ProfileSnapshot ReadProfile(string assetPath)
        {
            string fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                assetPath);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            string[] lines = File.ReadAllLines(fullPath);
            ProfileSnapshot snapshot = new ProfileSnapshot
            {
                AssetPath = assetPath,
                Name = ReadString(lines, "m_Name:"),
                BuildTarget = ReadInt(lines, "m_BuildTarget:"),
                Subtarget = ReadInt(lines, "m_Subtarget:"),
                OverrideGlobalSceneList =
                    ReadInt(lines, "m_OverrideGlobalSceneList:") == 1,
                HasScriptingDefines =
                    ReadInt(lines, "m_HasScriptingDefines:") == 1,
                DevelopmentBuild = ReadInt(lines, "m_Development:") == 1,
                ConnectProfiler = ReadInt(lines, "m_ConnectProfiler:") == 1,
                DeepProfiling =
                    ReadInt(lines, "m_BuildWithDeepProfilingSupport:") == 1,
                ScriptDebugging = ReadInt(lines, "m_AllowDebugging:") == 1,
                WaitForManagedDebugger =
                    ReadInt(lines, "m_WaitForManagedDebugger:") == 1,
                CopyPdbFiles = ReadInt(lines, "m_CopyPDBFiles:") == 1,
                CreateSolution = ReadInt(lines, "m_CreateSolution:") == 1,
                CompressionType = ReadInt(lines, "m_CompressionType:"),
                DiagnosticDataState =
                    ReadInt(lines, "m_BuildProfileEngineDiagnosticsState:"),
                Architecture = ReadInt(lines, "m_Architecture:"),
                Scenes = ReadScenes(lines),
                ScriptingDefines = ReadDefines(lines)
            };

            return snapshot;
        }

        private static void ValidateGlobalPlayerSettings(List<string> errors)
        {
            if (PlayerSettings.defaultScreenWidth != 1920 ||
                PlayerSettings.defaultScreenHeight != 1080)
            {
                errors.Add(
                    "Global Windows defaults must be 1920x1080; found " +
                    PlayerSettings.defaultScreenWidth + "x" +
                    PlayerSettings.defaultScreenHeight + ".");
            }

            if (!PlayerSettings.resizableWindow)
            {
                errors.Add("The Windows player window must be resizable.");
            }

            if (PlayerSettings.fullScreenMode != FullScreenMode.FullScreenWindow)
            {
                errors.Add("The approved default fullscreen mode is FullScreenWindow.");
            }

            if (PlayerSettings.runInBackground)
            {
                errors.Add("Run In Background must remain disabled for the approved player baseline.");
            }

            if (!PlayerSettings.usePlayerLog)
            {
                errors.Add("Player Log must remain enabled for Development, QA and H6 evidence.");
            }

            if (PlayerSettings.enableFrameTimingStats)
            {
                errors.Add(
                    "Global frame timing stats must remain disabled; use the dedicated Phase 3 build when required.");
            }

            if (!string.Equals(
                    PlayerSettings.companyName,
                    "VRM Games",
                    StringComparison.Ordinal) ||
                !string.Equals(
                    PlayerSettings.productName,
                    "Cartridge & Cloud",
                    StringComparison.Ordinal))
            {
                errors.Add(
                    "Company or product name changed; this would alter persistent data and Player.log paths.");
            }

            if (PlayerSettings.GetScriptingBackend(NamedBuildTarget.Standalone) !=
                ScriptingImplementation.Mono2x)
            {
                errors.Add(
                    "Standalone scripting backend must remain Mono until a separate IL2CPP gate is approved.");
            }
        }

        private static void ValidateProfile(
            string assetPath,
            string expectedName,
            IReadOnlyList<string> expectedScenes,
            IReadOnlyList<string> expectedDefines,
            bool developmentBuild,
            List<string> errors)
        {
            ProfileSnapshot profile = ReadProfile(assetPath);
            if (profile == null)
            {
                errors.Add("Build profile is missing: " + assetPath);
                return;
            }

            if (!string.Equals(profile.Name, expectedName, StringComparison.Ordinal))
            {
                errors.Add(assetPath + " has an unexpected profile name: " + profile.Name);
            }

            if (profile.BuildTarget != 19 || profile.Subtarget != 2)
            {
                errors.Add(assetPath + " is not configured for Windows Standalone x64.");
            }

            if (!profile.OverrideGlobalSceneList)
            {
                errors.Add(assetPath + " must override the global scene list.");
            }

            if (!profile.HasScriptingDefines)
            {
                errors.Add(assetPath + " must declare its profile-specific scripting define.");
            }

            if (!profile.Scenes.SequenceEqual(expectedScenes))
            {
                errors.Add(
                    assetPath + " has unexpected scenes. Expected: " +
                    string.Join(", ", expectedScenes) + "; actual: " +
                    string.Join(", ", profile.Scenes) + ".");
            }

            if (!profile.ScriptingDefines.OrderBy(value => value, StringComparer.Ordinal)
                .SequenceEqual(expectedDefines.OrderBy(value => value, StringComparer.Ordinal)))
            {
                errors.Add(
                    assetPath + " has unexpected scripting defines: " +
                    string.Join(", ", profile.ScriptingDefines) + ".");
            }

            if (profile.ScriptingDefines.Contains(TemporaryPhase3QaDefine))
            {
                errors.Add(
                    assetPath + " must not inherit the temporary Phase 3 performance instrumentation define.");
            }

            if (profile.DevelopmentBuild != developmentBuild)
            {
                errors.Add(
                    assetPath + " has an incorrect Development Build setting.");
            }

            if (profile.ConnectProfiler || profile.DeepProfiling ||
                profile.ScriptDebugging || profile.WaitForManagedDebugger ||
                profile.CopyPdbFiles || profile.CreateSolution)
            {
                errors.Add(
                    assetPath + " enables an unauthorized profiler, debugger, symbol or solution option.");
            }

            if (profile.CompressionType != 2)
            {
                errors.Add(assetPath + " must use the approved LZ4 compression baseline.");
            }

            if (profile.DiagnosticDataState != 2)
            {
                errors.Add(assetPath + " must explicitly disable Unity diagnostic data collection.");
            }

            if (profile.Architecture != 0)
            {
                errors.Add(assetPath + " must target Intel 64-bit architecture.");
            }

            if (!developmentBuild &&
                profile.Scenes.Contains("Assets/_Project/Scenes/Test/TestLab.unity"))
            {
                errors.Add("The H6 candidate profile must exclude TestLab.");
            }
        }

        private static void BuildApprovedProfile(
            string profileName,
            string[] scenes,
            string[] defines,
            bool developmentBuild,
            string outputPath)
        {
            IReadOnlyList<string> validationErrors = ValidateAllProfiles();
            if (validationErrors.Count > 0)
            {
                throw new BuildFailedException(
                    "Phase 6 profile validation failed before build:\n- " +
                    string.Join("\n- ", validationErrors));
            }

            string directory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new BuildFailedException("Build output directory is invalid.");
            }

            Directory.CreateDirectory(directory);
            BuildOptions buildOptions =
                BuildOptions.CleanBuildCache |
                BuildOptions.StrictMode;
            if (developmentBuild)
            {
                buildOptions |= BuildOptions.Development;
            }

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outputPath,
                target = BuildTarget.StandaloneWindows64,
                options = buildOptions,
                extraScriptingDefines = defines
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException(
                    profileName + " build failed with result " +
                    report.summary.result + ". Errors: " +
                    report.summary.totalErrors + "; warnings: " +
                    report.summary.totalWarnings + ".");
            }

            WriteManifest(profileName, scenes, defines, report.summary);
            Debug.Log(
                profileName + " Windows x64 build PASS: " +
                report.summary.outputPath + " (" +
                report.summary.totalSize + " bytes, " +
                report.summary.totalWarnings + " warning(s)).");
        }

        private static void WriteManifest(
            string profileName,
            string[] scenes,
            string[] defines,
            BuildSummary summary)
        {
            BuildManifest manifest = new BuildManifest
            {
                profile = profileName,
                createdUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
                unityVersion = global::UnityEngine.Application.unityVersion,
                applicationVersion = PlayerSettings.bundleVersion,
                target = BuildTarget.StandaloneWindows64.ToString(),
                outputPath = summary.outputPath,
                totalSizeBytes = summary.totalSize,
                warningCount = summary.totalWarnings,
                errorCount = summary.totalErrors,
                scenes = scenes,
                scriptingDefines = defines
            };

            string directory = Path.GetDirectoryName(summary.outputPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }

            File.WriteAllText(
                Path.Combine(directory, "BUILD_PROFILE_MANIFEST.json"),
                JsonUtility.ToJson(manifest, true));
        }

        private static string ReadString(string[] lines, string key)
        {
            string line = lines.FirstOrDefault(value => value.TrimStart().StartsWith(key, StringComparison.Ordinal));
            return line == null ? string.Empty : line.Substring(line.IndexOf(key, StringComparison.Ordinal) + key.Length).Trim();
        }

        private static int ReadInt(string[] lines, string key)
        {
            string value = ReadString(lines, key);
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed)
                ? parsed
                : int.MinValue;
        }

        private static string[] ReadScenes(string[] lines)
        {
            List<string> scenes = new List<string>();
            bool inScenes = false;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed == "m_Scenes:")
                {
                    inScenes = true;
                    continue;
                }

                if (inScenes && trimmed.StartsWith("m_HasScriptingDefines:", StringComparison.Ordinal))
                {
                    break;
                }

                if (inScenes && trimmed.StartsWith("m_path:", StringComparison.Ordinal))
                {
                    scenes.Add(trimmed.Substring("m_path:".Length).Trim());
                }
            }

            return scenes.ToArray();
        }

        private static string[] ReadDefines(string[] lines)
        {
            List<string> defines = new List<string>();
            bool inDefines = false;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed == "m_ScriptingDefines:")
                {
                    inDefines = true;
                    continue;
                }

                if (inDefines && trimmed.StartsWith("m_PlayerSettingsYaml:", StringComparison.Ordinal))
                {
                    break;
                }

                if (inDefines && trimmed.StartsWith("- ", StringComparison.Ordinal))
                {
                    defines.Add(trimmed.Substring(2).Trim());
                }
            }

            return defines.ToArray();
        }

        public sealed class ProfileSnapshot
        {
            public string AssetPath { get; set; }
            public string Name { get; set; }
            public int BuildTarget { get; set; }
            public int Subtarget { get; set; }
            public bool OverrideGlobalSceneList { get; set; }
            public bool HasScriptingDefines { get; set; }
            public bool DevelopmentBuild { get; set; }
            public bool ConnectProfiler { get; set; }
            public bool DeepProfiling { get; set; }
            public bool ScriptDebugging { get; set; }
            public bool WaitForManagedDebugger { get; set; }
            public bool CopyPdbFiles { get; set; }
            public bool CreateSolution { get; set; }
            public int CompressionType { get; set; }
            public int DiagnosticDataState { get; set; }
            public int Architecture { get; set; }
            public string[] Scenes { get; set; }
            public string[] ScriptingDefines { get; set; }
        }

        [Serializable]
        private sealed class BuildManifest
        {
            public string profile;
            public string createdUtc;
            public string unityVersion;
            public string applicationVersion;
            public string target;
            public string outputPath;
            public ulong totalSizeBytes;
            public int warningCount;
            public int errorCount;
            public string[] scenes;
            public string[] scriptingDefines;
        }
    }
}
