using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public sealed class CustomerSpawnResult
    {
        public bool Succeeded { get; }

        public CustomerSpawnFailureReason FailureReason { get; }

        public CustomerInstance Customer { get; }

        public int ActiveCountBefore { get; }

        public int ActiveCountAfter { get; }

        private CustomerSpawnResult(
            bool succeeded,
            CustomerSpawnFailureReason failureReason,
            CustomerInstance customer,
            int activeCountBefore,
            int activeCountAfter)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Customer = customer;
            ActiveCountBefore = activeCountBefore;
            ActiveCountAfter = activeCountAfter;
        }

        public static CustomerSpawnResult Success(
            CustomerInstance customer,
            int activeCountBefore,
            int activeCountAfter)
        {
            return new CustomerSpawnResult(
                true,
                CustomerSpawnFailureReason.None,
                customer,
                activeCountBefore,
                activeCountAfter);
        }

        public static CustomerSpawnResult Failure(
            CustomerSpawnFailureReason reason,
            int activeCount)
        {
            return new CustomerSpawnResult(
                false,
                reason,
                null,
                activeCount,
                activeCount);
        }
    }
}
