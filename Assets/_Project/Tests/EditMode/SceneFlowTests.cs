using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class SceneFlowTests
    {
        private static readonly string[] ExpectedScenePaths =
        {
            "Assets/_Project/Scenes/Bootstrap.unity",
            "Assets/_Project/Scenes/MainMenu.unity",
            "Assets/_Project/Scenes/StoreInitial.unity",
            "Assets/_Project/Scenes/TestLab.unity"
        };

        [Test]
        public void SceneAssets_ExistAtApprovedPaths()
        {
            foreach (string scenePath in ExpectedScenePaths)
            {
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                Assert.That(
                    sceneAsset,
                    Is.Not.Null,
                    $"Scene asset not found at approved path: {scenePath}");
            }
        }

        [Test]
        public void GlobalSceneList_ContainsOnlyApprovedScenesInApprovedOrder()
        {
            EditorBuildSettingsScene[] configuredScenes = EditorBuildSettings.scenes;

            Assert.That(
                configuredScenes.Length,
                Is.EqualTo(ExpectedScenePaths.Length),
                "The global Scene List contains an unexpected or stale entry.");

            Assert.That(
                configuredScenes.All(scene => scene.enabled),
                Is.True,
                "All approved scenes must be enabled.");

            CollectionAssert.AreEqual(
                ExpectedScenePaths,
                configuredScenes.Select(scene => scene.path).ToArray());
        }

        [Test]
        public void BuildIndexes_MatchApprovedOrder()
        {
            for (int index = 0; index < ExpectedScenePaths.Length; index++)
            {
                Assert.That(
                    SceneUtility.GetBuildIndexByScenePath(ExpectedScenePaths[index]),
                    Is.EqualTo(index),
                    $"Unexpected build index for {ExpectedScenePaths[index]}");
            }
        }

        [Test]
        public void TryEnter_WhenAlreadyEntered_RejectsConcurrentEntry()
        {
            SceneTransitionGate gate = new SceneTransitionGate();

            bool firstEntry = gate.TryEnter();
            bool concurrentEntry = gate.TryEnter();

            Assert.That(firstEntry, Is.True);
            Assert.That(concurrentEntry, Is.False);
            Assert.That(gate.IsEntered, Is.True);

            gate.Exit();

            Assert.That(gate.IsEntered, Is.False);
            Assert.That(gate.TryEnter(), Is.True);
        }
    }
}
