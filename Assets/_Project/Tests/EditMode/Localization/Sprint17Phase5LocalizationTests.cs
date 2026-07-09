using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor.Localization;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Localization;
using VRMGames.CartridgeAndCloud.Editor.Localization;
using VRMGames.CartridgeAndCloud.Infrastructure.Localization;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Localization
{
    public sealed class Sprint17Phase5LocalizationTests
    {
        [Test]
        public void Catalog_TranslatesStaticAndDynamicGoldenPathText()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();

            Assert.That(
                catalog.Translate("Continue", "es-ES"),
                Is.EqualTo("Continuar"));
            Assert.That(
                catalog.Translate("Day 7", "es-ES"),
                Is.EqualTo("Día 7"));
            Assert.That(
                catalog.Translate(
                    "Cash: 1100.00 EUR",
                    "es-ES"),
                Is.EqualTo("Caja: 1.100,00 €"));
            Assert.That(
                catalog.Translate(
                    "Cash: 1100.00 EUR",
                    "en-US"),
                Is.EqualTo("Cash: €1,100.00"));
        }

        [Test]
        public void Catalog_TranslatesRuntimeCompositesAndContentIds()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();

            Assert.That(
                catalog.Translate(
                    "Customers 8/8",
                    "es-ES"),
                Is.EqualTo("Clientes 8/8"));
            Assert.That(
                catalog.Translate(
                    "Queue 3 · Available · Checkout ready",
                    "es-ES"),
                Is.EqualTo(
                    "Cola 3 · Disponible · Caja disponible"));
            Assert.That(
                catalog.Translate(
                    "order-001 · console-vertex-one · 2 units",
                    "es-ES"),
                Is.EqualTo(
                    "Pedido 1 · Consola Vertex One · 2 unidades"));
            Assert.That(
                catalog.Translate(
                    "console-vertex-one",
                    "en-US"),
                Is.EqualTo("Vertex One Console"));
            Assert.That(
                catalog.Translate(
                    "Product warehouse",
                    "es-ES"),
                Is.EqualTo("Almacén de productos"));
            Assert.That(
                catalog.Translate(
                    "Transfer one received unit from backroom",
                    "es-ES"),
                Is.EqualTo(
                    "Transferir una unidad recibida desde el almacén"));
            Assert.That(
                catalog.Translate(
                    "Customer admitted.",
                    "es-ES"),
                Is.EqualTo("Cliente admitido."));
        }

        [Test]
        public void Catalog_TranslatesAuthoredTutorialCopyCompletely()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();
            string[,] expectations =
            {
                {
                    "Welcome to Cartridge & Cloud",
                    "Bienvenido a Cartridge & Cloud"
                },
                {
                    "Run the store one day at a time. " +
                    "The HUD keeps the important state visible.",
                    "Gestiona la tienda jornada a jornada. " +
                    "La interfaz mantiene visible la información importante."
                },
                {
                    "Press Next to continue.",
                    "Pulsa Siguiente para continuar."
                },
                {
                    "Movement and camera",
                    "Movimiento y cámara"
                },
                {
                    "Use the existing gameplay controls to move, " +
                    "orbit and zoom around the store.",
                    "Usa los controles del juego para moverte, " +
                    "girar la cámara y acercar o alejar la vista de la tienda."
                },
                {
                    "Try the camera, then press Next.",
                    "Prueba la cámara y después pulsa Siguiente."
                },
                {
                    "Management panels",
                    "Paneles de gestión"
                },
                {
                    "Open a panel from the left navigation. " +
                    "Gameplay input is blocked while a panel is open.",
                    "Abre un panel desde la navegación izquierda. " +
                    "Los controles de juego quedan bloqueados mientras " +
                    "haya un panel abierto."
                },
                {
                    "Open Inventory.",
                    "Abre Inventario."
                },
                {
                    "Review total stock, capacity and product locations.",
                    "Revisa el stock total, la capacidad y la ubicación " +
                    "de los productos."
                },
                {
                    "Check the Inventory panel.",
                    "Revisa el panel Inventario."
                },
                {
                    "Review placed orders, received units and costs.",
                    "Revisa los pedidos realizados, las unidades recibidas " +
                    "y los costes."
                },
                {
                    "Open Suppliers.",
                    "Abre Compras."
                },
                {
                    "Displays show assigned products and available stock.",
                    "Los expositores muestran los productos asignados " +
                    "y el stock disponible."
                },
                {
                    "Open Displays.",
                    "Abre Expositores."
                },
                {
                    "Track active customers, sessions, carts and reservations.",
                    "Consulta los clientes activos, las sesiones, " +
                    "los carritos y las reservas."
                },
                {
                    "Open Customers or Shopping.",
                    "Abre Clientes o Compras."
                },
                {
                    "Monitor the FIFO queue, station state and transactions.",
                    "Supervisa la cola por orden de llegada, " +
                    "el estado de la caja y las transacciones."
                },
                {
                    "Open Checkout.",
                    "Abre Caja."
                },
                {
                    "A day progresses from Before Open to Closed. " +
                    "Autosave only runs after a valid Closed state.",
                    "La jornada avanza desde Antes de abrir hasta Cerrada. " +
                    "El guardado automático solo se ejecuta después " +
                    "de un cierre válido."
                },
                {
                    "Open Day Cycle.",
                    "Abre Jornada."
                },
                {
                    "Review revenue, received supplier costs and gross result.",
                    "Revisa los ingresos, los costes de proveedor recibidos " +
                    "y el resultado bruto."
                },
                {
                    "Open Economy.",
                    "Abre Economía."
                },
                {
                    "The active slot is saved once after each closed day. " +
                    "The previous valid generation remains as backup.",
                    "La ranura activa se guarda una vez después de cerrar " +
                    "cada jornada. La generación válida anterior se conserva " +
                    "como copia de seguridad."
                },
                {
                    "Press Finish.",
                    "Pulsa Finalizar."
                }
            };

            for (int index = 0;
                 index < expectations.GetLength(0);
                 index++)
            {
                Assert.That(
                    catalog.Translate(
                        expectations[index, 0],
                        "es-ES"),
                    Is.EqualTo(expectations[index, 1]),
                    expectations[index, 0]);
            }
        }

        [Test]
        public void Catalog_ReplacesTechnicalIdentifiersInBothLocales()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();

            Assert.That(
                catalog.Translate(
                    "store-inventory",
                    "es-ES"),
                Is.EqualTo("Inventario de la tienda"));
            Assert.That(
                catalog.Translate(
                    "backroom-inventory",
                    "en-US"),
                Is.EqualTo("Backroom Inventory"));
            Assert.That(
                catalog.Translate(
                    "checkout-station-main",
                    "es-ES"),
                Is.EqualTo("Caja principal"));
            Assert.That(
                catalog.Translate(
                    "day-001",
                    "es-ES"),
                Is.EqualTo("Día 1"));
            Assert.That(
                catalog.Translate(
                    "Sale phase1-transaction-004",
                    "es-ES"),
                Is.EqualTo("Venta Transacción 4"));
            Assert.That(
                catalog.Translate(
                    "delivery-run-003 · console-vertex-one",
                    "en-US"),
                Is.EqualTo(
                    "Delivery 3 · Vertex One Console"));
            Assert.That(
                catalog.Translate(
                    "phase1-display-002",
                    "es-ES"),
                Is.EqualTo("Expositor 2"));
        }

        [Test]
        public void Catalog_TranslatesCheckoutStatusAndAudioChannels()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();

            Assert.That(
                catalog.Translate(
                    "Checkout is placed, active and reachable.",
                    "es-ES"),
                Is.EqualTo(
                    "La caja está colocada, activa y disponible."));
            Assert.That(
                catalog.Translate(
                    "Music: 100%",
                    "es-ES"),
                Is.EqualTo("Música: 100 %"));
            Assert.That(
                catalog.Translate(
                    "Ambience: 50%",
                    "es-ES"),
                Is.EqualTo("Sonido ambiente: 50 %"));
            Assert.That(
                catalog.Translate(
                    "Ui: 0%",
                    "es-ES"),
                Is.EqualTo("Interfaz: 0 %"));
            Assert.That(
                catalog.Translate(
                    "Effects: 100%",
                    "es-ES"),
                Is.EqualTo("Efectos: 100 %"));
        }

        [Test]
        public void Catalog_TranslatesWeeklyMultilineSummary()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();
            string source =
                "Detailed day breakdown is unavailable for this legacy save.\n\n" +
                "Gross result: 100.00 EUR\n" +
                "Tax due: 0.00 EUR (weekly result was not positive)\n\n" +
                "Accept this summary to continue.";

            string translated = catalog.Translate(source, "es-ES");

            StringAssert.Contains(
                "El desglose detallado por jornadas",
                translated);
            StringAssert.Contains(
                "Resultado bruto: 100,00 €",
                translated);
            StringAssert.Contains(
                "Impuestos: 0,00 €",
                translated);
            StringAssert.EndsWith(
                "Acepta este resumen para continuar.",
                translated);
        }

        [Test]
        public void Catalog_FormatsSlotDateByLocale()
        {
            RuntimeLocalizationCatalog catalog = LoadCatalog();
            string source =
                "Day 7 · 1100.00 EUR · " +
                "2026-07-09 11:54 UTC";

            StringAssert.Contains(
                "09/07/2026 11:54 UTC",
                catalog.Translate(source, "es-ES"));
            StringAssert.Contains(
                "07/09/2026 11:54 AM UTC",
                catalog.Translate(source, "en-US"));
        }

        [Test]
        public void UnityLocalizationAssets_ExistForEsEnAndPseudo()
        {
            Assert.That(
                LocalizationEditorSettings.GetLocale("es-ES"),
                Is.Not.Null);
            Assert.That(
                LocalizationEditorSettings.GetLocale("en-US"),
                Is.Not.Null);
            Assert.That(
                LocalizationEditorSettings.GetPseudoLocales()
                    .Any(locale =>
                        locale.Identifier.Code == "qps-ploc"),
                Is.True);

            string[] tables =
            {
                "UI_Common",
                "UI_MainMenu",
                "UI_Store",
                "UI_Operations",
                "Tutorial",
                "Feedback",
                "Errors",
                "Content",
                "Formats"
            };
            for (int index = 0;
                 index < tables.Length;
                 index++)
            {
                Assert.That(
                    LocalizationEditorSettings
                        .GetStringTableCollection(tables[index]),
                    Is.Not.Null,
                    tables[index]);
            }
        }

        [Test]
        public void LocalePreference_RoundTripsOutsideSaveSlots()
        {
            string directory = Path.Combine(
                Path.GetTempPath(),
                "CC_Phase5_" + Guid.NewGuid().ToString("N"));
            string path = Path.Combine(
                directory,
                "localization.json");

            try
            {
                JsonLocalePreferenceRepository repository =
                    new JsonLocalePreferenceRepository(path);
                repository.Save(
                    new LocalizationPreference(
                        LocalizationPreference
                            .CurrentSchemaVersion,
                        "en-US",
                        true));

                LocalizationPreference loaded =
                    repository.Load();
                Assert.That(loaded.LocaleCode, Is.EqualTo("en-US"));
                Assert.That(loaded.WasExplicitlySelected, Is.True);
            }
            finally
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }

        [Test]
        public void CorruptLocalePreference_FallsBackSafely()
        {
            string directory = Path.Combine(
                Path.GetTempPath(),
                "CC_Phase5_" + Guid.NewGuid().ToString("N"));
            string path = Path.Combine(
                directory,
                "localization.json");

            try
            {
                Directory.CreateDirectory(directory);
                File.WriteAllText(path, "not-json");
                LocalizationPreference loaded =
                    new JsonLocalePreferenceRepository(path)
                        .Load();

                Assert.That(loaded.LocaleCode, Is.Empty);
                Assert.That(loaded.WasExplicitlySelected, Is.False);
            }
            finally
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }

        [Test]
        public void Pseudolocale_ExpandsAndEncapsulatesText()
        {
            string translated = LoadCatalog().Translate(
                "Continue",
                "qps-ploc");

            StringAssert.StartsWith("[!! ", translated);
            StringAssert.EndsWith(" !!]", translated);
            Assert.That(translated.Length, Is.GreaterThan("Continue".Length));
        }

        private static RuntimeLocalizationCatalog LoadCatalog()
        {
            TextAsset asset = Resources.Load<TextAsset>(
                "Localization/CC_Localization_Runtime");
            Assert.That(asset, Is.Not.Null);
            return RuntimeLocalizationCatalog.FromJson(asset.text);
        }
    }
}
