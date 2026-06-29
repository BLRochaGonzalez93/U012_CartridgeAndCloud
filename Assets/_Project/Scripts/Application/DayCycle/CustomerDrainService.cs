using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public sealed class CustomerDrainResult
    {
        public int ActiveBefore { get; }

        public int DirectedToExit { get; }

        public int AlreadyLeaving { get; }

        public int AlreadyDespawned { get; }

        public int FailedTransitions { get; }

        public CustomerDrainResult(
            int activeBefore,
            int directedToExit,
            int alreadyLeaving,
            int alreadyDespawned,
            int failedTransitions)
        {
            ActiveBefore = activeBefore;
            DirectedToExit = directedToExit;
            AlreadyLeaving = alreadyLeaving;
            AlreadyDespawned = alreadyDespawned;
            FailedTransitions = failedTransitions;
        }
    }

    public sealed class CustomerDrainService
    {
        public CustomerDrainResult DirectAllToExit(
            CustomerInstanceRegistry customers)
        {
            if (customers == null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            int activeBefore = customers.ActiveCount;
            int directed = 0;
            int alreadyLeaving = 0;
            int alreadyDespawned = 0;
            int failed = 0;

            foreach (CustomerInstance customer
                     in customers.Instances)
            {
                switch (customer.State)
                {
                    case CustomerState.WaitingToEnter:
                    {
                        CustomerTransitionResult entering =
                            customer.BeginEntering();
                        if (!entering.Succeeded)
                        {
                            failed++;
                            break;
                        }

                        CustomerTransitionResult leaving =
                            customer.BeginLeaving();
                        if (leaving.Succeeded)
                        {
                            directed++;
                        }
                        else
                        {
                            failed++;
                        }

                        break;
                    }

                    case CustomerState.Entering:
                    case CustomerState.Browsing:
                    {
                        CustomerTransitionResult leaving =
                            customer.BeginLeaving();
                        if (leaving.Succeeded)
                        {
                            directed++;
                        }
                        else
                        {
                            failed++;
                        }

                        break;
                    }

                    case CustomerState.Leaving:
                        alreadyLeaving++;
                        break;

                    case CustomerState.Despawned:
                        alreadyDespawned++;
                        break;

                    default:
                        failed++;
                        break;
                }
            }

            return new CustomerDrainResult(
                activeBefore,
                directed,
                alreadyLeaving,
                alreadyDespawned,
                failed);
        }
    }
}
