using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class IntegratedSaveJsonCodecTests
    {
        private readonly IntegratedSaveJsonCodec _codec =
            new IntegratedSaveJsonCodec();

        [Test] public void Codec_RoundTripsClosedSnapshot()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    json,
                    source.SlotId,
                    out long generation);

            Assert.That(generation, Is.EqualTo(1));
            Assert.That(
                restored.TotalRecordCount,
                Is.EqualTo(source.TotalRecordCount));
        }

        [Test] public void Codec_RoundTripsOpenSnapshot()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.OpenSnapshot();
            string json = _codec.Encode(source, 7);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    json,
                    source.SlotId,
                    out long generation);

            Assert.That(generation, Is.EqualTo(7));
            Assert.That(
                restored.CheckoutStation.State,
                Is.EqualTo("Busy"));
            Assert.That(
                restored.QueueEntries.Count,
                Is.EqualTo(1));
        }

        [Test] public void Codec_PreservesCash()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot(
                    cashCents: 54321);
            string json = _codec.Encode(source, 2);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    json,
                    source.SlotId,
                    out _);

            Assert.That(
                restored.CashCents,
                Is.EqualTo(54321));
        }

        [Test] public void Codec_PreservesLedger()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 2);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    json,
                    source.SlotId,
                    out _);

            Assert.That(
                restored.LedgerEntries.Count,
                Is.EqualTo(2));
            Assert.That(
                restored.LedgerEntries[0].CurrencyCode,
                Is.EqualTo("EUR"));
        }


        [Test]
        public void CHAR_TIM_008_CodecPreservesSelectedSimulationSpeed()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.OpenSnapshot(
                    simulationSpeedMultiplier:
                        SimulationSpeedPolicy.Quadruple);
            string json = _codec.Encode(source, 4);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    json,
                    source.SlotId,
                    out _);

            Assert.That(
                restored.DayCycle
                    .SimulationSpeedMultiplier,
                Is.EqualTo(
                    SimulationSpeedPolicy.Quadruple));
        }


        [Test]
        public void Codec_LegacyDayWithoutSpeedDefaultsToNormal()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.OpenSnapshot();
            string encoded = _codec.Encode(source, 5);
            TestEnvelope envelope =
                JsonUtility.FromJson<TestEnvelope>(
                    encoded);

            envelope.payloadJson =
                Regex.Replace(
                    envelope.payloadJson,
                    ",\"simulationSpeedMultiplier\":" +
                    "(?:1(?:\\.0+)?)",
                    string.Empty);
            envelope.payloadSha256 =
                ComputeSha256(
                    envelope.payloadJson);

            IntegratedGameStateSnapshot restored =
                _codec.Decode(
                    JsonUtility.ToJson(
                        envelope,
                        true),
                    source.SlotId,
                    out _);

            Assert.That(
                restored.DayCycle
                    .SimulationSpeedMultiplier,
                Is.EqualTo(
                    SimulationSpeedPolicy.Normal));
        }

        [Test] public void Codec_IsDeterministicForSameInput()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();

            string first = _codec.Encode(source, 3);
            string second = _codec.Encode(source, 3);

            Assert.That(second, Is.EqualTo(first));
        }

        [Test] public void Codec_RejectsNullSnapshot()
        {
            Assert.Throws<ArgumentNullException>(
                () => _codec.Encode(null, 1));
        }

        [Test] public void Codec_RejectsZeroGeneration()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => _codec.Encode(
                    PersistenceTestFactory
                        .ClosedSnapshot(),
                    0));
        }

        [Test] public void Codec_RejectsEmptyJson()
        {
            IntegratedSaveFormatException exception =
                Assert.Throws<
                    IntegratedSaveFormatException>(
                    () => _codec.Decode(
                        "",
                        PersistenceTestFactory.Slot(),
                        out _));

            Assert.That(
                exception.Status,
                Is.EqualTo(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure));
        }

        [Test] public void Codec_RejectsWrongRequestedSlot()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);

            IntegratedSaveFormatException exception =
                Assert.Throws<
                    IntegratedSaveFormatException>(
                    () => _codec.Decode(
                        json,
                        PersistenceTestFactory.Slot(1),
                        out _));

            Assert.That(
                exception.Status,
                Is.EqualTo(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure));
        }

        [Test] public void Codec_RejectsUnsupportedSchema()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);
            json = json.Replace(
                "\"schemaVersion\": 2",
                "\"schemaVersion\": 99");

            IntegratedSaveFormatException exception =
                Assert.Throws<
                    IntegratedSaveFormatException>(
                    () => _codec.Decode(
                        json,
                        source.SlotId,
                        out _));

            Assert.That(
                exception.Status,
                Is.EqualTo(
                    IntegratedSaveRepositoryStatus
                        .UnsupportedSchema));
        }

        [Test] public void Codec_RejectsZeroEnvelopeGeneration()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);
            json = json.Replace(
                "\"generation\": 1",
                "\"generation\": 0");

            Assert.Throws<
                IntegratedSaveFormatException>(
                () => _codec.Decode(
                    json,
                    source.SlotId,
                    out _));
        }

        [Test] public void Codec_RejectsChecksumMismatch()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);
            json = Regex.Replace(
                json,
                "\"payloadSha256\"\\s*:\\s*\"[0-9a-f]+\"",
                "\"payloadSha256\": \"0000\"");

            IntegratedSaveFormatException exception =
                Assert.Throws<
                    IntegratedSaveFormatException>(
                    () => _codec.Decode(
                        json,
                        source.SlotId,
                        out _));

            Assert.That(
                exception.Message,
                Does.Contain("checksum"));
        }

        [Test] public void Codec_RejectsMissingPayload()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);
            json = Regex.Replace(
                json,
                "\"payloadJson\"\\s*:\\s*\"(?:\\\\.|[^\"])*\"",
                "\"payloadJson\": \"\"");

            Assert.Throws<
                IntegratedSaveFormatException>(
                () => _codec.Decode(
                    json,
                    source.SlotId,
                    out _));
        }

        [Test] public void Codec_RejectsEnvelopeTimestampMismatch()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 1);
            string original =
                "\"updatedUtcTicks\": " +
                source.UpdatedUtc.Ticks;
            string changed =
                "\"updatedUtcTicks\": " +
                (source.UpdatedUtc.Ticks + 1);
            json = json.Replace(
                original,
                changed);

            IntegratedSaveFormatException exception =
                Assert.Throws<
                    IntegratedSaveFormatException>(
                    () => _codec.Decode(
                        json,
                        source.SlotId,
                        out _));

            Assert.That(
                exception.Message,
                Does.Contain("timestamp"));
        }

        [Test] public void Codec_ReadGenerationValidatesFile()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            string json = _codec.Encode(source, 12);

            Assert.That(
                _codec.ReadGeneration(
                    json,
                    source.SlotId),
                Is.EqualTo(12));
        }
        private static string ComputeSha256(
            string value)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(value);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder =
                    new StringBuilder(hash.Length * 2);

                foreach (byte item in hash)
                {
                    builder.Append(item.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        [Serializable]
        private sealed class TestEnvelope
        {
            public int schemaVersion;
            public string slot;
            public long generation;
            public long updatedUtcTicks;
            public string payloadJson;
            public string payloadSha256;
        }

    }
}
