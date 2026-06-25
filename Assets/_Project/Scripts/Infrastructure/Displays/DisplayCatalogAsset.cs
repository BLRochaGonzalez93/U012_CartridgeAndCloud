using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Displays;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Displays
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Displays/Display Catalog",
        fileName = "CC_DisplayCatalog_")]
    public sealed class DisplayCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private List<DisplayDefinitionAsset> _definitions =
            new List<DisplayDefinitionAsset>();

        public IReadOnlyList<DisplayDefinitionAsset> Definitions =>
            _definitions;

        public DisplayDefinitionRegistry BuildRegistry()
        {
            List<DisplayDefinition> definitions =
                new List<DisplayDefinition>(_definitions.Count);

            foreach (DisplayDefinitionAsset definition in _definitions)
            {
                if (definition == null)
                {
                    throw new InvalidOperationException(
                        "Display catalog contains a missing asset reference.");
                }

                definitions.Add(definition.BuildDefinition());
            }

            return new DisplayDefinitionRegistry(definitions);
        }

        public void Configure(
            IEnumerable<DisplayDefinitionAsset> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            _definitions =
                new List<DisplayDefinitionAsset>(definitions);
        }
    }
}
