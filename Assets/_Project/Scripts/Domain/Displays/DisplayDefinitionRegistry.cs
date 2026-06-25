using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayDefinitionRegistry
    {
        private readonly Dictionary<DisplayDefinitionId, DisplayDefinition>
            _definitionsById;

        private readonly ReadOnlyCollection<DisplayDefinition> _definitions;

        public IReadOnlyList<DisplayDefinition> Definitions => _definitions;

        public int Count => _definitions.Count;

        public DisplayDefinitionRegistry(
            IEnumerable<DisplayDefinition> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            _definitionsById =
                new Dictionary<DisplayDefinitionId, DisplayDefinition>();

            List<DisplayDefinition> ordered =
                new List<DisplayDefinition>();

            foreach (DisplayDefinition definition in definitions)
            {
                if (definition == null)
                {
                    throw new ArgumentException(
                        "Display definitions cannot contain null.",
                        nameof(definitions));
                }

                if (_definitionsById.ContainsKey(definition.Id))
                {
                    throw new ArgumentException(
                        $"Display definition ID {definition.Id} is duplicated.",
                        nameof(definitions));
                }

                _definitionsById.Add(definition.Id, definition);
                ordered.Add(definition);
            }

            ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));

            _definitions =
                new ReadOnlyCollection<DisplayDefinition>(ordered);
        }

        public bool Contains(DisplayDefinitionId id)
        {
            return _definitionsById.ContainsKey(id);
        }

        public bool TryGet(
            DisplayDefinitionId id,
            out DisplayDefinition definition)
        {
            return _definitionsById.TryGetValue(id, out definition);
        }

        public DisplayDefinition Get(DisplayDefinitionId id)
        {
            if (!_definitionsById.TryGetValue(
                    id,
                    out DisplayDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Display definition {id} was not found.");
            }

            return definition;
        }
    }
}
