using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Domain.Checkout
{
    public enum CheckoutTransactionState
    {
        Pending = 0,
        Completed = 1,
        Failed = 2
    }

    public sealed class CheckoutTransaction
    {
        public CheckoutTransactionId Id { get; }

        public CheckoutStationId StationId { get; }

        public CustomerInstanceId CustomerId { get; }

        public ShoppingCartId CartId { get; }

        public int LineCount { get; }

        public int UnitCount { get; }

        public CheckoutTransactionState State { get; private set; }

        public string FailureCode { get; private set; }

        public CheckoutTransaction(
            CheckoutTransactionId id,
            CheckoutStationId stationId,
            CustomerInstanceId customerId,
            ShoppingCartId cartId,
            int lineCount,
            int unitCount)
        {
            if (string.IsNullOrWhiteSpace(stationId.Value))
            {
                throw new ArgumentException(
                    "Station ID must be initialized.",
                    nameof(stationId));
            }

            if (string.IsNullOrWhiteSpace(customerId.Value))
            {
                throw new ArgumentException(
                    "Customer ID must be initialized.",
                    nameof(customerId));
            }

            if (string.IsNullOrWhiteSpace(cartId.Value))
            {
                throw new ArgumentException(
                    "Cart ID must be initialized.",
                    nameof(cartId));
            }

            if (lineCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lineCount));
            }

            if (unitCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCount));
            }

            Id = id;
            StationId = stationId;
            CustomerId = customerId;
            CartId = cartId;
            LineCount = lineCount;
            UnitCount = unitCount;
            State = CheckoutTransactionState.Pending;
            FailureCode = string.Empty;
        }

        public bool TryComplete()
        {
            if (State != CheckoutTransactionState.Pending)
            {
                return false;
            }

            State = CheckoutTransactionState.Completed;
            return true;
        }

        public bool TryFail(string failureCode)
        {
            if (State != CheckoutTransactionState.Pending ||
                string.IsNullOrWhiteSpace(failureCode))
            {
                return false;
            }

            FailureCode = failureCode;
            State = CheckoutTransactionState.Failed;
            return true;
        }
    }

    public sealed class CheckoutTransactionRegistry
    {
        private readonly Dictionary<
            CheckoutTransactionId,
            CheckoutTransaction> _byId;

        private readonly HashSet<ShoppingCartId>
            _completedCarts;

        public int Count => _byId.Count;

        public CheckoutTransactionRegistry()
        {
            _byId =
                new Dictionary<
                    CheckoutTransactionId,
                    CheckoutTransaction>();
            _completedCarts = new HashSet<ShoppingCartId>();
        }

        public bool Contains(
            CheckoutTransactionId transactionId)
        {
            return _byId.ContainsKey(transactionId);
        }

        public bool HasCompletedCart(ShoppingCartId cartId)
        {
            return _completedCarts.Contains(cartId);
        }

        public bool TryRegister(
            CheckoutTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(
                    nameof(transaction));
            }

            if (_byId.ContainsKey(transaction.Id))
            {
                return false;
            }

            _byId.Add(transaction.Id, transaction);
            return true;
        }

        public bool TryRecordCompletion(
            CheckoutTransaction transaction)
        {
            if (transaction == null ||
                transaction.State !=
                    CheckoutTransactionState.Completed ||
                !_byId.TryGetValue(
                    transaction.Id,
                    out CheckoutTransaction registered) ||
                !ReferenceEquals(transaction, registered))
            {
                return false;
            }

            return _completedCarts.Add(transaction.CartId);
        }

        public CheckoutTransaction Get(
            CheckoutTransactionId transactionId)
        {
            if (!_byId.TryGetValue(
                    transactionId,
                    out CheckoutTransaction transaction))
            {
                throw new KeyNotFoundException(
                    $"Checkout transaction {transactionId} was not found.");
            }

            return transaction;
        }
    }
}
