using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.GameSession
{
    public sealed class JsonSaveGameRepository : ISaveGameRepository
    {
        public const string SaveDirectoryName = "SaveGames";

        private readonly string _rootDirectory;

        public JsonSaveGameRepository(string rootDirectory)
        {
            if (string.IsNullOrWhiteSpace(rootDirectory))
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
                Path.Combine(UnityEngine.Application.persistentDataPath, SaveDirectoryName));
        }

        public bool Exists(SaveSlotId slotId)
        {
            return File.Exists(GetSlotPath(slotId));
        }

        public void Save(GameSessionSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            Directory.CreateDirectory(_rootDirectory);

            string targetPath = GetSlotPath(snapshot.SlotId);
            string temporaryPath = targetPath + ".tmp";
            string json = JsonUtility.ToJson(SaveFileDto.FromSnapshot(snapshot), true);

            File.WriteAllText(temporaryPath, json);

            if (File.Exists(targetPath))
            {
                string backupPath = targetPath + ".bak";

                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }

                File.Replace(temporaryPath, targetPath, backupPath);

                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
            }
            else
            {
                File.Move(temporaryPath, targetPath);
            }
        }

        public bool TryLoad(
            SaveSlotId slotId,
            out GameSessionSnapshot snapshot)
        {
            string path = GetSlotPath(slotId);

            if (!File.Exists(path))
            {
                snapshot = null;
                return false;
            }

            string json = File.ReadAllText(path);
            SaveFileDto dto = JsonUtility.FromJson<SaveFileDto>(json);

            if (dto == null)
            {
                throw new InvalidDataException("The save file could not be deserialized.");
            }

            snapshot = dto.ToSnapshot();

            if (snapshot.SlotId != slotId)
            {
                throw new InvalidDataException(
                    $"Save file slot mismatch. Expected {slotId.Value}, got {snapshot.SlotId.Value}.");
            }

            return true;
        }

        public bool Delete(SaveSlotId slotId)
        {
            string path = GetSlotPath(slotId);

            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        private string GetSlotPath(SaveSlotId slotId)
        {
            return Path.Combine(_rootDirectory, $"slot_{slotId.Value}.json");
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

            public static SaveFileDto FromSnapshot(GameSessionSnapshot snapshot)
            {
                return new SaveFileDto
                {
                    schemaVersion = snapshot.SchemaVersion,
                    sessionId = snapshot.SessionId.Value,
                    slot = snapshot.SlotId.Value,
                    createdUtcTicks = snapshot.CreatedUtc.Ticks,
                    updatedUtcTicks = snapshot.UpdatedUtc.Ticks,
                    currentDay = snapshot.CurrentDay,
                    cashCents = snapshot.CashCents
                };
            }

            public GameSessionSnapshot ToSnapshot()
            {
                return new GameSessionSnapshot(
                    schemaVersion,
                    StableId.Parse(sessionId),
                    new SaveSlotId(slot),
                    new DateTime(createdUtcTicks, DateTimeKind.Utc),
                    new DateTime(updatedUtcTicks, DateTimeKind.Utc),
                    currentDay,
                    cashCents);
            }
        }
    }
}
