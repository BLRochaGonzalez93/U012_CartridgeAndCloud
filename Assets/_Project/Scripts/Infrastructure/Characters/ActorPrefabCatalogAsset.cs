using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Characters;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Characters
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Characters/Actor Prefab Catalog",
        fileName = "ActorPrefabCatalog")]
    public sealed class ActorPrefabCatalogAsset : ScriptableObject
    {
        [Serializable]
        public sealed class ActorEntry
        {
            public string id;
            public CharacterRole role;
            public GameObject prefab;
        }

        [Serializable]
        public sealed class AnimationEntry
        {
            public string stateId;
            public AnimationClip placeholderClip;
            public bool loop;
        }

        [SerializeField]
        private ActorEntry[] _actors = Array.Empty<ActorEntry>();

        [SerializeField]
        private AnimationEntry[] _animations = Array.Empty<AnimationEntry>();

        public ActorEntry[] Actors => _actors ?? Array.Empty<ActorEntry>();
        public AnimationEntry[] Animations =>
            _animations ?? Array.Empty<AnimationEntry>();

        public GameObject FindPrefab(string actorId, CharacterRole expectedRole)
        {
            if (string.IsNullOrWhiteSpace(actorId))
            {
                return null;
            }

            foreach (ActorEntry entry in Actors)
            {
                if (entry != null &&
                    entry.prefab != null &&
                    entry.role == expectedRole &&
                    string.Equals(entry.id, actorId, StringComparison.Ordinal))
                {
                    return entry.prefab;
                }
            }

            return null;
        }

        public void Configure(ActorEntry[] actors, AnimationEntry[] animations)
        {
            _actors = actors ?? Array.Empty<ActorEntry>();
            _animations = animations ?? Array.Empty<AnimationEntry>();
        }
    }
}
