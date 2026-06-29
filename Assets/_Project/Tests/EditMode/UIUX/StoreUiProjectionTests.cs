using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class StoreUiProjectionTests
    {
        private readonly StoreUiProjectionService
            _service =
                new StoreUiProjectionService();

        [Test] public void Hud_UsesCurrentDay()
        {
            StoreHudSnapshot hud =
                _service.BuildHud(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    DailyAutosaveStatus
                        .NotClosed);

            Assert.That(
                hud.CurrentDay,
                Is.EqualTo(2));
        }

        [Test] public void Hud_CountsActiveCustomers()
        {
            StoreHudSnapshot hud =
                _service.BuildHud(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    DailyAutosaveStatus
                        .NotClosed);

            Assert.That(
                hud.ActiveCustomers,
                Is.EqualTo(1));
        }

        [Test] public void Hud_CountsQueueEntries()
        {
            StoreHudSnapshot hud =
                _service.BuildHud(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    DailyAutosaveStatus
                        .NotClosed);

            Assert.That(
                hud.QueueLength,
                Is.EqualTo(1));
        }

        [Test] public void Hud_UsesStationState()
        {
            StoreHudSnapshot hud =
                _service.BuildHud(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    DailyAutosaveStatus
                        .NotClosed);

            Assert.That(
                hud.CheckoutState,
                Is.EqualTo("Busy"));
        }

        [Test] public void Hud_UsesAutosaveStatus()
        {
            StoreHudSnapshot hud =
                _service.BuildHud(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    DailyAutosaveStatus.Saved);

            Assert.That(
                hud.SaveStatus,
                Is.EqualTo("Saved"));
        }

        [Test] public void InventoryPanel_HasContainers()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Inventory);

            Assert.That(
                panel.Rows.Count,
                Is.GreaterThanOrEqualTo(4));
        }

        [Test] public void SupplierPanel_HasOrder()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Suppliers);

            Assert.That(
                panel.Rows[0].Label,
                Is.EqualTo("order-a"));
        }

        [Test] public void DisplayPanel_HasDisplay()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Displays);

            Assert.That(
                panel.Rows[0].Value,
                Is.EqualTo("product-a"));
        }

        [Test] public void CustomerPanel_HasCustomer()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Customers);

            Assert.That(
                panel.Rows[0].Label,
                Is.EqualTo("customer-a"));
        }

        [Test] public void ShoppingPanel_HasSessionAndReservation()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Shopping);

            Assert.That(
                panel.Rows.Count,
                Is.EqualTo(2));
        }

        [Test] public void CheckoutPanel_HasStationQueueAndTransactions()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Checkout);

            Assert.That(
                panel.Rows.Count,
                Is.EqualTo(3));
        }

        [Test] public void DayCyclePanel_HasThreeRows()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.DayCycle);

            Assert.That(
                panel.Rows.Count,
                Is.EqualTo(3));
        }

        [Test] public void EconomyPanel_CalculatesGross()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .PopulatedSnapshot(),
                    ManagementPanelId.Economy);

            Assert.That(
                panel.Rows[3].Value,
                Is.EqualTo("17.99 EUR"));
        }

        [Test] public void HelpPanel_IsNotEmpty()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .EmptySnapshot(),
                    ManagementPanelId.Help);

            Assert.That(
                panel.Rows.Count,
                Is.GreaterThan(0));
        }

        [Test] public void AccessibilityPanel_IsNotEmpty()
        {
            ManagementPanelSnapshot panel =
                _service.BuildPanel(
                    UIUXTestFactory
                        .EmptySnapshot(),
                    ManagementPanelId
                        .Accessibility);

            Assert.That(
                panel.Rows.Count,
                Is.GreaterThan(0));
        }

        [Test] public void InvalidPanel_IsRejected() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => _service.BuildPanel(
                    UIUXTestFactory
                        .EmptySnapshot(),
                    ManagementPanelId.None));

        [Test] public void FormatMoney_FormatsNegative()
        {
            Assert.That(
                StoreUiProjectionService
                    .FormatMoney(
                        -2398,
                        "EUR"),
                Is.EqualTo("-23.98 EUR"));
        }

        [Test] public void FormatMoney_FormatsLongMinValue()
        {
            string value =
                StoreUiProjectionService
                    .FormatMoney(
                        long.MinValue,
                        "EUR");

            Assert.That(
                value,
                Does.StartWith("-"));
            Assert.That(
                value,
                Does.EndWith(" EUR"));
        }
    }
}
