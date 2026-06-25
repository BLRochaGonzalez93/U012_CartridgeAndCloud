using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayInstanceRegistryTests
    {
        [Test]
        public void Constructor_NullCollection_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DisplayInstanceRegistry(null));
        }

        [Test]
        public void Constructor_NullEntry_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new DisplayInstanceRegistry(
                    new DisplayInstance[] { null }));
        }

        [Test]
        public void Constructor_DuplicateId_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new DisplayInstanceRegistry(
                    new[]
                    {
                        CreateInstance("same"),
                        CreateInstance("same")
                    }));
        }

        [Test]
        public void Instances_AreSortedById()
        {
            DisplayInstanceRegistry registry =
                new DisplayInstanceRegistry(
                    new[]
                    {
                        CreateInstance("z"),
                        CreateInstance("a")
                    });

            Assert.That(
                registry.Instances[0].Id.Value,
                Is.EqualTo("a"));
        }

        [Test]
        public void TryGet_ExistingId_ReturnsInstance()
        {
            DisplayInstance expected = CreateInstance("a");
            DisplayInstanceRegistry registry =
                new DisplayInstanceRegistry(new[] { expected });

            bool found = registry.TryGet(
                new DisplayInstanceId("a"),
                out DisplayInstance actual);

            Assert.That(found, Is.True);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void Get_MissingId_Throws()
        {
            DisplayInstanceRegistry registry =
                new DisplayInstanceRegistry(
                    Array.Empty<DisplayInstance>());

            Assert.Throws<KeyNotFoundException>(
                () => registry.Get(
                    new DisplayInstanceId("missing")));
        }

        private static DisplayInstance CreateInstance(string id)
        {
            return new DisplayInstance(
                new DisplayInstanceId(id),
                new DisplayDefinition(
                    new DisplayDefinitionId("definition"),
                    "display.name",
                    new InventoryCapacity(4),
                    2,
                    Array.Empty<ProductCategoryId>(),
                    "placement"));
        }
    }
}
