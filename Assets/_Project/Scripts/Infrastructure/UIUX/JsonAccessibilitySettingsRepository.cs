using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class JsonAccessibilitySettingsRepository :
        IAccessibilitySettingsRepository
    {
        private readonly string _path;

        public JsonAccessibilitySettingsRepository(
            string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "A settings path is required.",
                    nameof(path));
            }

            _path = path;
        }

        public UiAccessibilitySettings Load()
        {
            if (!File.Exists(_path))
            {
                return UiAccessibilitySettings.Default();
            }

            try
            {
                SettingsDto dto =
                    JsonUtility.FromJson<SettingsDto>(
                        File.ReadAllText(_path));

                if (dto == null)
                {
                    return UiAccessibilitySettings.Default();
                }

                return new UiAccessibilitySettings(
                    dto.uiScalePercent,
                    dto.textScalePercent,
                    dto.reduceMotion,
                    dto.messageDurationSeconds,
                    dto.tutorialEnabled,
                    dto.confirmDestructiveActions);
            }
            catch
            {
                return UiAccessibilitySettings.Default();
            }
        }

        public void Save(
            UiAccessibilitySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(
                    nameof(settings));
            }

            AtomicJsonFile.Write(
                _path,
                JsonUtility.ToJson(
                    new SettingsDto
                    {
                        uiScalePercent =
                            settings.UiScalePercent,
                        textScalePercent =
                            settings.TextScalePercent,
                        reduceMotion =
                            settings.ReduceMotion,
                        messageDurationSeconds =
                            settings.MessageDurationSeconds,
                        tutorialEnabled =
                            settings.TutorialEnabled,
                        confirmDestructiveActions =
                            settings
                                .ConfirmDestructiveActions
                    },
                    true));
        }

        [Serializable]
        private sealed class SettingsDto
        {
            public int uiScalePercent;
            public int textScalePercent;
            public bool reduceMotion;
            public int messageDurationSeconds;
            public bool tutorialEnabled;
            public bool confirmDestructiveActions;
        }
    }
}
