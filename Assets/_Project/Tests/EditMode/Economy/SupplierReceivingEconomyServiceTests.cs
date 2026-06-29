using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class SupplierReceivingEconomyServiceTests
    {
        [Test] public void ReceiptCost_Succeeds()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product),
                        new Quantity(3),
                        ledger);

            Assert.That(result.Succeeded, Is.True);
        }

        [Test] public void ReceiptCost_IsExact()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product,
                            unitCostCents: 1299),
                        new Quantity(6),
                        ledger);

            Assert.That(
                result.Cost.MinorUnits,
                Is.EqualTo(7794));
        }

        [Test] public void ReceiptCost_RejectsZeroQuantity()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product),
                        new Quantity(0),
                        ledger);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    SupplierReceivingEconomyFailureReason
                        .InvalidQuantity));
            Assert.That(ledger.Count, Is.EqualTo(0));
        }

        [Test] public void ReceiptCost_BlocksDuplicateReceipt()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            SupplierReceivingEconomyService service =
                new SupplierReceivingEconomyService();

            service.TryRecordReceivedCost(
                new StoreDayId("day-a"),
                "receipt-a",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);

            SupplierReceivingEconomyResult duplicate =
                service.TryRecordReceivedCost(
                    new StoreDayId("day-a"),
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                duplicate.FailureReason,
                Is.EqualTo(
                    SupplierReceivingEconomyFailureReason
                        .PostingAlreadyExists));
        }

        [Test] public void ReceiptCost_PostsOneEntry()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    new StoreDayId("day-a"),
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(ledger.Count, Is.EqualTo(1));
        }

        [Test] public void ReceiptCost_UsesCostPostingType()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    dayId,
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                ledger.GetPostingCount(
                    dayId,
                    EconomyPostingType
                        .SupplierReceivingCost),
                Is.EqualTo(1));
        }

        [Test] public void DifferentReceipts_AreAllowed()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            SupplierReceivingEconomyService service =
                new SupplierReceivingEconomyService();

            service.TryRecordReceivedCost(
                dayId,
                "receipt-a",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);
            service.TryRecordReceivedCost(
                dayId,
                "receipt-b",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);

            Assert.That(ledger.Count, Is.EqualTo(2));
        }

        [Test] public void ReceiptCost_IsAssignedToDay()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayA = new StoreDayId("day-a");
            StoreDayId dayB = new StoreDayId("day-b");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    dayA,
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                ledger.GetTotal(
                    dayB,
                    EconomyPostingType
                        .SupplierReceivingCost)
                    .IsZero,
                Is.True);
        }

        [Test] public void ReceiptCost_RejectsNullCatalogEntry()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        null,
                        new Quantity(1),
                        new EconomyLedger(
                            EconomyTestFactory.Eur)));
        }

        [Test] public void ReceiptCost_RejectsNullLedger()
        {
            var product = EconomyTestFactory.Product();

            Assert.Throws<System.ArgumentNullException>(
                () => new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(product),
                        new Quantity(1),
                        null));
        }
    }
}
