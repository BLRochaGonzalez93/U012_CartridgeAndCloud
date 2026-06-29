using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class SaveRecordTests
    {
        [Test] public void ProductQuantity_RejectsEmptyProduct()
        {
            Assert.Throws<System.ArgumentException>(
                () => new ProductQuantitySaveRecord(
                    "",
                    1));
        }

        [Test] public void ProductQuantity_RejectsNegativeQuantity()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new ProductQuantitySaveRecord(
                    "product",
                    -1));
        }

        [Test] public void Inventory_RejectsDuplicateProduct()
        {
            Assert.Throws<System.ArgumentException>(
                () => new InventoryContainerSaveRecord(
                    "inventory",
                    10,
                    new[]
                    {
                        new ProductQuantitySaveRecord(
                            "product",
                            1),
                        new ProductQuantitySaveRecord(
                            "product",
                            2)
                    }));
        }

        [Test] public void Inventory_RejectsCapacityOverflow()
        {
            Assert.Throws<System.ArgumentException>(
                () => new InventoryContainerSaveRecord(
                    "inventory",
                    2,
                    new[]
                    {
                        new ProductQuantitySaveRecord(
                            "product",
                            3)
                    }));
        }

        [Test] public void Inventory_AcceptsExactCapacity()
        {
            var record =
                new InventoryContainerSaveRecord(
                    "inventory",
                    3,
                    new[]
                    {
                        new ProductQuantitySaveRecord(
                            "product",
                            3)
                    });

            Assert.That(record.Capacity, Is.EqualTo(3));
            Assert.That(
                record.Products.Count,
                Is.EqualTo(1));
        }

        [Test] public void SupplierOrder_RejectsOverReceiving()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new SupplierOrderSaveRecord(
                    "order",
                    "supplier",
                    "product",
                    "Received",
                    2,
                    3,
                    100));
        }

        [Test] public void SupplierOrder_RejectsZeroCost()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new SupplierOrderSaveRecord(
                    "order",
                    "supplier",
                    "product",
                    "Placed",
                    2,
                    0,
                    0));
        }

        [Test] public void Display_RejectsEmptyId()
        {
            Assert.Throws<System.ArgumentException>(
                () => new DisplaySaveRecord(
                    "",
                    "definition",
                    "product",
                    "inventory"));
        }

        [Test] public void Customer_RejectsNegativePatience()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new CustomerSaveRecord(
                    "customer",
                    "profile",
                    "Browsing",
                    -1,
                    0));
        }

        [Test] public void ShoppingSession_RejectsZeroCartLimit()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new ShoppingSessionSaveRecord(
                    "customer",
                    "intent",
                    "cart",
                    "Searching",
                    0));
        }

        [Test] public void Reservation_RejectsZeroQuantity()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new ReservationSaveRecord(
                    "reservation",
                    "customer",
                    "cart",
                    "display",
                    "product",
                    0,
                    "Active"));
        }

        [Test] public void QueueEntry_RejectsZeroPosition()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new CheckoutQueueEntrySaveRecord(
                    "entry",
                    "customer",
                    "cart",
                    "Waiting",
                    0));
        }

        [Test] public void Transaction_RejectsZeroLines()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new CheckoutTransactionSaveRecord(
                    "transaction",
                    "customer",
                    "cart",
                    "station",
                    "Completed",
                    0,
                    1));
        }

        [Test] public void DayCycle_RejectsElapsedBeyondDuration()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new DayCycleSaveRecord(
                    "day",
                    "Open",
                    10,
                    11,
                    true));
        }

        [Test] public void Ledger_RejectsZeroAmount()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new EconomyLedgerSaveRecord(
                    "entry",
                    "CheckoutRevenue",
                    "source",
                    "day",
                    0,
                    "EUR"));
        }

        [Test] public void Ledger_RejectsInvalidCurrencyLength()
        {
            Assert.Throws<System.ArgumentException>(
                () => new EconomyLedgerSaveRecord(
                    "entry",
                    "CheckoutRevenue",
                    "source",
                    "day",
                    100,
                    "EU"));
        }

        [Test] public void Display_CanBeUnassigned()
        {
            var record =
                new DisplaySaveRecord(
                    "display",
                    "definition",
                    string.Empty,
                    "inventory");

            Assert.That(
                record.HasAssignedProduct,
                Is.False);
        }

        [Test] public void Ledger_NormalizesCurrency()
        {
            var record =
                new EconomyLedgerSaveRecord(
                    "entry",
                    "CheckoutRevenue",
                    "source",
                    "day",
                    100,
                    "eur");

            Assert.That(
                record.CurrencyCode,
                Is.EqualTo("EUR"));
        }
    }
}
