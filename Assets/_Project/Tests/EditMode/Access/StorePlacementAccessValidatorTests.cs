using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Access;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class StorePlacementAccessValidatorTests
    {
        [Test]
        public void Constructor_NullLayout_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StorePlacementAccessValidator(
                    null));
        }

        [Test]
        public void ValidateCandidate_OpenCandidate_IsValid()
        {
            StorePlacementAccessValidator validator =
                CreateValidator();

            AccessValidationResult result =
                validator.ValidateCandidate(
                    EmptyCells(),
                    new GridCoordinate(0, 1),
                    new GridSize(1, 1),
                    GridRotation.Degrees0);

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void ValidateCandidate_EntranceReserve_IsRejected()
        {
            StorePlacementAccessValidator validator =
                CreateValidator();

            AccessValidationResult result =
                validator.ValidateCandidate(
                    EmptyCells(),
                    new GridCoordinate(2, 0),
                    new GridSize(1, 1),
                    GridRotation.Degrees0);

            Assert.That(result.IsValid, Is.False);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason
                        .ReservedEntranceBlocked));
        }

        [Test]
        public void ValidateCandidate_ClosingOnlyGap_IsRejected()
        {
            StorePlacementAccessValidator validator =
                CreateValidator();

            AccessValidationResult result =
                validator.ValidateCandidate(
                    CreateBarrierWithGap(),
                    new GridCoordinate(3, 2),
                    new GridSize(1, 1),
                    GridRotation.Degrees0);

            Assert.That(result.IsValid, Is.False);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason
                        .RequiredAnchorUnreachable));
        }

        [Test]
        public void ValidateCandidate_RotationChangesAccessOutcome()
        {
            StorePlacementAccessValidator validator =
                CreateValidator();

            GridCoordinate[] existing =
            {
                new GridCoordinate(0, 2),
                new GridCoordinate(4, 2),
                new GridCoordinate(5, 2)
            };

            AccessValidationResult unrotated =
                validator.ValidateCandidate(
                    existing,
                    new GridCoordinate(1, 2),
                    new GridSize(4, 2),
                    GridRotation.Degrees0);

            AccessValidationResult rotated =
                validator.ValidateCandidate(
                    existing,
                    new GridCoordinate(1, 2),
                    new GridSize(4, 2),
                    GridRotation.Degrees90);

            Assert.That(unrotated.IsValid, Is.False);
            Assert.That(rotated.IsValid, Is.True);
        }

        private static StorePlacementAccessValidator
            CreateValidator()
        {
            return new StorePlacementAccessValidator(
                CreateLayout());
        }

        private static StoreAccessLayout CreateLayout()
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
            CreateBarrierWithGap()
        {
            return new[]
            {
                new GridCoordinate(0, 2),
                new GridCoordinate(1, 2),
                new GridCoordinate(2, 2),
                new GridCoordinate(4, 2),
                new GridCoordinate(5, 2)
            };
        }

        private static GridCoordinate[] EmptyCells()
        {
            return new GridCoordinate[0];
        }
    }
}
