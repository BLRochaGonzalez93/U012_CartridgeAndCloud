using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerAdmissionCharacterizationTests
    {
        private readonly StoreCustomerAdmissionPolicy _policy =
            new StoreCustomerAdmissionPolicy();

        [Test]
        public void CHAR_CUS_001_OpeningWithoutCheckoutIsRejected()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateOpening(
                    Context(
                        "BeforeOpen",
                        false,
                        false,
                        0));

            Assert.That(result.Allowed, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable));
        }

        [Test]
        public void CHAR_CUS_002_OpeningWithFunctionalCheckoutIsAllowed()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateOpening(
                    Context(
                        "BeforeOpen",
                        true,
                        false,
                        0));

            Assert.That(result.Allowed, Is.True);
        }

        [Test]
        public void CHAR_CUS_003_LastCheckoutCannotBeRemovedWhileOpen()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateCheckoutRemoval(
                    "Open",
                    1);

            Assert.That(result.Allowed, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable));
        }

        [Test]
        public void CHAR_CUS_004_InvalidCheckoutBlocksNewAdmissions()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateSpawn(
                    Context(
                        "Open",
                        false,
                        true,
                        2));

            Assert.That(result.Allowed, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable));
        }

        [Test]
        public void CHAR_CUS_005_EightActiveCustomersBlockTheNinth()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateSpawn(
                    Context(
                        "Open",
                        true,
                        false,
                        8));

            Assert.That(result.Allowed, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .CapacityReached));
        }

        [Test]
        public void CHAR_CUS_006_LeavingCustomerStillCountsAsActive()
        {
            ActiveCustomerRegistry registry =
                new ActiveCustomerRegistry();
            CustomerInstanceId id =
                new CustomerInstanceId("customer-001");

            registry.TryRegister(id);
            registry.TrySetState(
                id,
                ActiveCustomerState.Leaving);

            Assert.That(registry.ActiveCount, Is.EqualTo(1));
            Assert.That(registry.IsActive(id), Is.True);
        }

        [Test]
        public void CHAR_CUS_007_CapacityIsReleasedOnlyAtDespawned()
        {
            ActiveCustomerRegistry registry =
                new ActiveCustomerRegistry();
            CustomerInstanceId id =
                new CustomerInstanceId("customer-001");

            registry.TryRegister(id);
            registry.TrySetState(
                id,
                ActiveCustomerState.CheckingOut);
            Assert.That(registry.ActiveCount, Is.EqualTo(1));

            registry.TrySetState(
                id,
                ActiveCustomerState.Leaving);
            Assert.That(registry.ActiveCount, Is.EqualTo(1));

            registry.TrySetState(
                id,
                ActiveCustomerState.Despawned);
            Assert.That(registry.ActiveCount, Is.Zero);
        }

        [Test]
        public void CHAR_CUS_008_ClosingNeverAdmitsNewCustomers()
        {
            StoreCustomerAdmissionDecision result =
                _policy.EvaluateSpawn(
                    Context(
                        "Closing",
                        true,
                        false,
                        0));

            Assert.That(result.Allowed, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .StoreClosing));
        }

        [Test]
        public void CHAR_CUS_009_ClosingCompletesOnlyAfterCustomerDrain()
        {
            StoreCustomerAdmissionDecision blocked =
                _policy.EvaluateClosingCompletion(
                    Context(
                        "Closing",
                        true,
                        false,
                        1));
            StoreCustomerAdmissionDecision allowed =
                _policy.EvaluateClosingCompletion(
                    Context(
                        "Closing",
                        true,
                        false,
                        0));

            Assert.That(blocked.Allowed, Is.False);
            Assert.That(
                blocked.FailureReason,
                Is.EqualTo(
                    StoreCustomerAdmissionFailureReason
                        .ActiveCustomersRemain));
            Assert.That(allowed.Allowed, Is.True);
        }

        private static StoreCustomerAdmissionContext Context(
            string state,
            bool checkoutOperational,
            bool admissionBlocked,
            int activeCustomers)
        {
            return new StoreCustomerAdmissionContext(
                state,
                checkoutOperational,
                admissionBlocked,
                activeCustomers,
                8);
        }
    }
}
