using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Store/Store Presentation Catalog",
        fileName = "PresentationCatalog")]
    public sealed class StorePresentationCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private CharacterPresentationCatalogAsset _characterCatalog;

        [SerializeField]
        private FeedbackPresentationCatalogAsset _feedbackCatalog;

        // Embedded legacy-compatible fields preserve PresentationCatalog.asset.
        [SerializeField]
        private CharacterPresentationCatalogAsset.CharacterEntry[] _characters =
            Array.Empty<CharacterPresentationCatalogAsset.CharacterEntry>();

        [SerializeField]
        private CharacterPresentationCatalogAsset.AnimationEntry[] _animations =
            Array.Empty<CharacterPresentationCatalogAsset.AnimationEntry>();

        [SerializeField]
        private FeedbackPresentationCatalogAsset.FeedbackEntry[] _feedback =
            Array.Empty<FeedbackPresentationCatalogAsset.FeedbackEntry>();

        public CharacterPresentationCatalogAsset CharacterCatalog =>
            _characterCatalog;

        public FeedbackPresentationCatalogAsset FeedbackCatalog =>
            _feedbackCatalog;

        public CharacterPresentationCatalogAsset.CharacterEntry[] Characters
        {
            get
            {
                if (_characterCatalog != null)
                {
                    return _characterCatalog.Characters;
                }

                EnsureEmbeddedDefaults();
                return _characters;
            }
        }

        public CharacterPresentationCatalogAsset.AnimationEntry[] Animations
        {
            get
            {
                if (_characterCatalog != null)
                {
                    return _characterCatalog.Animations;
                }

                EnsureEmbeddedDefaults();
                return _animations;
            }
        }

        private void OnEnable()
        {
            EnsureEmbeddedDefaults();
        }

        public FeedbackPresentationCatalogAsset.FeedbackEntry FindFeedback(
            GameplayFeedbackType kind)
        {
            if (_feedbackCatalog != null)
            {
                return _feedbackCatalog.FindFeedback(kind);
            }

            EnsureEmbeddedDefaults();
            foreach (FeedbackPresentationCatalogAsset.FeedbackEntry entry in _feedback)
            {
                if (entry != null && entry.kind == kind)
                {
                    return entry;
                }
            }

            return null;
        }

        public void Configure(
            CharacterPresentationCatalogAsset characterCatalog,
            FeedbackPresentationCatalogAsset feedbackCatalog)
        {
            _characterCatalog = characterCatalog;
            _feedbackCatalog = feedbackCatalog;
        }

        public void Configure(
            CharacterPresentationCatalogAsset.CharacterEntry[] characters,
            CharacterPresentationCatalogAsset.AnimationEntry[] animations,
            FeedbackPresentationCatalogAsset.FeedbackEntry[] feedback)
        {
            _characterCatalog = null;
            _feedbackCatalog = null;
            _characters = characters ??
                Array.Empty<CharacterPresentationCatalogAsset.CharacterEntry>();
            _animations = animations ??
                Array.Empty<CharacterPresentationCatalogAsset.AnimationEntry>();
            _feedback = feedback ??
                Array.Empty<FeedbackPresentationCatalogAsset.FeedbackEntry>();
            EnsureEmbeddedDefaults();
        }

        private void EnsureEmbeddedDefaults()
        {
            if (_characterCatalog == null &&
                ((_characters == null || _characters.Length == 0) ||
                 (_animations == null || _animations.Length == 0)))
            {
                CharacterPresentationCatalogAsset defaults =
                    CreateInstance<CharacterPresentationCatalogAsset>();
                if (_characters == null || _characters.Length == 0)
                {
                    _characters = defaults.Characters;
                }

                if (_animations == null || _animations.Length == 0)
                {
                    _animations = defaults.Animations;
                }

                DestroyImmediate(defaults);
            }

            if (_feedbackCatalog == null &&
                (_feedback == null || _feedback.Length == 0))
            {
                FeedbackPresentationCatalogAsset defaults =
                    CreateInstance<FeedbackPresentationCatalogAsset>();
                _feedback = defaults.Entries;
                DestroyImmediate(defaults);
            }
        }
    }
}
