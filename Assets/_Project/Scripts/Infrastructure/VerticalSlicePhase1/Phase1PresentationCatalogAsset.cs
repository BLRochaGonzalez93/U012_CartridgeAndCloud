using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Presentation Catalog",
        fileName =
            "CC_S16_P1_PresentationCatalog")]
    public sealed class Phase1PresentationCatalogAsset :
        ScriptableObject
    {
        private const string AnimationRoot =
            "Sprint16Phase1/Animations/";

        [Serializable]
        public sealed class CharacterEntry
        {
            public string id;
            public Phase1CharacterRole role;
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

        [Serializable]
        public sealed class FeedbackEntry
        {
            public Phase1FeedbackKind kind;
            public string title;
            public string placeholderIconPath;
            public string materialVariantId;
            public bool showParticles = true;
            public bool showFloatingText = true;
            public float scalePulse = 1.08f;
        }

        [SerializeField]
        private CharacterEntry[] _characters =
            new CharacterEntry[0];

        [SerializeField]
        private AnimationEntry[] _animations =
            new AnimationEntry[0];

        [SerializeField]
        private FeedbackEntry[] _feedback =
            new FeedbackEntry[0];

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

        public FeedbackEntry FindFeedback(
            Phase1FeedbackKind kind)
        {
            EnsureDefaults();

            foreach (FeedbackEntry entry
                     in _feedback)
            {
                if (entry != null &&
                    entry.kind == kind)
                {
                    return entry;
                }
            }

            return null;
        }

        private void EnsureDefaults()
        {
            if (_characters == null ||
                _characters.Length == 0)
            {
                _characters =
                    new[]
                    {
                        Character(
                            "employee-main",
                            Phase1CharacterRole.Employee,
                            "Employee",
                            "character-employee",
                            2f),
                        Character(
                            "customer-base",
                            Phase1CharacterRole.Customer,
                            "Customer",
                            "character-customer",
                            2f),
                        Character(
                            "supplier-base",
                            Phase1CharacterRole.Supplier,
                            "Supplier",
                            "character-supplier",
                            1.8f)
                    };
            }

            if (_animations == null ||
                _animations.Length == 0)
            {
                _animations =
                    new[]
                    {
                        Animation("idle", "CC_S16_P1_Anim_Idle", true),
                        Animation("walk", "CC_S16_P1_Anim_Walk", true),
                        Animation("observe", "CC_S16_P1_Anim_Observe", false),
                        Animation("pick-product", "CC_S16_P1_Anim_PickProduct", false),
                        Animation("queue-wait", "CC_S16_P1_Anim_QueueWait", true),
                        Animation("checkout", "CC_S16_P1_Anim_Checkout", false),
                        Animation("satisfied", "CC_S16_P1_Anim_Satisfied", false),
                        Animation("frustrated", "CC_S16_P1_Anim_Frustrated", false),
                        Animation("move-crate", "CC_S16_P1_Anim_MoveCrate", true),
                        Animation("player-place", "CC_S16_P1_Anim_PlayerPlace", false),
                        Animation("player-remove", "CC_S16_P1_Anim_PlayerRemove", false)
                    };
            }

            if (_feedback == null ||
                _feedback.Length == 0)
            {
                _feedback =
                    CreateFeedbackEntries();
            }
        }

        private static CharacterEntry Character(
            string id,
            Phase1CharacterRole role,
            string prefabName,
            string materialVariantId,
            float moveSpeed)
        {
            return new CharacterEntry
            {
                id = id,
                role = role,
                prefabResourcePath =
                    "Sprint16Phase1/Prefabs/Characters/" +
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

        private static FeedbackEntry[]
            CreateFeedbackEntries()
        {
            Phase1FeedbackKind[] kinds =
                (Phase1FeedbackKind[])
                    Enum.GetValues(
                        typeof(Phase1FeedbackKind));

            FeedbackEntry[] entries =
                new FeedbackEntry[kinds.Length];

            for (int index = 0;
                 index < kinds.Length;
                 index++)
            {
                Phase1FeedbackKind kind =
                    kinds[index];

                entries[index] =
                    new FeedbackEntry
                    {
                        kind = kind,
                        title = kind.ToString(),
                        placeholderIconPath =
                            IconPath(index),
                        materialVariantId =
                            MaterialId(kind),
                        showParticles = true,
                        showFloatingText = true,
                        scalePulse =
                            StrongPulse(kind)
                                ? 1.12f
                                : 1.06f
                    };
            }

            return entries;
        }

        private static string IconPath(int index)
        {
            string[] ids =
            {
                "game-neon-drift",
                "case-cloud-runner",
                "console-vertex-one",
                "controller-orbit-pad",
                "headset-signal-pro",
                "accessory-memory-core"
            };

            string id =
                ids[index % ids.Length];

            return "Sprint16Phase1/Icons/" +
                id +
                "_icon";
        }

        private static string MaterialId(
            Phase1FeedbackKind kind)
        {
            switch (kind)
            {
                case Phase1FeedbackKind.PlacementInvalid:
                case Phase1FeedbackKind.OutOfStock:
                case Phase1FeedbackKind.CustomerFrustrated:
                case Phase1FeedbackKind.AutosaveFailed:
                    return "feedback-invalid";

                case Phase1FeedbackKind.Expense:
                case Phase1FeedbackKind.ClosingWarning:
                    return "feedback-warning";

                default:
                    return "feedback-valid";
            }
        }

        private static bool StrongPulse(
            Phase1FeedbackKind kind)
        {
            switch (kind)
            {
                case Phase1FeedbackKind.OrderReceived:
                case Phase1FeedbackKind.CustomerSatisfied:
                case Phase1FeedbackKind.CheckoutCompleted:
                case Phase1FeedbackKind.AutosaveSucceeded:
                    return true;

                default:
                    return false;
            }
        }
    }
}
