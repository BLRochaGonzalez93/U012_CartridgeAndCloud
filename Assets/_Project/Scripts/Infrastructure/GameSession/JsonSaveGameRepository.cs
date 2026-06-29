using System;
using System.IO;
using System.Text;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.GameSession
{
    public sealed class JsonSaveGameRepository :
        ISaveGameRepository
    {
        public const string SaveDirectoryName =
            "SaveGames";

        private readonly string _rootDirectory;

        public JsonSaveGameRepository(
            string rootDirectory)
        {
            if (string.IsNullOrWhiteSpace(
                    rootDirectory))
            {
                throw new ArgumentException(
                    "A save root directory is required.",
                    nameof(rootDirectory));
            }

            _rootDirectory = rootDirectory;
        }

        public static JsonSaveGameRepository CreateDefault()
        {
            return new JsonSaveGameRepository(
                Path.Combine(
                    UnityEngine.Application
                        .persistentDataPath,
                    SaveDirectoryName));
        }

        public bool Exists(SaveSlotId slotId)
        {
            return File.Exists(
                       GetSlotPath(slotId)) ||
                File.Exists(
                    GetBackupPath(slotId));
        }

        public void Save(
            GameSessionSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            Directory.CreateDirectory(
                _rootDirectory);

            string targetPath =
                GetSlotPath(snapshot.SlotId);
            string temporaryPath =
                GetTemporaryPath(snapshot.SlotId);
            string backupPath =
                GetBackupPath(snapshot.SlotId);
            string json =
                JsonUtility.ToJson(
                    SaveFileDto.FromSnapshot(
                        snapshot),
                    true);

            WriteDurable(
                temporaryPath,
                json);

            GameSessionSnapshot validation =
                ReadSnapshot(
                    temporaryPath,
                    snapshot.SlotId);

            if (validation.SessionId !=
                    snapshot.SessionId ||
                validation.UpdatedUtc !=
                    snapshot.UpdatedUtc)
            {
                DeleteIfExists(temporaryPath);
                throw new InvalidDataException(
                    "Temporary save validation failed.");
            }

            if (File.Exists(targetPath))
            {
                DeleteIfExists(backupPath);

                try
                {
                    File.Replace(
                        temporaryPath,
                        targetPath,
                        backupPath);
                }
                catch (PlatformNotSupportedException)
                {
                    File.Copy(
                        targetPath,
                        backupPath,
                        true);
                    File.Delete(targetPath);
                    File.Move(
                        temporaryPath,
                        targetPath);
                }
            }
            else
            {
                File.Move(
                    temporaryPath,
                    targetPath);
            }
        }

        public bool TryLoad(
            SaveSlotId slotId,
            out GameSessionSnapshot snapshot)
        {
            string primaryPath =
                GetSlotPath(slotId);
            string backupPath =
                GetBackupPath(slotId);

            Exception primaryFailure = null;

            if (File.Exists(primaryPath))
            {
                try
                {
                    snapshot =
                        ReadSnapshot(
                            primaryPath,
                            slotId);
                    return true;
                }
                catch (Exception exception)
                {
                    primaryFailure = exception;
                }
            }

            if (File.Exists(backupPath))
            {
                try
                {
                    snapshot =
                        ReadSnapshot(
                            backupPath,
                            slotId);
                    RestorePrimary(
                        slotId,
                        backupPath,
                        primaryPath);
                    return true;
                }
                catch (Exception backupFailure)
                {
                    throw new InvalidDataException(
                        "Neither primary nor backup " +
                        "save could be loaded.",
                        primaryFailure ??
                        backupFailure);
                }
            }

            if (primaryFailure != null)
            {
                throw new InvalidDataException(
                    "The primary save is invalid and " +
                    "no backup is available.",
                    primaryFailure);
            }

            snapshot = null;
            return false;
        }

        public bool Delete(SaveSlotId slotId)
        {
            bool deleted = false;

            deleted |= DeleteIfExists(
                GetSlotPath(slotId));
            deleted |= DeleteIfExists(
                GetBackupPath(slotId));
            deleted |= DeleteIfExists(
                GetTemporaryPath(slotId));
            deleted |= DeleteIfExists(
                GetRecoveryPath(slotId));

            return deleted;
        }

        public string GetSlotPath(
            SaveSlotId slotId)
        {
            return Path.Combine(
                _rootDirectory,
                $"slot_{slotId.Value}.json");
        }

        public string GetBackupPath(
            SaveSlotId slotId)
        {
            return GetSlotPath(slotId) + ".bak";
        }

        public string GetTemporaryPath(
            SaveSlotId slotId)
        {
            return GetSlotPath(slotId) + ".tmp";
        }

        private string GetRecoveryPath(
            SaveSlotId slotId)
        {
            return GetSlotPath(slotId) +
                ".recovery";
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

        private static GameSessionSnapshot
            ReadSnapshot(
                string path,
                SaveSlotId expectedSlot)
        {
            string json =
                File.ReadAllText(path);
            SaveFileDto dto =
                JsonUtility.FromJson<
                    SaveFileDto>(json);

            if (dto == null)
            {
                throw new InvalidDataException(
                    "The save file could not be deserialized.");
            }

            GameSessionSnapshot snapshot =
                dto.ToSnapshot();

            if (snapshot.SlotId != expectedSlot)
            {
                throw new InvalidDataException(
                    $"Save file slot mismatch. " +
                    $"Expected {expectedSlot.Value}, got " +
                    $"{snapshot.SlotId.Value}.");
            }

            return snapshot;
        }

        private void RestorePrimary(
            SaveSlotId slotId,
            string backupPath,
            string primaryPath)
        {
            string recoveryPath =
                GetRecoveryPath(slotId);

            DeleteIfExists(recoveryPath);
            File.Copy(
                backupPath,
                recoveryPath,
                true);

            using (FileStream stream =
                   new FileStream(
                       recoveryPath,
                       FileMode.Open,
                       FileAccess.ReadWrite,
                       FileShare.None))
            {
                stream.Flush(true);
            }

            if (File.Exists(primaryPath))
            {
                try
                {
                    File.Replace(
                        recoveryPath,
                        primaryPath,
                        null);
                    return;
                }
                catch (PlatformNotSupportedException)
                {
                    File.Delete(primaryPath);
                }
            }

            File.Move(
                recoveryPath,
                primaryPath);
        }

        private static bool DeleteIfExists(
            string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        [Serializable]
        private sealed class SaveFileDto
        {
            public int schemaVersion;
            public string sessionId;
            public int slot;
            public long createdUtcTicks;
            public long updatedUtcTicks;
            public int currentDay;
            public long cashCents;

            public static SaveFileDto FromSnapshot(
                GameSessionSnapshot snapshot)
            {
                return new SaveFileDto
                {
                    schemaVersion =
                        snapshot.SchemaVersion,
                    sessionId =
                        snapshot.SessionId.Value,
                    slot = snapshot.SlotId.Value,
                    createdUtcTicks =
                        snapshot.CreatedUtc.Ticks,
                    updatedUtcTicks =
                        snapshot.UpdatedUtc.Ticks,
                    currentDay =
                        snapshot.CurrentDay,
                    cashCents =
                        snapshot.CashCents
                };
            }

            public GameSessionSnapshot ToSnapshot()
            {
                return new GameSessionSnapshot(
                    schemaVersion,
                    StableId.Parse(sessionId),
                    new SaveSlotId(slot),
                    new DateTime(
                        createdUtcTicks,
                        DateTimeKind.Utc),
                    new DateTime(
                        updatedUtcTicks,
                        DateTimeKind.Utc),
                    currentDay,
                    cashCents);
            }
        }
    }
}
