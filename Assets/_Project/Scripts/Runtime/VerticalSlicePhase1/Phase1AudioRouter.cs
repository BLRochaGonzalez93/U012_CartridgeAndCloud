using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1AudioRouter :
        MonoBehaviour,
        IPhase1AudioRouter
    {
        private readonly Dictionary<
            Phase1AudioChannel,
            AudioSource> _sources =
                new Dictionary<
                    Phase1AudioChannel,
                    AudioSource>();

        private Phase1AudioCatalogAsset
            _catalog;

        public void Configure(
            Phase1AudioCatalogAsset catalog)
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

            Phase1AudioCatalogAsset.Entry entry =
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
            Phase1AudioChannel channel,
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
            Phase1AudioChannel channel)
        {
            return PlayerPrefs.GetFloat(
                Key(channel),
                0.8f);
        }

        private void EnsureSources()
        {
            foreach (Phase1AudioChannel channel
                     in Enum.GetValues(
                         typeof(Phase1AudioChannel)))
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
            Phase1AudioChannel channel)
        {
            return "CC_S16_P1_Audio_" +
                channel;
        }
    }
}
