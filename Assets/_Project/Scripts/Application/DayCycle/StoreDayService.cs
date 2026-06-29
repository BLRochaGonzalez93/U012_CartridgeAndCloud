using System;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public sealed class StoreDayServiceResult
    {
        public bool Succeeded { get; }

        public StoreDayTransitionFailureReason
            TransitionFailureReason { get; }

        public StoreClosureProgressResult ClosureProgress { get; }

        private StoreDayServiceResult(
            bool succeeded,
            StoreDayTransitionFailureReason
                transitionFailureReason,
            StoreClosureProgressResult closureProgress)
        {
            Succeeded = succeeded;
            TransitionFailureReason =
                transitionFailureReason;
            ClosureProgress = closureProgress;
        }

        public static StoreDayServiceResult Success(
            StoreClosureProgressResult closureProgress = null)
        {
            return new StoreDayServiceResult(
                true,
                StoreDayTransitionFailureReason.None,
                closureProgress);
        }

        public static StoreDayServiceResult Failure(
            StoreDayTransitionFailureReason reason)
        {
            return new StoreDayServiceResult(
                false,
                reason,
                null);
        }
    }

    public sealed class StoreDayService
    {
        private readonly StoreDay _day;
        private readonly StoreClosureCoordinator _closure;

        public StoreDay Day => _day;

        public StoreDayService(
            StoreDay day,
            StoreClosureCoordinator closure)
        {
            _day = day ??
                throw new ArgumentNullException(nameof(day));
            _closure = closure ??
                throw new ArgumentNullException(nameof(closure));
        }

        public StoreDayServiceResult TryOpen()
        {
            StoreDayTransitionResult result =
                _day.TryOpen();

            return result.Succeeded
                ? StoreDayServiceResult.Success()
                : StoreDayServiceResult.Failure(
                    result.FailureReason);
        }

        public StoreDayServiceResult Advance(
            int elapsedSeconds,
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSessionRegistry sessions,
            ShoppingReservationRegistry reservations,
            DisplayInstanceRegistry displays)
        {
            StoreDayTransitionResult advance =
                _day.Advance(elapsedSeconds);

            if (!advance.Succeeded)
            {
                return StoreDayServiceResult.Failure(
                    advance.FailureReason);
            }

            if (_day.State != StoreDayState.Closing)
            {
                return StoreDayServiceResult.Success();
            }

            return StoreDayServiceResult.Success(
                _closure.Progress(
                    _day,
                    customers,
                    queue,
                    station,
                    sessions,
                    reservations,
                    displays));
        }

        public StoreDayServiceResult RequestClose(
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSessionRegistry sessions,
            ShoppingReservationRegistry reservations,
            DisplayInstanceRegistry displays)
        {
            StoreDayTransitionResult transition =
                _day.TryBeginClosing();

            if (!transition.Succeeded)
            {
                return StoreDayServiceResult.Failure(
                    transition.FailureReason);
            }

            return StoreDayServiceResult.Success(
                _closure.Progress(
                    _day,
                    customers,
                    queue,
                    station,
                    sessions,
                    reservations,
                    displays));
        }

        public StoreDayServiceResult ContinueClosing(
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSessionRegistry sessions,
            ShoppingReservationRegistry reservations,
            DisplayInstanceRegistry displays)
        {
            if (_day.State != StoreDayState.Closing)
            {
                return StoreDayServiceResult.Failure(
                    StoreDayTransitionFailureReason.InvalidState);
            }

            return StoreDayServiceResult.Success(
                _closure.Progress(
                    _day,
                    customers,
                    queue,
                    station,
                    sessions,
                    reservations,
                    displays));
        }
    }
}
