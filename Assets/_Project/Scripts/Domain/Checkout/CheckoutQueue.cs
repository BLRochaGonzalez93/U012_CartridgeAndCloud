using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Domain.Checkout
{
    public enum CheckoutQueueEntryState
    {
        Waiting = 0,
        Called = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }

    public sealed class CheckoutQueueEntry
    {
        public CheckoutQueueEntryId Id { get; }
        public CustomerInstanceId CustomerId { get; }
        public ShoppingCartId CartId { get; }
        public CheckoutQueueEntryState State { get; private set; }

        public bool IsActive =>
            State == CheckoutQueueEntryState.Waiting ||
            State == CheckoutQueueEntryState.Called ||
            State == CheckoutQueueEntryState.Processing;

        public CheckoutQueueEntry(
            CheckoutQueueEntryId id,
            CustomerInstanceId customerId,
            ShoppingCartId cartId)
        {
            if (string.IsNullOrWhiteSpace(customerId.Value))
                throw new ArgumentException(
                    "Customer ID must be initialized.",
                    nameof(customerId));
            if (string.IsNullOrWhiteSpace(cartId.Value))
                throw new ArgumentException(
                    "Cart ID must be initialized.",
                    nameof(cartId));

            Id = id;
            CustomerId = customerId;
            CartId = cartId;
            State = CheckoutQueueEntryState.Waiting;
        }

        public bool TryCall()
        {
            if (State != CheckoutQueueEntryState.Waiting)
                return false;
            State = CheckoutQueueEntryState.Called;
            return true;
        }

        public bool TryBeginProcessing()
        {
            if (State != CheckoutQueueEntryState.Called)
                return false;
            State = CheckoutQueueEntryState.Processing;
            return true;
        }

        public bool TryComplete()
        {
            if (State != CheckoutQueueEntryState.Processing)
                return false;
            State = CheckoutQueueEntryState.Completed;
            return true;
        }

        public bool TryCancel()
        {
            if (State != CheckoutQueueEntryState.Waiting &&
                State != CheckoutQueueEntryState.Called)
                return false;
            State = CheckoutQueueEntryState.Cancelled;
            return true;
        }
    }

    public enum CheckoutQueueFailureReason
    {
        None = 0,
        QueueFull = 1,
        DuplicateEntryId = 2,
        CustomerAlreadyQueued = 3,
        CartAlreadyQueued = 4,
        QueueEmpty = 5,
        CurrentEntryBusy = 6,
        EntryNotFound = 7,
        EntryNotAtFront = 8,
        InvalidEntryState = 9,
        QueueSealed = 10
    }

    public sealed class CheckoutQueueResult
    {
        public bool Succeeded { get; }
        public CheckoutQueueFailureReason FailureReason { get; }
        public CheckoutQueueEntry Entry { get; }
        public int Position { get; }

        private CheckoutQueueResult(
            bool succeeded,
            CheckoutQueueFailureReason failureReason,
            CheckoutQueueEntry entry,
            int position)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Entry = entry;
            Position = position;
        }

        public static CheckoutQueueResult Success(
            CheckoutQueueEntry entry,
            int position)
        {
            return new CheckoutQueueResult(
                true,
                CheckoutQueueFailureReason.None,
                entry,
                position);
        }

        public static CheckoutQueueResult Failure(
            CheckoutQueueFailureReason reason,
            CheckoutQueueEntry entry = null,
            int position = 0)
        {
            return new CheckoutQueueResult(
                false,
                reason,
                entry,
                position);
        }
    }

    public sealed class CheckoutQueue
    {
        private readonly int _capacity;
        private readonly List<CheckoutQueueEntry> _active;
        private readonly Dictionary<
            CheckoutQueueEntryId,
            CheckoutQueueEntry> _allById;
        private bool _acceptingEntries;

        public int Capacity => _capacity;
        public int ActiveCount => _active.Count;
        public bool IsEmpty => _active.Count == 0;
        public bool IsAcceptingEntries => _acceptingEntries;

        public CheckoutQueueEntry CurrentEntry =>
            _active.Count == 0 ? null : _active[0];

        public IReadOnlyList<CheckoutQueueEntry> ActiveEntries =>
            new ReadOnlyCollection<CheckoutQueueEntry>(
                new List<CheckoutQueueEntry>(_active));

        public CheckoutQueue(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(capacity));

            _capacity = capacity;
            _active = new List<CheckoutQueueEntry>();
            _allById =
                new Dictionary<
                    CheckoutQueueEntryId,
                    CheckoutQueueEntry>();
            _acceptingEntries = true;
        }

        public bool TrySeal()
        {
            if (!_acceptingEntries)
                return false;

            _acceptingEntries = false;
            return true;
        }

        public CheckoutQueueResult TryEnqueue(
            CheckoutQueueEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            if (!_acceptingEntries)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.QueueSealed);
            }

            if (_active.Count >= _capacity)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.QueueFull);
            }

            if (_allById.ContainsKey(entry.Id))
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.DuplicateEntryId);
            }

            foreach (CheckoutQueueEntry existing in _active)
            {
                if (existing.CustomerId == entry.CustomerId)
                {
                    return CheckoutQueueResult.Failure(
                        CheckoutQueueFailureReason
                            .CustomerAlreadyQueued);
                }

                if (existing.CartId == entry.CartId)
                {
                    return CheckoutQueueResult.Failure(
                        CheckoutQueueFailureReason
                            .CartAlreadyQueued);
                }
            }

            _active.Add(entry);
            _allById.Add(entry.Id, entry);

            return CheckoutQueueResult.Success(
                entry,
                _active.Count);
        }

        public int GetPosition(CheckoutQueueEntryId entryId)
        {
            for (int index = 0; index < _active.Count; index++)
            {
                if (_active[index].Id == entryId)
                    return index + 1;
            }

            return 0;
        }

        public CheckoutQueueResult TryCallNext()
        {
            if (_active.Count == 0)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.QueueEmpty);
            }

            CheckoutQueueEntry entry = _active[0];
            if (entry.State != CheckoutQueueEntryState.Waiting)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.CurrentEntryBusy,
                    entry,
                    1);
            }

            if (!entry.TryCall())
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.InvalidEntryState,
                    entry,
                    1);
            }

            return CheckoutQueueResult.Success(entry, 1);
        }

        public CheckoutQueueResult TryBeginProcessing(
            CheckoutQueueEntryId entryId)
        {
            if (_active.Count == 0)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.QueueEmpty);
            }

            CheckoutQueueEntry entry = _active[0];
            if (entry.Id != entryId)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.EntryNotAtFront);
            }

            if (!entry.TryBeginProcessing())
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.InvalidEntryState,
                    entry,
                    1);
            }

            return CheckoutQueueResult.Success(entry, 1);
        }

        public CheckoutQueueResult TryCompleteCurrent(
            CheckoutQueueEntryId entryId)
        {
            if (_active.Count == 0)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.QueueEmpty);
            }

            CheckoutQueueEntry entry = _active[0];
            if (entry.Id != entryId)
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.EntryNotAtFront);
            }

            if (!entry.TryComplete())
            {
                return CheckoutQueueResult.Failure(
                    CheckoutQueueFailureReason.InvalidEntryState,
                    entry,
                    1);
            }

            _active.RemoveAt(0);
            return CheckoutQueueResult.Success(entry, 0);
        }

        public CheckoutQueueResult TryCancel(
            CheckoutQueueEntryId entryId)
        {
            for (int index = 0; index < _active.Count; index++)
            {
                CheckoutQueueEntry entry = _active[index];
                if (entry.Id != entryId)
                    continue;

                int previousPosition = index + 1;
                if (!entry.TryCancel())
                {
                    return CheckoutQueueResult.Failure(
                        CheckoutQueueFailureReason.InvalidEntryState,
                        entry,
                        previousPosition);
                }

                _active.RemoveAt(index);
                return CheckoutQueueResult.Success(
                    entry,
                    previousPosition);
            }

            return CheckoutQueueResult.Failure(
                CheckoutQueueFailureReason.EntryNotFound);
        }
    }
}
