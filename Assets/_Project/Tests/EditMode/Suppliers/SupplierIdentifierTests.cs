using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Receiving;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Suppliers
{
    public sealed class SupplierIdentifierTests
    {
        [Test]
        public void SupplierId_Whitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new SupplierId(" "));
        }

        [Test]
        public void SupplierId_SameOrdinalValue_AreEqual()
        {
            SupplierId left = new SupplierId("supplier-a");
            SupplierId right = new SupplierId("supplier-a");

            Assert.That(left, Is.EqualTo(right));
            Assert.That(left == right, Is.True);
        }

        [Test]
        public void SupplierCatalogId_DifferentCase_AreNotEqual()
        {
            SupplierCatalogId left =
                new SupplierCatalogId("catalog-a");

            SupplierCatalogId right =
                new SupplierCatalogId("Catalog-A");

            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void PurchaseOrderId_ToString_ReturnsValue()
        {
            PurchaseOrderId id =
                new PurchaseOrderId("order-001");

            Assert.That(id.ToString(), Is.EqualTo("order-001"));
        }

        [Test]
        public void DeliveryId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new DeliveryId(string.Empty));
        }

        [Test]
        public void ShipmentBoxId_SameValue_HasSameHashCode()
        {
            ShipmentBoxId left =
                new ShipmentBoxId("box-001");

            ShipmentBoxId right =
                new ShipmentBoxId("box-001");

            Assert.That(
                left.GetHashCode(),
                Is.EqualTo(right.GetHashCode()));
        }

        [Test]
        public void SupplierDefinition_EmptyNameKey_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new SupplierDefinition(
                    new SupplierId("supplier-a"),
                    string.Empty));
        }

        [Test]
        public void SupplierDefinition_ValidData_CapturesValues()
        {
            SupplierDefinition supplier =
                new SupplierDefinition(
                    new SupplierId("supplier-a"),
                    "suppliers.a.name");

            Assert.That(
                supplier.Id,
                Is.EqualTo(new SupplierId("supplier-a")));

            Assert.That(
                supplier.DisplayNameKey,
                Is.EqualTo("suppliers.a.name"));
        }
    }
}
