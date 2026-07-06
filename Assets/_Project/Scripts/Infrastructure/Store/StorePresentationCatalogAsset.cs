using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Characters;
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
        private ActorPrefabCatalogAsset _actorCatalog;

        [SerializeField]
        private FeedbackPresentationCatalogAsset _feedbackCatalog;

        [SerializeField]
        private GameObject _supplierDeliveryViewPrefab;

        [SerializeField]
        private ActorPrefabCatalogAsset.ActorEntry[] _actors =
            Array.Empty<ActorPrefabCatalogAsset.ActorEntry>();

        [SerializeField]
        private ActorPrefabCatalogAsset.AnimationEntry[] _animations =
            Array.Empty<ActorPrefabCatalogAsset.AnimationEntry>();

        [SerializeField]
        private FeedbackPresentationCatalogAsset.FeedbackEntry[] _feedback =
            Array.Empty<FeedbackPresentationCatalogAsset.FeedbackEntry>();

        public ActorPrefabCatalogAsset ActorCatalog => _actorCatalog;
        public FeedbackPresentationCatalogAsset FeedbackCatalog => _feedbackCatalog;
        public GameObject SupplierDeliveryViewPrefab => _supplierDeliveryViewPrefab;

        public ActorPrefabCatalogAsset.ActorEntry[] Actors =>
            _actorCatalog != null ? _actorCatalog.Actors : _actors;

        public ActorPrefabCatalogAsset.AnimationEntry[] Animations =>
            _actorCatalog != null ? _actorCatalog.Animations : _animations;

        public GameObject FindActorPrefab(
            string actorId,
            CharacterRole expectedRole)
        {
            if (_actorCatalog != null)
            {
                return _actorCatalog.FindPrefab(actorId, expectedRole);
            }

            if (_actors == null || string.IsNullOrWhiteSpace(actorId))
            {
                return null;
            }

            foreach (ActorPrefabCatalogAsset.ActorEntry entry in _actors)
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

        public FeedbackPresentationCatalogAsset.FeedbackEntry FindFeedback(
            GameplayFeedbackType kind)
        {
            if (_feedbackCatalog != null)
            {
                return _feedbackCatalog.FindFeedback(kind);
            }

            if (_feedback == null)
            {
                return null;
            }

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
            ActorPrefabCatalogAsset actorCatalog,
            FeedbackPresentationCatalogAsset feedbackCatalog)
        {
            _actorCatalog = actorCatalog;
            _feedbackCatalog = feedbackCatalog;
        }

        public void Configure(
            ActorPrefabCatalogAsset.ActorEntry[] actors,
            ActorPrefabCatalogAsset.AnimationEntry[] animations,
            FeedbackPresentationCatalogAsset.FeedbackEntry[] feedback)
        {
            _actorCatalog = null;
            _feedbackCatalog = null;
            _actors = actors ?? Array.Empty<ActorPrefabCatalogAsset.ActorEntry>();
            _animations = animations ?? Array.Empty<ActorPrefabCatalogAsset.AnimationEntry>();
            _feedback = feedback ?? Array.Empty<FeedbackPresentationCatalogAsset.FeedbackEntry>();
        }
    }
}
