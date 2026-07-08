using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public enum ActiveCustomerState
    {
        Entering = 0,
        Browsing = 1,
        Queueing = 2,
        CheckingOut = 3,
        Leaving = 4,
        Despawned = 5
    }

    public sealed class ActiveCustomerRecord
    {
        public CustomerInstanceId Id { get; }

        public ActiveCustomerState State { get; private set; }

        public bool IsActive =>
            State != ActiveCustomerState.Despawned;

        public ActiveCustomerRecord(
            CustomerInstanceId id,
            ActiveCustomerState state)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Customer instance ID must be initialized.",
                    nameof(id));
            }

            Id = id;
            State = state;
        }

        internal bool TrySetState(
            ActiveCustomerState state)
        {
            if (State == ActiveCustomerState.Despawned)
            {
                return state == ActiveCustomerState.Despawned;
            }

            State = state;
            return true;
        }
    }

    public sealed class ActiveCustomerRegistry
    {
        private readonly Dictionary<
            CustomerInstanceId,
            ActiveCustomerRecord> _recordsById =
                new Dictionary<
                    CustomerInstanceId,
                    ActiveCustomerRecord>();
        private readonly List<ActiveCustomerRecord> _ordered =
            new List<ActiveCustomerRecord>();
        private ReadOnlyCollection<ActiveCustomerRecord> _readOnly;

        public IReadOnlyList<ActiveCustomerRecord> Records =>
            _readOnly ??
            (_readOnly =
                new ReadOnlyCollection<ActiveCustomerRecord>(
                    _ordered));

        public int Count => _ordered.Count;

        public int ActiveCount
        {
            get
            {
                int count = 0;

                foreach (ActiveCustomerRecord record in _ordered)
                {
                    if (record.IsActive)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public event Action Changed;

        public bool TryRegister(
            CustomerInstanceId id,
            ActiveCustomerState initialState =
                ActiveCustomerState.Entering)
        {
            if (_recordsById.ContainsKey(id))
            {
                return false;
            }

            ActiveCustomerRecord record =
                new ActiveCustomerRecord(
                    id,
                    initialState);

            _recordsById.Add(id, record);
            _ordered.Add(record);
            _ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));
            _readOnly = null;
            Changed?.Invoke();
            return true;
        }

        public bool TrySetState(
            CustomerInstanceId id,
            ActiveCustomerState state)
        {
            if (!_recordsById.TryGetValue(
                    id,
                    out ActiveCustomerRecord record) ||
                !record.TrySetState(state))
            {
                return false;
            }

            Changed?.Invoke();
            return true;
        }

        public bool Contains(CustomerInstanceId id)
        {
            return _recordsById.ContainsKey(id);
        }

        public bool IsActive(CustomerInstanceId id)
        {
            return _recordsById.TryGetValue(
                       id,
                       out ActiveCustomerRecord record) &&
                   record.IsActive;
        }

        public bool TryGet(
            CustomerInstanceId id,
            out ActiveCustomerRecord record)
        {
            return _recordsById.TryGetValue(
                id,
                out record);
        }

        public void Clear()
        {
            if (_ordered.Count == 0)
            {
                return;
            }

            _recordsById.Clear();
            _ordered.Clear();
            _readOnly = null;
            Changed?.Invoke();
        }
    }
}
