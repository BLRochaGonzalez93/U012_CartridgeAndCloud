using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Audio Catalog",
        fileName =
            "CC_S16_P1_AudioCatalog")]
    public sealed class Phase1AudioCatalogAsset :
        ScriptableObject
    {
        private const string ResourceRoot =
            "Sprint16Phase1/Audio/";

        [Serializable]
        public sealed class Entry
        {
            public string eventId;
            public Phase1AudioChannel channel;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
            [Range(0.5f, 1.5f)] public float pitch = 1f;
            public bool loop;
        }

        [SerializeField]
        private Entry[] _entries =
            new Entry[0];

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public Entry Find(string eventId)
        {
            EnsureDefaults();

            foreach (Entry entry in _entries)
            {
                if (entry != null &&
                    string.Equals(
                        entry.eventId,
                        eventId,
                        StringComparison.Ordinal))
                {
                    return entry;
                }
            }

            return null;
        }

        private void EnsureDefaults()
        {
            if (_entries != null &&
                _entries.Length > 0)
            {
                return;
            }

            _entries =
                new[]
                {
                    EntryFor(
                        "music.store",
                        Phase1AudioChannel.Music,
                        "CC_S16_P1_MusicStore",
                        0.45f,
                        true),
                    EntryFor(
                        "ambience.store",
                        Phase1AudioChannel.Ambience,
                        "CC_S16_P1_AmbienceStore",
                        0.45f,
                        true),
                    Feedback(
                        Phase1FeedbackKind.PlacementValid,
                        "CC_S16_P1_PlacementValid"),
                    Feedback(
                        Phase1FeedbackKind.PlacementInvalid,
                        "CC_S16_P1_PlacementInvalid"),
                    Feedback(
                        Phase1FeedbackKind.ObjectSelected,
                        "CC_S16_P1_UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.ObjectHovered,
                        "CC_S16_P1_UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.ProductAssigned,
                        "CC_S16_P1_UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.OutOfStock,
                        "CC_S16_P1_UiError"),
                    Feedback(
                        Phase1FeedbackKind.Reserved,
                        "CC_S16_P1_UiConfirm"),
                    Feedback(
                        Phase1FeedbackKind.Restocked,
                        "CC_S16_P1_OrderReceived"),
                    Feedback(
                        Phase1FeedbackKind.OrderReceived,
                        "CC_S16_P1_OrderReceived"),
                    Feedback(
                        Phase1FeedbackKind.CustomerSatisfied,
                        "CC_S16_P1_Checkout"),
                    Feedback(
                        Phase1FeedbackKind.CustomerFrustrated,
                        "CC_S16_P1_UiError"),
                    Feedback(
                        Phase1FeedbackKind.QueueEntered,
                        "CC_S16_P1_Checkout"),
                    Feedback(
                        Phase1FeedbackKind.CheckoutCompleted,
                        "CC_S16_P1_Checkout"),
                    Feedback(
                        Phase1FeedbackKind.Revenue,
                        "CC_S16_P1_Checkout"),
                    Feedback(
                        Phase1FeedbackKind.Expense,
                        "CC_S16_P1_UiConfirm"),
                    Feedback(
                        Phase1FeedbackKind.ClosingWarning,
                        "CC_S16_P1_DayClosed"),
                    Feedback(
                        Phase1FeedbackKind.DayClosed,
                        "CC_S16_P1_DayClosed"),
                    Feedback(
                        Phase1FeedbackKind.AutosaveSucceeded,
                        "CC_S16_P1_UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.AutosaveFailed,
                        "CC_S16_P1_UiError",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.DoorOpened,
                        "CC_S16_P1_Door"),
                    Feedback(
                        Phase1FeedbackKind.DoorClosed,
                        "CC_S16_P1_Door")
                };
        }

        private static Entry Feedback(
            Phase1FeedbackKind kind,
            string resourceName,
            Phase1AudioChannel channel =
                Phase1AudioChannel.Effects)
        {
            return EntryFor(
                "feedback." +
                kind.ToString()
                    .ToLowerInvariant(),
                channel,
                resourceName,
                0.75f,
                false);
        }

        private static Entry EntryFor(
            string eventId,
            Phase1AudioChannel channel,
            string resourceName,
            float volume,
            bool loop)
        {
            return new Entry
            {
                eventId = eventId,
                channel = channel,
                clip =
                    Resources.Load<AudioClip>(
                        ResourceRoot +
                        resourceName),
                volume = volume,
                pitch = 1f,
                loop = loop
            };
        }
    }
}
