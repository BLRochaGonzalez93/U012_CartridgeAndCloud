using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Installs the authored player wrapper from the presentation catalog and
    /// aligns its authored foot plane with the bottom of the technical capsule.
    /// </summary>
    public static class PlayerCharacterVisualBootstrap
    {
        private const string ActorId = "player-clerk-male-01";
        private const string VisualInstanceName = "AuthoredPlayerVisual";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallForInitialScene() => Install();

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Install();

        private static void Install()
        {
            ClickToMoveAgent player = Object.FindFirstObjectByType<ClickToMoveAgent>(
                FindObjectsInactive.Include);
            if (player == null || player.transform.Find(VisualInstanceName) != null)
            {
                return;
            }

            StoreRuntimeAssetRegistry registry = StoreRuntimeAssetRegistry.FindLoaded();
            GameObject prefab = registry?.PresentationCatalog?.FindActorPrefab(
                ActorId,
                CharacterRole.Player);
            if (prefab == null)
            {
                Debug.LogError(
                    "[Characters] Authored player prefab is missing from PresentationCatalog.");
                return;
            }

            GameObject visual = Object.Instantiate(prefab, player.transform, false);
            visual.name = VisualInstanceName;
            visual.transform.localRotation = Quaternion.identity;
            visual.transform.localScale = Vector3.one;
            visual.transform.position = ResolveCapsuleBottom(player.gameObject);

            GroundingUtility.AlignVisualRootToGround(visual.transform);

            CharacterLocomotionAnimator locomotion =
                visual.GetComponent<CharacterLocomotionAnimator>();
            if (locomotion != null)
            {
                locomotion.Configure(visual.GetComponentInChildren<Animator>(true));
            }

            DisablePhysicalComponents(visual);
            DisableTechnicalRenderers(player.gameObject, visual.transform);
        }

        private static Vector3 ResolveCapsuleBottom(GameObject playerRoot)
        {
            CharacterController controller =
                playerRoot.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 localBottom = controller.center +
                    Vector3.down * (controller.height * 0.5f);
                return playerRoot.transform.TransformPoint(localBottom);
            }

            CapsuleCollider capsule = playerRoot.GetComponent<CapsuleCollider>();
            if (capsule != null)
            {
                Vector3 localBottom = capsule.center +
                    Vector3.down * (capsule.height * 0.5f);
                return playerRoot.transform.TransformPoint(localBottom);
            }

            return playerRoot.transform.position;
        }

        private static void DisablePhysicalComponents(GameObject visual)
        {
            foreach (Collider collider in visual.GetComponentsInChildren<Collider>(true))
            {
                collider.enabled = false;
            }

            foreach (NavMeshAgent agent in visual.GetComponentsInChildren<NavMeshAgent>(true))
            {
                agent.enabled = false;
            }

            ActorPrefabAuthoring authoring = visual.GetComponent<ActorPrefabAuthoring>();
            if (authoring != null)
            {
                authoring.enabled = false;
            }
        }

        private static void DisableTechnicalRenderers(
            GameObject playerRoot,
            Transform preservedVisual)
        {
            foreach (Renderer renderer in playerRoot.GetComponentsInChildren<Renderer>(true))
            {
                bool belongsToVisual = renderer.transform == preservedVisual ||
                    renderer.transform.IsChildOf(preservedVisual);
                renderer.enabled = belongsToVisual;
            }
        }
    }
}
