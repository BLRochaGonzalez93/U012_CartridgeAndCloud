using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Checkout
{
    public enum CheckoutFailureReason
    {
        None = 0,
        DuplicateTransactionId = 1,
        CartAlreadyCheckedOut = 2,
        QueueEntryNotProcessing = 3,
        StationNotProcessing = 4,
        StationEntryMismatch = 5,
        SessionNotReady = 6,
        EmptyCart = 7,
        OwnershipMismatch = 8,
        ReservationMissing = 9,
        ReservationNotActive = 10,
        ReservationMismatch = 11,
        DisplayMissing = 12,
        DisplayProductMismatch = 13,
        InsufficientStock = 14,
        TransactionRegistrationFailed = 15,
        InventoryCommitFailed = 16,
        CartCommitFailed = 17,
        ReservationCommitFailed = 18,
        SessionCommitFailed = 19,
        QueueCommitFailed = 20,
        StationCommitFailed = 21,
        TransactionCommitFailed = 22
    }

    public sealed class CheckoutResult
    {
        public bool Succeeded { get; }

        public CheckoutFailureReason FailureReason { get; }

        public CheckoutTransaction Transaction { get; }

        public int ProcessedLines { get; }

        public int ProcessedUnits { get; }

        private CheckoutResult(
            bool succeeded,
            CheckoutFailureReason failureReason,
            CheckoutTransaction transaction,
            int processedLines,
            int processedUnits)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Transaction = transaction;
            ProcessedLines = processedLines;
            ProcessedUnits = processedUnits;
        }

        public static CheckoutResult Success(
            CheckoutTransaction transaction)
        {
            return new CheckoutResult(
                true,
                CheckoutFailureReason.None,
                transaction,
                transaction.LineCount,
                transaction.UnitCount);
        }

        public static CheckoutResult Failure(
            CheckoutFailureReason reason,
            CheckoutTransaction transaction = null)
        {
            return new CheckoutResult(
                false,
                reason,
                transaction,
                0,
                0);
        }
    }

    public sealed class CheckoutService
    {
        private sealed class StockCommit
        {
            public DisplayInstance Display { get; }

            public ProductDefinitionId ProductId { get; }

            public Quantity Quantity { get; private set; }

            public StockCommit(
                DisplayInstance display,
                ProductDefinitionId productId,
                Quantity quantity)
            {
                Display = display;
                ProductId = productId;
                Quantity = quantity;
            }

            public void Add(Quantity quantity)
            {
                Quantity = Quantity.Add(quantity);
            }
        }

        public CheckoutResult TryCheckout(
            CheckoutTransactionId transactionId,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSession session,
            DisplayInstanceRegistry displays,
            ShoppingReservationRegistry reservations,
            CheckoutTransactionRegistry transactions)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (displays == null)
                throw new ArgumentNullException(nameof(displays));
            if (reservations == null)
                throw new ArgumentNullException(nameof(reservations));
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            if (transactions.Contains(transactionId))
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.DuplicateTransactionId);
            }

            if (transactions.HasCompletedCart(session.Cart.Id) ||
                session.State == CustomerShoppingState.CheckedOut)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.CartAlreadyCheckedOut);
            }

            CheckoutQueueEntry entry = queue.CurrentEntry;
            if (entry == null ||
                entry.State != CheckoutQueueEntryState.Processing)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.QueueEntryNotProcessing);
            }

            if (!station.IsBusy)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.StationNotProcessing);
            }

            if (station.CurrentEntryId != entry.Id)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.StationEntryMismatch);
            }

            if (session.State !=
                CustomerShoppingState.ReadyForCheckout)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.SessionNotReady);
            }

            if (session.Cart.IsEmpty)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.EmptyCart);
            }

            if (entry.CustomerId != session.CustomerId ||
                entry.CartId != session.Cart.Id)
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason.OwnershipMismatch);
            }

            List<ShoppingReservation> validatedReservations =
                new List<ShoppingReservation>();
            Dictionary<DisplayInstanceId, StockCommit>
                stockByDisplay =
                    new Dictionary<DisplayInstanceId, StockCommit>();

            foreach (ShoppingCartLine line in session.Cart.Lines)
            {
                if (!reservations.TryGet(
                        line.ReservationId,
                        out ShoppingReservation reservation))
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason.ReservationMissing);
                }

                if (!reservation.IsActive)
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason
                            .ReservationNotActive);
                }

                if (reservation.CustomerId != session.CustomerId ||
                    reservation.DisplayId != line.DisplayId ||
                    reservation.ProductId != line.ProductId ||
                    reservation.Quantity != line.Quantity)
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason.ReservationMismatch);
                }

                if (!displays.TryGet(
                        line.DisplayId,
                        out DisplayInstance display))
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason.DisplayMissing);
                }

                if (!display.HasAssignedProduct ||
                    display.AssignedProductId != line.ProductId)
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason
                            .DisplayProductMismatch);
                }

                if (stockByDisplay.TryGetValue(
                        line.DisplayId,
                        out StockCommit existing))
                {
                    existing.Add(line.Quantity);
                }
                else
                {
                    stockByDisplay.Add(
                        line.DisplayId,
                        new StockCommit(
                            display,
                            line.ProductId,
                            line.Quantity));
                }

                validatedReservations.Add(reservation);
            }

            foreach (StockCommit stock in stockByDisplay.Values)
            {
                Quantity onHand =
                    stock.Display.Inventory.GetQuantity(
                        stock.ProductId);
                if (stock.Quantity > onHand)
                {
                    return CheckoutResult.Failure(
                        CheckoutFailureReason.InsufficientStock);
                }
            }

            CheckoutTransaction transaction =
                new CheckoutTransaction(
                    transactionId,
                    station.Id,
                    session.CustomerId,
                    session.Cart.Id,
                    session.Cart.LineCount,
                    session.Cart.TotalUnits);

            if (!transactions.TryRegister(transaction))
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason
                        .TransactionRegistrationFailed);
            }

            List<StockCommit> removedStock =
                new List<StockCommit>();

            foreach (StockCommit stock in stockByDisplay.Values)
            {
                InventoryMutationResult removal =
                    stock.Display.Inventory.TryRemove(
                        stock.ProductId,
                        stock.Quantity);

                if (!removal.Succeeded)
                {
                    RollbackStock(removedStock);
                    transaction.TryFail(
                        "inventory_commit_failed");
                    return CheckoutResult.Failure(
                        CheckoutFailureReason
                            .InventoryCommitFailed,
                        transaction);
                }

                removedStock.Add(stock);
            }

            List<ShoppingReservation> removedCartLines =
                new List<ShoppingReservation>();

            foreach (ShoppingReservation reservation
                     in validatedReservations)
            {
                ShoppingCartMutationResult removal =
                    session.Cart.TryRemove(reservation.Id);

                if (!removal.Succeeded)
                {
                    RollbackCart(
                        session.Cart,
                        removedCartLines);
                    RollbackStock(removedStock);
                    transaction.TryFail(
                        "cart_commit_failed");
                    return CheckoutResult.Failure(
                        CheckoutFailureReason.CartCommitFailed,
                        transaction);
                }

                removedCartLines.Add(reservation);
            }

            List<ShoppingReservation> consumed =
                new List<ShoppingReservation>();

            foreach (ShoppingReservation reservation
                     in validatedReservations)
            {
                if (!reservation.TryConsume())
                {
                    RollbackReservations(consumed);
                    RollbackCart(
                        session.Cart,
                        removedCartLines);
                    RollbackStock(removedStock);
                    transaction.TryFail(
                        "reservation_commit_failed");
                    return CheckoutResult.Failure(
                        CheckoutFailureReason
                            .ReservationCommitFailed,
                        transaction);
                }

                consumed.Add(reservation);
            }

            if (!session.TryMarkCheckedOut())
            {
                RollbackReservations(consumed);
                RollbackCart(
                    session.Cart,
                    removedCartLines);
                RollbackStock(removedStock);
                transaction.TryFail(
                    "session_commit_failed");
                return CheckoutResult.Failure(
                    CheckoutFailureReason.SessionCommitFailed,
                    transaction);
            }

            CheckoutQueueResult queueResult =
                queue.TryCompleteCurrent(entry.Id);
            if (!queueResult.Succeeded)
            {
                transaction.TryFail(
                    "queue_commit_failed");
                return CheckoutResult.Failure(
                    CheckoutFailureReason.QueueCommitFailed,
                    transaction);
            }

            if (!station.TryCompleteProcessing(entry.Id))
            {
                transaction.TryFail(
                    "station_commit_failed");
                return CheckoutResult.Failure(
                    CheckoutFailureReason.StationCommitFailed,
                    transaction);
            }

            if (!transaction.TryComplete() ||
                !transactions.TryRecordCompletion(transaction))
            {
                return CheckoutResult.Failure(
                    CheckoutFailureReason
                        .TransactionCommitFailed,
                    transaction);
            }

            return CheckoutResult.Success(transaction);
        }

        private static void RollbackStock(
            IReadOnlyList<StockCommit> removedStock)
        {
            for (int index = removedStock.Count - 1;
                 index >= 0;
                 index--)
            {
                StockCommit stock = removedStock[index];
                stock.Display.Inventory.TryAdd(
                    stock.ProductId,
                    stock.Quantity);
            }
        }

        private static void RollbackCart(
            ShoppingCart cart,
            IReadOnlyList<ShoppingReservation> reservations)
        {
            for (int index = 0;
                 index < reservations.Count;
                 index++)
            {
                cart.TryAdd(reservations[index]);
            }
        }

        private static void RollbackReservations(
            IReadOnlyList<ShoppingReservation> reservations)
        {
            for (int index = reservations.Count - 1;
                 index >= 0;
                 index--)
            {
                reservations[index].TryRollbackConsumption();
            }
        }
    }
}
