using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VRMGames.CartridgeAndCloud.Infrastructure.Localization;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.Localization
{
    public sealed class Sprint17Phase5LocalizationPlayModeTests
    {
        [UnityTest]
        public IEnumerator ProceduralText_RefreshesWhenLocaleChanges()
        {
            RuntimeLocalizationCatalog catalog =
                RuntimeLocalizationCatalog.FromJson(
                    Resources.Load<TextAsset>(
                        "Localization/CC_Localization_Runtime").text);
            string locale = "en-US";
            RuntimeTextLocalizationBridge.Configure(
                value => catalog.Translate(value, locale));

            GameObject root = new GameObject("LocalizationTestRoot");
            Text text = ProceduralUiFactory.CreateText(
                root.transform,
                "ContinueText",
                "Continue",
                18,
                TextAnchor.MiddleCenter,
                Color.white);

            yield return null;
            Assert.That(text.text, Is.EqualTo("Continue"));

            locale = "es-ES";
            RuntimeTextLocalizationBridge.NotifyChanged();
            yield return null;

            Assert.That(text.text, Is.EqualTo("Continuar"));
            Object.Destroy(root);
            RuntimeTextLocalizationBridge.Configure(value => value);
        }

        [UnityTest]
        public IEnumerator DynamicText_UsesTemplateAndKeepsValue()
        {
            RuntimeLocalizationCatalog catalog =
                RuntimeLocalizationCatalog.FromJson(
                    Resources.Load<TextAsset>(
                        "Localization/CC_Localization_Runtime").text);
            RuntimeTextLocalizationBridge.Configure(
                value => catalog.Translate(value, "es-ES"));

            GameObject root = new GameObject("LocalizationDynamicTest");
            Text text = ProceduralUiFactory.CreateText(
                root.transform,
                "CashText",
                "Cash: 1100.00 EUR",
                18,
                TextAnchor.MiddleCenter,
                Color.white);

            yield return null;
            Assert.That(text.text, Is.EqualTo("Caja: 1.100,00 €"));

            text.text = "Cash: 999.99 EUR";
            yield return null;
            Assert.That(text.text, Is.EqualTo("Caja: 999,99 €"));

            Object.Destroy(root);
            RuntimeTextLocalizationBridge.Configure(value => value);
        }
    }
}
