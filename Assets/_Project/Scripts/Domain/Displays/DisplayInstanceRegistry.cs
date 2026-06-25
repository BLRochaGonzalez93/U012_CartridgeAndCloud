using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayInstanceRegistry
    {
        private readonly Dictionary<DisplayInstanceId, DisplayInstance>
            _instancesById;

        private readonly ReadOnlyCollection<DisplayInstance> _instances;

        public IReadOnlyList<DisplayInstance> Instances => _instances;

        public int Count => _instances.Count;

        public DisplayInstanceRegistry(
            IEnumerable<DisplayInstance> instances)
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            _instancesById =
                new Dictionary<DisplayInstanceId, DisplayInstance>();

            List<DisplayInstance> ordered =
                new List<DisplayInstance>();

            foreach (DisplayInstance instance in instances)
            {
                if (instance == null)
                {
                    throw new ArgumentException(
                        "Display instances cannot contain null.",
                        nameof(instances));
                }

                if (_instancesById.ContainsKey(instance.Id))
                {
                    throw new ArgumentException(
                        $"Display instance ID {instance.Id} is duplicated.",
                        nameof(instances));
                }

                _instancesById.Add(instance.Id, instance);
                ordered.Add(instance);
            }

            ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));

            _instances =
                new ReadOnlyCollection<DisplayInstance>(ordered);
        }

        public bool Contains(DisplayInstanceId id)
        {
            return _instancesById.ContainsKey(id);
        }

        public bool TryGet(
            DisplayInstanceId id,
            out DisplayInstance instance)
        {
            return _instancesById.TryGetValue(id, out instance);
        }

        public DisplayInstance Get(DisplayInstanceId id)
        {
            if (!_instancesById.TryGetValue(
                    id,
                    out DisplayInstance instance))
            {
                throw new KeyNotFoundException(
                    $"Display instance {id} was not found.");
            }

            return instance;
        }
    }
}
