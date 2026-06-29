using System;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public enum IntegratedSaveOperationStatus
    {
        Success = 0,
        RecoveredFromBackup = 1,
        SlotEmpty = 2,
        CaptureFailure = 3,
        RepositoryFailure = 4,
        RestoreRejected = 5,
        RestoreFailure = 6
    }

    public sealed class IntegratedSaveOperationResult
    {
        public IntegratedSaveOperationStatus Status { get; }

        public string Detail { get; }

        public bool Succeeded =>
            Status == IntegratedSaveOperationStatus.Success ||
            Status ==
                IntegratedSaveOperationStatus
                    .RecoveredFromBackup;

        public bool RecoveredFromBackup =>
            Status ==
            IntegratedSaveOperationStatus
                .RecoveredFromBackup;

        public IntegratedSaveOperationResult(
            IntegratedSaveOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }
    }

    public sealed class IntegratedSaveService
    {
        private readonly IIntegratedSaveRepository _repository;
        private readonly IUtcClock _clock;

        public IntegratedSaveService(
            IIntegratedSaveRepository repository,
            IUtcClock clock)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _clock = clock ??
                throw new ArgumentNullException(nameof(clock));
        }

        public IntegratedSaveOperationResult Save(
            SaveSlotId slotId,
            IIntegratedGameStateCaptureSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            IntegratedGameStateSnapshot snapshot;

            try
            {
                snapshot = source.Capture(
                    slotId,
                    _clock.UtcNow);
            }
            catch (Exception exception)
            {
                return new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus
                        .CaptureFailure,
                    exception.Message);
            }

            if (snapshot == null ||
                snapshot.SlotId != slotId)
            {
                return new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus
                        .CaptureFailure,
                    "Capture source returned an invalid slot.");
            }

            IntegratedSaveRepositoryResult result =
                _repository.Save(snapshot);

            return result.Succeeded
                ? new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus.Success,
                    result.Detail)
                : new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus
                        .RepositoryFailure,
                    result.Detail);
        }

        public IntegratedSaveOperationResult Load(
            SaveSlotId slotId,
            IIntegratedGameStateRestoreTarget target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            IntegratedSaveRepositoryResult result =
                _repository.Load(
                    slotId,
                    out IntegratedGameStateSnapshot snapshot);

            if (!result.Succeeded)
            {
                return new IntegratedSaveOperationResult(
                    result.Status ==
                        IntegratedSaveRepositoryStatus
                            .SlotEmpty
                        ? IntegratedSaveOperationStatus
                            .SlotEmpty
                        : IntegratedSaveOperationStatus
                            .RepositoryFailure,
                    result.Detail);
            }

            bool accepted;

            try
            {
                accepted = target.CanRestore(
                    snapshot,
                    out string rejectionReason);

                if (!accepted)
                {
                    return new IntegratedSaveOperationResult(
                        IntegratedSaveOperationStatus
                            .RestoreRejected,
                        rejectionReason);
                }
            }
            catch (Exception exception)
            {
                return new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus
                        .RestoreRejected,
                    exception.Message);
            }

            try
            {
                target.Restore(snapshot);
            }
            catch (Exception exception)
            {
                return new IntegratedSaveOperationResult(
                    IntegratedSaveOperationStatus
                        .RestoreFailure,
                    exception.Message);
            }

            return new IntegratedSaveOperationResult(
                result.RecoveredFromBackup
                    ? IntegratedSaveOperationStatus
                        .RecoveredFromBackup
                    : IntegratedSaveOperationStatus.Success,
                result.Detail);
        }

        public bool SlotExists(SaveSlotId slotId)
        {
            try
            {
                return _repository.Exists(slotId);
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(SaveSlotId slotId)
        {
            try
            {
                return _repository.Delete(slotId);
            }
            catch
            {
                return false;
            }
        }
    }
}
