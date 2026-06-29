using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public enum SlotOperationStatus
    {
        Success = 0,
        RecoveredFromBackup = 1,
        SlotEmpty = 2,
        ConfirmationRequired = 3,
        InvalidSlotState = 4,
        StorageFailure = 5
    }

    public sealed class SlotOperationResult
    {
        public SlotOperationStatus Status { get; }
        public string Detail { get; }

        public bool Succeeded =>
            Status == SlotOperationStatus.Success ||
            Status ==
                SlotOperationStatus
                    .RecoveredFromBackup;

        public bool RecoveredFromBackup =>
            Status ==
                SlotOperationStatus
                    .RecoveredFromBackup;

        public SlotOperationResult(
            SlotOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }
    }

    public sealed class SlotSelectionService
    {
        private readonly IIntegratedSaveRepository
            _repository;
        private readonly ITutorialProgressRepository
            _tutorialRepository;
        private readonly IAutosaveMarkerRepository
            _autosaveMarkerRepository;
        private readonly ActiveGameSessionService
            _activeSession;
        private readonly DefaultIntegratedGameStateFactory
            _factory;
        private readonly IUtcClock _clock;

        public SlotSelectionService(
            IIntegratedSaveRepository repository,
            ITutorialProgressRepository tutorialRepository,
            IAutosaveMarkerRepository autosaveMarkerRepository,
            ActiveGameSessionService activeSession,
            DefaultIntegratedGameStateFactory factory,
            IUtcClock clock)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _tutorialRepository =
                tutorialRepository ??
                throw new ArgumentNullException(
                    nameof(tutorialRepository));
            _autosaveMarkerRepository =
                autosaveMarkerRepository ??
                throw new ArgumentNullException(
                    nameof(autosaveMarkerRepository));
            _activeSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
            _factory = factory ??
                throw new ArgumentNullException(
                    nameof(factory));
            _clock = clock ??
                throw new ArgumentNullException(
                    nameof(clock));
        }

        public IReadOnlyList<SlotDescriptor>
            InspectAll()
        {
            List<SlotDescriptor> descriptors =
                new List<SlotDescriptor>(3);

            for (int value =
                     SaveSlotId.MinimumValue;
                 value <=
                     SaveSlotId.MaximumValue;
                 value++)
            {
                descriptors.Add(
                    Inspect(new SaveSlotId(value)));
            }

            return descriptors;
        }

        public SlotDescriptor Inspect(
            SaveSlotId slotId)
        {
            IntegratedSaveRepositoryResult result =
                _repository.Load(
                    slotId,
                    out IntegratedGameStateSnapshot
                        snapshot);

            if (result.Succeeded && snapshot != null)
            {
                return new SlotDescriptor(
                    slotId,
                    result.RecoveredFromBackup
                        ? SlotPresentationState.Recovered
                        : SlotPresentationState.Ready,
                    snapshot.CurrentDay,
                    snapshot.CashCents,
                    snapshot.UpdatedUtc,
                    result.Detail);
            }

            switch (result.Status)
            {
                case IntegratedSaveRepositoryStatus
                    .SlotEmpty:
                    return SlotDescriptor.Empty(slotId);

                case IntegratedSaveRepositoryStatus
                    .UnsupportedSchema:
                    return new SlotDescriptor(
                        slotId,
                        SlotPresentationState
                            .UnsupportedSchema,
                        0,
                        0,
                        null,
                        result.Detail);

                case IntegratedSaveRepositoryStatus
                    .CorruptPrimaryNoBackup:
                case IntegratedSaveRepositoryStatus
                    .ValidationFailure:
                    return new SlotDescriptor(
                        slotId,
                        SlotPresentationState.Corrupt,
                        0,
                        0,
                        null,
                        result.Detail);

                default:
                    return new SlotDescriptor(
                        slotId,
                        SlotPresentationState
                            .StorageFailure,
                        0,
                        0,
                        null,
                        result.Detail);
            }
        }

        public SlotOperationResult CreateNew(
            SaveSlotId slotId,
            bool overwriteConfirmed)
        {
            SlotDescriptor descriptor =
                Inspect(slotId);

            if (descriptor.State !=
                    SlotPresentationState.Empty &&
                !overwriteConfirmed)
            {
                return new SlotOperationResult(
                    SlotOperationStatus
                        .ConfirmationRequired,
                    "Creating a new game will replace " +
                    "the existing slot.");
            }

            if (descriptor.State !=
                SlotPresentationState.Empty)
            {
                _repository.Delete(slotId);
                _tutorialRepository.Delete(slotId);
                _autosaveMarkerRepository.Delete(
                    slotId);
            }

            IntegratedGameStateSnapshot snapshot =
                _factory.Create(
                    slotId,
                    _clock.UtcNow);

            IntegratedSaveRepositoryResult save =
                _repository.Save(snapshot);

            if (!save.Succeeded)
            {
                return new SlotOperationResult(
                    SlotOperationStatus.StorageFailure,
                    save.Detail);
            }

            _activeSession.Activate(
                slotId,
                snapshot);

            return new SlotOperationResult(
                SlotOperationStatus.Success,
                "New game created.");
        }

        public SlotOperationResult Continue(
            SaveSlotId slotId)
        {
            IntegratedSaveRepositoryResult result =
                _repository.Load(
                    slotId,
                    out IntegratedGameStateSnapshot
                        snapshot);

            if (!result.Succeeded ||
                snapshot == null)
            {
                return new SlotOperationResult(
                    result.Status ==
                        IntegratedSaveRepositoryStatus
                            .SlotEmpty
                        ? SlotOperationStatus.SlotEmpty
                        : SlotOperationStatus
                            .StorageFailure,
                    result.Detail);
            }

            _activeSession.Activate(
                slotId,
                snapshot);

            return new SlotOperationResult(
                result.RecoveredFromBackup
                    ? SlotOperationStatus
                        .RecoveredFromBackup
                    : SlotOperationStatus.Success,
                result.Detail);
        }

        public SlotOperationResult Delete(
            SaveSlotId slotId,
            bool confirmed)
        {
            if (!confirmed)
            {
                return new SlotOperationResult(
                    SlotOperationStatus
                        .ConfirmationRequired,
                    "Delete requires confirmation.");
            }

            bool deleted =
                _repository.Delete(slotId);
            _tutorialRepository.Delete(slotId);
            _autosaveMarkerRepository.Delete(slotId);

            if (_activeSession.HasActiveSession &&
                _activeSession.ActiveSlotId == slotId)
            {
                _activeSession.Clear();
            }

            return new SlotOperationResult(
                deleted
                    ? SlotOperationStatus.Success
                    : SlotOperationStatus.SlotEmpty,
                deleted
                    ? "Slot deleted."
                    : "Slot was already empty.");
        }
    }
}
