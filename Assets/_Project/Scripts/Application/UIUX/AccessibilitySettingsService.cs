using System;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class AccessibilitySettingsService
    {
        private readonly IAccessibilitySettingsRepository
            _repository;

        public UiAccessibilitySettings Current {
            get;
            private set;
        }

        public event Action<
            UiAccessibilitySettings> Changed;

        public AccessibilitySettingsService(
            IAccessibilitySettingsRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            Current = _repository.Load() ??
                UiAccessibilitySettings.Default();
        }

        public void Apply(
            UiAccessibilitySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(
                    nameof(settings));
            }

            if (settings.Equals(Current))
            {
                return;
            }

            _repository.Save(settings);
            Current = settings;
            Changed?.Invoke(Current);
        }

        public void Reset()
        {
            Apply(
                UiAccessibilitySettings.Default());
        }
    }
}
