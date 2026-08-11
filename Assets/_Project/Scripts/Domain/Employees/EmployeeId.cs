using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public readonly struct EmployeeId : IEquatable<EmployeeId>
    {
        public string Value { get; }

        public bool IsInitialized =>
            !string.IsNullOrEmpty(Value);

        public EmployeeId(string value)
        {
            if (!TryNormalize(value, out string normalized))
            {
                throw new ArgumentException(
                    "Employee ID must be a 32-character GUID in N format.",
                    nameof(value));
            }

            Value = normalized;
        }

        private EmployeeId(StableId stableId)
            : this(stableId.Value)
        {
        }

        public static EmployeeId New()
        {
            return new EmployeeId(StableId.New());
        }

        public static EmployeeId Parse(string value)
        {
            return new EmployeeId(value);
        }

        public static bool TryParse(
            string value,
            out EmployeeId employeeId)
        {
            if (!TryNormalize(value, out string normalized))
            {
                employeeId = default;
                return false;
            }

            employeeId = new EmployeeId(normalized);
            return true;
        }

        public StableId ToStableId()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException(
                    "An uninitialized employee ID cannot be converted to StableId.");
            }

            return StableId.Parse(Value);
        }

        public bool Equals(EmployeeId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is EmployeeId other && Equals(other);
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
            EmployeeId left,
            EmployeeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            EmployeeId left,
            EmployeeId right)
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
