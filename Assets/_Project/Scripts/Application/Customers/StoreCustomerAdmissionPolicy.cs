using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public enum StoreCustomerAdmissionFailureReason
    {
        None = 0,
        CheckoutUnavailable = 1,
        StoreNotOpen = 2,
        StoreClosing = 3,
        AdmissionBlocked = 4,
        CapacityReached = 5,
        DuplicateCustomer = 6,
        ActiveCustomersRemain = 7
    }

    public sealed class StoreCustomerAdmissionDecision
    {
        public bool Allowed { get; }

        public StoreCustomerAdmissionFailureReason
            FailureReason { get; }

        public string Detail { get; }

        private StoreCustomerAdmissionDecision(
            bool allowed,
            StoreCustomerAdmissionFailureReason failureReason,
            string detail)
        {
            Allowed = allowed;
            FailureReason = failureReason;
            Detail = detail ?? string.Empty;
        }

        public static StoreCustomerAdmissionDecision Allow(
            string detail)
        {
            return new StoreCustomerAdmissionDecision(
                true,
                StoreCustomerAdmissionFailureReason.None,
                detail);
        }

        public static StoreCustomerAdmissionDecision Block(
            StoreCustomerAdmissionFailureReason reason,
            string detail)
        {
            if (reason ==
                StoreCustomerAdmissionFailureReason.None)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(reason));
            }

            return new StoreCustomerAdmissionDecision(
                false,
                reason,
                detail);
        }
    }

    public readonly struct StoreCustomerAdmissionContext
    {
        public string StoreState { get; }

        public bool CheckoutOperational { get; }

        public bool AdmissionBlocked { get; }

        public int ActiveCustomers { get; }

        public int MaximumCustomers { get; }

        public StoreCustomerAdmissionContext(
            string storeState,
            bool checkoutOperational,
            bool admissionBlocked,
            int activeCustomers,
            int maximumCustomers)
        {
            if (string.IsNullOrWhiteSpace(storeState))
            {
                throw new ArgumentException(
                    "Store state is required.",
                    nameof(storeState));
            }

            if (activeCustomers < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(activeCustomers));
            }

            if (maximumCustomers <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maximumCustomers));
            }

            StoreState = storeState;
            CheckoutOperational = checkoutOperational;
            AdmissionBlocked = admissionBlocked;
            ActiveCustomers = activeCustomers;
            MaximumCustomers = maximumCustomers;
        }
    }

    public sealed class StoreCustomerAdmissionPolicy
    {
        public StoreCustomerAdmissionDecision EvaluateOpening(
            StoreCustomerAdmissionContext context)
        {
            if (!context.CheckoutOperational)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable,
                    "A functional and accessible checkout is required before opening the store.");
            }

            return StoreCustomerAdmissionDecision.Allow(
                "Checkout is operational. The store can open.");
        }

        public StoreCustomerAdmissionDecision EvaluateSpawn(
            StoreCustomerAdmissionContext context)
        {
            if (string.Equals(
                    context.StoreState,
                    "Closing",
                    StringComparison.Ordinal) ||
                string.Equals(
                    context.StoreState,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .StoreClosing,
                    "Customer admission is closed while the store is closing or closed.");
            }

            if (!string.Equals(
                    context.StoreState,
                    "Open",
                    StringComparison.Ordinal))
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .StoreNotOpen,
                    "Open the store before admitting customers.");
            }

            if (!context.CheckoutOperational)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable,
                    "Customer admission is blocked because checkout is not operational.");
            }

            if (context.AdmissionBlocked)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .AdmissionBlocked,
                    "Customer admission is temporarily blocked while store operations recover.");
            }

            if (context.ActiveCustomers >=
                context.MaximumCustomers)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .CapacityReached,
                    "Maximum active customer capacity reached.");
            }

            return StoreCustomerAdmissionDecision.Allow(
                "Customer admission is available.");
        }

        public StoreCustomerAdmissionDecision
            EvaluateCheckoutRemoval(
                string storeState,
                int checkoutCount)
        {
            if (string.IsNullOrWhiteSpace(storeState))
            {
                throw new ArgumentException(
                    "Store state is required.",
                    nameof(storeState));
            }

            if (checkoutCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(checkoutCount));
            }

            bool operating =
                string.Equals(
                    storeState,
                    "Open",
                    StringComparison.Ordinal) ||
                string.Equals(
                    storeState,
                    "Closing",
                    StringComparison.Ordinal);

            if (operating && checkoutCount <= 1)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .CheckoutUnavailable,
                    "The last functional checkout cannot be removed while the store is open or closing.");
            }

            return StoreCustomerAdmissionDecision.Allow(
                "Checkout removal preserves an operational station.");
        }

        public StoreCustomerAdmissionDecision
            EvaluateClosingCompletion(
                StoreCustomerAdmissionContext context)
        {
            if (context.ActiveCustomers > 0)
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .ActiveCustomersRemain,
                    context.ActiveCustomers +
                    " active customer(s) must finish or leave before closing can complete.");
            }

            return StoreCustomerAdmissionDecision.Allow(
                "All active customers have been resolved.");
        }
    }

    public interface IStoreOperationalGate
    {
        int ActiveCustomerCount { get; }

        int MaximumCustomerCount { get; }

        bool CheckoutOperational { get; }

        bool AdmissionBlocked { get; }

        string StatusDetail { get; }

        StoreCustomerAdmissionDecision EvaluateOpening();

        StoreCustomerAdmissionDecision EvaluateAdmission();

        StoreCustomerAdmissionDecision
            EvaluateClosingCompletion();
    }

    public interface ICustomerActivityRuntime :
        IStoreOperationalGate
    {
        StoreCustomerAdmissionDecision TryAdmit(
            CustomerInstanceId customerId);

        bool TrySetState(
            CustomerInstanceId customerId,
            ActiveCustomerState state);
    }
}
