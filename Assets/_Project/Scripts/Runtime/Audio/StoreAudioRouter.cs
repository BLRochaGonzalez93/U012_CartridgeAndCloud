using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Audio;
using VRMGames.CartridgeAndCloud.Infrastructure.Audio;
namespace VRMGames.CartridgeAndCloud.Runtime.Audio
{
    public sealed class StoreAudioRouter :
        MonoBehaviour,
        IAudioRouter
    {
        private readonly Dictionary<
            AudioChannel,
            AudioSource> _sources =
                new Dictionary<
                    AudioChannel,
                    AudioSource>();

        private AudioEventCatalogAsset
            _catalog;

        public void Configure(
            AudioEventCatalogAsset catalog)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));

            EnsureSources();
        }

        public void Play(string eventId)
        {
            if (_catalog == null ||
                string.IsNullOrWhiteSpace(eventId))
            {
                return;
            }

            AudioEventCatalogAsset.Entry entry =
                _catalog.Find(eventId);

            if (entry == null ||
                entry.clip == null)
            {
                return;
            }

            EnsureSources();

            AudioSource source =
                _sources[entry.channel];

            source.volume =
                entry.volume *
                GetChannelVolume(entry.channel);
            source.pitch = entry.pitch;

            if (entry.loop)
            {
                if (source.clip == entry.clip &&
                    source.isPlaying)
                {
                    return;
                }

                source.Stop();
                source.clip = entry.clip;
                source.loop = true;
                source.Play();
                return;
            }

            source.PlayOneShot(
                entry.clip,
                entry.volume *
                GetChannelVolume(entry.channel));
        }

        public void SetChannelVolume(
            AudioChannel channel,
            float normalizedVolume)
        {
            float value =
                Mathf.Clamp01(normalizedVolume);

            PlayerPrefs.SetFloat(
                Key(channel),
                value);
            PlayerPrefs.Save();

            if (_sources.TryGetValue(
                    channel,
                    out AudioSource source))
            {
                source.volume = value;
            }
        }

        public float GetChannelVolume(
            AudioChannel channel)
        {
            return PlayerPrefs.GetFloat(
                Key(channel),
                0.8f);
        }

        private void EnsureSources()
        {
            foreach (AudioChannel channel
                     in Enum.GetValues(
                         typeof(AudioChannel)))
            {
                if (_sources.ContainsKey(channel))
                {
                    continue;
                }

                GameObject child =
                    new GameObject(
                        "Audio_" + channel);
                child.transform.SetParent(
                    transform,
                    false);

                AudioSource source =
                    child.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                source.volume =
                    GetChannelVolume(channel);

                _sources.Add(
                    channel,
                    source);
            }
        }

        private static string Key(
            AudioChannel channel)
        {
            return "CC_S16_P1_Audio_" +
                channel;
        }
    }
}
