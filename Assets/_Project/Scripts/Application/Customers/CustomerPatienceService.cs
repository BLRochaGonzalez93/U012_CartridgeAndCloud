using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public sealed class CustomerPatienceService
    {
        public CustomerPatienceTickResult AdvanceAll(
            CustomerInstanceRegistry registry,
            int elapsedSeconds)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (elapsedSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedSeconds));
            }

            int inspected = 0;
            int advanced = 0;
            int newlyLeaving = 0;

            foreach (CustomerInstance customer in registry.Instances)
            {
                inspected++;
                if (customer.State != CustomerState.Browsing)
                {
                    continue;
                }

                CustomerTransitionResult result =
                    customer.AdvancePatience(elapsedSeconds);
                if (result.Succeeded)
                {
                    advanced++;
                    if (result.StateBefore != CustomerState.Leaving &&
                        result.StateAfter == CustomerState.Leaving)
                    {
                        newlyLeaving++;
                    }
                }
            }

            return new CustomerPatienceTickResult(
                inspected,
                advanced,
                newlyLeaving);
        }
    }
}
