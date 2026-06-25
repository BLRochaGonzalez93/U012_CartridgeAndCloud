using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerInstanceRegistry
    {
        private readonly Dictionary<CustomerInstanceId, CustomerInstance>
            _instancesById =
                new Dictionary<CustomerInstanceId, CustomerInstance>();
        private readonly List<CustomerInstance> _ordered =
            new List<CustomerInstance>();
        private ReadOnlyCollection<CustomerInstance> _readOnly;

        public IReadOnlyList<CustomerInstance> Instances =>
            _readOnly ??
            (_readOnly = new ReadOnlyCollection<CustomerInstance>(_ordered));

        public int Count => _ordered.Count;

        public int ActiveCount
        {
            get
            {
                int count = 0;
                foreach (CustomerInstance instance in _ordered)
                {
                    if (instance.IsActive)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Add(CustomerInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (_instancesById.ContainsKey(instance.Id))
            {
                return false;
            }

            _instancesById.Add(instance.Id, instance);
            _ordered.Add(instance);
            _ordered.Sort(
                (left, right) => StringComparer.Ordinal.Compare(
                    left.Id.Value,
                    right.Id.Value));
            _readOnly = null;
            return true;
        }

        public bool Remove(CustomerInstanceId id)
        {
            if (!_instancesById.TryGetValue(id, out CustomerInstance instance))
            {
                return false;
            }

            _instancesById.Remove(id);
            _ordered.Remove(instance);
            _readOnly = null;
            return true;
        }

        public bool Contains(CustomerInstanceId id)
        {
            return _instancesById.ContainsKey(id);
        }

        public bool TryGet(
            CustomerInstanceId id,
            out CustomerInstance instance)
        {
            return _instancesById.TryGetValue(id, out instance);
        }

        public CustomerInstance Get(CustomerInstanceId id)
        {
            if (!_instancesById.TryGetValue(id, out CustomerInstance instance))
            {
                throw new KeyNotFoundException(
                    $"Customer instance {id} was not found.");
            }

            return instance;
        }
    }
}
