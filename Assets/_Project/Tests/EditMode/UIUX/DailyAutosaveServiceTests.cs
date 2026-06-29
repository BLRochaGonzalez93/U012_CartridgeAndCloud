using System;
using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class DailyAutosaveServiceTests
    {
        private static void WithService(
            bool closed,
            Action<
                DailyAutosaveService,
                JsonIntegratedSaveRepository,
                JsonAutosaveMarkerRepository,
                ActiveGameSessionService> action)
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                JsonIntegratedSaveRepository saves =
                    new JsonIntegratedSaveRepository(
                        Path.Combine(root, "Saves"));
                JsonAutosaveMarkerRepository markers =
                    new JsonAutosaveMarkerRepository(
                        Path.Combine(root, "Autosave"));
                ActiveGameSessionService active =
                    new ActiveGameSessionService();

                var snapshot = closed
                    ? UIUXTestFactory.ClosedSnapshot()
                    : UIUXTestFactory.EmptySnapshot();

                active.Activate(
                    snapshot.SlotId,
                    snapshot);

                saves.Save(snapshot);

                DailyAutosaveService service =
                    new DailyAutosaveService(
                        saves,
                        markers,
                        active);

                action(
                    service,
                    saves,
                    markers,
                    active);
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void OpenDay_IsNotSaved()
        {
            WithService(
                false,
                (service, _, __, ___) =>
                    Assert.That(
                        service.TryAutosave().Status,
                        Is.EqualTo(
                            DailyAutosaveStatus
                                .NotClosed)));
        }

        [Test] public void ClosedDay_IsSaved()
        {
            WithService(
                true,
                (service, _, __, ___) =>
                    Assert.That(
                        service.TryAutosave().Status,
                        Is.EqualTo(
                            DailyAutosaveStatus.Saved)));
        }

        [Test] public void ClosedDay_WritesMarker()
        {
            WithService(
                true,
                (service, _, markers, active) =>
                {
                    service.TryAutosave();

                    Assert.That(
                        markers.LoadLastSavedDay(
                            active.ActiveSlotId),
                        Is.EqualTo("day-001"));
                });
        }

        [Test] public void SecondAttempt_IsIdempotent()
        {
            WithService(
                true,
                (service, _, __, ___) =>
                {
                    service.TryAutosave();

                    Assert.That(
                        service.TryAutosave().Status,
                        Is.EqualTo(
                            DailyAutosaveStatus
                                .AlreadySaved));
                });
        }

        [Test] public void Autosave_CreatesBackup()
        {
            WithService(
                true,
                (service, saves, __, active) =>
                {
                    service.TryAutosave();

                    Assert.That(
                        File.Exists(
                            saves.GetBackupPath(
                                active.ActiveSlotId)),
                        Is.True);
                });
        }

        [Test] public void SavedStatus_IsCurrentStatus()
        {
            WithService(
                true,
                (service, _, __, ___) =>
                {
                    service.TryAutosave();

                    Assert.That(
                        service.CurrentStatus,
                        Is.EqualTo(
                            DailyAutosaveStatus.Saved));
                });
        }

        [Test] public void Event_IsRaisedOnce()
        {
            WithService(
                true,
                (service, _, __, ___) =>
                {
                    int count = 0;
                    service.Completed += _ => count++;

                    service.TryAutosave();

                    Assert.That(count, Is.EqualTo(1));
                });
        }

        [Test] public void Event_ContainsDayId()
        {
            WithService(
                true,
                (service, _, __, ___) =>
                {
                    string dayId = null;
                    service.Completed +=
                        result => dayId = result.DayId;

                    service.TryAutosave();

                    Assert.That(
                        dayId,
                        Is.EqualTo("day-001"));
                });
        }

        [Test] public void NoActiveSession_Fails()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                DailyAutosaveService service =
                    new DailyAutosaveService(
                        new JsonIntegratedSaveRepository(
                            Path.Combine(root, "Saves")),
                        new JsonAutosaveMarkerRepository(
                            Path.Combine(root, "Autosave")),
                        new ActiveGameSessionService());

                Assert.That(
                    service.TryAutosave().Status,
                    Is.EqualTo(
                        DailyAutosaveStatus.Failed));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Marker_IsPerSlot()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                JsonAutosaveMarkerRepository markers =
                    new JsonAutosaveMarkerRepository(root);

                markers.SaveLastSavedDay(
                    new SaveSlotId(0),
                    "day-001");

                Assert.That(
                    markers.LoadLastSavedDay(
                        new SaveSlotId(1)),
                    Is.Empty);
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Result_SucceededForSaved()
        {
            DailyAutosaveResult result =
                new DailyAutosaveResult(
                    DailyAutosaveStatus.Saved,
                    "day",
                    "");

            Assert.That(result.Succeeded, Is.True);
        }

        [Test] public void Result_FailedIsNotSucceeded()
        {
            DailyAutosaveResult result =
                new DailyAutosaveResult(
                    DailyAutosaveStatus.Failed,
                    "day",
                    "");

            Assert.That(result.Succeeded, Is.False);
        }
    }
}
