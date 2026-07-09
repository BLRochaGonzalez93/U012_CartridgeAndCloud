using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

using VRMGames.CartridgeAndCloud.Runtime.Composition;
using VRMGames.CartridgeAndCloud.Runtime.UIUX;
namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.UIUX
{
    public sealed class UIUXRuntimePlayModeTests
    {
        [UnityTest]
        public IEnumerator RuntimeRoot_IsInstalled()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot.Instance,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSettings()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Settings,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAccessibility()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Accessibility,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasTutorialService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Tutorial,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSlotService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Slots,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAutosaveService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Autosave,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasManualSaveServices()
        {
            yield return null;

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;

            Assert.That(root.ManualSave, Is.Not.Null);
            Assert.That(root.SaveMutations, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasProjectionService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Projection,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasActiveSessionService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession,
                Is.Not.Null);
        }


        [UnityTest]
        public IEnumerator RuntimeRoot_HasSimulationClockAndPauseService()
        {
            yield return null;

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;

            Assert.That(
                root.SimulationClock,
                Is.Not.Null);
            Assert.That(
                root.PauseService,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeBridge_DetectsRuntime()
        {
            yield return null;

            GameObject gameObject =
                new GameObject("RuntimeBridgeTest");

            try
            {
                StoreUiStateBridge bridge =
                    gameObject.AddComponent<
                        StoreUiStateBridge>();

                Assert.That(
                    bridge.HasRuntime,
                    Is.True);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [UnityTest]
        public IEnumerator InputGate_EntersAndExits()
        {
            yield return null;

            UiInputContextGate gate =
                new UiInputContextGate();

            gate.EnterUiExclusive();
            Assert.That(
                gate.IsUiExclusive,
                Is.True);

            gate.ExitUiExclusive();
            Assert.That(
                gate.IsUiExclusive,
                Is.False);
        }

        [UnityTest]
        public IEnumerator WeekEnd_BlocksAdvanceUntilSummaryIsAccepted()
        {
            yield return null;

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;
            SaveSlotId slot = new SaveSlotId(0);
            FixedManagementStateProvider provider =
                new FixedManagementStateProvider(
                    SettledWeekState(slot));
            IStoreManagementStateProvider previousProvider =
                root.StoreManagementStateProvider;
            bool hadSession =
                root.ActiveSession.HasActiveSession;
            SaveSlotId previousSlot =
                root.ActiveSession.ActiveSlotId;
            IntegratedGameStateSnapshot previousSnapshot =
                root.ActiveSession.Snapshot;

            root.ActiveSession.Clear();
            root.RegisterStoreManagementStateProvider(
                provider);

            try
            {
                root.ActiveSession.Activate(
                    slot,
                    ClosedWeekSnapshot(slot));

                Assert.That(
                    root.RequiresWeeklySummaryAcknowledgement,
                    Is.True);
                Assert.Throws<InvalidOperationException>(
                    () => root.BeginNextDay());
                Assert.That(
                    root.TryBuildPendingWeeklySummary(
                        out WeeklySummarySnapshot summary),
                    Is.True);
                Assert.That(summary.TaxCents, Is.EqualTo(700));

                root.AcknowledgeWeeklySummaryAndBeginNextDay();

                Assert.That(
                    root.ActiveSession.Snapshot.CurrentDay,
                    Is.EqualTo(8));
                Assert.That(
                    root.ActiveSession.Snapshot.DayCycle.State,
                    Is.EqualTo("BeforeOpen"));
            }
            finally
            {
                root.UnregisterStoreManagementStateProvider(
                    provider);

                if (previousProvider != null)
                {
                    root.RegisterStoreManagementStateProvider(
                        previousProvider);
                }

                root.ActiveSession.Clear();
                if (hadSession)
                {
                    root.ActiveSession.Activate(
                        previousSlot,
                        previousSnapshot);
                }
            }
        }

        [UnityTest]
        public IEnumerator AutosaveWithoutSession_FailsSafely()
        {
            yield return null;

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;
            root.ActiveSession.Clear();

            DailyAutosaveResult result =
                root.Autosave.TryAutosave();

            Assert.That(
                result.Status,
                Is.EqualTo(
                    DailyAutosaveStatus.Failed));
        }
        private static IntegratedGameStateSnapshot
            ClosedWeekSnapshot(
                SaveSlotId slot)
        {
            DateTime created = new DateTime(
                2026,
                7,
                9,
                12,
                0,
                0,
                DateTimeKind.Utc);

            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                StableId.Parse(
                    "88888888888888888888888888888888"),
                slot,
                created,
                created.AddMinutes(1),
                7,
                106300,
                "EUR",
                new[]
                {
                    new InventoryContainerSaveRecord(
                        "store-inventory",
                        100,
                        new ProductQuantitySaveRecord[0]),
                    new InventoryContainerSaveRecord(
                        "backroom-inventory",
                        200,
                        new ProductQuantitySaveRecord[0])
                },
                new SupplierOrderSaveRecord[0],
                new DisplaySaveRecord[0],
                new CustomerSaveRecord[0],
                new ShoppingSessionSaveRecord[0],
                new ReservationSaveRecord[0],
                new CheckoutQueueEntrySaveRecord[0],
                new CheckoutStationSaveRecord(
                    "station",
                    "Closed",
                    string.Empty),
                new CheckoutTransactionSaveRecord[0],
                new DayCycleSaveRecord(
                    "day-007",
                    "Closed",
                    300,
                    300,
                    true),
                new EconomyLedgerSaveRecord[0]);
        }

        private static StoreOperationsState
            SettledWeekState(
                SaveSlotId slot)
        {
            StoreDailySummaryRecord[] days =
                new StoreDailySummaryRecord[7];

            for (int index = 0;
                 index < days.Length;
                 index++)
            {
                int day = index + 1;
                days[index] =
                    new StoreDailySummaryRecord(
                        day,
                        "day-" + day.ToString("000"),
                        100000 + index * 1000,
                        101000 + index * 1000,
                        2,
                        1,
                        1,
                        1,
                        1,
                        1,
                        2000,
                        1000,
                        day == 7 ? 700 : 0);
            }

            return new StoreOperationsState(
                slot,
                "weekly-session",
                1,
                1,
                1,
                1,
                1,
                7,
                14000,
                7700,
                700,
                0,
                0,
                1,
                7000,
                700,
                new StoreOrderRecord[0],
                new StoreDeliveryRunRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0],
                new StoreManagementHistory(
                    new StoreManagementDayDetailRecord[0],
                    days));
        }

        private sealed class FixedManagementStateProvider :
            IStoreManagementStateProvider
        {
            public StoreOperationsState ManagementState {
                get;
            }

            public FixedManagementStateProvider(
                StoreOperationsState state)
            {
                ManagementState = state;
            }
        }

    }
}
