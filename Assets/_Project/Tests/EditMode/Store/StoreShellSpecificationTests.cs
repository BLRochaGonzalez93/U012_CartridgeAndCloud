using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Store;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class StoreShellSpecificationTests
    {
        [Test]
        public void InitialTier_UsesTwentyByThirtyCells()
        {
            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Assert.That(
                specification.WidthCells,
                Is.EqualTo(20));

            Assert.That(
                specification.DepthCells,
                Is.EqualTo(30));
        }

        [Test]
        public void InitialTier_UsesHalfMetreCells()
        {
            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Assert.That(
                specification.CellSize,
                Is.EqualTo(0.5f));
        }

        [Test]
        public void InitialTier_ComputesTenByFifteenMetres()
        {
            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Assert.That(
                specification.WidthMeters,
                Is.EqualTo(10f));

            Assert.That(
                specification.DepthMeters,
                Is.EqualTo(15f));
        }

        [Test]
        public void InitialTier_UsesTwoMetreEntrance()
        {
            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Assert.That(
                specification.EntranceWidthCells,
                Is.EqualTo(4));

            Assert.That(
                specification.EntranceWidthMeters,
                Is.EqualTo(2f));
        }

        [Test]
        public void InitialTier_ComputesFourMetreFrontWallSegments()
        {
            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Assert.That(
                specification.FrontWallSegmentWidthMeters,
                Is.EqualTo(4f));
        }

        [Test]
        public void Constructor_RejectsNonPositiveWidth()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 0,
                        depthCells: 30,
                        entranceWidthCells: 4,
                        cellSize: 0.5f,
                        wallHeight: 3f,
                        wallThickness: 0.2f));
        }

        [Test]
        public void Constructor_RejectsNonPositiveDepth()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 20,
                        depthCells: 0,
                        entranceWidthCells: 4,
                        cellSize: 0.5f,
                        wallHeight: 3f,
                        wallThickness: 0.2f));
        }

        [Test]
        public void Constructor_RejectsInvalidEntranceAndMetrics()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 20,
                        depthCells: 30,
                        entranceWidthCells: 20,
                        cellSize: 0.5f,
                        wallHeight: 3f,
                        wallThickness: 0.2f));

            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 20,
                        depthCells: 30,
                        entranceWidthCells: 4,
                        cellSize: 0f,
                        wallHeight: 3f,
                        wallThickness: 0.2f));

            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 20,
                        depthCells: 30,
                        entranceWidthCells: 4,
                        cellSize: 0.5f,
                        wallHeight: 0f,
                        wallThickness: 0.2f));

            Assert.Throws<
                ArgumentOutOfRangeException>(
                    () => new StoreShellSpecification(
                        widthCells: 20,
                        depthCells: 30,
                        entranceWidthCells: 4,
                        cellSize: 0.5f,
                        wallHeight: 3f,
                        wallThickness: 20f));
        }
    }
}
