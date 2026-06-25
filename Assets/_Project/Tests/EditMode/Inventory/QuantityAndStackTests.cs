using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Inventory
{
    public sealed class QuantityAndStackTests
    {
        [Test]
        public void Quantity_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Quantity(-1));
        }

        [Test]
        public void Quantity_Zero_IsValidAndZero()
        {
            Quantity quantity = new Quantity(0);

            Assert.That(quantity.IsZero, Is.True);
            Assert.That(quantity, Is.EqualTo(Quantity.Zero));
        }

        [Test]
        public void Add_ValidQuantities_ReturnsSum()
        {
            Quantity result =
                new Quantity(4).Add(new Quantity(3));

            Assert.That(result.Value, Is.EqualTo(7));
        }

        [Test]
        public void Subtract_AvailableQuantity_ReturnsDifference()
        {
            Quantity result =
                new Quantity(7).Subtract(new Quantity(3));

            Assert.That(result.Value, Is.EqualTo(4));
        }

        [Test]
        public void Subtract_InsufficientQuantity_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(
                () => new Quantity(2).Subtract(new Quantity(3)));
        }

        [Test]
        public void Comparison_DifferentQuantities_UsesNumericOrder()
        {
            Quantity low = new Quantity(2);
            Quantity high = new Quantity(3);

            Assert.That(low < high, Is.True);
            Assert.That(high > low, Is.True);
            Assert.That(low.CompareTo(high), Is.LessThan(0));
        }

        [Test]
        public void InventoryCapacity_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new InventoryCapacity(-1));
        }

        [Test]
        public void InventoryContainerId_Whitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new InventoryContainerId(" "));
        }

        [Test]
        public void InventoryStack_ZeroQuantity_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new InventoryStack(
                    new ProductDefinitionId("product-a"),
                    Quantity.Zero));
        }

        [Test]
        public void InventoryStack_ValidData_CapturesProductAndQuantity()
        {
            ProductDefinitionId productId =
                new ProductDefinitionId("product-a");

            InventoryStack stack =
                new InventoryStack(
                    productId,
                    new Quantity(4));

            Assert.That(stack.ProductId, Is.EqualTo(productId));
            Assert.That(stack.Quantity.Value, Is.EqualTo(4));
        }
    }
}
