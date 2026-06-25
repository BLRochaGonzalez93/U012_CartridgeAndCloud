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
}
