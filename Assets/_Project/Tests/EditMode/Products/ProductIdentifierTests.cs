using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Products
{
    public sealed class ProductIdentifierTests
    {
        [Test]
        public void ProductDefinitionId_Whitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductDefinitionId(" "));
        }

        [Test]
        public void ProductDefinitionId_SameOrdinalValue_AreEqual()
        {
            ProductDefinitionId left =
                new ProductDefinitionId("cartridge-basic");

            ProductDefinitionId right =
                new ProductDefinitionId("cartridge-basic");

            Assert.That(left, Is.EqualTo(right));
            Assert.That(left == right, Is.True);
        }

        [Test]
        public void ProductDefinitionId_DifferentCase_AreNotEqual()
        {
            ProductDefinitionId left =
                new ProductDefinitionId("cartridge-basic");

            ProductDefinitionId right =
                new ProductDefinitionId("Cartridge-Basic");

            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void ProductDefinitionId_ToString_ReturnsValue()
        {
            ProductDefinitionId id =
                new ProductDefinitionId("cloud-basic");

            Assert.That(id.ToString(), Is.EqualTo("cloud-basic"));
        }

        [Test]
        public void ProductCategoryId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductCategoryId(string.Empty));
        }

        [Test]
        public void ProductTagId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductTagId(string.Empty));
        }
    }
}
