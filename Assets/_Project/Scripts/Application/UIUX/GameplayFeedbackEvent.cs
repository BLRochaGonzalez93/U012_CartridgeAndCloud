using System;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class GameplayFeedbackEvent
    {
        public GameplayFeedbackType Kind { get; }
        public string Message { get; }
        public string AnchorId { get; }
        public long MinorUnits { get; }
        public string CurrencyCode { get; }

        public bool HasMoney =>
            MinorUnits != 0 &&
            !string.IsNullOrWhiteSpace(
                CurrencyCode);

        public GameplayFeedbackEvent(
            GameplayFeedbackType kind,
            string message,
            string anchorId = "",
            long minorUnits = 0,
            string currencyCode = "")
        {
            if (!Enum.IsDefined(
                    typeof(GameplayFeedbackType),
                    kind))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(kind));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(
                    "A feedback message is required.",
                    nameof(message));
            }

            if (minorUnits != 0 &&
                (string.IsNullOrWhiteSpace(
                     currencyCode) ||
                 currencyCode.Length != 3))
            {
                throw new ArgumentException(
                    "Money feedback requires a three-character currency.",
                    nameof(currencyCode));
            }

            Kind = kind;
            Message = message;
            AnchorId = anchorId ?? string.Empty;
            MinorUnits = minorUnits;
            CurrencyCode =
                currencyCode == null
                    ? string.Empty
                    : currencyCode.ToUpperInvariant();
        }
    }

    public enum GameplayFeedbackType
    {
        PlacementValid = 0,
        PlacementInvalid = 1,
        ObjectSelected = 2,
        ObjectHovered = 3,
        ProductAssigned = 4,
        OutOfStock = 5,
        Reserved = 6,
        Restocked = 7,
        OrderReceived = 8,
        CustomerSatisfied = 9,
        CustomerFrustrated = 10,
        QueueEntered = 11,
        CheckoutCompleted = 12,
        Revenue = 13,
        Expense = 14,
        ClosingWarning = 15,
        DayClosed = 16,
        AutosaveSucceeded = 17,
        AutosaveFailed = 18,
        DoorOpened = 19,
        DoorClosed = 20
    }

    public interface IGameplayFeedbackSink
    {
        void Publish(
            GameplayFeedbackEvent feedbackEvent);
    }
}
