using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Runtime/Audio Catalog",
        fileName =
            "AudioCatalog")]
    public sealed class Phase1AudioCatalogAsset :
        ScriptableObject
    {
        private const string ResourceRoot =
            "Audio/";

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
                        "MusicStore",
                        0.45f,
                        true),
                    EntryFor(
                        "ambience.store",
                        Phase1AudioChannel.Ambience,
                        "AmbienceStore",
                        0.45f,
                        true),
                    Feedback(
                        Phase1FeedbackKind.PlacementValid,
                        "PlacementValid"),
                    Feedback(
                        Phase1FeedbackKind.PlacementInvalid,
                        "PlacementInvalid"),
                    Feedback(
                        Phase1FeedbackKind.ObjectSelected,
                        "UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.ObjectHovered,
                        "UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.ProductAssigned,
                        "UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.OutOfStock,
                        "UiError"),
                    Feedback(
                        Phase1FeedbackKind.Reserved,
                        "UiConfirm"),
                    Feedback(
                        Phase1FeedbackKind.Restocked,
                        "OrderReceived"),
                    Feedback(
                        Phase1FeedbackKind.OrderReceived,
                        "OrderReceived"),
                    Feedback(
                        Phase1FeedbackKind.CustomerSatisfied,
                        "Checkout"),
                    Feedback(
                        Phase1FeedbackKind.CustomerFrustrated,
                        "UiError"),
                    Feedback(
                        Phase1FeedbackKind.QueueEntered,
                        "Checkout"),
                    Feedback(
                        Phase1FeedbackKind.CheckoutCompleted,
                        "Checkout"),
                    Feedback(
                        Phase1FeedbackKind.Revenue,
                        "Checkout"),
                    Feedback(
                        Phase1FeedbackKind.Expense,
                        "UiConfirm"),
                    Feedback(
                        Phase1FeedbackKind.ClosingWarning,
                        "DayClosed"),
                    Feedback(
                        Phase1FeedbackKind.DayClosed,
                        "DayClosed"),
                    Feedback(
                        Phase1FeedbackKind.AutosaveSucceeded,
                        "UiConfirm",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.AutosaveFailed,
                        "UiError",
                        Phase1AudioChannel.Ui),
                    Feedback(
                        Phase1FeedbackKind.DoorOpened,
                        "Door"),
                    Feedback(
                        Phase1FeedbackKind.DoorClosed,
                        "Door")
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
