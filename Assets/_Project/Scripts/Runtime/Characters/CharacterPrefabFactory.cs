using System;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    public static class CharacterPrefabFactory
    {
        public static GameObject Instantiate(
            StorePresentationCatalogAsset catalog,
            string actorId,
            Transform parent,
            string instanceName,
            CharacterRole role,
            Vector3 worldPosition)
        {
            return Instantiate(
                catalog,
                actorId,
                parent,
                instanceName,
                instanceName,
                role,
                worldPosition);
        }

        public static GameObject Instantiate(
            StorePresentationCatalogAsset catalog,
            string actorId,
            Transform parent,
            string instanceName,
            string characterId,
            CharacterRole role,
            Vector3 worldPosition)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(nameof(catalog));
            }

            if (string.IsNullOrWhiteSpace(instanceName))
            {
                throw new ArgumentException(
                    "Character instance name is required.",
                    nameof(instanceName));
            }

            if (string.IsNullOrWhiteSpace(characterId))
            {
                throw new ArgumentException(
                    "Character identity is required.",
                    nameof(characterId));
            }

            GameObject prefab = catalog.FindActorPrefab(actorId, role);
            if (prefab == null)
            {
                throw new InvalidOperationException(
                    $"Actor prefab '{actorId}' for role '{role}' is not configured.");
            }

            GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
            instance.name = instanceName;
            instance.transform.SetPositionAndRotation(
                worldPosition,
                Quaternion.identity);
            instance.transform.localScale = Vector3.one;

            CharacterPresence presence = instance.GetComponent<CharacterPresence>();
            if (presence == null)
            {
                throw new InvalidOperationException(
                    $"Actor prefab '{prefab.name}' is missing CharacterPresence.");
            }
            presence.Configure(characterId, role);

            CharacterLocomotionAnimator locomotion =
                instance.GetComponent<CharacterLocomotionAnimator>();
            if (locomotion == null)
            {
                throw new InvalidOperationException(
                    $"Actor prefab '{prefab.name}' is missing CharacterLocomotionAnimator.");
            }

            ActorPrefabAuthoring authoring =
                instance.GetComponent<ActorPrefabAuthoring>();
            if (authoring == null)
            {
                throw new InvalidOperationException(
                    $"Actor prefab '{prefab.name}' is missing ActorPrefabAuthoring.");
            }

            if (authoring.Role != role)
            {
                throw new InvalidOperationException(
                    $"Actor prefab '{prefab.name}' has role '{authoring.Role}' " +
                    $"but '{role}' was requested.");
            }

            NavMeshAgent agent = authoring.ConfigureAgent();
            agent.avoidancePriority = ResolveAvoidancePriority(role, instanceName);

            if (NavMesh.SamplePosition(
                    worldPosition,
                    out NavMeshHit hit,
                    4f,
                    agent.areaMask))
            {
                agent.Warp(hit.position);
            }

            GroundingUtility.AlignVisualRootToGround(instance.transform);
            locomotion.Configure(instance.GetComponentInChildren<Animator>(true));
            return instance;
        }

        private static int ResolveAvoidancePriority(
            CharacterRole role,
            string instanceName)
        {
            int basePriority = role switch
            {
                CharacterRole.Employee => 10,
                CharacterRole.Supplier => 25,
                CharacterRole.Customer => 50,
                CharacterRole.Player => 5,
                _ => 40
            };

            int spread = role == CharacterRole.Customer ? 20 : 10;
            return Mathf.Clamp(
                basePriority + StableHash(instanceName) % spread,
                0,
                99);
        }

        private static int StableHash(string value)
        {
            unchecked
            {
                int hash = 17;
                if (!string.IsNullOrEmpty(value))
                {
                    foreach (char character in value)
                    {
                        hash = hash * 31 + character;
                    }
                }

                return hash & int.MaxValue;
            }
        }
    }
}
