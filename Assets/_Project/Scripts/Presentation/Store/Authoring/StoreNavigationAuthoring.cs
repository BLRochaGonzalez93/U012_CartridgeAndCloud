using System;
using System.Text;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Serialized navigation contract for an authored store environment.
    /// The runtime NavMesh surface may only collect geometry below NavigationRoot.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StoreNavigationAuthoring : MonoBehaviour
    {
        [SerializeField]
        private Transform _navigationRoot;

        [SerializeField]
        private BoxCollider[] _walkableSources = Array.Empty<BoxCollider>();

        [SerializeField]
        private BoxCollider[] _blockingSources = Array.Empty<BoxCollider>();

        public Transform NavigationRoot => _navigationRoot;
        public BoxCollider[] WalkableSources => _walkableSources;
        public BoxCollider[] BlockingSources => _blockingSources;

        public bool TryValidate(out string report)
        {
            StringBuilder errors = new StringBuilder();

            if (_navigationRoot == null)
            {
                errors.AppendLine("- Missing NavigationRoot.");
            }
            else if (!_navigationRoot.IsChildOf(transform))
            {
                errors.AppendLine("- NavigationRoot must belong to this environment prefab.");
            }

            ValidateSources(
                _walkableSources,
                3,
                "WalkableSources",
                "Interior, exterior and entrance walkable sources are required.",
                errors);
            ValidateSources(
                _blockingSources,
                7,
                "BlockingSources",
                "Perimeter, facade and backroom navigation blockers are required.",
                errors);

            report = errors.Length == 0
                ? "Store navigation contract is valid."
                : errors.ToString().TrimEnd();
            return errors.Length == 0;
        }

        public void ValidateOrThrow()
        {
            if (!TryValidate(out string report))
            {
                throw new InvalidOperationException(
                    "Store navigation contract is invalid:\n" + report);
            }
        }

        private void ValidateSources(
            BoxCollider[] sources,
            int minimumCount,
            string collectionName,
            string missingMessage,
            StringBuilder errors)
        {
            if (sources == null || sources.Length < minimumCount)
            {
                errors.AppendLine("- " + missingMessage);
                return;
            }

            for (int index = 0; index < sources.Length; index++)
            {
                BoxCollider source = sources[index];
                if (source == null)
                {
                    errors.AppendLine($"- {collectionName}[{index}] is missing.");
                    continue;
                }

                if (source.isTrigger)
                {
                    errors.AppendLine($"- {source.name} cannot be a trigger.");
                }

                if (_navigationRoot != null &&
                    source.transform != _navigationRoot &&
                    !source.transform.IsChildOf(_navigationRoot))
                {
                    errors.AppendLine($"- {source.name} is outside NavigationRoot.");
                }
            }
        }
    }
}
