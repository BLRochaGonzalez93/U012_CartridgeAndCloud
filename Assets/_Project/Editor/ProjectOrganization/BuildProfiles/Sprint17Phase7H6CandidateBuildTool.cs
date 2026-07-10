using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Validation;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.BuildProfiles
{
    /// <summary>
    /// Produces the immutable Windows x64 H6 candidate for Sprint 17 Phase 7.
    /// The tool validates the approved Phase 6 profile, builds through that
    /// profile, audits the output, writes traceability records and freezes the
    /// exact player as a checksummed ZIP without using external tooling.
    /// </summary>
    public static class Sprint17Phase7H6CandidateBuildTool
    {
        public const string ExpectedApplicationVersion = "0.0.26";
        public const string ProfileName = "Windows_H6_Candidate";
        public const string ProfilePath =
            Sprint17Phase6BuildProfileTool.H6CandidateProfilePath;
        public const string CandidateRootPath =
            "Builds/Windows/Candidates";
        public const string ExecutableName = "CartridgeAndCloud.exe";
        public const string ManifestFileName = "H6_CANDIDATE_MANIFEST.json";
        public const string InventoryFileName = "H6_CANDIDATE_FILE_INVENTORY.json";
        public const string FreezeRecordSuffix = ".freeze.json";
        public const string ValidationStatus =
            "BUILT_PENDING_EXTERNAL_VALIDATION";

        private const string BuildIdPattern = @"^H6-WIN-CAND-[0-9]{3}$";
        private const string CommitShaPattern = @"^[0-9a-fA-F]{40}$";
        private const string BuildIdJsonField = "\"buildId\"";

        private static readonly string[] ProhibitedExtensions =
        {
            ".pdb",
            ".mdb",
            ".dbg",
            ".sln",
            ".csproj",
            ".user"
        };

        private static readonly string[] ProhibitedPathFragments =
        {
            "BurstDebugInformation_DoNotShip",
            "BackUpThisFolder_ButDontShipItWithYourGame",
            "DebugSymbols",
            "Symbols"
        };

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 7/Open H6 Candidate Builder")]
        public static void OpenWindow()
        {
            Sprint17Phase7H6CandidateBuildWindow window =
                EditorWindow.GetWindow<Sprint17Phase7H6CandidateBuildWindow>();
            window.titleContent = new GUIContent("H6 Candidate");
            window.minSize = new Vector2(560f, 310f);
            window.Show();
        }

        [MenuItem("Cartridge & Cloud/Sprint 17/Phase 7/Validate H6 Candidate Preflight")]
        public static void ValidatePreflightFromMenu()
        {
            IReadOnlyList<string> errors = ValidateConfiguration();
            if (errors.Count == 0)
            {
                Debug.Log(
                    "Sprint 17 Phase 7 H6 candidate preflight PASS. " +
                    "Version 0.0.26, profile, scenes, diagnostics, save schema " +
                    "and release structure match the approved baseline.");
                return;
            }

            Debug.LogError(
                "Sprint 17 Phase 7 H6 candidate preflight FAILED:\n- " +
                string.Join("\n- ", errors));
        }

        /// <summary>
        /// Builds and freezes a single H6 candidate. Existing candidate paths
        /// are never overwritten; a new Build ID is required for a new build.
        /// </summary>
        public static CandidateBuildResult BuildAndFreezeCandidate(
            string buildId,
            string commitSha)
        {
            DateTime buildTimeUtc = DateTime.UtcNow;
            IReadOnlyList<string> errors = ValidatePreflight(
                buildId,
                commitSha,
                buildTimeUtc,
                GetProjectRoot(),
                CandidateRootPath);
            if (errors.Count > 0)
            {
                throw new BuildFailedException(
                    "Sprint 17 Phase 7 preflight failed:\n- " +
                    string.Join("\n- ", errors));
            }

            if (BuildPipeline.isBuildingPlayer)
            {
                throw new BuildFailedException(
                    "Unity is already building a Player. The H6 candidate was not started.");
            }

            string normalizedSha = commitSha.Trim().ToLowerInvariant();
            CandidatePaths paths = CreateCandidatePaths(
                GetProjectRoot(),
                CandidateRootPath,
                buildId,
                buildTimeUtc);
            string stagingDirectory = CreateStagingDirectory(paths);
            bool finalDirectoryCreated = false;
            bool freezeCompleted = false;

            try
            {
                BuildProfile buildProfile =
                    AssetDatabase.LoadAssetAtPath<BuildProfile>(ProfilePath);
                if (buildProfile == null)
                {
                    throw new BuildFailedException(
                        "The approved H6 Build Profile could not be loaded: " +
                        ProfilePath);
                }

                string stagingExecutablePath = Path.Combine(
                    stagingDirectory,
                    ExecutableName);
                BuildPlayerWithProfileOptions options =
                    new BuildPlayerWithProfileOptions
                    {
                        buildProfile = buildProfile,
                        locationPathName = stagingExecutablePath,
                        options = BuildOptions.CleanBuildCache |
                                  BuildOptions.StrictMode
                    };

                BuildReport report = BuildPipeline.BuildPlayer(options);
                BuildSummary summary = report.summary;
                if (summary.result != BuildResult.Succeeded ||
                    summary.totalErrors > 0)
                {
                    throw new BuildFailedException(
                        "H6 candidate build failed with result " +
                        summary.result + ". Errors: " +
                        summary.totalErrors + "; warnings: " +
                        summary.totalWarnings + ".");
                }

                IReadOnlyList<string> outputErrors =
                    ValidateBuiltPlayerOutput(stagingDirectory);
                if (outputErrors.Count > 0)
                {
                    throw new BuildFailedException(
                        "H6 candidate output audit failed:\n- " +
                        string.Join("\n- ", outputErrors));
                }

                CandidateManifest manifest = CreateManifest(
                    buildId,
                    normalizedSha,
                    buildTimeUtc,
                    paths,
                    summary,
                    stagingDirectory);
                WriteJson(
                    Path.Combine(stagingDirectory, ManifestFileName),
                    manifest);

                CandidateFileInventory inventory = CreateFileInventory(
                    stagingDirectory,
                    paths.BuildName,
                    buildTimeUtc,
                    InventoryFileName);
                WriteJson(
                    Path.Combine(stagingDirectory, InventoryFileName),
                    inventory);

                Directory.Move(stagingDirectory, paths.CandidateDirectory);
                finalDirectoryCreated = true;
                RemoveEmptyStagingRoot(paths.StagingRootDirectory);

                CreateCandidateArchive(
                    paths.CandidateDirectory,
                    paths.ZipPath,
                    paths.BuildName);
                string zipSha256 = ComputeSha256(paths.ZipPath);
                File.WriteAllText(
                    paths.ChecksumPath,
                    zipSha256 + "  " + Path.GetFileName(paths.ZipPath) +
                    Environment.NewLine,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

                CandidateFreezeRecord freezeRecord = new CandidateFreezeRecord
                {
                    schemaVersion = 1,
                    buildId = buildId.Trim(),
                    buildName = paths.BuildName,
                    applicationVersion = PlayerSettings.bundleVersion,
                    commitSha = normalizedSha,
                    zipFileName = Path.GetFileName(paths.ZipPath),
                    zipSha256 = zipSha256,
                    frozenUtc = DateTime.UtcNow.ToString(
                        "O",
                        CultureInfo.InvariantCulture),
                    validationStatus = ValidationStatus
                };
                WriteJson(paths.FreezeRecordPath, freezeRecord);
                freezeCompleted = true;

                CandidateBuildResult result = new CandidateBuildResult
                {
                    BuildId = buildId.Trim(),
                    BuildName = paths.BuildName,
                    CandidateDirectory = paths.CandidateDirectory,
                    ZipPath = paths.ZipPath,
                    ChecksumPath = paths.ChecksumPath,
                    FreezeRecordPath = paths.FreezeRecordPath,
                    ZipSha256 = zipSha256,
                    WarningCount = summary.totalWarnings,
                    TotalSizeBytes = summary.totalSize
                };

                Debug.Log(
                    "Sprint 17 Phase 7 H6 candidate BUILT and FROZEN. " +
                    "Build ID: " + result.BuildId + "; version: " +
                    PlayerSettings.bundleVersion + "; SHA: " +
                    normalizedSha + "; ZIP SHA-256: " + zipSha256 +
                    "; status: " + ValidationStatus + ". " +
                    "External validation and Player.log review are still required.");
                return result;
            }
            catch
            {
                if (!freezeCompleted)
                {
                    if (Directory.Exists(stagingDirectory))
                    {
                        Directory.Delete(stagingDirectory, recursive: true);
                    }

                    if (finalDirectoryCreated &&
                        Directory.Exists(paths.CandidateDirectory))
                    {
                        Directory.Delete(
                            paths.CandidateDirectory,
                            recursive: true);
                    }

                    foreach (string generatedFile in new[]
                    {
                        paths.ZipPath,
                        paths.ChecksumPath,
                        paths.FreezeRecordPath
                    })
                    {
                        if (File.Exists(generatedFile))
                        {
                            File.Delete(generatedFile);
                        }
                    }

                    RemoveEmptyStagingRoot(paths.StagingRootDirectory);
                }

                throw;
            }
        }

        public static IReadOnlyList<string> ValidateConfiguration()
        {
            List<string> errors = new List<string>();
            errors.AddRange(Sprint17Phase6BuildProfileTool.ValidateAllProfiles());
            errors.AddRange(W8ReleaseValidationTool.ValidateProject());

            if (!string.Equals(
                    PlayerSettings.bundleVersion,
                    ExpectedApplicationVersion,
                    StringComparison.Ordinal))
            {
                errors.Add(
                    "PlayerSettings.bundleVersion must be " +
                    ExpectedApplicationVersion + "; found " +
                    PlayerSettings.bundleVersion + ".");
            }

            string globalDefines = PlayerSettings.GetScriptingDefineSymbols(
                NamedBuildTarget.Standalone);
            string[] normalizedGlobalDefines = SplitDefines(globalDefines);
            if (normalizedGlobalDefines.Length > 0)
            {
                errors.Add(
                    "Standalone global scripting defines must be empty for the " +
                    "H6 candidate; found: " +
                    string.Join(", ", normalizedGlobalDefines) + ".");
            }

            BuildProfile profile =
                AssetDatabase.LoadAssetAtPath<BuildProfile>(ProfilePath);
            if (profile == null)
            {
                errors.Add("H6 Build Profile is missing or unreadable: " + ProfilePath);
            }

            string profileGuid = AssetDatabase.AssetPathToGUID(ProfilePath);
            if (!IsGuid(profileGuid))
            {
                errors.Add("H6 Build Profile GUID is missing or invalid.");
            }

            foreach (string scenePath in
                     Sprint17Phase6BuildProfileTool.ProductionScenes)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) == null)
                {
                    errors.Add("Production scene is missing: " + scenePath);
                    continue;
                }

                string sceneGuid = AssetDatabase.AssetPathToGUID(scenePath);
                if (!IsGuid(sceneGuid))
                {
                    errors.Add("Production scene GUID is invalid: " + scenePath);
                }
            }

            int currentSaveSchemaVersion =
                IntegratedGameStateSnapshot.CurrentSchemaVersion;
            if (currentSaveSchemaVersion != 2)
            {
                errors.Add(
                    "Integrated save schema changed from the approved Phase 7 " +
                    "baseline value 2; found " +
                    currentSaveSchemaVersion + ".");
            }

            return errors;
        }

        public static IReadOnlyList<string> ValidatePreflight(
            string buildId,
            string commitSha,
            DateTime buildTimeUtc,
            string projectRoot,
            string candidateRootPath)
        {
            List<string> errors = new List<string>();
            errors.AddRange(ValidateConfiguration());
            errors.AddRange(ValidateCandidateIdentity(buildId, commitSha));

            if (errors.Count > 0)
            {
                return errors;
            }

            CandidatePaths paths = CreateCandidatePaths(
                projectRoot,
                candidateRootPath,
                buildId,
                buildTimeUtc);
            errors.AddRange(ValidateOutputAvailability(paths));
            errors.AddRange(ValidateBuildIdIsUnused(
                paths.CandidateRootDirectory,
                buildId));
            return errors;
        }

        public static IReadOnlyList<string> ValidateCandidateIdentity(
            string buildId,
            string commitSha)
        {
            List<string> errors = new List<string>();
            string normalizedBuildId = buildId == null ? string.Empty : buildId.Trim();
            string normalizedSha = commitSha == null ? string.Empty : commitSha.Trim();

            if (!Regex.IsMatch(
                    normalizedBuildId,
                    BuildIdPattern,
                    RegexOptions.CultureInvariant))
            {
                errors.Add(
                    "Build ID must use H6-WIN-CAND-NNN with exactly three digits.");
            }

            if (!Regex.IsMatch(
                    normalizedSha,
                    CommitShaPattern,
                    RegexOptions.CultureInvariant))
            {
                errors.Add("Commit SHA must contain exactly 40 hexadecimal characters.");
            }
            else if (normalizedSha.All(character => character == '0'))
            {
                errors.Add("Commit SHA cannot be the all-zero placeholder value.");
            }

            return errors;
        }

        public static string CreateBuildName(
            string applicationVersion,
            string buildId,
            DateTime buildTimeUtc)
        {
            int iteration = ParseBuildIteration(buildId);
            return "CAC_v" + applicationVersion +
                   "_H6_Candidate_Windows_x64_" +
                   buildTimeUtc.ToUniversalTime().ToString(
                       "yyyy-MM-dd",
                       CultureInfo.InvariantCulture) +
                   "_Build" + iteration.ToString("D3", CultureInfo.InvariantCulture);
        }

        public static int ParseBuildIteration(string buildId)
        {
            string normalized = buildId == null ? string.Empty : buildId.Trim();
            Match match = Regex.Match(
                normalized,
                @"([0-9]+)$",
                RegexOptions.CultureInvariant);
            if (!match.Success ||
                !int.TryParse(
                    match.Groups[1].Value,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out int iteration) ||
                iteration <= 0)
            {
                throw new ArgumentException(
                    "Build ID does not contain a positive numeric iteration.",
                    nameof(buildId));
            }

            return iteration;
        }

        public static CandidatePaths CreateCandidatePaths(
            string projectRoot,
            string candidateRootPath,
            string buildId,
            DateTime buildTimeUtc)
        {
            string root = Path.GetFullPath(projectRoot);
            string buildName = CreateBuildName(
                ExpectedApplicationVersion,
                buildId,
                buildTimeUtc);
            string candidatesRoot = Path.GetFullPath(
                Path.Combine(root, candidateRootPath));

            return new CandidatePaths
            {
                BuildName = buildName,
                CandidateRootDirectory = candidatesRoot,
                CandidateDirectory = Path.Combine(candidatesRoot, buildName),
                StagingRootDirectory = Path.Combine(candidatesRoot, ".staging"),
                ZipPath = Path.Combine(candidatesRoot, buildName + ".zip"),
                ChecksumPath = Path.Combine(candidatesRoot, buildName + ".sha256"),
                FreezeRecordPath = Path.Combine(
                    candidatesRoot,
                    buildName + FreezeRecordSuffix)
            };
        }

        public static IReadOnlyList<string> ValidateOutputAvailability(
            CandidatePaths paths)
        {
            List<string> errors = new List<string>();
            if (Directory.Exists(paths.CandidateDirectory))
            {
                errors.Add(
                    "Candidate directory already exists and cannot be overwritten: " +
                    paths.CandidateDirectory);
            }

            foreach (string filePath in new[]
            {
                paths.ZipPath,
                paths.ChecksumPath,
                paths.FreezeRecordPath
            })
            {
                if (File.Exists(filePath))
                {
                    errors.Add(
                        "Frozen candidate record already exists and cannot be overwritten: " +
                        filePath);
                }
            }

            return errors;
        }


        public static IReadOnlyList<string> ValidateBuildIdIsUnused(
            string candidateRootDirectory,
            string buildId)
        {
            List<string> errors = new List<string>();
            if (!Directory.Exists(candidateRootDirectory))
            {
                return errors;
            }

            string normalizedBuildId = buildId.Trim();
            foreach (string recordPath in Directory.GetFiles(
                         candidateRootDirectory,
                         "*" + FreezeRecordSuffix,
                         SearchOption.TopDirectoryOnly))
            {
                CandidateFreezeRecord record;
                try
                {
                    record = JsonUtility.FromJson<CandidateFreezeRecord>(
                        File.ReadAllText(recordPath));
                }
                catch (Exception exception)
                {
                    errors.Add(
                        "Existing freeze record could not be read: " +
                        recordPath + " (" + exception.Message + ").");
                    continue;
                }

                if (record != null && string.Equals(
                        record.buildId,
                        normalizedBuildId,
                        StringComparison.Ordinal))
                {
                    errors.Add(
                        "Build ID has already been frozen and cannot be reused: " +
                        normalizedBuildId + ".");
                }
            }

            foreach (string manifestPath in Directory.GetFiles(
                         candidateRootDirectory,
                         ManifestFileName,
                         SearchOption.AllDirectories))
            {
                string manifestText;
                try
                {
                    manifestText = File.ReadAllText(manifestPath);
                }
                catch (Exception exception)
                {
                    errors.Add(
                        "Existing candidate manifest could not be read: " +
                        manifestPath + " (" + exception.Message + ").");
                    continue;
                }

                if (manifestText.IndexOf(
                        BuildIdJsonField,
                        StringComparison.Ordinal) >= 0 &&
                    manifestText.IndexOf(
                        "\"" + normalizedBuildId + "\"",
                        StringComparison.Ordinal) >= 0)
                {
                    errors.Add(
                        "Build ID already exists in a candidate manifest and " +
                        "cannot be reused: " + normalizedBuildId + ".");
                }
            }

            return errors.Distinct(StringComparer.Ordinal).ToArray();
        }

        public static IReadOnlyList<string> ValidateBuiltPlayerOutput(
            string candidateDirectory)
        {
            List<string> errors = new List<string>();
            if (!Directory.Exists(candidateDirectory))
            {
                errors.Add("Candidate output directory does not exist.");
                return errors;
            }

            string executablePath = Path.Combine(
                candidateDirectory,
                ExecutableName);
            if (!File.Exists(executablePath))
            {
                errors.Add("The expected game executable is missing: " + ExecutableName);
            }

            string dataDirectory = Path.Combine(
                candidateDirectory,
                "CartridgeAndCloud_Data");
            if (!Directory.Exists(dataDirectory))
            {
                errors.Add("The expected CartridgeAndCloud_Data directory is missing.");
            }

            errors.AddRange(FindUnauthorizedArtifactFiles(candidateDirectory));
            return errors;
        }

        public static IReadOnlyList<string> FindUnauthorizedArtifactFiles(
            string rootDirectory)
        {
            List<string> errors = new List<string>();
            if (!Directory.Exists(rootDirectory))
            {
                return errors;
            }

            foreach (string directory in Directory.GetDirectories(
                         rootDirectory,
                         "*",
                         SearchOption.AllDirectories))
            {
                string normalized = NormalizeSeparators(directory);
                if (ProhibitedPathFragments.Any(fragment =>
                        normalized.IndexOf(
                            fragment,
                            StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    errors.Add(
                        "Unauthorized diagnostic directory found: " +
                        MakeRelativePath(rootDirectory, directory));
                }
            }

            foreach (string filePath in Directory.GetFiles(
                         rootDirectory,
                         "*",
                         SearchOption.AllDirectories))
            {
                string extension = Path.GetExtension(filePath);
                if (ProhibitedExtensions.Contains(
                        extension,
                        StringComparer.OrdinalIgnoreCase))
                {
                    errors.Add(
                        "Unauthorized symbol, solution or debug file found: " +
                        MakeRelativePath(rootDirectory, filePath));
                }
            }

            return errors;
        }

        public static CandidateFileInventory CreateFileInventory(
            string rootDirectory,
            string rootDirectoryName,
            DateTime generatedUtc,
            string excludedRelativePath)
        {
            string normalizedExcludedPath = NormalizeSeparators(
                excludedRelativePath ?? string.Empty);
            CandidateInventoryEntry[] entries = Directory.GetFiles(
                    rootDirectory,
                    "*",
                    SearchOption.AllDirectories)
                .Select(path => new
                {
                    FullPath = path,
                    RelativePath = MakeRelativePath(rootDirectory, path)
                })
                .Where(value => !string.Equals(
                    value.RelativePath,
                    normalizedExcludedPath,
                    StringComparison.Ordinal))
                .OrderBy(value => value.RelativePath, StringComparer.Ordinal)
                .Select(value => new CandidateInventoryEntry
                {
                    relativePath = value.RelativePath,
                    sizeBytes = new FileInfo(value.FullPath).Length,
                    sha256 = ComputeSha256(value.FullPath)
                })
                .ToArray();

            return new CandidateFileInventory
            {
                schemaVersion = 1,
                generatedUtc = generatedUtc.ToUniversalTime().ToString(
                    "O",
                    CultureInfo.InvariantCulture),
                rootDirectoryName = rootDirectoryName,
                inventoryExcludesSelf = true,
                files = entries
            };
        }

        public static string ComputeSha256(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            using (SHA256 algorithm = SHA256.Create())
            {
                byte[] hash = algorithm.ComputeHash(stream);
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                foreach (byte value in hash)
                {
                    builder.Append(value.ToString("x2", CultureInfo.InvariantCulture));
                }

                return builder.ToString();
            }
        }

        public static void CreateCandidateArchive(
            string sourceDirectory,
            string zipPath,
            string rootDirectoryName)
        {
            if (File.Exists(zipPath))
            {
                throw new IOException(
                    "Candidate ZIP already exists and cannot be overwritten: " +
                    zipPath);
            }

            string zipDirectory = Path.GetDirectoryName(zipPath);
            if (string.IsNullOrWhiteSpace(zipDirectory))
            {
                throw new IOException("Candidate ZIP directory is invalid.");
            }

            Directory.CreateDirectory(zipDirectory);
            string temporaryZipPath = zipPath + ".tmp-" +
                                      Guid.NewGuid().ToString("N");
            try
            {
                using (FileStream stream = new FileStream(
                           temporaryZipPath,
                           FileMode.CreateNew,
                           FileAccess.ReadWrite,
                           FileShare.None))
                using (ZipArchive archive = new ZipArchive(
                           stream,
                           ZipArchiveMode.Create,
                           leaveOpen: false))
                {
                    foreach (string filePath in Directory.GetFiles(
                                 sourceDirectory,
                                 "*",
                                 SearchOption.AllDirectories)
                             .OrderBy(
                                 path => MakeRelativePath(sourceDirectory, path),
                                 StringComparer.Ordinal))
                    {
                        string relativePath = MakeRelativePath(
                            sourceDirectory,
                            filePath);
                        string entryName = NormalizeSeparators(
                            rootDirectoryName + "/" + relativePath);
                        ZipArchiveEntry entry = archive.CreateEntry(
                            entryName,
                            System.IO.Compression.CompressionLevel.Optimal);
                        entry.LastWriteTime = File.GetLastWriteTime(filePath);

                        using (Stream input = File.OpenRead(filePath))
                        using (Stream output = entry.Open())
                        {
                            input.CopyTo(output);
                        }
                    }
                }

                File.Move(temporaryZipPath, zipPath);
            }
            finally
            {
                if (File.Exists(temporaryZipPath))
                {
                    File.Delete(temporaryZipPath);
                }
            }
        }

        private static CandidateManifest CreateManifest(
            string buildId,
            string commitSha,
            DateTime buildTimeUtc,
            CandidatePaths paths,
            BuildSummary summary,
            string stagingDirectory)
        {
            Sprint17Phase6BuildProfileTool.ProfileSnapshot profile =
                Sprint17Phase6BuildProfileTool.ReadProfile(ProfilePath);
            CandidateSceneEntry[] scenes =
                Sprint17Phase6BuildProfileTool.ProductionScenes
                    .Select(path => new CandidateSceneEntry
                    {
                        path = path,
                        guid = AssetDatabase.AssetPathToGUID(path)
                    })
                    .ToArray();
            CandidateOutputFileEntry[] playerFiles = Directory.GetFiles(
                    stagingDirectory,
                    "*",
                    SearchOption.AllDirectories)
                .OrderBy(
                    path => MakeRelativePath(stagingDirectory, path),
                    StringComparer.Ordinal)
                .Select(path => new CandidateOutputFileEntry
                {
                    relativePath = MakeRelativePath(stagingDirectory, path),
                    sizeBytes = new FileInfo(path).Length
                })
                .ToArray();

            return new CandidateManifest
            {
                schemaVersion = 1,
                buildId = buildId.Trim(),
                buildName = paths.BuildName,
                applicationVersion = PlayerSettings.bundleVersion,
                buildIteration = ParseBuildIteration(buildId),
                commitSha = commitSha,
                buildDateUtc = buildTimeUtc.ToUniversalTime().ToString(
                    "O",
                    CultureInfo.InvariantCulture),
                unityVersion = global::UnityEngine.Application.unityVersion,
                profileName = ProfileName,
                profileAssetPath = ProfilePath,
                profileAssetGuid = AssetDatabase.AssetPathToGUID(ProfilePath),
                platform = BuildTarget.StandaloneWindows64.ToString(),
                architecture = "Intel64",
                scriptingBackend = PlayerSettings.GetScriptingBackend(
                        NamedBuildTarget.Standalone)
                    .ToString(),
                compression = "LZ4",
                developmentBuild = false,
                connectProfiler = false,
                deepProfiling = false,
                scriptDebugging = false,
                waitForManagedDebugger = false,
                copyPdbFiles = false,
                diagnosticDataEnabled = false,
                playerLogEnabled = PlayerSettings.usePlayerLog,
                saveSchemaVersion =
                    IntegratedGameStateSnapshot.CurrentSchemaVersion,
                scenes = scenes,
                scriptingDefines = profile.ScriptingDefines,
                globalScriptingDefines = SplitDefines(
                    PlayerSettings.GetScriptingDefineSymbols(
                        NamedBuildTarget.Standalone)),
                playerFiles = playerFiles,
                playerFileCount = playerFiles.Length,
                totalSizeBytes = summary.totalSize,
                warningCount = summary.totalWarnings,
                errorCount = summary.totalErrors,
                validationStatus = ValidationStatus
            };
        }

        private static string CreateStagingDirectory(CandidatePaths paths)
        {
            Directory.CreateDirectory(paths.StagingRootDirectory);
            string directory = Path.Combine(
                paths.StagingRootDirectory,
                paths.BuildName + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            return directory;
        }

        private static void RemoveEmptyStagingRoot(string stagingRoot)
        {
            if (Directory.Exists(stagingRoot) &&
                !Directory.EnumerateFileSystemEntries(stagingRoot).Any())
            {
                Directory.Delete(stagingRoot);
            }
        }

        private static string GetProjectRoot()
        {
            return Path.GetFullPath(
                Path.Combine(global::UnityEngine.Application.dataPath, ".."));
        }

        private static void WriteJson<T>(string path, T value)
        {
            File.WriteAllText(
                path,
                JsonUtility.ToJson(value, prettyPrint: true) + Environment.NewLine,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private static string[] SplitDefines(string defines)
        {
            return (defines ?? string.Empty)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .Where(value => value.Length > 0)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(value => value, StringComparer.Ordinal)
                .ToArray();
        }

        private static bool IsGuid(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   Regex.IsMatch(
                       value,
                       "^[0-9a-fA-F]{32}$",
                       RegexOptions.CultureInvariant);
        }

        private static string MakeRelativePath(string rootDirectory, string path)
        {
            string root = Path.GetFullPath(rootDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) +
                Path.DirectorySeparatorChar;
            string fullPath = Path.GetFullPath(path);
            if (!fullPath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Path is outside the expected root directory.",
                    nameof(path));
            }

            return NormalizeSeparators(fullPath.Substring(root.Length));
        }

        private static string NormalizeSeparators(string path)
        {
            return (path ?? string.Empty).Replace('\\', '/');
        }

        public sealed class CandidatePaths
        {
            public string BuildName { get; set; }
            public string CandidateRootDirectory { get; set; }
            public string CandidateDirectory { get; set; }
            public string StagingRootDirectory { get; set; }
            public string ZipPath { get; set; }
            public string ChecksumPath { get; set; }
            public string FreezeRecordPath { get; set; }
        }

        public sealed class CandidateBuildResult
        {
            public string BuildId { get; set; }
            public string BuildName { get; set; }
            public string CandidateDirectory { get; set; }
            public string ZipPath { get; set; }
            public string ChecksumPath { get; set; }
            public string FreezeRecordPath { get; set; }
            public string ZipSha256 { get; set; }
            public int WarningCount { get; set; }
            public ulong TotalSizeBytes { get; set; }
        }

        [Serializable]
        public sealed class CandidateManifest
        {
            public int schemaVersion;
            public string buildId;
            public string buildName;
            public string applicationVersion;
            public int buildIteration;
            public string commitSha;
            public string buildDateUtc;
            public string unityVersion;
            public string profileName;
            public string profileAssetPath;
            public string profileAssetGuid;
            public string platform;
            public string architecture;
            public string scriptingBackend;
            public string compression;
            public bool developmentBuild;
            public bool connectProfiler;
            public bool deepProfiling;
            public bool scriptDebugging;
            public bool waitForManagedDebugger;
            public bool copyPdbFiles;
            public bool diagnosticDataEnabled;
            public bool playerLogEnabled;
            public int saveSchemaVersion;
            public CandidateSceneEntry[] scenes;
            public string[] scriptingDefines;
            public string[] globalScriptingDefines;
            public CandidateOutputFileEntry[] playerFiles;
            public int playerFileCount;
            public ulong totalSizeBytes;
            public int warningCount;
            public int errorCount;
            public string validationStatus;
        }

        [Serializable]
        public sealed class CandidateSceneEntry
        {
            public string path;
            public string guid;
        }

        [Serializable]
        public sealed class CandidateOutputFileEntry
        {
            public string relativePath;
            public long sizeBytes;
        }

        [Serializable]
        public sealed class CandidateFileInventory
        {
            public int schemaVersion;
            public string generatedUtc;
            public string rootDirectoryName;
            public bool inventoryExcludesSelf;
            public CandidateInventoryEntry[] files;
        }

        [Serializable]
        public sealed class CandidateInventoryEntry
        {
            public string relativePath;
            public long sizeBytes;
            public string sha256;
        }

        [Serializable]
        private sealed class CandidateFreezeRecord
        {
            public int schemaVersion;
            public string buildId;
            public string buildName;
            public string applicationVersion;
            public string commitSha;
            public string zipFileName;
            public string zipSha256;
            public string frozenUtc;
            public string validationStatus;
        }
    }

    /// <summary>
    /// Captures the externally supplied identity required to build the exact
    /// Sprint 17 Phase 7 H6 candidate from inside the Unity Editor.
    /// </summary>
    public sealed class Sprint17Phase7H6CandidateBuildWindow : EditorWindow
    {
        private string buildId = "H6-WIN-CAND-001";
        private string commitSha = string.Empty;
        private Vector2 scrollPosition;

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField(
                "Sprint 17 Phase 7 — H6 Candidate",
                EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This command uses Windows_H6_Candidate, creates a Windows x64 " +
                "Player in staging, audits it, writes the manifest and inventory, " +
                "then freezes the exact candidate as a checksummed ZIP. It does " +
                "not approve H6 or the Vertical Slice.",
                MessageType.Info);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField(
                    "Application Version",
                    PlayerSettings.bundleVersion);
                EditorGUILayout.TextField(
                    "Build Profile",
                    Sprint17Phase7H6CandidateBuildTool.ProfileName);
                EditorGUILayout.TextField(
                    "Output Root",
                    Sprint17Phase7H6CandidateBuildTool.CandidateRootPath);
            }

            EditorGUILayout.Space(6f);
            buildId = EditorGUILayout.TextField("Build ID", buildId);
            commitSha = EditorGUILayout.TextField("Commit SHA (40 hex)", commitSha);

            IReadOnlyList<string> identityErrors =
                Sprint17Phase7H6CandidateBuildTool.ValidateCandidateIdentity(
                    buildId,
                    commitSha);
            if (identityErrors.Count > 0)
            {
                EditorGUILayout.HelpBox(
                    string.Join("\n", identityErrors),
                    MessageType.Warning);
            }

            EditorGUILayout.Space(10f);
            if (GUILayout.Button("Validate Configuration", GUILayout.Height(28f)))
            {
                Sprint17Phase7H6CandidateBuildTool.ValidatePreflightFromMenu();
            }

            using (new EditorGUI.DisabledScope(identityErrors.Count > 0))
            {
                if (GUILayout.Button(
                        "Build and Freeze H6 Candidate",
                        GUILayout.Height(36f)))
                {
                    BuildCandidate();
                }
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.HelpBox(
                "Before building, confirm that the supplied SHA is the clean " +
                "candidate commit. After freezing, execute all Phase 7 checks " +
                "outside the Editor, review Player.log in full and never modify " +
                "the evaluated ZIP.",
                MessageType.None);
            EditorGUILayout.EndScrollView();
        }

        private void BuildCandidate()
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Freeze H6 Candidate",
                "Build ID: " + buildId.Trim() + "\n" +
                "Version: " + PlayerSettings.bundleVersion + "\n" +
                "Commit SHA: " + commitSha.Trim().ToLowerInvariant() + "\n\n" +
                "The resulting candidate paths cannot be overwritten. Continue?",
                "Build and Freeze",
                "Cancel");
            if (!confirmed)
            {
                return;
            }

            try
            {
                Sprint17Phase7H6CandidateBuildTool.CandidateBuildResult result =
                    Sprint17Phase7H6CandidateBuildTool.BuildAndFreezeCandidate(
                        buildId,
                        commitSha);
                EditorUtility.DisplayDialog(
                    "H6 Candidate Frozen",
                    "Build: " + result.BuildName + "\n" +
                    "ZIP: " + result.ZipPath + "\n" +
                    "SHA-256: " + result.ZipSha256 + "\n\n" +
                    "External validation and Player.log review are still pending.",
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "H6 Candidate Failed",
                    exception.Message,
                    "OK");
            }
        }
    }
}
