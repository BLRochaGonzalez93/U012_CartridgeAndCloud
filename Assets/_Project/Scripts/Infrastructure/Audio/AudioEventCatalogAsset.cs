using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Audio;
using VRMGames.CartridgeAndCloud.Application.UIUX;
namespace VRMGames.CartridgeAndCloud.Infrastructure.Audio
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Runtime/Audio Catalog",
        fileName =
            "AudioCatalog")]
    public sealed class AudioEventCatalogAsset :
        ScriptableObject
    {
        private const string ResourceRoot =
            "Audio/";

        [Serializable]
        public sealed class Entry
        {
            public string eventId;
            public AudioChannel channel;
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
                        AudioChannel.Music,
                        "MusicStore",
                        0.45f,
                        true),
                    EntryFor(
                        "ambience.store",
                        AudioChannel.Ambience,
                        "AmbienceStore",
                        0.45f,
                        true),
                    Feedback(
                        GameplayFeedbackType.PlacementValid,
                        "PlacementValid"),
                    Feedback(
                        GameplayFeedbackType.PlacementInvalid,
                        "PlacementInvalid"),
                    Feedback(
                        GameplayFeedbackType.ObjectSelected,
                        "UiConfirm",
                        AudioChannel.Ui),
                    Feedback(
                        GameplayFeedbackType.ObjectHovered,
                        "UiConfirm",
                        AudioChannel.Ui),
                    Feedback(
                        GameplayFeedbackType.ProductAssigned,
                        "UiConfirm",
                        AudioChannel.Ui),
                    Feedback(
                        GameplayFeedbackType.OutOfStock,
                        "UiError"),
                    Feedback(
                        GameplayFeedbackType.Reserved,
                        "UiConfirm"),
                    Feedback(
                        GameplayFeedbackType.Restocked,
                        "OrderReceived"),
                    Feedback(
                        GameplayFeedbackType.OrderReceived,
                        "OrderReceived"),
                    Feedback(
                        GameplayFeedbackType.CustomerSatisfied,
                        "Checkout"),
                    Feedback(
                        GameplayFeedbackType.CustomerFrustrated,
                        "UiError"),
                    Feedback(
                        GameplayFeedbackType.QueueEntered,
                        "Checkout"),
                    Feedback(
                        GameplayFeedbackType.CheckoutCompleted,
                        "Checkout"),
                    Feedback(
                        GameplayFeedbackType.Revenue,
                        "Checkout"),
                    Feedback(
                        GameplayFeedbackType.Expense,
                        "UiConfirm"),
                    Feedback(
                        GameplayFeedbackType.ClosingWarning,
                        "DayClosed"),
                    Feedback(
                        GameplayFeedbackType.DayClosed,
                        "DayClosed"),
                    Feedback(
                        GameplayFeedbackType.AutosaveSucceeded,
                        "UiConfirm",
                        AudioChannel.Ui),
                    Feedback(
                        GameplayFeedbackType.AutosaveFailed,
                        "UiError",
                        AudioChannel.Ui),
                    Feedback(
                        GameplayFeedbackType.DoorOpened,
                        "Door"),
                    Feedback(
                        GameplayFeedbackType.DoorClosed,
                        "Door")
                };
        }

        private static Entry Feedback(
            GameplayFeedbackType kind,
            string resourceName,
            AudioChannel channel =
                AudioChannel.Effects)
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
            AudioChannel channel,
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
