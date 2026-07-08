using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/UI/Feedback Presentation Catalog",
        fileName = "FeedbackPresentationCatalog")]
    public sealed class FeedbackPresentationCatalogAsset : ScriptableObject
    {
        [Serializable]
        public sealed class FeedbackEntry
        {
            public GameplayFeedbackType kind;
            public string title;
            public string placeholderIconPath;
            public string materialVariantId;
            public bool showParticles = true;
            public bool showFloatingText = true;
            public float scalePulse = 1.08f;
        }

        [SerializeField]
        private FeedbackEntry[] _entries =
            new FeedbackEntry[0];

        public FeedbackEntry[] Entries
        {
            get
            {
                EnsureDefaults();
                return _entries;
            }
        }

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public FeedbackEntry FindFeedback(GameplayFeedbackType kind)
        {
            EnsureDefaults();

            foreach (FeedbackEntry entry in _entries)
            {
                if (entry != null && entry.kind == kind)
                {
                    return entry;
                }
            }

            return null;
        }

        public void Configure(FeedbackEntry[] entries)
        {
            _entries = entries ?? new FeedbackEntry[0];
        }

        private void EnsureDefaults()
        {
            if (_entries == null || _entries.Length == 0)
            {
                _entries = CreateFeedbackEntries();
            }
        }

        private static FeedbackEntry[]
            CreateFeedbackEntries()
        {
            GameplayFeedbackType[] kinds =
                (GameplayFeedbackType[])
                    Enum.GetValues(
                        typeof(GameplayFeedbackType));

            FeedbackEntry[] entries =
                new FeedbackEntry[kinds.Length];

            for (int index = 0;
                 index < kinds.Length;
                 index++)
            {
                GameplayFeedbackType kind =
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

            return "Products/Icons/" +
                id +
                "_icon";
        }

        private static string MaterialId(
            GameplayFeedbackType kind)
        {
            switch (kind)
            {
                case GameplayFeedbackType.PlacementInvalid:
                case GameplayFeedbackType.OutOfStock:
                case GameplayFeedbackType.CustomerFrustrated:
                case GameplayFeedbackType.AutosaveFailed:
                    return "feedback-invalid";

                case GameplayFeedbackType.Expense:
                case GameplayFeedbackType.ClosingWarning:
                    return "feedback-warning";

                default:
                    return "feedback-valid";
            }
        }

        private static bool StrongPulse(
            GameplayFeedbackType kind)
        {
            switch (kind)
            {
                case GameplayFeedbackType.DeliveryRunStarted:
                case GameplayFeedbackType.OrderReceived:
                case GameplayFeedbackType.CustomerSatisfied:
                case GameplayFeedbackType.CheckoutCompleted:
                case GameplayFeedbackType.AutosaveSucceeded:
                    return true;

                default:
                    return false;
            }
        }
    }
}
