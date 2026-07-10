using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.BuildProfiles;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.BuildProfiles
{
    public sealed class Sprint17Phase7H6CandidateBuildToolTests
    {
        private string temporaryRoot;

        [SetUp]
        public void SetUp()
        {
            temporaryRoot = Path.Combine(
                Path.GetTempPath(),
                "CartridgeAndCloud_Phase7_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temporaryRoot);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(temporaryRoot))
            {
                Directory.Delete(temporaryRoot, recursive: true);
            }
        }

        [Test]
        public void ProjectVersion_IsFrozenAtApprovedPhase7Version()
        {
            Assert.That(
                PlayerSettings.bundleVersion,
                Is.EqualTo(
                    Sprint17Phase7H6CandidateBuildTool
                        .ExpectedApplicationVersion));
        }

        [Test]
        public void H6Profile_IsLoadableThroughUnityBuildProfileApi()
        {
            BuildProfile profile = AssetDatabase.LoadAssetAtPath<BuildProfile>(
                Sprint17Phase7H6CandidateBuildTool.ProfilePath);

            Assert.That(profile, Is.Not.Null);
            Assert.That(
                AssetDatabase.AssetPathToGUID(
                    Sprint17Phase7H6CandidateBuildTool.ProfilePath),
                Has.Length.EqualTo(32));
        }

        [Test]
        public void H6Configuration_MatchesPhase7PreflightRequirements()
        {
            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ValidateConfiguration(),
                Is.Empty);
        }

        [Test]
        public void CandidateIdentity_AcceptsFormalBuildIdAndFullCommitSha()
        {
            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ValidateCandidateIdentity(
                    "H6-WIN-CAND-001",
                    "0123456789abcdef0123456789abcdef01234567"),
                Is.Empty);
        }

        [TestCase("H6-WIN-CAND-01")]
        [TestCase("H6-WIN-CAND-0001")]
        [TestCase("H6-CAND-001")]
        [TestCase("")]
        public void CandidateIdentity_RejectsInvalidBuildIds(string buildId)
        {
            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ValidateCandidateIdentity(
                    buildId,
                    "0123456789abcdef0123456789abcdef01234567"),
                Is.Not.Empty);
        }

        [TestCase("abc")]
        [TestCase("0123456789abcdef0123456789abcdef0123456g")]
        [TestCase("0000000000000000000000000000000000000000")]
        public void CandidateIdentity_RejectsInvalidCommitShas(string commitSha)
        {
            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ValidateCandidateIdentity(
                    "H6-WIN-CAND-001",
                    commitSha),
                Is.Not.Empty);
        }

        [Test]
        public void BuildName_UsesVersionDatePlatformAndBuildIteration()
        {
            string name =
                Sprint17Phase7H6CandidateBuildTool.CreateBuildName(
                    "0.0.26",
                    "H6-WIN-CAND-007",
                    new DateTime(
                        2026,
                        7,
                        10,
                        15,
                        30,
                        0,
                        DateTimeKind.Utc));

            Assert.That(
                name,
                Is.EqualTo(
                    "CAC_v0.0.26_H6_Candidate_Windows_x64_2026-07-10_Build007"));
        }

        [Test]
        public void CandidatePaths_BlockExistingFrozenArtifacts()
        {
            Sprint17Phase7H6CandidateBuildTool.CandidatePaths paths =
                Sprint17Phase7H6CandidateBuildTool.CreateCandidatePaths(
                    temporaryRoot,
                    "Candidates",
                    "H6-WIN-CAND-001",
                    new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc));
            Directory.CreateDirectory(paths.CandidateDirectory);
            Directory.CreateDirectory(paths.CandidateRootDirectory);
            File.WriteAllText(paths.ZipPath, "existing");
            File.WriteAllText(paths.ChecksumPath, "existing");
            File.WriteAllText(paths.FreezeRecordPath, "existing");

            Assert.That(
                Sprint17Phase7H6CandidateBuildTool
                    .ValidateOutputAvailability(paths),
                Has.Count.EqualTo(4));
        }

        [Test]
        public void BuildId_CannotBeReusedAcrossDifferentCandidateDates()
        {
            string candidatesRoot = Path.Combine(temporaryRoot, "Candidates");
            Directory.CreateDirectory(candidatesRoot);
            File.WriteAllText(
                Path.Combine(
                    candidatesRoot,
                    "OldCandidate" +
                    Sprint17Phase7H6CandidateBuildTool.FreezeRecordSuffix),
                "{\n  \"buildId\": \"H6-WIN-CAND-001\"\n}");

            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ValidateBuildIdIsUnused(
                    candidatesRoot,
                    "H6-WIN-CAND-001"),
                Is.Not.Empty);
        }

        [Test]
        public void OutputAudit_RejectsPdbAndUnauthorizedDiagnosticFolders()
        {
            string output = Path.Combine(temporaryRoot, "Candidate");
            Directory.CreateDirectory(output);
            File.WriteAllText(
                Path.Combine(
                    output,
                    Sprint17Phase7H6CandidateBuildTool.ExecutableName),
                "player");
            Directory.CreateDirectory(
                Path.Combine(output, "CartridgeAndCloud_Data"));
            File.WriteAllText(Path.Combine(output, "GameAssembly.pdb"), "symbols");
            Directory.CreateDirectory(
                Path.Combine(output, "BurstDebugInformation_DoNotShip"));

            Assert.That(
                Sprint17Phase7H6CandidateBuildTool
                    .ValidateBuiltPlayerOutput(output),
                Has.Count.EqualTo(2));
        }

        [Test]
        public void FileInventory_IsSortedAndContainsDeterministicHashes()
        {
            string root = Path.Combine(temporaryRoot, "Inventory");
            Directory.CreateDirectory(Path.Combine(root, "Sub"));
            File.WriteAllText(
                Path.Combine(root, "z.txt"),
                "z",
                new UTF8Encoding(false));
            File.WriteAllText(
                Path.Combine(root, "Sub", "a.txt"),
                "a",
                new UTF8Encoding(false));

            Sprint17Phase7H6CandidateBuildTool.CandidateFileInventory inventory =
                Sprint17Phase7H6CandidateBuildTool.CreateFileInventory(
                    root,
                    "Inventory",
                    new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc),
                    Sprint17Phase7H6CandidateBuildTool.InventoryFileName);

            CollectionAssert.AreEqual(
                new[] { "Sub/a.txt", "z.txt" },
                inventory.files.Select(entry => entry.relativePath).ToArray());
            Assert.That(
                inventory.files.All(entry => entry.sha256.Length == 64),
                Is.True);
            Assert.That(inventory.inventoryExcludesSelf, Is.True);
        }

        [Test]
        public void Sha256_IsStableForKnownContent()
        {
            string path = Path.Combine(temporaryRoot, "known.txt");
            File.WriteAllText(path, "abc", new UTF8Encoding(false));

            Assert.That(
                Sprint17Phase7H6CandidateBuildTool.ComputeSha256(path),
                Is.EqualTo(
                    "ba7816bf8f01cfea414140de5dae2223" +
                    "b00361a396177a9cb410ff61f20015ad"));
        }

        [Test]
        public void CandidateArchive_IncludesSingleBuildRootAndAllFiles()
        {
            string source = Path.Combine(temporaryRoot, "Build001");
            Directory.CreateDirectory(Path.Combine(source, "Data"));
            File.WriteAllText(Path.Combine(source, "Game.exe"), "game");
            File.WriteAllText(Path.Combine(source, "Data", "data.bin"), "data");
            string zip = Path.Combine(temporaryRoot, "Build001.zip");

            Sprint17Phase7H6CandidateBuildTool.CreateCandidateArchive(
                source,
                zip,
                "Build001");

            using (FileStream stream = File.OpenRead(zip))
            using (ZipArchive archive = new ZipArchive(
                       stream,
                       ZipArchiveMode.Read,
                       leaveOpen: false))
            {
                CollectionAssert.AreEqual(
                    new[]
                    {
                        "Build001/Data/data.bin",
                        "Build001/Game.exe"
                    },
                    archive.Entries
                        .Select(entry => entry.FullName)
                        .OrderBy(name => name, StringComparer.Ordinal)
                        .ToArray());
            }
        }

        [Test]
        public void Phase7Baseline_KeepsSaveSchemaAndGlobalDefinesUnchanged()
        {
            Assert.That(
                IntegratedGameStateSnapshot.CurrentSchemaVersion,
                Is.EqualTo(2));
            Assert.That(
                PlayerSettings.GetScriptingDefineSymbols(
                    NamedBuildTarget.Standalone),
                Is.Empty);
        }

        [Test]
        public void NewPhase7Scripts_HaveMetaFiles()
        {
            string root = Directory.GetCurrentDirectory();
            foreach (string path in new[]
            {
                "Assets/_Project/Editor/ProjectOrganization/BuildProfiles/" +
                "Sprint17Phase7H6CandidateBuildTool.cs.meta",
                "Assets/_Project/Tests/EditMode/BuildProfiles/" +
                "Sprint17Phase7H6CandidateBuildToolTests.cs.meta"
            })
            {
                Assert.That(
                    File.Exists(Path.Combine(root, path)),
                    Is.True,
                    "Missing meta file: " + path);
            }
        }
    }
}
