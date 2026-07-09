using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.BuildProfiles;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.BuildProfiles
{
    public sealed class Sprint17Phase6BuildProfileTests
    {
        [Test]
        public void AllProfiles_MatchApprovedPhase6Matrix()
        {
            Assert.That(
                Sprint17Phase6BuildProfileTool.ValidateAllProfiles(),
                Is.Empty);
        }

        [Test]
        public void DevelopmentProfile_IsDiagnosticSafeAndKeepsTestLab()
        {
            Sprint17Phase6BuildProfileTool.ProfileSnapshot profile =
                Sprint17Phase6BuildProfileTool.ReadProfile(
                    Sprint17Phase6BuildProfileTool.DevelopmentProfilePath);

            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.DevelopmentBuild, Is.True);
            Assert.That(profile.ConnectProfiler, Is.False);
            Assert.That(profile.DeepProfiling, Is.False);
            Assert.That(profile.ScriptDebugging, Is.False);
            Assert.That(profile.WaitForManagedDebugger, Is.False);
            Assert.That(
                profile.Scenes,
                Does.Contain("Assets/_Project/Scenes/Test/TestLab.unity"));
            CollectionAssert.AreEquivalent(
                new[] { Sprint17Phase6BuildProfileTool.DevelopmentDefine },
                profile.ScriptingDefines);
        }

        [Test]
        public void QaProfile_KeepsTestLabWithoutTemporaryPerformanceInstrumentation()
        {
            Sprint17Phase6BuildProfileTool.ProfileSnapshot profile =
                Sprint17Phase6BuildProfileTool.ReadProfile(
                    Sprint17Phase6BuildProfileTool.QaProfilePath);

            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.DevelopmentBuild, Is.True);
            Assert.That(
                profile.Scenes,
                Does.Contain("Assets/_Project/Scenes/Test/TestLab.unity"));
            Assert.That(
                profile.ScriptingDefines,
                Does.Not.Contain(
                    Sprint17Phase6BuildProfileTool.TemporaryPhase3QaDefine));
            CollectionAssert.AreEquivalent(
                new[] { Sprint17Phase6BuildProfileTool.QaDefine },
                profile.ScriptingDefines);
        }

        [Test]
        public void H6Candidate_ContainsOnlyProductionScenesAndNoDevelopmentDiagnostics()
        {
            Sprint17Phase6BuildProfileTool.ProfileSnapshot profile =
                Sprint17Phase6BuildProfileTool.ReadProfile(
                    Sprint17Phase6BuildProfileTool.H6CandidateProfilePath);

            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.DevelopmentBuild, Is.False);
            Assert.That(profile.ConnectProfiler, Is.False);
            Assert.That(profile.DeepProfiling, Is.False);
            Assert.That(profile.ScriptDebugging, Is.False);
            Assert.That(profile.WaitForManagedDebugger, Is.False);
            Assert.That(profile.CopyPdbFiles, Is.False);
            Assert.That(profile.CreateSolution, Is.False);
            Assert.That(profile.DiagnosticDataState, Is.EqualTo(2));
            CollectionAssert.AreEqual(
                Sprint17Phase6BuildProfileTool.ProductionScenes,
                profile.Scenes);
            Assert.That(
                profile.Scenes,
                Does.Not.Contain("Assets/_Project/Scenes/Test/TestLab.unity"));
            CollectionAssert.AreEquivalent(
                new[] { Sprint17Phase6BuildProfileTool.H6CandidateDefine },
                profile.ScriptingDefines);
        }

        [Test]
        public void GlobalPlayerSettings_ArePreparedForWindowsProfiles()
        {
            Assert.That(PlayerSettings.defaultScreenWidth, Is.EqualTo(1920));
            Assert.That(PlayerSettings.defaultScreenHeight, Is.EqualTo(1080));
            Assert.That(PlayerSettings.resizableWindow, Is.True);
            Assert.That(
                PlayerSettings.fullScreenMode,
                Is.EqualTo(FullScreenMode.FullScreenWindow));
            Assert.That(PlayerSettings.runInBackground, Is.False);
            Assert.That(PlayerSettings.usePlayerLog, Is.True);
            Assert.That(PlayerSettings.enableFrameTimingStats, Is.False);
            Assert.That(
                PlayerSettings.GetScriptingBackend(NamedBuildTarget.Standalone),
                Is.EqualTo(ScriptingImplementation.Mono2x));
        }

        [Test]
        public void BuildProfileMatrix_DocumentsAllThreeProfiles()
        {
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(
                Sprint17Phase6BuildProfileTool.MatrixPath);
            Assert.That(asset, Is.Not.Null);

            BuildProfileMatrix matrix =
                JsonUtility.FromJson<BuildProfileMatrix>(asset.text);
            Assert.That(matrix, Is.Not.Null);
            Assert.That(matrix.schemaVersion, Is.EqualTo(1));
            Assert.That(matrix.profiles, Is.Not.Null);
            Assert.That(matrix.profiles.Length, Is.EqualTo(3));

            CollectionAssert.AreEquivalent(
                new[]
                {
                    "Windows_Development",
                    "Windows_QA",
                    "Windows_H6_Candidate"
                },
                matrix.profiles.Select(profile => profile.name).ToArray());

            BuildProfileMatrixEntry h6 = matrix.profiles.Single(
                profile => profile.name == "Windows_H6_Candidate");
            Assert.That(h6.developmentBuild, Is.False);
            Assert.That(h6.testLabIncluded, Is.False);
            Assert.That(h6.diagnosticData, Is.EqualTo("Disabled"));
        }

        [Test]
        public void ProfileAssets_HaveMetaFiles()
        {
            string root = Directory.GetCurrentDirectory();
            foreach (string path in new[]
            {
                Sprint17Phase6BuildProfileTool.DevelopmentProfilePath,
                Sprint17Phase6BuildProfileTool.QaProfilePath,
                Sprint17Phase6BuildProfileTool.H6CandidateProfilePath,
                Sprint17Phase6BuildProfileTool.MatrixPath
            })
            {
                Assert.That(
                    File.Exists(Path.Combine(root, path + ".meta")),
                    Is.True,
                    "Missing meta file for " + path);
            }
        }

        [Serializable]
        private sealed class BuildProfileMatrix
        {
            public int schemaVersion;
            public BuildProfileMatrixEntry[] profiles;
        }

        [Serializable]
        private sealed class BuildProfileMatrixEntry
        {
            public string name;
            public bool developmentBuild;
            public bool testLabIncluded;
            public string diagnosticData;
        }
    }
}
