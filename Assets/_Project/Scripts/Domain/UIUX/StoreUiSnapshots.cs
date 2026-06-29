using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.UIUX
{
    public sealed class StoreHudSnapshot
    {
        public int CurrentDay { get; }
        public string StoreState { get; }
        public int ElapsedSeconds { get; }
        public int DurationSeconds { get; }
        public long CashCents { get; }
        public string CurrencyCode { get; }
        public int ActiveCustomers { get; }
        public int QueueLength { get; }
        public string CheckoutState { get; }
        public string SaveStatus { get; }

        public StoreHudSnapshot(
            int currentDay,
            string storeState,
            int elapsedSeconds,
            int durationSeconds,
            long cashCents,
            string currencyCode,
            int activeCustomers,
            int queueLength,
            string checkoutState,
            string saveStatus)
        {
            if (currentDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentDay));
            }

            if (elapsedSeconds < 0 ||
                durationSeconds < 1 ||
                elapsedSeconds > durationSeconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedSeconds));
            }

            if (activeCustomers < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(activeCustomers));
            }

            if (queueLength < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(queueLength));
            }

            CurrentDay = currentDay;
            StoreState = Required(
                storeState,
                nameof(storeState));
            ElapsedSeconds = elapsedSeconds;
            DurationSeconds = durationSeconds;
            CashCents = cashCents;
            CurrencyCode = Required(
                currencyCode,
                nameof(currencyCode));
            ActiveCustomers = activeCustomers;
            QueueLength = queueLength;
            CheckoutState = Required(
                checkoutState,
                nameof(checkoutState));
            SaveStatus = Required(
                saveStatus,
                nameof(saveStatus));
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class ManagementPanelRow
    {
        public string Label { get; }
        public string Value { get; }
        public string StatusText { get; }

        public ManagementPanelRow(
            string label,
            string value,
            string statusText)
        {
            Label = Required(label, nameof(label));
            Value = value ?? string.Empty;
            StatusText = statusText ?? string.Empty;
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class ManagementPanelSnapshot
    {
        private readonly ReadOnlyCollection<
            ManagementPanelRow> _rows;

        public ManagementPanelId PanelId { get; }
        public string Title { get; }

        public IReadOnlyList<ManagementPanelRow>
            Rows => _rows;

        public ManagementPanelSnapshot(
            ManagementPanelId panelId,
            string title,
            IEnumerable<ManagementPanelRow> rows)
        {
            if (panelId == ManagementPanelId.None)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(panelId));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "A title is required.",
                    nameof(title));
            }

            if (rows == null)
            {
                throw new ArgumentNullException(
                    nameof(rows));
            }

            List<ManagementPanelRow> copy =
                new List<ManagementPanelRow>();

            foreach (ManagementPanelRow row in rows)
            {
                if (row == null)
                {
                    throw new ArgumentException(
                        "Rows cannot contain null.",
                        nameof(rows));
                }

                copy.Add(row);
            }

            PanelId = panelId;
            Title = title;
            _rows =
                new ReadOnlyCollection<
                    ManagementPanelRow>(copy);
        }
    }
}
