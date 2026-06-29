using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Domain.Shopping
{
    public sealed class CustomerShoppingSessionRegistry
    {
        private readonly Dictionary<
            CustomerInstanceId,
            CustomerShoppingSession> _byCustomer;
        private readonly List<CustomerShoppingSession> _ordered;
        private ReadOnlyCollection<
            CustomerShoppingSession> _readOnly;

        public int Count => _ordered.Count;

        public IReadOnlyList<CustomerShoppingSession> Sessions =>
            _readOnly ??
            (_readOnly =
                new ReadOnlyCollection<
                    CustomerShoppingSession>(_ordered));

        public int PendingCount
        {
            get
            {
                int count = 0;

                foreach (CustomerShoppingSession session
                         in _ordered)
                {
                    if (session.State !=
                            CustomerShoppingState.Abandoned &&
                        session.State !=
                            CustomerShoppingState.CheckedOut)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public CustomerShoppingSessionRegistry()
        {
            _byCustomer =
                new Dictionary<
                    CustomerInstanceId,
                    CustomerShoppingSession>();
            _ordered = new List<CustomerShoppingSession>();
        }

        public bool TryRegister(
            CustomerShoppingSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (_byCustomer.ContainsKey(session.CustomerId))
            {
                return false;
            }

            _byCustomer.Add(session.CustomerId, session);
            _ordered.Add(session);
            _ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.CustomerId.Value,
                        right.CustomerId.Value));
            _readOnly = null;
            return true;
        }

        public bool Contains(CustomerInstanceId customerId)
        {
            return _byCustomer.ContainsKey(customerId);
        }

        public bool TryGet(
            CustomerInstanceId customerId,
            out CustomerShoppingSession session)
        {
            return _byCustomer.TryGetValue(
                customerId,
                out session);
        }

        public CustomerShoppingSession Get(
            CustomerInstanceId customerId)
        {
            if (!_byCustomer.TryGetValue(
                    customerId,
                    out CustomerShoppingSession session))
            {
                throw new KeyNotFoundException(
                    $"Shopping session for customer " +
                    $"{customerId} was not found.");
            }

            return session;
        }
    }
}
