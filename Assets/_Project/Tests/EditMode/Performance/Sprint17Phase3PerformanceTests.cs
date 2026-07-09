using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Performance;
using VRMGames.CartridgeAndCloud.Runtime.Performance;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Performance
{
    public sealed class Sprint17Phase3PerformanceTests
    {
        [Test]
        public void EmptyAccumulator_ProducesZeroSnapshot()
        {
            PerformanceMetricSnapshot snapshot =
                new PerformanceMetricAccumulator()
                    .CreateSnapshot();

            Assert.That(snapshot.FrameCount, Is.Zero);
            Assert.That(snapshot.AverageFps, Is.Zero);
            Assert.That(
                snapshot.MaximumFrameMilliseconds,
                Is.Zero);
        }

        [Test]
        public void Accumulator_CalculatesFramePacingAndFps()
        {
            PerformanceMetricAccumulator accumulator =
                new PerformanceMetricAccumulator();

            for (int index = 0; index < 90; index++)
            {
                accumulator.RecordFrame(
                    10d,
                    0L,
                    7d,
                    8d);
            }

            for (int index = 0; index < 10; index++)
            {
                accumulator.RecordFrame(
                    40d,
                    0L,
                    30d,
                    32d);
            }

            PerformanceMetricSnapshot snapshot =
                accumulator.CreateSnapshot();

            Assert.That(snapshot.FrameCount, Is.EqualTo(100));
            Assert.That(
                snapshot.AverageFrameMilliseconds,
                Is.EqualTo(13d).Within(0.001d));
            Assert.That(
                snapshot.AverageFps,
                Is.EqualTo(1000d / 13d)
                    .Within(0.01d));
            Assert.That(
                snapshot.P50FrameMilliseconds,
                Is.EqualTo(10.5d));
            Assert.That(
                snapshot.P95FrameMilliseconds,
                Is.EqualTo(40.5d));
            Assert.That(
                snapshot.FramesOver33Milliseconds,
                Is.EqualTo(10));
            Assert.That(
                snapshot.MaximumFrameMilliseconds,
                Is.EqualTo(40d));
        }

        [Test]
        public void Accumulator_TracksGcMainThreadAndGpu()
        {
            PerformanceMetricAccumulator accumulator =
                new PerformanceMetricAccumulator();

            accumulator.RecordFrame(
                16d,
                128L,
                8d,
                10d);
            accumulator.RecordFrame(
                20d,
                512L,
                12d,
                14d);

            PerformanceMetricSnapshot snapshot =
                accumulator.CreateSnapshot();

            Assert.That(
                snapshot.TotalGcAllocatedBytes,
                Is.EqualTo(640L));
            Assert.That(
                snapshot.MaximumGcAllocatedBytesPerFrame,
                Is.EqualTo(512L));
            Assert.That(
                snapshot.FramesWithGcAllocations,
                Is.EqualTo(2L));
            Assert.That(
                snapshot.AverageMainThreadMilliseconds,
                Is.EqualTo(10d));
            Assert.That(
                snapshot.MaximumGpuMilliseconds,
                Is.EqualTo(14d));
        }

        [Test]
        public void Accumulator_RejectsInvalidFrameTime()
        {
            PerformanceMetricAccumulator accumulator =
                new PerformanceMetricAccumulator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => accumulator.RecordFrame(
                    -1d,
                    0L,
                    0d,
                    0d));
        }

        [Test]
        public void CaptureSanitizer_RejectsInvalidGpuTimings()
        {
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(double.NaN),
                Is.Zero);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(
                        double.PositiveInfinity),
                Is.Zero);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(0d),
                Is.Zero);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(100000000d),
                Is.Zero);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(4.25d),
                Is.EqualTo(4.25d));
        }

        [Test]
        public void CaptureSanitizer_CountsOnlyActiveRuntimeStates()
        {
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveCustomerState(
                        CustomerState.Browsing.ToString()),
                Is.True);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveCustomerState(
                        CustomerState.Despawned.ToString()),
                Is.False);

            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveQueueState(
                        CheckoutQueueEntryState.Waiting
                            .ToString()),
                Is.True);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveQueueState(
                        CheckoutQueueEntryState.Completed
                            .ToString()),
                Is.False);

            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveOrderState(
                        StoreOrderStatus.Reserved),
                Is.True);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveOrderState(
                        StoreOrderStatus.InTransit),
                Is.True);
            Assert.That(
                Sprint17Phase3PerformanceCapture
                    .PerformanceCaptureValueSanitizer
                    .IsActiveOrderState(
                        StoreOrderStatus.Received),
                Is.False);
        }

        [Test]
        public void QaBuild_ContainsProductionScenesAndTestLab()
        {
            IReadOnlyList<string> scenes =
                Sprint17Phase3QaBuildTool.QaScenePaths;

            Assert.That(
                scenes,
                Does.Contain(
                    "Assets/_Project/Scenes/Production/Bootstrap.unity"));
            Assert.That(
                scenes,
                Does.Contain(
                    "Assets/_Project/Scenes/Production/MainMenu.unity"));
            Assert.That(
                scenes,
                Does.Contain(
                    "Assets/_Project/Scenes/Production/StoreInitial.unity"));
            Assert.That(
                scenes,
                Does.Contain(
                    "Assets/_Project/Scenes/Test/TestLab.unity"));
        }

        [Test]
        public void ReportAudit_FlagsMissingTargetLoadAndErrors()
        {
            Sprint17Phase3QaBuildTool.PerformanceSummary summary =
                new Sprint17Phase3QaBuildTool
                    .PerformanceSummary
                {
                    frameCount = 1200L,
                    averageFps = 60d,
                    p95FrameMilliseconds = 17d,
                    p99FrameMilliseconds = 25d,
                    maximumFrameMilliseconds = 80d,
                    maximumObservedCustomers = 7,
                    maximumGcAllocatedBytesPerFrame = 0L,
                    usedMemoryGrowthBytes = 0L,
                    errorCount = 1
                };

            List<string> observations =
                Sprint17Phase3QaBuildTool
                    .EvaluateSummary(summary);

            Assert.That(observations.Count, Is.EqualTo(2));
            Assert.That(
                observations[0],
                Does.Contain("eight simultaneous customers"));
            Assert.That(
                observations[1],
                Does.Contain("error"));
        }

        [Test]
        public void ReportAudit_FlagsSustainedGcAndInvalidGpuSamples()
        {
            Sprint17Phase3QaBuildTool.PerformanceSummary summary =
                new Sprint17Phase3QaBuildTool
                    .PerformanceSummary
                {
                    frameCount = 1000L,
                    averageFps = 60d,
                    p95FrameMilliseconds = 16d,
                    p99FrameMilliseconds = 20d,
                    maximumFrameMilliseconds = 50d,
                    maximumObservedCustomers = 8,
                    totalGcAllocatedBytes = 4L * 1024L * 1000L,
                    maximumGcAllocatedBytesPerFrame = 8192L,
                    framesWithGcAllocations = 900L,
                    invalidGpuTimingSampleCount = 10L,
                    usedMemoryGrowthBytes = 0L,
                    errorCount = 0
                };

            List<string> observations =
                Sprint17Phase3QaBuildTool
                    .EvaluateSummary(summary);

            Assert.That(observations.Count, Is.EqualTo(2));
            Assert.That(
                observations[0],
                Does.Contain("Managed allocations"));
            Assert.That(
                observations[1],
                Does.Contain("GPU timing"));
        }

        [Test]
        public void ReportAudit_AcceptsRepresentativeHealthyMetrics()
        {
            Sprint17Phase3QaBuildTool.PerformanceSummary summary =
                new Sprint17Phase3QaBuildTool
                    .PerformanceSummary
                {
                    frameCount = 3600L,
                    averageFps = 60d,
                    p95FrameMilliseconds = 18d,
                    p99FrameMilliseconds = 24d,
                    maximumFrameMilliseconds = 70d,
                    maximumObservedCustomers = 8,
                    totalGcAllocatedBytes = 0L,
                    maximumGcAllocatedBytesPerFrame = 1024L,
                    framesWithGcAllocations = 0L,
                    validGpuTimingSampleCount = 3600L,
                    invalidGpuTimingSampleCount = 0L,
                    gpuTimingReliable = true,
                    usedMemoryGrowthBytes =
                        32L * 1024L * 1024L,
                    errorCount = 0
                };

            Assert.That(
                Sprint17Phase3QaBuildTool
                    .EvaluateSummary(summary),
                Is.Empty);
        }
    }
}
