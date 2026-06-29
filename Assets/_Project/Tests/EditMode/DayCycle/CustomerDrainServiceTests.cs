using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class CustomerDrainServiceTests
    {
        private static CustomerDrainResult Drain(
            params CustomerInstance[] customers)
        {
            var registry =
                new CustomerInstanceRegistry();

            foreach (CustomerInstance customer in customers)
            {
                registry.Add(customer);
            }

            return new CustomerDrainService()
                .DirectAllToExit(registry);
        }

        [Test] public void Drain_EmptyRegistryReturnsZeros()
        {
            CustomerDrainResult result =
                Drain();
            Assert.That(result.ActiveBefore, Is.EqualTo(0));
            Assert.That(
                result.DirectedToExit,
                Is.EqualTo(0));
        }

        [Test] public void Drain_WaitingCustomerBecomesLeaving()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.WaitingToEnter);
            CustomerDrainResult result = Drain(customer);
            Assert.That(
                customer.State,
                Is.EqualTo(CustomerState.Leaving));
            Assert.That(
                result.DirectedToExit,
                Is.EqualTo(1));
        }

        [Test] public void Drain_EnteringCustomerBecomesLeaving()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Entering);
            Drain(customer);
            Assert.That(
                customer.State,
                Is.EqualTo(CustomerState.Leaving));
        }

        [Test] public void Drain_BrowsingCustomerBecomesLeaving()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Browsing);
            Drain(customer);
            Assert.That(
                customer.State,
                Is.EqualTo(CustomerState.Leaving));
        }

        [Test] public void Drain_LeavingCustomerIsPreserved()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Leaving);
            CustomerDrainResult result = Drain(customer);
            Assert.That(
                customer.State,
                Is.EqualTo(CustomerState.Leaving));
            Assert.That(
                result.AlreadyLeaving,
                Is.EqualTo(1));
        }

        [Test] public void Drain_DespawnedCustomerIsIgnored()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Despawned);
            CustomerDrainResult result = Drain(customer);
            Assert.That(
                result.AlreadyDespawned,
                Is.EqualTo(1));
            Assert.That(
                result.ActiveBefore,
                Is.EqualTo(0));
        }

        [Test] public void Drain_CountsActiveBefore()
        {
            CustomerDrainResult result =
                Drain(
                    DayCycleTestFactory.Customer(
                        "a",
                        CustomerState.Browsing),
                    DayCycleTestFactory.Customer(
                        "b",
                        CustomerState.Leaving),
                    DayCycleTestFactory.Customer(
                        "c",
                        CustomerState.Despawned));
            Assert.That(
                result.ActiveBefore,
                Is.EqualTo(2));
        }

        [Test] public void Drain_MultipleCustomersAreDirected()
        {
            CustomerDrainResult result =
                Drain(
                    DayCycleTestFactory.Customer(
                        "a",
                        CustomerState.WaitingToEnter),
                    DayCycleTestFactory.Customer(
                        "b",
                        CustomerState.Entering),
                    DayCycleTestFactory.Customer(
                        "c",
                        CustomerState.Browsing));
            Assert.That(
                result.DirectedToExit,
                Is.EqualTo(3));
        }

        [Test] public void Drain_HasNoFailuresForValidStates()
        {
            CustomerDrainResult result =
                Drain(
                    DayCycleTestFactory.Customer(
                        "a",
                        CustomerState.WaitingToEnter),
                    DayCycleTestFactory.Customer(
                        "b",
                        CustomerState.Browsing));
            Assert.That(
                result.FailedTransitions,
                Is.EqualTo(0));
        }

        [Test] public void LeavingCustomer_CanDespawnAfterDrain()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Browsing);
            Drain(customer);
            Assert.That(
                customer.ArriveAtCurrentTarget()
                    .Succeeded,
                Is.True);
            Assert.That(
                customer.State,
                Is.EqualTo(CustomerState.Despawned));
        }

        [Test] public void Drain_IsIdempotentForLeavingCustomer()
        {
            CustomerInstance customer =
                DayCycleTestFactory.Customer(
                    state: CustomerState.Browsing);
            Drain(customer);
            CustomerDrainResult second = Drain(customer);
            Assert.That(
                second.DirectedToExit,
                Is.EqualTo(0));
            Assert.That(
                second.AlreadyLeaving,
                Is.EqualTo(1));
        }

        [Test] public void Drain_RejectsNullRegistry()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new CustomerDrainService()
                    .DirectAllToExit(null));
        }
    }
}
