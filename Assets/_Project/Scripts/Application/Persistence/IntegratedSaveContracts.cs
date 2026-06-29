using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public enum IntegratedSaveRepositoryStatus
    {
        Success = 0,
        SlotEmpty = 1,
        RecoveredFromBackup = 2,
        ValidationFailure = 3,
        UnsupportedSchema = 4,
        CorruptPrimaryNoBackup = 5,
        StorageFailure = 6
    }

    public sealed class IntegratedSaveRepositoryResult
    {
        public IntegratedSaveRepositoryStatus Status { get; }

        public string Detail { get; }

        public bool Succeeded =>
            Status == IntegratedSaveRepositoryStatus.Success ||
            Status ==
                IntegratedSaveRepositoryStatus
                    .RecoveredFromBackup;

        public bool RecoveredFromBackup =>
            Status ==
            IntegratedSaveRepositoryStatus
                .RecoveredFromBackup;

        public IntegratedSaveRepositoryResult(
            IntegratedSaveRepositoryStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static IntegratedSaveRepositoryResult Success()
        {
            return new IntegratedSaveRepositoryResult(
                IntegratedSaveRepositoryStatus.Success,
                string.Empty);
        }

        public static IntegratedSaveRepositoryResult Failure(
            IntegratedSaveRepositoryStatus status,
            string detail)
        {
            if (status ==
                    IntegratedSaveRepositoryStatus.Success ||
                status ==
                    IntegratedSaveRepositoryStatus
                        .RecoveredFromBackup)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(status));
            }

            return new IntegratedSaveRepositoryResult(
                status,
                detail);
        }

        public static IntegratedSaveRepositoryResult Recovered(
            string detail)
        {
            return new IntegratedSaveRepositoryResult(
                IntegratedSaveRepositoryStatus
                    .RecoveredFromBackup,
                detail);
        }
    }

    public interface IIntegratedSaveRepository
    {
        bool Exists(SaveSlotId slotId);

        IntegratedSaveRepositoryResult Save(
            IntegratedGameStateSnapshot snapshot);

        IntegratedSaveRepositoryResult Load(
            SaveSlotId slotId,
            out IntegratedGameStateSnapshot snapshot);

        bool Delete(SaveSlotId slotId);
    }

    public interface IIntegratedGameStateCaptureSource
    {
        IntegratedGameStateSnapshot Capture(
            SaveSlotId slotId,
            DateTime updatedUtc);
    }

    public interface IIntegratedGameStateRestoreTarget
    {
        bool CanRestore(
            IntegratedGameStateSnapshot snapshot,
            out string reason);

        void Restore(
            IntegratedGameStateSnapshot snapshot);
    }
}
