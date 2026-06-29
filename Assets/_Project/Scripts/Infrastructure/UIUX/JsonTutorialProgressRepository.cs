using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class JsonTutorialProgressRepository :
        ITutorialProgressRepository
    {
        private readonly string _directory;

        public JsonTutorialProgressRepository(
            string directory)
        {
            if (string.IsNullOrWhiteSpace(
                    directory))
            {
                throw new ArgumentException(
                    "A tutorial directory is required.",
                    nameof(directory));
            }

            _directory = directory;
        }

        public TutorialProgress Load(
            SaveSlotId slotId)
        {
            string path = PathFor(slotId);

            if (!File.Exists(path))
            {
                return TutorialProgress.New();
            }

            try
            {
                TutorialDto dto =
                    JsonUtility.FromJson<TutorialDto>(
                        File.ReadAllText(path));

                if (dto == null)
                {
                    return TutorialProgress.New();
                }

                return new TutorialProgress(
                    (TutorialProgressState)dto.state,
                    (TutorialStepId)dto.currentStep);
            }
            catch
            {
                return TutorialProgress.New();
            }
        }

        public void Save(
            SaveSlotId slotId,
            TutorialProgress progress)
        {
            if (progress == null)
            {
                throw new ArgumentNullException(
                    nameof(progress));
            }

            AtomicJsonFile.Write(
                PathFor(slotId),
                JsonUtility.ToJson(
                    new TutorialDto
                    {
                        state = (int)progress.State,
                        currentStep =
                            (int)progress.CurrentStep
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
                $"slot_{slotId.Value}.tutorial.json");
        }

        [Serializable]
        private sealed class TutorialDto
        {
            public int state;
            public int currentStep;
        }
    }
}
