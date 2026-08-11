using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public readonly struct CandidateId : IEquatable<CandidateId>
    {
        public string Value { get; }

        public bool IsInitialized =>
            !string.IsNullOrEmpty(Value);

        public CandidateId(string value)
        {
            if (!TryNormalize(value, out string normalized))
            {
                throw new ArgumentException(
                    "Candidate ID must be a 32-character GUID in N format.",
                    nameof(value));
            }

            Value = normalized;
        }

        private CandidateId(StableId stableId)
            : this(stableId.Value)
        {
        }

        public static CandidateId New()
        {
            return new CandidateId(StableId.New());
        }

        public static CandidateId Parse(string value)
        {
            return new CandidateId(value);
        }

        public static bool TryParse(
            string value,
            out CandidateId candidateId)
        {
            if (!TryNormalize(value, out string normalized))
            {
                candidateId = default;
                return false;
            }

            candidateId = new CandidateId(normalized);
            return true;
        }

        public StableId ToStableId()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException(
                    "An uninitialized candidate ID cannot be converted to StableId.");
            }

            return StableId.Parse(Value);
        }

        public bool Equals(CandidateId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CandidateId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            CandidateId left,
            CandidateId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            CandidateId left,
            CandidateId right)
        {
            return !left.Equals(right);
        }

        private static bool TryNormalize(
            string value,
            out string normalized)
        {
            normalized = string.Empty;

            if (string.IsNullOrWhiteSpace(value) ||
                value.Length != 32 ||
                !Guid.TryParseExact(
                    value,
                    "N",
                    out Guid parsed))
            {
                return false;
            }

            normalized = parsed.ToString("N");
            return true;
        }
    }
}
