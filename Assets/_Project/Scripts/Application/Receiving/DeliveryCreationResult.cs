using VRMGames.CartridgeAndCloud.Domain.Receiving;

namespace VRMGames.CartridgeAndCloud.Application.Receiving
{
    public sealed class DeliveryCreationResult
    {
        public bool Succeeded { get; }

        public DeliveryCreationFailureReason FailureReason { get; }

        public Delivery Delivery { get; }

        private DeliveryCreationResult(
            bool succeeded,
            DeliveryCreationFailureReason failureReason,
            Delivery delivery)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Delivery = delivery;
        }

        public static DeliveryCreationResult Success(Delivery delivery)
        {
            return new DeliveryCreationResult(
                true,
                DeliveryCreationFailureReason.None,
                delivery);
        }

        public static DeliveryCreationResult Failure(
            DeliveryCreationFailureReason failureReason)
        {
            return new DeliveryCreationResult(
                false,
                failureReason,
                null);
        }
    }
}
