using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class CoreDataValueObjectTests
    {
        [Test]
        public void StableId_New_CreatesParseableUniqueValues()
        {
            StableId first = StableId.New();
            StableId second = StableId.New();

            Assert.That(first, Is.Not.EqualTo(second));
            Assert.That(StableId.Parse(first.Value), Is.EqualTo(first));
        }

        [TestCase("")]
        [TestCase("not-a-guid")]
        public void StableId_InvalidValue_Throws(string value)
        {
            Assert.Throws<ArgumentException>(() => new StableId(value));
        }

        [TestCase(-1)]
        [TestCase(3)]
        public void SaveSlotId_OutsideSupportedRange_Throws(int value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SaveSlotId(value));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void SaveSlotId_ValidRange_IsAccepted(int value)
        {
            Assert.That(new SaveSlotId(value).Value, Is.EqualTo(value));
        }
    }
}
