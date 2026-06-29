using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomyLedgerTests
    {
        private static EconomyLedgerEntry Entry(
            string id = "entry-a",
            EconomyPostingType type =
                EconomyPostingType.CheckoutRevenue,
            string sourceId = "source-a",
            string dayId = "day-a",
            long cents = 1000,
            CurrencyCode? currency = null)
        {
            return new EconomyLedgerEntry(
                new EconomyLedgerEntryId(id),
                new EconomyPostingKey(type, sourceId),
                new StoreDayId(dayId),
                new Money(
                    cents,
                    currency ??
                    EconomyTestFactory.Eur));
        }

        [Test] public void EntryId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new EconomyLedgerEntryId(" "));

        [Test] public void PostingKey_RejectsEmptySource() =>
            Assert.Throws<System.ArgumentException>(
                () => new EconomyPostingKey(
                    EconomyPostingType.CheckoutRevenue,
                    ""));

        [Test] public void PostingKey_UsesTypeAndSource()
        {
            Assert.That(
                new EconomyPostingKey(
                    EconomyPostingType.CheckoutRevenue,
                    "x") !=
                new EconomyPostingKey(
                    EconomyPostingType.SupplierReceivingCost,
                    "x"),
                Is.True);
        }

        [Test] public void LedgerEntry_RejectsZeroAmount()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Entry(cents: 0));
        }

        [Test] public void LedgerEntry_RejectsNegativeAmount()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Entry(cents: -1));
        }

        [Test] public void Ledger_StartsEmpty()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Assert.That(ledger.Count, Is.EqualTo(0));
        }

        [Test] public void Ledger_PostsEntry()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            EconomyLedgerPostResult result =
                ledger.TryPost(Entry());
            Assert.That(result.Succeeded, Is.True);
            Assert.That(ledger.Count, Is.EqualTo(1));
        }

        [Test] public void Ledger_RejectsDuplicateEntryId()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry());
            EconomyLedgerPostResult result =
                ledger.TryPost(
                    Entry(
                        sourceId: "source-b"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomyLedgerPostFailureReason
                        .DuplicateEntryId));
        }

        [Test] public void Ledger_RejectsDuplicatePosting()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry());
            EconomyLedgerPostResult result =
                ledger.TryPost(
                    Entry(
                        id: "entry-b"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomyLedgerPostFailureReason
                        .DuplicatePosting));
        }

        [Test] public void Ledger_RejectsCurrencyMismatch()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            EconomyLedgerPostResult result =
                ledger.TryPost(
                    Entry(
                        currency:
                            new CurrencyCode("USD")));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomyLedgerPostFailureReason
                        .CurrencyMismatch));
        }

        [Test] public void Ledger_ContainsPosting()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            EconomyLedgerEntry entry = Entry();
            ledger.TryPost(entry);
            Assert.That(
                ledger.ContainsPosting(
                    entry.PostingKey),
                Is.True);
        }

        [Test] public void Ledger_SumsRevenueForDay()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry(cents: 1000));
            ledger.TryPost(
                Entry(
                    id: "entry-b",
                    sourceId: "source-b",
                    cents: 2500));

            Assert.That(
                ledger.GetTotal(
                    new StoreDayId("day-a"),
                    EconomyPostingType.CheckoutRevenue)
                    .MinorUnits,
                Is.EqualTo(3500));
        }

        [Test] public void Ledger_SeparatesPostingTypes()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry(cents: 1000));
            ledger.TryPost(
                Entry(
                    id: "entry-b",
                    type:
                        EconomyPostingType
                            .SupplierReceivingCost,
                    sourceId: "receipt",
                    cents: 600));

            Assert.That(
                ledger.GetTotal(
                    new StoreDayId("day-a"),
                    EconomyPostingType.CheckoutRevenue)
                    .MinorUnits,
                Is.EqualTo(1000));
            Assert.That(
                ledger.GetTotal(
                    new StoreDayId("day-a"),
                    EconomyPostingType
                        .SupplierReceivingCost)
                    .MinorUnits,
                Is.EqualTo(600));
        }

        [Test] public void Ledger_SeparatesDays()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry(cents: 1000));
            ledger.TryPost(
                Entry(
                    id: "entry-b",
                    sourceId: "source-b",
                    dayId: "day-b",
                    cents: 2500));

            Assert.That(
                ledger.GetTotal(
                    new StoreDayId("day-a"),
                    EconomyPostingType.CheckoutRevenue)
                    .MinorUnits,
                Is.EqualTo(1000));
        }

        [Test] public void Ledger_CountsPostings()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(Entry());
            ledger.TryPost(
                Entry(
                    id: "entry-b",
                    sourceId: "source-b"));

            Assert.That(
                ledger.GetPostingCount(
                    new StoreDayId("day-a"),
                    EconomyPostingType.CheckoutRevenue),
                Is.EqualTo(2));
        }

        [Test] public void Ledger_ReturnsZeroForMissingTotals()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Money total =
                ledger.GetTotal(
                    new StoreDayId("missing"),
                    EconomyPostingType.CheckoutRevenue);

            Assert.That(total.IsZero, Is.True);
            Assert.That(
                total.Currency,
                Is.EqualTo(EconomyTestFactory.Eur));
        }

        [Test] public void Ledger_OrdersEntriesById()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            ledger.TryPost(
                Entry(
                    id: "z-entry",
                    sourceId: "z-source"));
            ledger.TryPost(
                Entry(
                    id: "a-entry",
                    sourceId: "a-source"));

            Assert.That(
                ledger.Entries[0].Id.Value,
                Is.EqualTo("a-entry"));
        }

        [Test] public void Ledger_SnapshotReflectsNewEntries()
        {
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            var initial = ledger.Entries;
            ledger.TryPost(Entry());

            Assert.That(initial.Count, Is.EqualTo(0));
            Assert.That(
                ledger.Entries.Count,
                Is.EqualTo(1));
        }
    }
}
