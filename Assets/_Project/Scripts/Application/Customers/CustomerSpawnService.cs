using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public sealed class CustomerSpawnService
    {
        private readonly CustomerProfileRegistry _profiles;
        private readonly CustomerInstanceRegistry _instances;

        public CustomerSpawnService(
            CustomerProfileRegistry profiles,
            CustomerInstanceRegistry instances)
        {
            _profiles = profiles ??
                throw new ArgumentNullException(nameof(profiles));
            _instances = instances ??
                throw new ArgumentNullException(nameof(instances));
        }

        public CustomerSpawnResult TrySpawnNext(
            CustomerSpawnQueue queue,
            CustomerSpawnPolicy policy)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            int activeBefore = _instances.ActiveCount;

            if (!queue.TryPeek(out CustomerSpawnRequest request))
            {
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.QueueEmpty,
                    activeBefore);
            }

            if (activeBefore >= policy.MaxActiveCustomers)
            {
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.PopulationLimitReached,
                    activeBefore);
            }

            if (!_profiles.TryGet(request.ProfileId, out CustomerProfile profile))
            {
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.ProfileMissing,
                    activeBefore);
            }

            if (_instances.Contains(request.InstanceId))
            {
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.DuplicateInstanceId,
                    activeBefore);
            }

            CustomerInstance customer = new CustomerInstance(
                request.InstanceId,
                request.ProfileId,
                request.NavigationPlan,
                profile.PatienceSeconds);
            customer.BeginEntering();

            if (!_instances.Add(customer))
            {
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.DuplicateInstanceId,
                    activeBefore);
            }

            if (!queue.TryDequeue(out CustomerSpawnRequest dequeued) ||
                dequeued.RequestId != request.RequestId)
            {
                _instances.Remove(customer.Id);
                return CustomerSpawnResult.Failure(
                    CustomerSpawnFailureReason.QueueStateChanged,
                    activeBefore);
            }

            return CustomerSpawnResult.Success(
                customer,
                activeBefore,
                _instances.ActiveCount);
        }
    }
}
