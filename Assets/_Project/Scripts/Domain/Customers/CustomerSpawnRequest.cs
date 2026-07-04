using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerSpawnRequest
    {
        public CustomerSpawnRequestId RequestId { get; }

        public CustomerInstanceId InstanceId { get; }

        public CustomerProfileId ProfileId { get; }

        public CustomerNavigationPlan NavigationPlan { get; }

        public CustomerSpawnRequest(
            CustomerSpawnRequestId requestId,
            CustomerInstanceId instanceId,
            CustomerProfileId profileId,
            CustomerNavigationPlan navigationPlan)
        {
            if (string.IsNullOrWhiteSpace(requestId.Value))
            {
                throw new ArgumentException(
                    "Spawn request ID must be initialized.",
                    nameof(requestId));
            }

            if (string.IsNullOrWhiteSpace(instanceId.Value))
            {
                throw new ArgumentException(
                    "Customer instance ID must be initialized.",
                    nameof(instanceId));
            }

            if (string.IsNullOrWhiteSpace(profileId.Value))
            {
                throw new ArgumentException(
                    "Customer profile ID must be initialized.",
                    nameof(profileId));
            }

            RequestId = requestId;
            InstanceId = instanceId;
            ProfileId = profileId;
            NavigationPlan = navigationPlan ??
                throw new ArgumentNullException(nameof(navigationPlan));
        }
    }

    public readonly struct CustomerSpawnRequestId : IEquatable<CustomerSpawnRequestId>
    {
        public string Value { get; }

        public CustomerSpawnRequestId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer spawn request ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerSpawnRequestId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerSpawnRequestId other && Equals(other);
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

        public static bool operator ==(CustomerSpawnRequestId left, CustomerSpawnRequestId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerSpawnRequestId left, CustomerSpawnRequestId right)
        {
            return !left.Equals(right);
        }
    }
}
