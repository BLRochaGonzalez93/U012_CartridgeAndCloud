using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Characters;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Characters
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Characters/Character Presentation Catalog",
        fileName = "CharacterPresentationCatalog")]
    public sealed class CharacterPresentationCatalogAsset : ScriptableObject
    {
        private const string AnimationRoot = "Animations/";

        [Serializable]
        public sealed class CharacterEntry
        {
            public string id;
            public CharacterRole role;
            public string prefabResourcePath;
            public string materialVariantId;
            public float moveSpeed = 2f;
        }

        [Serializable]
        public sealed class AnimationEntry
        {
            public string stateId;
            public AnimationClip placeholderClip;
            public bool loop;
        }

        [SerializeField]
        private CharacterEntry[] _characters =
            new CharacterEntry[0];

        [SerializeField]
        private AnimationEntry[] _animations =
            new AnimationEntry[0];

        public CharacterEntry[] Characters
        {
            get
            {
                EnsureDefaults();
                return _characters;
            }
        }

        public AnimationEntry[] Animations
        {
            get
            {
                EnsureDefaults();
                return _animations;
            }
        }

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public void Configure(
            CharacterEntry[] characters,
            AnimationEntry[] animations)
        {
            _characters = characters ?? new CharacterEntry[0];
            _animations = animations ?? new AnimationEntry[0];
        }

        private void EnsureDefaults()
        {
            if (_characters == null || _characters.Length == 0)
            {
                _characters = new[]
                {
                    Character("employee-main", CharacterRole.Employee, "Employee", "character-employee", 2f),
                    Character("customer-base", CharacterRole.Customer, "Customer", "character-customer", 2f),
                    Character("supplier-base", CharacterRole.Supplier, "Supplier", "character-supplier", 1.8f)
                };
            }

            if (_animations == null || _animations.Length == 0)
            {
                _animations = new[]
                {
                    Animation("idle", "Idle", true),
                    Animation("walk", "Walk", true),
                    Animation("observe", "Observe", false),
                    Animation("pick-product", "PickProduct", false),
                    Animation("queue-wait", "QueueWait", true),
                    Animation("checkout", "Checkout", false),
                    Animation("satisfied", "Satisfied", false),
                    Animation("frustrated", "Frustrated", false),
                    Animation("move-crate", "MoveCrate", true),
                    Animation("player-place", "PlayerPlace", false),
                    Animation("player-remove", "PlayerRemove", false)
                };
            }
        }

        private static CharacterEntry Character(
            string id,
            CharacterRole role,
            string prefabName,
            string materialVariantId,
            float moveSpeed)
        {
            return new CharacterEntry
            {
                id = id,
                role = role,
                prefabResourcePath =
                    "Characters/" +
                    prefabName,
                materialVariantId =
                    materialVariantId,
                moveSpeed = moveSpeed
            };
        }

        private static AnimationEntry Animation(
            string stateId,
            string resourceName,
            bool loop)
        {
            return new AnimationEntry
            {
                stateId = stateId,
                placeholderClip =
                    Resources.Load<AnimationClip>(
                        AnimationRoot +
                        resourceName),
                loop = loop
            };
        }
    }
}
