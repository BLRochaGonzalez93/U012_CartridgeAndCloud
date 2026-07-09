using UnityEngine;
using UnityEngine.UI;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Localization
{
    [DisallowMultipleComponent]
    public sealed class LocalizedTextWatcher : MonoBehaviour
    {
        private Text _text;
        private string _sourceText = string.Empty;
        private string _localizedText = string.Empty;
        private int _observedVersion = int.MinValue;

        private void Awake()
        {
            _text = GetComponent<Text>();
            CaptureSource();
            Refresh(force: true);
        }

        private void OnEnable()
        {
            if (_text == null)
            {
                _text = GetComponent<Text>();
            }

            CaptureSource();
            Refresh(force: true);
        }

        private void LateUpdate()
        {
            if (_text == null)
            {
                return;
            }

            if (!string.Equals(
                    _text.text,
                    _localizedText,
                    System.StringComparison.Ordinal))
            {
                _sourceText = _text.text ?? string.Empty;
            }

            Refresh(force: false);
        }

        public void SetSourceText(string sourceText)
        {
            _sourceText = sourceText ?? string.Empty;
            Refresh(force: true);
        }

        private void CaptureSource()
        {
            if (_text == null)
            {
                return;
            }

            string current = _text.text ?? string.Empty;
            if (!string.Equals(
                    current,
                    _localizedText,
                    System.StringComparison.Ordinal))
            {
                _sourceText = current;
            }
        }

        private void Refresh(bool force)
        {
            if (_text == null)
            {
                return;
            }

            int version = RuntimeTextLocalizationBridge.Version;
            if (!force && version == _observedVersion &&
                string.Equals(
                    _text.text,
                    _localizedText,
                    System.StringComparison.Ordinal))
            {
                return;
            }

            _localizedText =
                RuntimeTextLocalizationBridge.Localize(
                    _sourceText);
            _text.text = _localizedText;
            _observedVersion = version;
        }
    }
}
