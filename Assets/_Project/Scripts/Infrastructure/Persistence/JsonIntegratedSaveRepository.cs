using System;
using System.IO;
using System.Text;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Persistence
{
    public sealed class JsonIntegratedSaveRepository :
        IIntegratedSaveRepository
    {
        public const string SaveDirectoryName =
            "IntegratedSaveGames";

        private readonly string _rootDirectory;
        private readonly IntegratedSaveJsonCodec _codec;

        public JsonIntegratedSaveRepository(
            string rootDirectory,
            IntegratedSaveJsonCodec codec = null)
        {
            if (string.IsNullOrWhiteSpace(rootDirectory))
            {
                throw new ArgumentException(
                    "A save root directory is required.",
                    nameof(rootDirectory));
            }

            _rootDirectory = rootDirectory;
            _codec = codec ??
                new IntegratedSaveJsonCodec();
        }

        public bool Exists(SaveSlotId slotId)
        {
            return File.Exists(
                       GetPrimaryPath(slotId)) ||
                File.Exists(
                    GetBackupPath(slotId));
        }

        public IntegratedSaveRepositoryResult Save(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            string primary =
                GetPrimaryPath(snapshot.SlotId);
            string backup =
                GetBackupPath(snapshot.SlotId);
            string temporary =
                GetTemporaryPath(snapshot.SlotId);

            try
            {
                Directory.CreateDirectory(
                    _rootDirectory);

                long generation =
                    checked(
                        GetLatestGeneration(
                            snapshot.SlotId) + 1);
                string json =
                    _codec.Encode(
                        snapshot,
                        generation);

                WriteDurable(
                    temporary,
                    json);

                IntegratedGameStateSnapshot validation =
                    DecodeFile(
                        temporary,
                        snapshot.SlotId,
                        out long validatedGeneration);

                if (validation == null ||
                    validatedGeneration != generation)
                {
                    throw new InvalidDataException(
                        "Temporary save validation failed.");
                }

                CommitTemporary(
                    temporary,
                    primary,
                    backup);

                return IntegratedSaveRepositoryResult
                    .Success();
            }
            catch (IntegratedSaveFormatException exception)
            {
                DeleteIfExists(temporary);
                return IntegratedSaveRepositoryResult.Failure(
                    exception.Status,
                    exception.Message);
            }
            catch (Exception exception)
            {
                DeleteIfExists(temporary);
                return IntegratedSaveRepositoryResult.Failure(
                    IntegratedSaveRepositoryStatus
                        .StorageFailure,
                    exception.Message);
            }
        }

        public IntegratedSaveRepositoryResult Load(
            SaveSlotId slotId,
            out IntegratedGameStateSnapshot snapshot)
        {
            string primary = GetPrimaryPath(slotId);
            string backup = GetBackupPath(slotId);

            snapshot = null;

            if (!File.Exists(primary) &&
                !File.Exists(backup))
            {
                return IntegratedSaveRepositoryResult.Failure(
                    IntegratedSaveRepositoryStatus.SlotEmpty,
                    "No primary or backup save exists.");
            }

            IntegratedSaveFormatException primaryError =
                null;

            if (File.Exists(primary))
            {
                try
                {
                    snapshot = DecodeFile(
                        primary,
                        slotId,
                        out _);
                    return IntegratedSaveRepositoryResult
                        .Success();
                }
                catch (IntegratedSaveFormatException exception)
                {
                    primaryError = exception;
                }
                catch (Exception exception)
                {
                    primaryError =
                        new IntegratedSaveFormatException(
                            IntegratedSaveRepositoryStatus
                                .StorageFailure,
                            exception.Message);
                }
            }

            if (File.Exists(backup))
            {
                try
                {
                    snapshot = DecodeFile(
                        backup,
                        slotId,
                        out _);
                    RecoverPrimary(
                        slotId,
                        backup,
                        primary);

                    return IntegratedSaveRepositoryResult
                        .Recovered(
                            primaryError == null
                                ? "Primary save was missing; " +
                                  "backup restored."
                                : "Primary save was invalid; " +
                                  "backup restored.");
                }
                catch (IntegratedSaveFormatException backupError)
                {
                    snapshot = null;

                    if (primaryError != null &&
                        primaryError.Status ==
                            IntegratedSaveRepositoryStatus
                                .UnsupportedSchema)
                    {
                        return IntegratedSaveRepositoryResult
                            .Failure(
                                primaryError.Status,
                                primaryError.Message);
                    }

                    if (backupError.Status ==
                        IntegratedSaveRepositoryStatus
                            .UnsupportedSchema)
                    {
                        return IntegratedSaveRepositoryResult
                            .Failure(
                                backupError.Status,
                                backupError.Message);
                    }

                    return IntegratedSaveRepositoryResult
                        .Failure(
                            IntegratedSaveRepositoryStatus
                                .CorruptPrimaryNoBackup,
                            "Neither primary nor backup save " +
                            "could be validated.");
                }
                catch (Exception exception)
                {
                    snapshot = null;
                    return IntegratedSaveRepositoryResult
                        .Failure(
                            IntegratedSaveRepositoryStatus
                                .StorageFailure,
                            exception.Message);
                }
            }

            snapshot = null;

            if (primaryError != null &&
                primaryError.Status ==
                    IntegratedSaveRepositoryStatus
                        .UnsupportedSchema)
            {
                return IntegratedSaveRepositoryResult
                    .Failure(
                        primaryError.Status,
                        primaryError.Message);
            }

            return IntegratedSaveRepositoryResult.Failure(
                IntegratedSaveRepositoryStatus
                    .CorruptPrimaryNoBackup,
                primaryError == null
                    ? "No valid save could be loaded."
                    : primaryError.Message);
        }

        public bool Delete(SaveSlotId slotId)
        {
            bool deleted = false;

            deleted |= DeleteIfExists(
                GetPrimaryPath(slotId));
            deleted |= DeleteIfExists(
                GetBackupPath(slotId));
            deleted |= DeleteIfExists(
                GetTemporaryPath(slotId));
            deleted |= DeleteIfExists(
                GetRecoveryPath(slotId));

            return deleted;
        }

        public string GetPrimaryPath(SaveSlotId slotId)
        {
            return Path.Combine(
                _rootDirectory,
                $"slot_{slotId.Value}.integrated.json");
        }

        public string GetBackupPath(SaveSlotId slotId)
        {
            return GetPrimaryPath(slotId) + ".bak";
        }

        public string GetTemporaryPath(SaveSlotId slotId)
        {
            return GetPrimaryPath(slotId) + ".tmp";
        }

        public string GetRecoveryPath(SaveSlotId slotId)
        {
            return GetPrimaryPath(slotId) + ".recovery";
        }

        private long GetLatestGeneration(
            SaveSlotId slotId)
        {
            long generation = 0;

            TryReadGeneration(
                GetPrimaryPath(slotId),
                slotId,
                ref generation);
            TryReadGeneration(
                GetBackupPath(slotId),
                slotId,
                ref generation);

            return generation;
        }

        private void TryReadGeneration(
            string path,
            SaveSlotId slotId,
            ref long highest)
        {
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                long generation =
                    _codec.ReadGeneration(
                        json,
                        slotId);
                highest = Math.Max(highest, generation);
            }
            catch
            {
                // Invalid previous data does not block a new save.
            }
        }

        private IntegratedGameStateSnapshot DecodeFile(
            string path,
            SaveSlotId slotId,
            out long generation)
        {
            string json = File.ReadAllText(path);
            return _codec.Decode(
                json,
                slotId,
                out generation);
        }

        private static void WriteDurable(
            string path,
            string content)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(content);

            using (FileStream stream =
                   new FileStream(
                       path,
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                stream.Write(
                    bytes,
                    0,
                    bytes.Length);
                stream.Flush(true);
            }
        }

        private static void CommitTemporary(
            string temporary,
            string primary,
            string backup)
        {
            if (!File.Exists(primary))
            {
                File.Move(
                    temporary,
                    primary);
                return;
            }

            DeleteIfExists(backup);

            try
            {
                File.Replace(
                    temporary,
                    primary,
                    backup);
            }
            catch (PlatformNotSupportedException)
            {
                File.Copy(
                    primary,
                    backup,
                    true);
                File.Delete(primary);
                File.Move(
                    temporary,
                    primary);
            }
        }

        private void RecoverPrimary(
            SaveSlotId slotId,
            string backup,
            string primary)
        {
            string recovery =
                GetRecoveryPath(slotId);

            DeleteIfExists(recovery);
            File.Copy(
                backup,
                recovery,
                true);

            using (FileStream stream =
                   new FileStream(
                       recovery,
                       FileMode.Open,
                       FileAccess.ReadWrite,
                       FileShare.None))
            {
                stream.Flush(true);
            }

            if (File.Exists(primary))
            {
                try
                {
                    File.Replace(
                        recovery,
                        primary,
                        null);
                    return;
                }
                catch (PlatformNotSupportedException)
                {
                    File.Delete(primary);
                }
            }

            File.Move(recovery, primary);
        }

        private static bool DeleteIfExists(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }
    }
}
