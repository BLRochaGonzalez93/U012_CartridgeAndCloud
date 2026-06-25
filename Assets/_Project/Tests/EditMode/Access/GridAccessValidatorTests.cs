using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Access;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridAccessValidatorTests
    {
        private GridAccessValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator =
                new GridAccessValidator();
        }

        [Test]
        public void Validate_OpenInitialLayout_IsValid()
        {
            AccessValidationResult result =
                _validator.Validate(
                    StoreAccessLayout.InitialTier(),
                    EmptyCells());

            Assert.That(result.IsValid, Is.True);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason.None));
            Assert.That(
                result.ReachableCellCount,
                Is.EqualTo(600));
        }

        [Test]
        public void Validate_NullArguments_Throw()
        {
            Assert.Throws<ArgumentNullException>(
                () => _validator.Validate(
                    null,
                    EmptyCells()));

            Assert.Throws<ArgumentNullException>(
                () => _validator.Validate(
                    StoreAccessLayout.InitialTier(),
                    null));
        }

        [Test]
        public void Validate_BlockedCellOutsideBounds_IsInvalid()
        {
            AccessValidationResult result =
                _validator.Validate(
                    StoreAccessLayout.InitialTier(),
                    new[]
                    {
                        new GridCoordinate(-1, 0)
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .BlockedCellOutsideBounds);
        }

        [Test]
        public void Validate_AllEntranceCellsBlocked_IsInvalid()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    CopyEntranceCells(layout));

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .NoOpenEntrance);
        }

        [Test]
        public void Validate_OnlyOneOpenEntranceCell_IsInvalid()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    new[]
                    {
                        layout.EntranceCells[0],
                        layout.EntranceCells[1],
                        layout.EntranceCells[2]
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .EntranceWidthInsufficient);
        }

        [Test]
        public void Validate_TwoAdjacentEntranceCellsOpen_IsValid()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    new[]
                    {
                        layout.EntranceCells[0],
                        layout.EntranceCells[1]
                    });

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_TwoSeparatedEntranceCellsOpen_IsInvalid()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    new[]
                    {
                        layout.EntranceCells[1],
                        layout.EntranceCells[2]
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .EntranceWidthInsufficient);
        }

        [Test]
        public void ValidateWithCandidate_ReservedEntranceCell_IsInvalid()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessValidationResult result =
                _validator.ValidateWithCandidate(
                    layout,
                    EmptyCells(),
                    new[]
                    {
                        layout.EntranceCells[0]
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .ReservedEntranceBlocked);
        }

        [Test]
        public void Validate_RequiredAnchorBlocked_ReturnsAnchorId()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            AccessAnchor anchor =
                layout.RequiredAnchors[1];

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    new[]
                    {
                        anchor.Cell
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .RequiredAnchorBlocked);

            Assert.That(
                result.FailedAnchorId.HasValue,
                Is.True);

            Assert.That(
                result.FailedAnchorId.Value,
                Is.EqualTo(anchor.Id));
        }

        [Test]
        public void Validate_CompleteHorizontalBarrier_IsUnreachable()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    CreateCompleteBarrier(
                        layout.Bounds.Size.Width,
                        z: 2));

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .RequiredAnchorUnreachable);
        }

        [Test]
        public void Validate_BarrierWithGap_IsValid()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            AccessValidationResult result =
                _validator.Validate(
                    layout,
                    CreateBarrierWithGap(
                        layout.Bounds.Size.Width,
                        z: 2,
                        gapX: 3));

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void ValidateWithCandidate_ClosingOnlyGap_IsInvalid()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            AccessValidationResult result =
                _validator.ValidateWithCandidate(
                    layout,
                    CreateBarrierWithGap(
                        layout.Bounds.Size.Width,
                        z: 2,
                        gapX: 3),
                    new[]
                    {
                        new GridCoordinate(3, 2)
                    });

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .RequiredAnchorUnreachable);
        }

        [Test]
        public void ValidateWithCandidate_UnrotatedFootprintBlocksRoute()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(4, 2));

            GridCoordinate[] candidateCells =
                footprint.GetOccupiedCells(
                    new GridCoordinate(1, 2),
                    GridRotation.Degrees0);

            AccessValidationResult result =
                _validator.ValidateWithCandidate(
                    layout,
                    CreateThreeGapBarrier(),
                    candidateCells);

            AssertInvalid(
                result,
                AccessValidationFailureReason
                    .RequiredAnchorUnreachable);
        }

        [Test]
        public void ValidateWithCandidate_RotatedFootprintPreservesRoute()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(4, 2));

            GridCoordinate[] candidateCells =
                footprint.GetOccupiedCells(
                    new GridCoordinate(1, 2),
                    GridRotation.Degrees90);

            AccessValidationResult result =
                _validator.ValidateWithCandidate(
                    layout,
                    CreateThreeGapBarrier(),
                    candidateCells);

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_RemovingCandidateRestoresRoute()
        {
            StoreAccessLayout layout =
                CreateCorridorLayout();

            GridCoordinate[] existing =
                CreateBarrierWithGap(
                    layout.Bounds.Size.Width,
                    z: 2,
                    gapX: 3);

            AccessValidationResult blocked =
                _validator.ValidateWithCandidate(
                    layout,
                    existing,
                    new[]
                    {
                        new GridCoordinate(3, 2)
                    });

            AccessValidationResult restored =
                _validator.Validate(
                    layout,
                    existing);

            Assert.That(blocked.IsValid, Is.False);
            Assert.That(restored.IsValid, Is.True);
        }

        private static StoreAccessLayout
            CreateCorridorLayout()
        {
            return new StoreAccessLayout(
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(6, 6)),
                new[]
                {
                    new GridCoordinate(2, 0),
                    new GridCoordinate(3, 0)
                },
                new[]
                {
                    new AccessAnchor(
                        new AccessAnchorId("target"),
                        new GridCoordinate(3, 5))
                },
                minimumOpenEntranceWidthCells: 2);
        }

        private static GridCoordinate[]
            CreateCompleteBarrier(
                int width,
                int z)
        {
            GridCoordinate[] cells =
                new GridCoordinate[width];

            for (int x = 0; x < width; x++)
            {
                cells[x] =
                    new GridCoordinate(x, z);
            }

            return cells;
        }

        private static GridCoordinate[]
            CreateBarrierWithGap(
                int width,
                int z,
                int gapX)
        {
            List<GridCoordinate> cells =
                new List<GridCoordinate>();

            for (int x = 0; x < width; x++)
            {
                if (x == gapX)
                {
                    continue;
                }

                cells.Add(
                    new GridCoordinate(x, z));
            }

            return cells.ToArray();
        }

        private static GridCoordinate[]
            CreateThreeGapBarrier()
        {
            return new[]
            {
                new GridCoordinate(0, 2),
                new GridCoordinate(4, 2),
                new GridCoordinate(5, 2)
            };
        }

        private static GridCoordinate[]
            CopyEntranceCells(
                StoreAccessLayout layout)
        {
            GridCoordinate[] cells =
                new GridCoordinate[
                    layout.EntranceCells.Count];

            for (int index = 0;
                 index < cells.Length;
                 index++)
            {
                cells[index] =
                    layout.EntranceCells[index];
            }

            return cells;
        }

        private static GridCoordinate[] EmptyCells()
        {
            return new GridCoordinate[0];
        }

        private static void AssertInvalid(
            AccessValidationResult result,
            AccessValidationFailureReason expectedReason)
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(expectedReason));
        }
    }
}
