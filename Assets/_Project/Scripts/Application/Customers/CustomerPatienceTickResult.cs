namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public sealed class CustomerPatienceTickResult
    {
        public int InspectedCustomers { get; }

        public int AdvancedCustomers { get; }

        public int NewlyLeavingCustomers { get; }

        public CustomerPatienceTickResult(
            int inspectedCustomers,
            int advancedCustomers,
            int newlyLeavingCustomers)
        {
            InspectedCustomers = inspectedCustomers;
            AdvancedCustomers = advancedCustomers;
            NewlyLeavingCustomers = newlyLeavingCustomers;
        }
    }
}
