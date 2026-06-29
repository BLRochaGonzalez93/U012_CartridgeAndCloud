using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Domain.Economy
{
    public readonly struct EconomyLedgerEntryId :
        IEquatable<EconomyLedgerEntryId>
    {
        public string Value { get; }

        public EconomyLedgerEntryId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Economy ledger entry ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(EconomyLedgerEntryId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is EconomyLedgerEntryId other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            EconomyLedgerEntryId left,
            EconomyLedgerEntryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            EconomyLedgerEntryId left,
            EconomyLedgerEntryId right)
        {
            return !left.Equals(right);
        }
    }

    public enum EconomyPostingType
    {
        CheckoutRevenue = 0,
        SupplierReceivingCost = 1
    }

    public readonly struct EconomyPostingKey :
        IEquatable<EconomyPostingKey>
    {
        public EconomyPostingType Type { get; }

        public string SourceId { get; }

        public EconomyPostingKey(
            EconomyPostingType type,
            string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId))
            {
                throw new ArgumentException(
                    "Economy posting source ID cannot be empty.",
                    nameof(sourceId));
            }

            Type = type;
            SourceId = sourceId;
        }

        public bool Equals(EconomyPostingKey other)
        {
            return Type == other.Type &&
                string.Equals(
                    SourceId,
                    other.SourceId,
                    StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is EconomyPostingKey other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Type * 397) ^
                    StringComparer.Ordinal.GetHashCode(
                        SourceId ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"{Type}:{SourceId}";
        }

        public static bool operator ==(
            EconomyPostingKey left,
            EconomyPostingKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            EconomyPostingKey left,
            EconomyPostingKey right)
        {
            return !left.Equals(right);
        }
    }

    public sealed class EconomyLedgerEntry
    {
        public EconomyLedgerEntryId Id { get; }

        public EconomyPostingKey PostingKey { get; }

        public StoreDayId DayId { get; }

        public Money Amount { get; }

        public EconomyLedgerEntry(
            EconomyLedgerEntryId id,
            EconomyPostingKey postingKey,
            StoreDayId dayId,
            Money amount)
        {
            if (string.IsNullOrWhiteSpace(dayId.Value))
            {
                throw new ArgumentException(
                    "Store day ID must be initialized.",
                    nameof(dayId));
            }

            if (!amount.IsPositive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Ledger amount must be greater than zero.");
            }

            Id = id;
            PostingKey = postingKey;
            DayId = dayId;
            Amount = amount;
        }
    }

    public enum EconomyLedgerPostFailureReason
    {
        None = 0,
        DuplicateEntryId = 1,
        DuplicatePosting = 2,
        CurrencyMismatch = 3
    }

    public sealed class EconomyLedgerPostResult
    {
        public bool Succeeded { get; }

        public EconomyLedgerPostFailureReason FailureReason { get; }

        public EconomyLedgerEntry Entry { get; }

        private EconomyLedgerPostResult(
            bool succeeded,
            EconomyLedgerPostFailureReason failureReason,
            EconomyLedgerEntry entry)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Entry = entry;
        }

        public static EconomyLedgerPostResult Success(
            EconomyLedgerEntry entry)
        {
            return new EconomyLedgerPostResult(
                true,
                EconomyLedgerPostFailureReason.None,
                entry);
        }

        public static EconomyLedgerPostResult Failure(
            EconomyLedgerPostFailureReason reason,
            EconomyLedgerEntry entry = null)
        {
            return new EconomyLedgerPostResult(
                false,
                reason,
                entry);
        }
    }

    public sealed class EconomyLedger
    {
        private readonly Dictionary<
            EconomyLedgerEntryId,
            EconomyLedgerEntry> _byId;

        private readonly Dictionary<
            EconomyPostingKey,
            EconomyLedgerEntry> _byPosting;

        private readonly List<EconomyLedgerEntry> _ordered;
        private ReadOnlyCollection<EconomyLedgerEntry> _readOnly;

        public CurrencyCode Currency { get; }

        public int Count => _ordered.Count;

        public IReadOnlyList<EconomyLedgerEntry> Entries =>
            _readOnly ??
            (_readOnly =
                new ReadOnlyCollection<EconomyLedgerEntry>(
                    new List<EconomyLedgerEntry>(_ordered)));

        public EconomyLedger(CurrencyCode currency)
        {
            if (string.IsNullOrWhiteSpace(currency.Value))
            {
                throw new ArgumentException(
                    "Currency must be initialized.",
                    nameof(currency));
            }

            Currency = currency;
            _byId =
                new Dictionary<
                    EconomyLedgerEntryId,
                    EconomyLedgerEntry>();
            _byPosting =
                new Dictionary<
                    EconomyPostingKey,
                    EconomyLedgerEntry>();
            _ordered = new List<EconomyLedgerEntry>();
        }

        public bool ContainsPosting(EconomyPostingKey key)
        {
            return _byPosting.ContainsKey(key);
        }

        public EconomyLedgerPostResult TryPost(
            EconomyLedgerEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry.Amount.Currency != Currency)
            {
                return EconomyLedgerPostResult.Failure(
                    EconomyLedgerPostFailureReason
                        .CurrencyMismatch,
                    entry);
            }

            if (_byId.ContainsKey(entry.Id))
            {
                return EconomyLedgerPostResult.Failure(
                    EconomyLedgerPostFailureReason
                        .DuplicateEntryId,
                    entry);
            }

            if (_byPosting.ContainsKey(entry.PostingKey))
            {
                return EconomyLedgerPostResult.Failure(
                    EconomyLedgerPostFailureReason
                        .DuplicatePosting,
                    entry);
            }

            _byId.Add(entry.Id, entry);
            _byPosting.Add(entry.PostingKey, entry);
            _ordered.Add(entry);
            _ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));
            _readOnly = null;

            return EconomyLedgerPostResult.Success(entry);
        }

        public Money GetTotal(
            StoreDayId dayId,
            EconomyPostingType postingType)
        {
            Money total = Money.Zero(Currency);

            foreach (EconomyLedgerEntry entry in _ordered)
            {
                if (entry.DayId == dayId &&
                    entry.PostingKey.Type == postingType)
                {
                    total = total.Add(entry.Amount);
                }
            }

            return total;
        }

        public int GetPostingCount(
            StoreDayId dayId,
            EconomyPostingType postingType)
        {
            int count = 0;

            foreach (EconomyLedgerEntry entry in _ordered)
            {
                if (entry.DayId == dayId &&
                    entry.PostingKey.Type == postingType)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
