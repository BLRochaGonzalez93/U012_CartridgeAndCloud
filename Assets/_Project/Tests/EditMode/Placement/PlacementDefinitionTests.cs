using NUnit.Framework;
using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlacementDefinitionTests
    {
        [Test]
        public void Configure_StoresFootprintAndHeight()
        {
            TechnicalPlaceableDefinition definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            try
            {
                definition.Configure(4, 2, 1.2f);

                Assert.That(
                    definition.GridSize,
                    Is.EqualTo(new GridSize(4, 2)));

                Assert.That(
                    definition.PreviewHeight,
                    Is.EqualTo(1.2f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Configure_InvalidWidth_Throws()
        {
            TechnicalPlaceableDefinition definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            try
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => definition.Configure(0, 2, 1f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Configure_InvalidHeight_Throws()
        {
            TechnicalPlaceableDefinition definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            try
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => definition.Configure(1, 1, 0f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Constructor_StoresNonEmptyValue()
        {
            PlacementInstanceId id =
                new PlacementInstanceId(
                    "technical-shelf-0001");

            Assert.That(
                id.Value,
                Is.EqualTo(
                    "technical-shelf-0001"));
        }

        [Test]
        public void Constructor_EmptyValue_Throws()
        {
            Assert.Throws<
                ArgumentException>(
                    () => new PlacementInstanceId(" "));
        }

        [Test]
        public void Equality_IsOrdinalAndCaseSensitive()
        {
            PlacementInstanceId lower =
                new PlacementInstanceId("shelf");

            PlacementInstanceId same =
                new PlacementInstanceId("shelf");

            PlacementInstanceId upper =
                new PlacementInstanceId("SHELF");

            Assert.That(lower, Is.EqualTo(same));
            Assert.That(lower, Is.Not.EqualTo(upper));
        }
    }
}
