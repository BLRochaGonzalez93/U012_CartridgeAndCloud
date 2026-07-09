using System;

namespace VRMGames.CartridgeAndCloud.Runtime.Performance
{
    /// <summary>
    /// Allocation-free aggregate used by the Sprint 17 Phase 3 QA capture.
    /// Frame percentiles are calculated from a bounded histogram so long
    /// sessions do not retain one managed object per rendered frame.
    /// </summary>
    public sealed class PerformanceMetricAccumulator
    {
        private const double DefaultHistogramStepMilliseconds = 0.5d;
        private const double DefaultHistogramMaximumMilliseconds = 200d;

        private readonly double _histogramStepMilliseconds;
        private readonly long[] _frameHistogram;
        private long _frameCount;
        private double _frameMillisecondsTotal;
        private double _minimumFrameMilliseconds = double.MaxValue;
        private double _maximumFrameMilliseconds;
        private long _framesOver16Milliseconds;
        private long _framesOver33Milliseconds;
        private long _framesOver50Milliseconds;
        private long _totalGcAllocatedBytes;
        private long _maximumGcAllocatedBytes;
        private long _framesWithGcAllocations;
        private long _mainThreadSampleCount;
        private double _mainThreadMillisecondsTotal;
        private double _maximumMainThreadMilliseconds;
        private long _gpuSampleCount;
        private double _gpuMillisecondsTotal;
        private double _maximumGpuMilliseconds;

        public PerformanceMetricAccumulator()
            : this(
                DefaultHistogramStepMilliseconds,
                DefaultHistogramMaximumMilliseconds)
        {
        }

        public PerformanceMetricAccumulator(
            double histogramStepMilliseconds,
            double histogramMaximumMilliseconds)
        {
            if (histogramStepMilliseconds <= 0d)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(histogramStepMilliseconds));
            }

