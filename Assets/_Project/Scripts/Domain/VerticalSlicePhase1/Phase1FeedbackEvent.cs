using System;

namespace VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1
{
    public sealed class Phase1FeedbackEvent
    {
        public Phase1FeedbackKind Kind { get; }
        public string Message { get; }
        public string AnchorId { get; }
        public long MinorUnits { get; }
        public string CurrencyCode { get; }

        public bool HasMoney =>
            MinorUnits != 0 &&
            !string.IsNullOrWhiteSpace(
                CurrencyCode);

        public Phase1FeedbackEvent(
            Phase1FeedbackKind kind,
            string message,
            string anchorId = "",
            long minorUnits = 0,
            string currencyCode = "")
        {
            if (!Enum.IsDefined(
                    typeof(Phase1FeedbackKind),
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
}
