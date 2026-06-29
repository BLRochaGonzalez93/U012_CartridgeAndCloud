using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class UIUXRepositoryTests
    {
        [Test] public void Accessibility_MissingUsesDefault()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonAccessibilitySettingsRepository(
                        Path.Combine(root, "settings.json"));

                Assert.That(
                    repository.Load(),
                    Is.EqualTo(
                        UiAccessibilitySettings.Default()));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Accessibility_RoundTrips()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                string path =
                    Path.Combine(root, "settings.json");
                var repository =
                    new JsonAccessibilitySettingsRepository(
                        path);
                var settings =
                    new UiAccessibilitySettings(
                        120,
                        130,
                        true,
                        9,
                        false,
                        false);

                repository.Save(settings);

                Assert.That(
                    repository.Load(),
                    Is.EqualTo(settings));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Accessibility_CorruptUsesDefault()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                string path =
                    Path.Combine(root, "settings.json");
                File.WriteAllText(path, "{broken");

                var repository =
                    new JsonAccessibilitySettingsRepository(
                        path);

                Assert.That(
                    repository.Load(),
                    Is.EqualTo(
                        UiAccessibilitySettings.Default()));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Tutorial_MissingUsesNew()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonTutorialProgressRepository(
                        root);

                Assert.That(
                    repository.Load(
                        new SaveSlotId(0)).State,
                    Is.EqualTo(
                        TutorialProgressState
                            .NotStarted));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Tutorial_RoundTrips()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonTutorialProgressRepository(
                        root);
                SaveSlotId slot =
                    new SaveSlotId(0);
                TutorialProgress progress =
                    TutorialProgress.New()
                        .Start()
                        .Advance()
                        .Advance();

                repository.Save(slot, progress);

                Assert.That(
                    repository.Load(slot).CurrentStep,
                    Is.EqualTo(progress.CurrentStep));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void Tutorial_DeleteRemovesProgress()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonTutorialProgressRepository(
                        root);
                SaveSlotId slot =
                    new SaveSlotId(0);
                repository.Save(
                    slot,
                    TutorialProgress.New().Start());

                Assert.That(
                    repository.Delete(slot),
                    Is.True);
                Assert.That(
                    repository.Load(slot).State,
                    Is.EqualTo(
                        TutorialProgressState
                            .NotStarted));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void AutosaveMarker_RoundTrips()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonAutosaveMarkerRepository(
                        root);
                SaveSlotId slot =
                    new SaveSlotId(0);

                repository.SaveLastSavedDay(
                    slot,
                    "day-003");

                Assert.That(
                    repository.LoadLastSavedDay(
                        slot),
                    Is.EqualTo("day-003"));
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }

        [Test] public void AutosaveMarker_DeleteRemovesMarker()
        {
            string root =
                UIUXTestFactory.TempDirectory();

            try
            {
                var repository =
                    new JsonAutosaveMarkerRepository(
                        root);
                SaveSlotId slot =
                    new SaveSlotId(0);
                repository.SaveLastSavedDay(
                    slot,
                    "day-003");

                Assert.That(
                    repository.Delete(slot),
                    Is.True);
                Assert.That(
                    repository.LoadLastSavedDay(
                        slot),
                    Is.Empty);
            }
            finally
            {
                UIUXTestFactory.DeleteDirectory(root);
            }
        }
    }
}