            if (histogramMaximumMilliseconds <=
                histogramStepMilliseconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(histogramMaximumMilliseconds));
            }

            _histogramStepMilliseconds =
                histogramStepMilliseconds;
            int binCount = checked(
                (int)Math.Ceiling(
                    histogramMaximumMilliseconds /
                    histogramStepMilliseconds) + 1);
            _frameHistogram = new long[binCount];
        }

        public long FrameCount => _frameCount;

        public void RecordFrame(
            double frameMilliseconds,
            long gcAllocatedBytes,
            double mainThreadMilliseconds,
            double gpuMilliseconds)
        {
            if (double.IsNaN(frameMilliseconds) ||
                double.IsInfinity(frameMilliseconds) ||
                frameMilliseconds < 0d)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(frameMilliseconds));
            }

            if (gcAllocatedBytes < 0L)
            {
                gcAllocatedBytes = 0L;
            }

            _frameCount++;
            _frameMillisecondsTotal += frameMilliseconds;
            _minimumFrameMilliseconds = Math.Min(
                _minimumFrameMilliseconds,
                frameMilliseconds);
            _maximumFrameMilliseconds = Math.Max(
                _maximumFrameMilliseconds,
                frameMilliseconds);

            if (frameMilliseconds > 16.6666667d)
            {
                _framesOver16Milliseconds++;
            }

            if (frameMilliseconds > 33.3333333d)
            {
                _framesOver33Milliseconds++;
            }

            if (frameMilliseconds > 50d)
            {
                _framesOver50Milliseconds++;
            }

            int bin = (int)Math.Floor(
                frameMilliseconds /
                _histogramStepMilliseconds);
            if (bin < 0)
            {
                bin = 0;
            }
            else if (bin >= _frameHistogram.Length)
            {
                bin = _frameHistogram.Length - 1;
            }

            _frameHistogram[bin]++;

            _totalGcAllocatedBytes = checked(
                _totalGcAllocatedBytes + gcAllocatedBytes);
            _maximumGcAllocatedBytes = Math.Max(
                _maximumGcAllocatedBytes,
                gcAllocatedBytes);
            if (gcAllocatedBytes > 0L)
            {
                _framesWithGcAllocations++;
            }

            if (mainThreadMilliseconds > 0d &&
                !double.IsNaN(mainThreadMilliseconds) &&
                !double.IsInfinity(mainThreadMilliseconds))
            {
                _mainThreadSampleCount++;
                _mainThreadMillisecondsTotal +=
                    mainThreadMilliseconds;
                _maximumMainThreadMilliseconds = Math.Max(
                    _maximumMainThreadMilliseconds,
                    mainThreadMilliseconds);
            }

            if (gpuMilliseconds > 0d &&
                !double.IsNaN(gpuMilliseconds) &&
                !double.IsInfinity(gpuMilliseconds))
            {
                _gpuSampleCount++;
                _gpuMillisecondsTotal += gpuMilliseconds;
                _maximumGpuMilliseconds = Math.Max(
                    _maximumGpuMilliseconds,
                    gpuMilliseconds);
            }
        }

        public PerformanceMetricSnapshot CreateSnapshot()
        {
            if (_frameCount == 0L)
            {
                return PerformanceMetricSnapshot.Empty();
            }

            double averageFrameMilliseconds =
                _frameMillisecondsTotal / _frameCount;

            return new PerformanceMetricSnapshot(
                _frameCount,
                averageFrameMilliseconds > 0d
                    ? 1000d / averageFrameMilliseconds
                    : 0d,
                averageFrameMilliseconds,
                _minimumFrameMilliseconds,
                _maximumFrameMilliseconds,
                Percentile(0.50d),
                Percentile(0.95d),
                Percentile(0.99d),
                _framesOver16Milliseconds,
                _framesOver33Milliseconds,
                _framesOver50Milliseconds,
                _totalGcAllocatedBytes,
                _maximumGcAllocatedBytes,
                _framesWithGcAllocations,
                _mainThreadSampleCount == 0L
                    ? 0d
                    : _mainThreadMillisecondsTotal /
                      _mainThreadSampleCount,
                _maximumMainThreadMilliseconds,
                _gpuSampleCount == 0L
                    ? 0d
                    : _gpuMillisecondsTotal /
                      _gpuSampleCount,
                _maximumGpuMilliseconds);
        }

        private double Percentile(double percentile)
        {
            long target = Math.Max(
                1L,
                (long)Math.Ceiling(
                    _frameCount * percentile));
            long cumulative = 0L;

            for (int index = 0;
                 index < _frameHistogram.Length;
                 index++)
            {
                cumulative += _frameHistogram[index];
                if (cumulative < target)
                {
                    continue;
                }

                if (index == _frameHistogram.Length - 1)
                {
                    return _maximumFrameMilliseconds;
                }

                return (index + 1) *
                       _histogramStepMilliseconds;
            }

            return _maximumFrameMilliseconds;
        }
    }

    public sealed class PerformanceMetricSnapshot
    {
        public long FrameCount { get; }
        public double AverageFps { get; }
        public double AverageFrameMilliseconds { get; }
        public double MinimumFrameMilliseconds { get; }
        public double MaximumFrameMilliseconds { get; }
        public double P50FrameMilliseconds { get; }
        public double P95FrameMilliseconds { get; }
        public double P99FrameMilliseconds { get; }
        public long FramesOver16Milliseconds { get; }
        public long FramesOver33Milliseconds { get; }
        public long FramesOver50Milliseconds { get; }
        public long TotalGcAllocatedBytes { get; }
        public long MaximumGcAllocatedBytesPerFrame { get; }
        public long FramesWithGcAllocations { get; }
        public double AverageMainThreadMilliseconds { get; }
        public double MaximumMainThreadMilliseconds { get; }
        public double AverageGpuMilliseconds { get; }
        public double MaximumGpuMilliseconds { get; }

        public PerformanceMetricSnapshot(
            long frameCount,
            double averageFps,
            double averageFrameMilliseconds,
            double minimumFrameMilliseconds,
            double maximumFrameMilliseconds,
            double p50FrameMilliseconds,
            double p95FrameMilliseconds,
            double p99FrameMilliseconds,
            long framesOver16Milliseconds,
            long framesOver33Milliseconds,
            long framesOver50Milliseconds,
            long totalGcAllocatedBytes,
            long maximumGcAllocatedBytesPerFrame,
            long framesWithGcAllocations,
            double averageMainThreadMilliseconds,
            double maximumMainThreadMilliseconds,
            double averageGpuMilliseconds,
            double maximumGpuMilliseconds)
        {
            FrameCount = frameCount;
            AverageFps = averageFps;
            AverageFrameMilliseconds =
                averageFrameMilliseconds;
            MinimumFrameMilliseconds =
                minimumFrameMilliseconds;
            MaximumFrameMilliseconds =
                maximumFrameMilliseconds;
            P50FrameMilliseconds =
                p50FrameMilliseconds;
            P95FrameMilliseconds =
                p95FrameMilliseconds;
            P99FrameMilliseconds =
                p99FrameMilliseconds;
            FramesOver16Milliseconds =
                framesOver16Milliseconds;
            FramesOver33Milliseconds =
                framesOver33Milliseconds;
            FramesOver50Milliseconds =
                framesOver50Milliseconds;
            TotalGcAllocatedBytes =
                totalGcAllocatedBytes;
            MaximumGcAllocatedBytesPerFrame =
                maximumGcAllocatedBytesPerFrame;
            FramesWithGcAllocations =
                framesWithGcAllocations;
            AverageMainThreadMilliseconds =
                averageMainThreadMilliseconds;
            MaximumMainThreadMilliseconds =
                maximumMainThreadMilliseconds;
            AverageGpuMilliseconds =
                averageGpuMilliseconds;
            MaximumGpuMilliseconds =
                maximumGpuMilliseconds;
        }

        public static PerformanceMetricSnapshot Empty()
        {
            return new PerformanceMetricSnapshot(
                0L,
                0d,
                0d,
                0d,
                0d,
                0d,
                0d,
                0d,
                0L,
                0L,
                0L,
                0L,
                0L,
                0L,
                0d,
                0d,
                0d,
                0d);
        }
    }
}
