using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class SlotDescriptorTests
    {
        [Test] public void Empty_CanCreate() =>
            Assert.That(
                SlotDescriptor.Empty(
                    UIUXTestFactory.Slot())
                    .CanCreate,
                Is.True);

        [Test] public void Empty_CannotContinue() =>
            Assert.That(
                SlotDescriptor.Empty(
                    UIUXTestFactory.Slot())
                    .CanContinue,
                Is.False);

        [Test] public void Empty_CannotDelete() =>
            Assert.That(
                SlotDescriptor.Empty(
                    UIUXTestFactory.Slot())
                    .CanDelete,
                Is.False);

        [Test] public void Ready_CanContinue() =>
            Assert.That(
                Ready().CanContinue,
                Is.True);

        [Test] public void Ready_CanDelete() =>
            Assert.That(
                Ready().CanDelete,
                Is.True);

        [Test] public void Ready_CannotCreateWithoutReplace() =>
            Assert.That(
                Ready().CanCreate,
                Is.False);

        [Test] public void Recovered_CanContinue() =>
            Assert.That(
                Create(
                    SlotPresentationState
                        .Recovered)
                    .CanContinue,
                Is.True);

        [Test] public void Corrupt_CanCreate() =>
            Assert.That(
                Create(
                    SlotPresentationState
                        .Corrupt,
                    validData: false)
                    .CanCreate,
                Is.True);

        [Test] public void Unsupported_CanCreate() =>
            Assert.That(
                Create(
                    SlotPresentationState
                        .UnsupportedSchema,
                    validData: false)
                    .CanCreate,
                Is.True);

        [Test] public void Ready_RejectsZeroDay() =>
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new SlotDescriptor(
                    UIUXTestFactory.Slot(),
                    SlotPresentationState.Ready,
                    0,
                    0,
                    UIUXTestFactory.Utc(),
                    ""));

        [Test] public void Descriptor_RejectsLocalTime() =>
            Assert.Throws<ArgumentException>(
                () => new SlotDescriptor(
                    UIUXTestFactory.Slot(),
                    SlotPresentationState.Ready,
                    1,
                    0,
                    DateTime.SpecifyKind(
                        UIUXTestFactory.Utc(),
                        DateTimeKind.Local),
                    ""));

        [Test] public void Descriptor_StoresDetail() =>
            Assert.That(
                Create(
                    SlotPresentationState
                        .StorageFailure,
                    validData: false,
                    detail: "disk")
                    .Detail,
                Is.EqualTo("disk"));

        private static SlotDescriptor Ready()
        {
            return Create(
                SlotPresentationState.Ready);
        }

        private static SlotDescriptor Create(
            SlotPresentationState state,
            bool validData = true,
            string detail = "")
        {
            return new SlotDescriptor(
                UIUXTestFactory.Slot(),
                state,
                validData ? 2 : 0,
                validData ? 1000 : 0,
                validData
                    ? UIUXTestFactory.Utc()
                    : (DateTime?)null,
                detail);
        }
    }
}
