using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class JsonAutosaveMarkerRepository :
        IAutosaveMarkerRepository
    {
        private readonly string _directory;

        public JsonAutosaveMarkerRepository(
            string directory)
        {
            if (string.IsNullOrWhiteSpace(
                    directory))
            {
                throw new ArgumentException(
                    "An autosave directory is required.",
                    nameof(directory));
            }

            _directory = directory;
        }

        public string LoadLastSavedDay(
            SaveSlotId slotId)
        {
            string path = PathFor(slotId);

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            try
            {
                MarkerDto dto =
                    JsonUtility.FromJson<MarkerDto>(
                        File.ReadAllText(path));

                return dto == null
                    ? string.Empty
                    : dto.dayId ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SaveLastSavedDay(
            SaveSlotId slotId,
            string dayId)
        {
            if (string.IsNullOrWhiteSpace(dayId))
            {
                throw new ArgumentException(
                    "A day ID is required.",
                    nameof(dayId));
            }

            AtomicJsonFile.Write(
                PathFor(slotId),
                JsonUtility.ToJson(
                    new MarkerDto
                    {
                        dayId = dayId
                    },
                    true));
        }

        public bool Delete(SaveSlotId slotId)
        {
            string path = PathFor(slotId);

            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        private string PathFor(
            SaveSlotId slotId)
        {
            return Path.Combine(
                _directory,
                $"slot_{slotId.Value}.autosave.json");
        }

        [Serializable]
        private sealed class MarkerDto
        {
            public string dayId;
        }
    }
}
