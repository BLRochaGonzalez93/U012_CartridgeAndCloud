using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class AssemblyDefinitionSmokeTests
    {
        private const string DomainPath =
            "Assets/_Project/Scripts/Domain/VRMGames.CartridgeAndCloud.Domain.asmdef";

        private const string ApplicationPath =
            "Assets/_Project/Scripts/Application/VRMGames.CartridgeAndCloud.Application.asmdef";

        private const string InfrastructurePath =
            "Assets/_Project/Scripts/Infrastructure/VRMGames.CartridgeAndCloud.Infrastructure.asmdef";

        private const string PresentationPath =
            "Assets/_Project/Scripts/Presentation/VRMGames.CartridgeAndCloud.Presentation.asmdef";

        private const string EditModePath =
            "Assets/_Project/Tests/EditMode/VRMGames.CartridgeAndCloud.Tests.EditMode.asmdef";

        private const string PlayModePath =
            "Assets/_Project/Tests/PlayMode/VRMGames.CartridgeAndCloud.Tests.PlayMode.asmdef";

        [Test]
        public void ProductionAssemblyDefinitions_MatchApprovedDependencyGraph()
        {
            AssertAssemblyDefinition(
                DomainPath,
                "VRMGames.CartridgeAndCloud.Domain",
                Array.Empty<string>(),
                noEngineReferences: true,
                includePlatforms: Array.Empty<string>(),
                overrideReferences: false,
                precompiledReferences: Array.Empty<string>());

            AssertAssemblyDefinition(
                ApplicationPath,
                "VRMGames.CartridgeAndCloud.Application",
                new[] { "VRMGames.CartridgeAndCloud.Domain" },
                noEngineReferences: true,
                includePlatforms: Array.Empty<string>(),
                overrideReferences: false,
                precompiledReferences: Array.Empty<string>());

            AssertAssemblyDefinition(
                InfrastructurePath,
                "VRMGames.CartridgeAndCloud.Infrastructure",
                new[]
                {
                    "VRMGames.CartridgeAndCloud.Domain",
                    "VRMGames.CartridgeAndCloud.Application"
                },
                noEngineReferences: false,
                includePlatforms: Array.Empty<string>(),
                overrideReferences: false,
                precompiledReferences: Array.Empty<string>());

            AssertAssemblyDefinition(
                PresentationPath,
                "VRMGames.CartridgeAndCloud.Presentation",
                new[]
                {
                    "VRMGames.CartridgeAndCloud.Domain",
                    "VRMGames.CartridgeAndCloud.Application"
                },
                noEngineReferences: false,
                includePlatforms: Array.Empty<string>(),
                overrideReferences: false,
                precompiledReferences: Array.Empty<string>());
        }

        [Test]
        public void TestAssemblyDefinitions_MatchApprovedReferencesAndPlatforms()
        {
            AssertAssemblyDefinition(
                EditModePath,
                "VRMGames.CartridgeAndCloud.Tests.EditMode",
                new[]
                {
                    "VRMGames.CartridgeAndCloud.Domain",
                    "VRMGames.CartridgeAndCloud.Application",
                    "VRMGames.CartridgeAndCloud.Infrastructure",
                    "VRMGames.CartridgeAndCloud.Presentation",
                    "UnityEngine.TestRunner",
                    "UnityEditor.TestRunner"
                },
                noEngineReferences: false,
                includePlatforms: new[] { "Editor" },
                overrideReferences: true,
                precompiledReferences: new[] { "nunit.framework.dll" });

            AssertAssemblyDefinition(
                PlayModePath,
                "VRMGames.CartridgeAndCloud.Tests.PlayMode",
                new[]
                {
                    "VRMGames.CartridgeAndCloud.Domain",
                    "VRMGames.CartridgeAndCloud.Application",
                    "VRMGames.CartridgeAndCloud.Infrastructure",
                    "VRMGames.CartridgeAndCloud.Presentation",
                    "UnityEngine.TestRunner"
                },
                noEngineReferences: false,
                includePlatforms: Array.Empty<string>(),
                overrideReferences: true,
                precompiledReferences: new[] { "nunit.framework.dll" });
        }

        private static void AssertAssemblyDefinition(
            string assetPath,
            string expectedName,
            string[] expectedReferences,
            bool noEngineReferences,
            string[] includePlatforms,
            bool overrideReferences,
            string[] precompiledReferences)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);

            Assert.That(
                File.Exists(fullPath),
                Is.True,
                $"Assembly Definition not found: {assetPath}");

            AssemblyDefinitionData data =
                JsonUtility.FromJson<AssemblyDefinitionData>(File.ReadAllText(fullPath));

            Assert.That(data, Is.Not.Null);
            Assert.That(data.name, Is.EqualTo(expectedName));
            Assert.That(data.rootNamespace, Is.EqualTo(expectedName));
            Assert.That(data.autoReferenced, Is.False);
            Assert.That(data.noEngineReferences, Is.EqualTo(noEngineReferences));
            Assert.That(data.overrideReferences, Is.EqualTo(overrideReferences));

            CollectionAssert.AreEquivalent(
                expectedReferences,
                data.references ?? Array.Empty<string>(),
                $"Unexpected references in {expectedName}");

            CollectionAssert.AreEquivalent(
                includePlatforms,
                data.includePlatforms ?? Array.Empty<string>(),
                $"Unexpected platform configuration in {expectedName}");

            CollectionAssert.AreEquivalent(
                precompiledReferences,
                data.precompiledReferences ?? Array.Empty<string>(),
                $"Unexpected precompiled references in {expectedName}");
        }

        [Serializable]
        private sealed class AssemblyDefinitionData
        {
            public string name;
            public string rootNamespace;
            public string[] references;
            public string[] includePlatforms;
            public bool autoReferenced;
            public bool noEngineReferences;
            public bool overrideReferences;
            public string[] precompiledReferences;
        }
    }
}
