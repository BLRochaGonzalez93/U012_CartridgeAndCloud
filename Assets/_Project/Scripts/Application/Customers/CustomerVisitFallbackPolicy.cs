namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public static class CustomerVisitFallbackPolicy
    {
        public static bool ShouldBrowseWithoutPurchase(
            bool hasVisibleProduct,
            bool admissionAllowed)
        {
            return admissionAllowed && !hasVisibleProduct;
        }
    }
}
