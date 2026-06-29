using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class SlotSelectionServiceTests
    {
        private static void WithService(
            Action<
                SlotSelectionService,
                JsonIntegratedSaveRepository,
                ActiveGameSessionService,
                JsonTutorialProgressRepository,
                JsonAutosaveMarkerRepository> action)
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                JsonIntegratedSaveRepository saves =
                    new JsonIntegratedSaveRepository(
                        System.IO.Path.Combine(
                            root,
                            "Saves"));
                JsonTutorialProgressRepository tutorial =
                    new JsonTutorialProgressRepository(
                        System.IO.Path.Combine(
                            root,
                            "Tutorial"));
                JsonAutosaveMarkerRepository markers =
                    new JsonAutosaveMarkerRepository(
                        System.IO.Path.Combine(
                            root,
                            "Autosave"));
                ActiveGameSessionService active =
                    new ActiveGameSessionService();

                SlotSelectionService service =
                    new SlotSelectionService(
                        saves,
                        tutorial,
                        markers,
                        active,
                        new DefaultIntegratedGameStateFactory(
                            "EUR",
                            100000,
                            300),
                        new UIUXTestFactory.FixedClock(
                            UIUXTestFactory.Utc()));

                action(
                    service,
                    saves,
                    active,
                    tutorial,
                    markers);
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void InspectAll_ReturnsThreeSlots()
        {
            WithService(
                (service, _, __, ___, ____) =>
                    Assert.That(
                        service.InspectAll().Count,
                        Is.EqualTo(3)));
        }

        [Test] public void Inspect_EmptySlotIsEmpty()
        {
            WithService(
                (service, _, __, ___, ____) =>
                    Assert.That(
                        service.Inspect(
                            UIUXTestFactory.Slot())
                            .State,
                        Is.EqualTo(
                            SlotPresentationState.Empty)));
        }

        [Test] public void CreateNew_Succeeds()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    SlotOperationResult result =
                        service.CreateNew(
                            UIUXTestFactory.Slot(),
                            false);

                    Assert.That(
                        result.Succeeded,
                        Is.True);
                });
        }

        [Test] public void CreateNew_ActivatesSession()
        {
            WithService(
                (service, _, active, ___, ____) =>
                {
                    service.CreateNew(
                        UIUXTestFactory.Slot(),
                        false);

                    Assert.That(
                        active.HasActiveSession,
                        Is.True);
                    Assert.That(
                        active.ActiveSlotId,
                        Is.EqualTo(
                            UIUXTestFactory.Slot()));
                });
        }

        [Test] public void CreateNew_PersistsSlot()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    service.CreateNew(
                        UIUXTestFactory.Slot(),
                        false);

                    Assert.That(
                        service.Inspect(
                            UIUXTestFactory.Slot())
                            .State,
                        Is.EqualTo(
                            SlotPresentationState.Ready));
                });
        }

        [Test] public void CreateNew_ExistingRequiresConfirmation()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);

                    SlotOperationResult second =
                        service.CreateNew(slot, false);

                    Assert.That(
                        second.Status,
                        Is.EqualTo(
                            SlotOperationStatus
                                .ConfirmationRequired));
                });
        }

        [Test] public void CreateNew_ConfirmedReplacesSlot()
        {
            WithService(
                (service, _, active, ___, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);
                    string firstSession =
                        active.Snapshot.SessionId.Value;

                    SlotOperationResult second =
                        service.CreateNew(slot, true);

                    Assert.That(
                        second.Succeeded,
                        Is.True);
                    Assert.That(
                        active.Snapshot.SessionId.Value,
                        Is.Not.EqualTo(firstSession));
                });
        }

        [Test] public void Continue_EmptyReturnsSlotEmpty()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    SlotOperationResult result =
                        service.Continue(
                            UIUXTestFactory.Slot());

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            SlotOperationStatus.SlotEmpty));
                });
        }

        [Test] public void Continue_LoadsExistingSlot()
        {
            WithService(
                (service, _, active, ___, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);
                    active.Clear();

                    SlotOperationResult result =
                        service.Continue(slot);

                    Assert.That(
                        result.Succeeded,
                        Is.True);
                    Assert.That(
                        active.HasActiveSession,
                        Is.True);
                });
        }

        [Test] public void Slots_AreIndependent()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    service.CreateNew(
                        UIUXTestFactory.Slot(0),
                        false);
                    service.CreateNew(
                        UIUXTestFactory.Slot(1),
                        false);

                    Assert.That(
                        service.Inspect(
                            UIUXTestFactory.Slot(0))
                            .State,
                        Is.EqualTo(
                            SlotPresentationState.Ready));
                    Assert.That(
                        service.Inspect(
                            UIUXTestFactory.Slot(1))
                            .State,
                        Is.EqualTo(
                            SlotPresentationState.Ready));
                    Assert.That(
                        service.Inspect(
                            UIUXTestFactory.Slot(2))
                            .State,
                        Is.EqualTo(
                            SlotPresentationState.Empty));
                });
        }

        [Test] public void Delete_RequiresConfirmation()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    service.CreateNew(
                        UIUXTestFactory.Slot(),
                        false);

                    SlotOperationResult result =
                        service.Delete(
                            UIUXTestFactory.Slot(),
                            false);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            SlotOperationStatus
                                .ConfirmationRequired));
                });
        }

        [Test] public void Delete_RemovesSave()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);

                    SlotOperationResult result =
                        service.Delete(slot, true);

                    Assert.That(
                        result.Succeeded,
                        Is.True);
                    Assert.That(
                        service.Inspect(slot).State,
                        Is.EqualTo(
                            SlotPresentationState.Empty));
                });
        }

        [Test] public void Delete_ClearsActiveSession()
        {
            WithService(
                (service, _, active, ___, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);
                    service.Delete(slot, true);

                    Assert.That(
                        active.HasActiveSession,
                        Is.False);
                });
        }

        [Test] public void Delete_RemovesTutorialProgress()
        {
            WithService(
                (service, _, __, tutorial, ____) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);
                    tutorial.Save(
                        slot,
                        TutorialProgress.New()
                            .Start()
                            .Advance());

                    service.Delete(slot, true);

                    Assert.That(
                        tutorial.Load(slot).State,
                        Is.EqualTo(
                            TutorialProgressState
                                .NotStarted));
                });
        }

        [Test] public void Delete_RemovesAutosaveMarker()
        {
            WithService(
                (service, _, __, ___, markers) =>
                {
                    SaveSlotId slot =
                        UIUXTestFactory.Slot();
                    service.CreateNew(slot, false);
                    markers.SaveLastSavedDay(
                        slot,
                        "day-001");

                    service.Delete(slot, true);

                    Assert.That(
                        markers.LoadLastSavedDay(
                            slot),
                        Is.Empty);
                });
        }

        [Test] public void Delete_EmptyReturnsSlotEmpty()
        {
            WithService(
                (service, _, __, ___, ____) =>
                {
                    SlotOperationResult result =
                        service.Delete(
                            UIUXTestFactory.Slot(),
                            true);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            SlotOperationStatus.SlotEmpty));
                });
        }

        [Test] public void Factory_CreatesBeforeOpenDay()
        {
            DefaultIntegratedGameStateFactory factory =
                new DefaultIntegratedGameStateFactory(
                    "EUR",
                    100000,
                    300);

            Assert.That(
                factory.Create(
                    UIUXTestFactory.Slot(),
                    UIUXTestFactory.Utc())
                    .DayCycle.State,
                Is.EqualTo("BeforeOpen"));
        }

        [Test] public void Factory_RejectsInvalidCurrency() =>
            Assert.Throws<ArgumentException>(
                () =>
                    new DefaultIntegratedGameStateFactory(
                        "EU",
                        0,
                        300));
    }
}
