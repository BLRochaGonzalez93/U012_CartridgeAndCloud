using System;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Application.Economy
{
    public enum SupplierReceivingEconomyFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        PostingAlreadyExists = 2,
        LedgerPostFailed = 3
    }

    public sealed class SupplierReceivingEconomyResult
    {
        public bool Succeeded { get; }

        public SupplierReceivingEconomyFailureReason
            FailureReason { get; }

        public Money Cost { get; }

        public EconomyLedgerPostResult LedgerResult { get; }

        private SupplierReceivingEconomyResult(
            bool succeeded,
            SupplierReceivingEconomyFailureReason failureReason,
            Money cost,
            EconomyLedgerPostResult ledgerResult)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Cost = cost;
            LedgerResult = ledgerResult;
        }

        public static SupplierReceivingEconomyResult Success(
            Money cost,
            EconomyLedgerPostResult ledgerResult)
        {
            return new SupplierReceivingEconomyResult(
                true,
                SupplierReceivingEconomyFailureReason.None,
                cost,
                ledgerResult);
        }

        public static SupplierReceivingEconomyResult Failure(
            SupplierReceivingEconomyFailureReason reason,
            Money cost,
            EconomyLedgerPostResult ledgerResult = null)
        {
            return new SupplierReceivingEconomyResult(
                false,
                reason,
                cost,
                ledgerResult);
        }
    }

    public sealed class SupplierReceivingEconomyService
    {
        public SupplierReceivingEconomyResult
            TryRecordReceivedCost(
                StoreDayId dayId,
                string receiptId,
                SupplierCatalogEntry catalogEntry,
                Quantity receivedQuantity,
                EconomyLedger ledger)
        {
            if (catalogEntry == null)
            {
                throw new ArgumentNullException(
                    nameof(catalogEntry));
            }

            if (ledger == null)
            {
                throw new ArgumentNullException(nameof(ledger));
            }

            if (receivedQuantity.IsZero)
            {
                return SupplierReceivingEconomyResult.Failure(
                    SupplierReceivingEconomyFailureReason
                        .InvalidQuantity,
                    Money.Zero(ledger.Currency));
            }

            EconomyPostingKey postingKey =
                new EconomyPostingKey(
                    EconomyPostingType.SupplierReceivingCost,
                    receiptId);

            if (ledger.ContainsPosting(postingKey))
            {
                return SupplierReceivingEconomyResult.Failure(
                    SupplierReceivingEconomyFailureReason
                        .PostingAlreadyExists,
                    Money.Zero(ledger.Currency));
            }

            Money cost =
                new Money(
                    checked(
                        (long)catalogEntry.UnitCostCents *
                        receivedQuantity.Value),
                    ledger.Currency);

            EconomyLedgerEntry ledgerEntry =
                new EconomyLedgerEntry(
                    new EconomyLedgerEntryId(
                        "supplier-cost-" + receiptId),
                    postingKey,
                    dayId,
                    cost);

            EconomyLedgerPostResult ledgerResult =
                ledger.TryPost(ledgerEntry);

            if (!ledgerResult.Succeeded)
            {
                return SupplierReceivingEconomyResult.Failure(
                    SupplierReceivingEconomyFailureReason
                        .LedgerPostFailed,
                    cost,
                    ledgerResult);
            }

            return SupplierReceivingEconomyResult.Success(
                cost,
                ledgerResult);
        }
    }
}
