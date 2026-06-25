using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Displays;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayIdentifierTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void DisplayDefinitionId_EmptyValue_Throws(string value)
        {
            Assert.Throws<ArgumentException>(
                () => new DisplayDefinitionId(value));
        }

        [Test]
        public void DisplayDefinitionId_UsesOrdinalEquality()
        {
            Assert.That(
                new DisplayDefinitionId("Shelf"),
                Is.Not.EqualTo(new DisplayDefinitionId("shelf")));
        }

        [Test]
        public void DisplayDefinitionId_SameValue_HasSameHashCode()
        {
            DisplayDefinitionId left =
                new DisplayDefinitionId("display-a");

            DisplayDefinitionId right =
                new DisplayDefinitionId("display-a");

            Assert.That(
                left.GetHashCode(),
                Is.EqualTo(right.GetHashCode()));
        }

        [Test]
        public void DisplayInstanceId_Value_IsExposed()
        {
            DisplayInstanceId id =
                new DisplayInstanceId("instance-a");

            Assert.That(id.Value, Is.EqualTo("instance-a"));
        }

        [Test]
        public void RestockTaskId_ToString_ReturnsValue()
        {
            RestockTaskId id =
                new RestockTaskId("restock-a");

            Assert.That(id.ToString(), Is.EqualTo("restock-a"));
        }

        [Test]
        public void DisplayInstanceId_DifferentValues_AreNotEqual()
        {
            Assert.That(
                new DisplayInstanceId("a"),
                Is.Not.EqualTo(new DisplayInstanceId("b")));
        }
    }
}
