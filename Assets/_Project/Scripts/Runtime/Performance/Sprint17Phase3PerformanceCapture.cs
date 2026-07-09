using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Runtime.Composition;
using Debug = UnityEngine.Debug;

namespace VRMGames.CartridgeAndCloud.Runtime.Performance
{
    /// <summary>
    /// Lightweight in-player capture used only by Sprint 17 Phase 3 QA builds.
    /// It records frame pacing, memory, allocations, actor load and explicit
    /// save/load operations without requiring an external profiler connection.
    /// </summary>
    [DefaultExecutionOrder(-20000)]
    public sealed class Sprint17Phase3PerformanceCapture :
        MonoBehaviour
    {
        public const string QaScriptingDefine =
            "CC_SPRINT17_PHASE3_QA";
        public const string ReportFolderName =
            "Sprint17Phase3Performance";

        private const double SampleIntervalSeconds = 1d;
        private const double FlushIntervalSeconds = 10d;
        private const double MeasurementWarmupSeconds = 5d;
        private const long NanosecondsPerMillisecond = 1000000L;

        private readonly PerformanceMetricAccumulator
            _metrics = new PerformanceMetricAccumulator();
        private readonly Dictionary<int, ActiveOperation>
            _activeOperations =
                new Dictionary<int, ActiveOperation>();
        private readonly Dictionary<string, int>
            _asynchronousOperations =
                new Dictionary<string, int>(
                    StringComparer.Ordinal);
        private readonly FrameTiming[] _frameTimings =
            new FrameTiming[1];

        private ProfilerRecorder _gcAllocatedRecorder;
        private ProfilerRecorder _mainThreadRecorder;
        private ProfilerRecorder _totalUsedMemoryRecorder;
        private ProfilerRecorder _totalReservedMemoryRecorder;
        private StreamWriter _sampleWriter;
        private StreamWriter _operationWriter;
        private string _reportDirectory = string.Empty;
        private string _sessionStartedUtc = string.Empty;
        private double _nextSampleRealtime;
        private double _nextFlushRealtime;
        private double _measurementStartRealtime;
        private long _captureStartupAllocatedMemoryBytes;
        private long _captureStartupReservedMemoryBytes;
        private long _initialAllocatedMemoryBytes;
        private long _initialReservedMemoryBytes;
        private long _maximumAllocatedMemoryBytes;
        private long _maximumReservedMemoryBytes;
        private long _maximumManagedMemoryBytes;
        private long _excludedWarmupFrameCount;
        private long _excludedOverlayFrameCount;
        private long _validGpuTimingSampleCount;
        private long _invalidGpuTimingSampleCount;
        private int _nextOperationId = 1;
        private int _warningCount;
        private int _errorCount;
        private int _manualMarkerSequence;
        private bool _measurementStarted;
        private bool _expandedOverlay;
        private bool _finalized;
        private GUIStyle _labelStyle;
        private GUIStyle _headerStyle;

        public static Sprint17Phase3PerformanceCapture
            Instance { get; private set; }

        public string ReportDirectory => _reportDirectory;

#if CC_SPRINT17_PHASE3_QA
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstallForQaBuild()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject root = new GameObject(
                "Sprint17Phase3PerformanceCapture");
            root.AddComponent<
                Sprint17Phase3PerformanceCapture>();
        }
#endif

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCapture();
        }

        private void Update()
        {
            double realtime = Time.realtimeSinceStartupAsDouble;
            double frameMilliseconds =
                Math.Max(0d, Time.unscaledDeltaTime * 1000d);
            long gcAllocatedBytes = ReadRecorderValue(
                _gcAllocatedRecorder);
            double mainThreadMilliseconds =
                ReadRecorderValue(_mainThreadRecorder) /
                (double)NanosecondsPerMillisecond;

            long allocatedMemoryBytes =
                ReadAllocatedMemoryBytes();
            long reservedMemoryBytes =
                ReadReservedMemoryBytes();
            long managedMemoryBytes =
                GC.GetTotalMemory(false);

            foreach (ActiveOperation operation
                     in _activeOperations.Values)
            {
                operation.MaximumObservedFrameMilliseconds =
                    Math.Max(
                        operation.MaximumObservedFrameMilliseconds,
                        frameMilliseconds);
                operation.MaximumObservedGcAllocatedBytes =
                    Math.Max(
                        operation.MaximumObservedGcAllocatedBytes,
                        gcAllocatedBytes);
            }

            if (!_measurementStarted)
            {
                if (realtime < _measurementStartRealtime)
                {
                    _excludedWarmupFrameCount++;
                }
                else
                {
                    BeginMeasurement(
                        allocatedMemoryBytes,
                        reservedMemoryBytes,
                        managedMemoryBytes);
                }
            }

            if (_measurementStarted)
            {
                _maximumAllocatedMemoryBytes = Math.Max(
                    _maximumAllocatedMemoryBytes,
                    allocatedMemoryBytes);
                _maximumReservedMemoryBytes = Math.Max(
                    _maximumReservedMemoryBytes,
                    reservedMemoryBytes);
                _maximumManagedMemoryBytes = Math.Max(
                    _maximumManagedMemoryBytes,
                    managedMemoryBytes);

                if (_expandedOverlay)
                {
                    _excludedOverlayFrameCount++;
                }
                else
                {
                    double gpuMilliseconds =
                        ReadGpuMilliseconds();

                    _metrics.RecordFrame(
                        frameMilliseconds,
                        gcAllocatedBytes,
                        mainThreadMilliseconds,
                        gpuMilliseconds);
                }

                if (realtime >= _nextSampleRealtime)
                {
                    WritePeriodicSample(
                        realtime,
                        allocatedMemoryBytes,
                        reservedMemoryBytes,
                        managedMemoryBytes);
                    _nextSampleRealtime =
                        realtime + SampleIntervalSeconds;
                }
            }

            if (realtime >= _nextFlushRealtime)
            {
                FlushWriters();
                _nextFlushRealtime =
                    realtime + FlushIntervalSeconds;
            }
        }

        private void LateUpdate()
        {
            FrameTimingManager.CaptureFrameTimings();
        }

        private void OnGUI()
        {
            Event currentEvent = Event.current;
            if (currentEvent != null &&
                currentEvent.type == EventType.KeyDown)
            {
                if (currentEvent.keyCode == KeyCode.F8)
                {
                    _expandedOverlay = !_expandedOverlay;
                    currentEvent.Use();
                }
                else if (currentEvent.keyCode == KeyCode.F9)
                {
                    WriteSummary();
                    FlushWriters();
                    currentEvent.Use();
                }
                else if (currentEvent.keyCode == KeyCode.F10)
                {
                    _manualMarkerSequence++;
                    RecordMarker(
                        "ManualMarker-" +
                        _manualMarkerSequence.ToString(
                            "000",
                            CultureInfo.InvariantCulture));
                    currentEvent.Use();
                }
            }

            if (!_expandedOverlay)
            {
                return;
            }

            EnsureGuiStyles();

            Rect panel = new Rect(
                Screen.width - 378f,
                8f,
                370f,
                260f);
            GUI.Box(panel, GUIContent.none);

            PerformanceMetricSnapshot snapshot =
                _metrics.CreateSnapshot();
            float x = panel.x + 12f;
            float y = panel.y + 10f;
            float width = panel.width - 24f;

            GUI.Label(
                new Rect(x, y, width, 24f),
                "SPRINT 17 · PHASE 3 · QA PERFORMANCE",
                _headerStyle);
            y += 28f;

            GUI.Label(
                new Rect(x, y, width, 20f),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "FPS {0:0.0} | Frame avg {1:0.00} ms | p95 {2:0.00} ms | max {3:0.00} ms",
                    snapshot.AverageFps,
                    snapshot.AverageFrameMilliseconds,
                    snapshot.P95FrameMilliseconds,
                    snapshot.MaximumFrameMilliseconds),
                _labelStyle);
            y += 22f;

            GUI.Label(
                new Rect(x, y, width, 20f),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Memory {0:0.0} MB used | {1:0.0} MB reserved | GC max {2} B/frame",
                    BytesToMegabytes(ReadAllocatedMemoryBytes()),
                    BytesToMegabytes(ReadReservedMemoryBytes()),
                    snapshot.MaximumGcAllocatedBytesPerFrame),
                _labelStyle);
            y += 22f;

            CaptureLoad load = CaptureLoad.Read();
            GUI.Label(
                new Rect(x, y, width, 20f),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Scene {0} | Customers {1} | Queue {2} | Deliveries {3}",
                    SceneManager.GetActiveScene().name,
                    load.ActiveCustomers,
                    load.QueueEntries,
                    load.ActiveDeliveryRuns),
                _labelStyle);
            y += 22f;

            GUI.Label(
                new Rect(x, y, width, 20f),
                "F8 hide/show | F9 export | F10 marker",
                _labelStyle);
            y += 22f;

            GUI.Label(
                new Rect(x, y, width, 38f),
                "Reports: " + _reportDirectory,
                _labelStyle);
            y += 42f;

            if (GUI.Button(
                    new Rect(x, y, 104f, 30f),
                    "EXPORT"))
            {
                WriteSummary();
                FlushWriters();
            }

            if (GUI.Button(
                    new Rect(x + 112f, y, 104f, 30f),
                    "MARK"))
            {
                _manualMarkerSequence++;
                RecordMarker(
                    "ManualMarker-" +
                    _manualMarkerSequence.ToString(
                        "000",
                        CultureInfo.InvariantCulture));
            }

            if (GUI.Button(
                    new Rect(x + 224f, y, 104f, 30f),
                    "HIDE"))
            {
                _expandedOverlay = false;
            }
        }

        private void OnApplicationQuit()
        {
            FinalizeCapture();
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            FinalizeCapture();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            global::UnityEngine.Application.logMessageReceived -= HandleLogMessage;
            DisposeRecorder(ref _gcAllocatedRecorder);
            DisposeRecorder(ref _mainThreadRecorder);
            DisposeRecorder(ref _totalUsedMemoryRecorder);
            DisposeRecorder(ref _totalReservedMemoryRecorder);
            Instance = null;
        }

        public static IDisposable MeasureOperation(
            string operationName)
        {
            Sprint17Phase3PerformanceCapture capture =
                Instance;
            if (capture == null)
            {
                return NoOperationScope.Instance;
            }

            return new OperationScope(
                capture,
                capture.StartOperation(operationName));
        }

        public static void BeginAsynchronousOperation(
            string operationName)
        {
            if (Instance == null ||
                string.IsNullOrWhiteSpace(operationName))
            {
                return;
            }

            Instance.StartAsynchronousOperation(
                operationName);
        }

        public static void EndAsynchronousOperation(
            string operationName)
        {
            if (Instance == null ||
                string.IsNullOrWhiteSpace(operationName))
            {
                return;
            }

            Instance.FinishAsynchronousOperation(
                operationName);
        }

        public static void RecordMarker(string markerName)
        {
            Instance?.WriteMarker(markerName);
        }

        private void InitializeCapture()
        {
            _sessionStartedUtc =
                DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
            _reportDirectory = CreateReportDirectory();
            Directory.CreateDirectory(_reportDirectory);

            _sampleWriter = new StreamWriter(
                Path.Combine(
                    _reportDirectory,
                    "samples.csv"),
                false);
            _sampleWriter.WriteLine(
                "utc,realtime_seconds,scene,day,day_state,simulation_speed,active_customers,queue_entries,active_delivery_runs,active_orders,frame_count,average_fps,average_frame_ms,p95_frame_ms,p99_frame_ms,max_frame_ms,main_thread_ms,gpu_ms,gc_allocated_bytes,total_used_memory_bytes,total_reserved_memory_bytes,managed_memory_bytes");

            _operationWriter = new StreamWriter(
                Path.Combine(
                    _reportDirectory,
                    "operations.csv"),
                false);
            _operationWriter.WriteLine(
                "name,started_utc,ended_utc,duration_ms,scene,start_used_memory_bytes,end_used_memory_bytes,used_memory_delta_bytes,start_managed_memory_bytes,end_managed_memory_bytes,managed_memory_delta_bytes,max_observed_frame_ms,max_observed_gc_allocated_bytes,success,detail");

            _gcAllocatedRecorder = StartRecorder(
                ProfilerCategory.Memory,
                "GC Allocated In Frame");
            _mainThreadRecorder = StartRecorder(
                ProfilerCategory.Internal,
                "Main Thread");
            _totalUsedMemoryRecorder = StartRecorder(
                ProfilerCategory.Memory,
                "Total Used Memory");
            _totalReservedMemoryRecorder = StartRecorder(
                ProfilerCategory.Memory,
                "Total Reserved Memory");

            _captureStartupAllocatedMemoryBytes =
                ReadAllocatedMemoryBytes();
            _captureStartupReservedMemoryBytes =
                ReadReservedMemoryBytes();

            double realtime = Time.realtimeSinceStartupAsDouble;
            _measurementStartRealtime =
                realtime + MeasurementWarmupSeconds;
            _nextSampleRealtime = _measurementStartRealtime;
            _nextFlushRealtime = realtime + FlushIntervalSeconds;
            CaptureLoad.ResetMaximums();

            SceneManager.sceneLoaded += HandleSceneLoaded;
            global::UnityEngine.Application.logMessageReceived += HandleLogMessage;

            WriteSessionInformation();
            RecordMarker("CaptureStarted");
            Debug.Log(
                "[S17-P3] Performance capture active. " +
                "F8 overlay, F9 export, F10 marker. Reports: " +
                _reportDirectory);
        }

        private void BeginMeasurement(
            long allocatedMemoryBytes,
            long reservedMemoryBytes,
            long managedMemoryBytes)
        {
            if (_measurementStarted)
            {
                return;
            }

            _initialAllocatedMemoryBytes = allocatedMemoryBytes;
            _initialReservedMemoryBytes = reservedMemoryBytes;
            _maximumAllocatedMemoryBytes = allocatedMemoryBytes;
            _maximumReservedMemoryBytes = reservedMemoryBytes;
            _maximumManagedMemoryBytes = managedMemoryBytes;
            _measurementStarted = true;
            RecordMarker("MeasurementStarted");
        }

        private int StartOperation(string operationName)
        {
            if (string.IsNullOrWhiteSpace(operationName))
            {
                operationName = "UnnamedOperation";
            }

            int id = _nextOperationId++;
            _activeOperations.Add(
                id,
                new ActiveOperation(
                    id,
                    operationName,
                    DateTime.UtcNow,
                    Stopwatch.GetTimestamp(),
                    SceneManager.GetActiveScene().name,
                    ReadAllocatedMemoryBytes(),
                    GC.GetTotalMemory(false)));
            return id;
        }

        private void EndOperation(
            int operationId,
            bool success,
            string detail)
        {
            if (!_activeOperations.TryGetValue(
                    operationId,
                    out ActiveOperation operation))
            {
                return;
            }

            _activeOperations.Remove(operationId);
            WriteOperation(
                operation,
                success,
                detail ?? string.Empty);
        }

        private void StartAsynchronousOperation(
            string operationName)
        {
            if (_asynchronousOperations.TryGetValue(
                    operationName,
                    out int previousId))
            {
                EndOperation(
                    previousId,
                    false,
                    "Replaced by a new operation with the same name.");
            }

            _asynchronousOperations[operationName] =
                StartOperation(operationName);
        }

        private void FinishAsynchronousOperation(
            string operationName)
        {
            if (!_asynchronousOperations.TryGetValue(
                    operationName,
                    out int operationId))
            {
                return;
            }

            _asynchronousOperations.Remove(operationName);
            EndOperation(operationId, true, string.Empty);
        }

        private void WriteOperation(
            ActiveOperation operation,
            bool success,
            string detail)
        {
            DateTime endedUtc = DateTime.UtcNow;
            long elapsedTicks =
                Stopwatch.GetTimestamp() -
                operation.StartTimestamp;
            double durationMilliseconds =
                elapsedTicks * 1000d /
                Stopwatch.Frequency;
            long endUsedMemoryBytes =
                ReadAllocatedMemoryBytes();
            long endManagedMemoryBytes =
                GC.GetTotalMemory(false);

            _operationWriter.WriteLine(
                string.Join(
                    ",",
                    Csv(operation.Name),
                    Csv(operation.StartedUtc.ToString(
                        "O",
                        CultureInfo.InvariantCulture)),
                    Csv(endedUtc.ToString(
                        "O",
                        CultureInfo.InvariantCulture)),
                    durationMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    Csv(operation.SceneName),
                    operation.StartUsedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    endUsedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    (endUsedMemoryBytes -
                     operation.StartUsedMemoryBytes).ToString(
                        CultureInfo.InvariantCulture),
                    operation.StartManagedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    endManagedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    (endManagedMemoryBytes -
                     operation.StartManagedMemoryBytes).ToString(
                        CultureInfo.InvariantCulture),
                    operation.MaximumObservedFrameMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    operation.MaximumObservedGcAllocatedBytes.ToString(
                        CultureInfo.InvariantCulture),
                    success ? "true" : "false",
                    Csv(detail)));
        }

        private void WriteMarker(string markerName)
        {
            ActiveOperation marker = new ActiveOperation(
                0,
                string.IsNullOrWhiteSpace(markerName)
                    ? "Marker"
                    : markerName,
                DateTime.UtcNow,
                Stopwatch.GetTimestamp(),
                SceneManager.GetActiveScene().name,
                ReadAllocatedMemoryBytes(),
                GC.GetTotalMemory(false));
            WriteOperation(marker, true, "Marker");
        }

        private void WritePeriodicSample(
            double realtime,
            long allocatedMemoryBytes,
            long reservedMemoryBytes,
            long managedMemoryBytes)
        {
            PerformanceMetricSnapshot snapshot =
                _metrics.CreateSnapshot();
            CaptureLoad load = CaptureLoad.Read();

            _sampleWriter.WriteLine(
                string.Join(
                    ",",
                    Csv(DateTime.UtcNow.ToString(
                        "O",
                        CultureInfo.InvariantCulture)),
                    realtime.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    Csv(SceneManager.GetActiveScene().name),
                    load.CurrentDay.ToString(
                        CultureInfo.InvariantCulture),
                    Csv(load.DayState),
                    load.SimulationSpeed.ToString(
                        "0.00",
                        CultureInfo.InvariantCulture),
                    load.ActiveCustomers.ToString(
                        CultureInfo.InvariantCulture),
                    load.QueueEntries.ToString(
                        CultureInfo.InvariantCulture),
                    load.ActiveDeliveryRuns.ToString(
                        CultureInfo.InvariantCulture),
                    load.ActiveOrders.ToString(
                        CultureInfo.InvariantCulture),
                    snapshot.FrameCount.ToString(
                        CultureInfo.InvariantCulture),
                    snapshot.AverageFps.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.AverageFrameMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.P95FrameMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.P99FrameMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.MaximumFrameMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.AverageMainThreadMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    snapshot.AverageGpuMilliseconds.ToString(
                        "0.000",
                        CultureInfo.InvariantCulture),
                    ReadRecorderValue(_gcAllocatedRecorder).ToString(
                        CultureInfo.InvariantCulture),
                    allocatedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    reservedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture),
                    managedMemoryBytes.ToString(
                        CultureInfo.InvariantCulture)));

        }

        private void WriteSessionInformation()
        {
            SessionInformation report =
                new SessionInformation
                {
                    captureVersion = "2",
                    startedUtc = _sessionStartedUtc,
                    buildGuid = global::UnityEngine.Application.buildGUID,
                    applicationVersion = global::UnityEngine.Application.version,
                    unityVersion = global::UnityEngine.Application.unityVersion,
                    platform = global::UnityEngine.Application.platform.ToString(),
                    operatingSystem = SystemInfo.operatingSystem,
                    processor = SystemInfo.processorType,
                    processorCount = SystemInfo.processorCount,
                    processorFrequencyMhz = SystemInfo.processorFrequency,
                    systemMemoryMb = SystemInfo.systemMemorySize,
                    graphicsDevice = SystemInfo.graphicsDeviceName,
                    graphicsDeviceType =
                        SystemInfo.graphicsDeviceType.ToString(),
                    graphicsMemoryMb = SystemInfo.graphicsMemorySize,
                    graphicsApiVersion =
                        SystemInfo.graphicsDeviceVersion,
                    graphicsMultiThreaded =
                        SystemInfo.graphicsMultiThreaded,
                    screenWidth = Screen.width,
                    screenHeight = Screen.height,
                    refreshRateRatio =
                        Screen.currentResolution.refreshRateRatio
                            .value.ToString(
                                "0.###",
                                CultureInfo.InvariantCulture),
                    fullScreenMode =
                        Screen.fullScreenMode.ToString(),
                    qualityLevel =
                        QualitySettings.GetQualityLevel(),
                    qualityName = ReadQualityName(),
                    renderPipeline =
                        QualitySettings.renderPipeline == null
                            ? "Built-in"
                            : QualitySettings.renderPipeline.name,
                    vSyncCount = QualitySettings.vSyncCount,
                    targetFrameRate =
                        global::UnityEngine.Application.targetFrameRate,
                    measurementWarmupSeconds =
                        MeasurementWarmupSeconds,
                    overlayHotkeys =
                        "F8 toggle, F9 export, F10 marker",
                    reportDirectory = _reportDirectory,
                    notes =
                        "QA-only instrumentation. The first five seconds and expanded-overlay frames are excluded. Invalid GPU timing values are discarded."
                };

            File.WriteAllText(
                Path.Combine(
                    _reportDirectory,
                    "session_info.json"),
                JsonUtility.ToJson(report, true));
        }

        private void WriteSummary()
        {
            if (_finalized)
            {
                return;
            }

            if (!_measurementStarted)
            {
                BeginMeasurement(
                    ReadAllocatedMemoryBytes(),
                    ReadReservedMemoryBytes(),
                    GC.GetTotalMemory(false));
            }

            PerformanceMetricSnapshot metrics =
                _metrics.CreateSnapshot();
            CaptureLoad load = CaptureLoad.Read();

            SummaryInformation summary =
                new SummaryInformation
                {
                    startedUtc = _sessionStartedUtc,
                    updatedUtc = DateTime.UtcNow.ToString(
                        "O",
                        CultureInfo.InvariantCulture),
                    activeScene =
                        SceneManager.GetActiveScene().name,
                    currentDay = load.CurrentDay,
                    dayState = load.DayState,
                    maximumObservedCustomers =
                        CaptureLoad.MaximumObservedCustomers,
                    maximumObservedQueueEntries =
                        CaptureLoad.MaximumObservedQueueEntries,
                    maximumObservedDeliveryRuns =
                        CaptureLoad.MaximumObservedDeliveryRuns,
                    maximumObservedActiveOrders =
                        CaptureLoad.MaximumObservedActiveOrders,
                    measurementWarmupSeconds =
                        MeasurementWarmupSeconds,
                    excludedWarmupFrameCount =
                        _excludedWarmupFrameCount,
                    excludedOverlayFrameCount =
                        _excludedOverlayFrameCount,
                    validGpuTimingSampleCount =
                        _validGpuTimingSampleCount,
                    invalidGpuTimingSampleCount =
                        _invalidGpuTimingSampleCount,
                    gpuTimingReliable =
                        _validGpuTimingSampleCount > 0L &&
                        _invalidGpuTimingSampleCount == 0L,
                    captureStartupUsedMemoryBytes =
                        _captureStartupAllocatedMemoryBytes,
                    captureStartupReservedMemoryBytes =
                        _captureStartupReservedMemoryBytes,
                    frameCount = metrics.FrameCount,
                    averageFps = metrics.AverageFps,
                    averageFrameMilliseconds =
                        metrics.AverageFrameMilliseconds,
                    minimumFrameMilliseconds =
                        metrics.MinimumFrameMilliseconds,
                    maximumFrameMilliseconds =
                        metrics.MaximumFrameMilliseconds,
                    p50FrameMilliseconds =
                        metrics.P50FrameMilliseconds,
                    p95FrameMilliseconds =
                        metrics.P95FrameMilliseconds,
                    p99FrameMilliseconds =
                        metrics.P99FrameMilliseconds,
                    framesOver16Milliseconds =
                        metrics.FramesOver16Milliseconds,
                    framesOver33Milliseconds =
                        metrics.FramesOver33Milliseconds,
                    framesOver50Milliseconds =
                        metrics.FramesOver50Milliseconds,
                    totalGcAllocatedBytes =
                        metrics.TotalGcAllocatedBytes,
                    maximumGcAllocatedBytesPerFrame =
                        metrics.MaximumGcAllocatedBytesPerFrame,
                    framesWithGcAllocations =
                        metrics.FramesWithGcAllocations,
                    averageMainThreadMilliseconds =
                        metrics.AverageMainThreadMilliseconds,
                    maximumMainThreadMilliseconds =
                        metrics.MaximumMainThreadMilliseconds,
                    averageGpuMilliseconds =
                        metrics.AverageGpuMilliseconds,
                    maximumGpuMilliseconds =
                        metrics.MaximumGpuMilliseconds,
                    initialUsedMemoryBytes =
                        _initialAllocatedMemoryBytes,
                    maximumUsedMemoryBytes =
                        _maximumAllocatedMemoryBytes,
                    usedMemoryGrowthBytes =
                        _maximumAllocatedMemoryBytes -
                        _initialAllocatedMemoryBytes,
                    initialReservedMemoryBytes =
                        _initialReservedMemoryBytes,
                    maximumReservedMemoryBytes =
                        _maximumReservedMemoryBytes,
                    reservedMemoryGrowthBytes =
                        _maximumReservedMemoryBytes -
                        _initialReservedMemoryBytes,
                    maximumManagedMemoryBytes =
                        _maximumManagedMemoryBytes,
                    warningCount = _warningCount,
                    errorCount = _errorCount,
                    activeOperationCount =
                        _activeOperations.Count,
                    targetFrameBudgetMilliseconds =
                        16.6666667d,
                    importantSpikeMilliseconds = 33.3333333d,
                    severeSpikeMilliseconds = 50d
                };

            string path = Path.Combine(
                _reportDirectory,
                "summary.json");
            string temporaryPath = path + ".tmp";
            File.WriteAllText(
                temporaryPath,
                JsonUtility.ToJson(summary, true));

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Move(temporaryPath, path);
        }

        private void FinalizeCapture()
        {
            if (_finalized)
            {
                return;
            }

            List<int> activeIds =
                new List<int>(_activeOperations.Keys);
            foreach (int id in activeIds)
            {
                EndOperation(
                    id,
                    false,
                    "Capture ended before the operation completed.");
            }

            _asynchronousOperations.Clear();
            WriteSummary();
            FlushWriters();
            _sampleWriter?.Dispose();
            _operationWriter?.Dispose();
            _sampleWriter = null;
            _operationWriter = null;
            _finalized = true;
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            EndAsynchronousOperation(
                "SceneLoad:" + scene.name);
            RecordMarker("SceneLoaded:" + scene.name);
        }

        private void HandleLogMessage(
            string condition,
            string stackTrace,
            LogType type)
        {
            if (type == LogType.Warning)
            {
                _warningCount++;
            }
            else if (type == LogType.Error ||
                     type == LogType.Exception ||
                     type == LogType.Assert)
            {
                _errorCount++;
            }
        }

        private double ReadGpuMilliseconds()
        {
            uint count = FrameTimingManager.GetLatestTimings(
                1,
                _frameTimings);
            if (count == 0U)
            {
                return 0d;
            }

            double rawMilliseconds =
                _frameTimings[0].gpuFrameTime;
            double sanitizedMilliseconds =
                PerformanceCaptureValueSanitizer
                    .SanitizeGpuMilliseconds(
                        rawMilliseconds);

            if (sanitizedMilliseconds > 0d)
            {
                _validGpuTimingSampleCount++;
            }
            else if (rawMilliseconds > 0d)
            {
                _invalidGpuTimingSampleCount++;
            }

            return sanitizedMilliseconds;
        }

        private long ReadAllocatedMemoryBytes()
        {
            long recorded = ReadRecorderValue(
                _totalUsedMemoryRecorder);
            return recorded > 0L
                ? recorded
                : Profiler.GetTotalAllocatedMemoryLong();
        }

        private long ReadReservedMemoryBytes()
        {
            long recorded = ReadRecorderValue(
                _totalReservedMemoryRecorder);
            return recorded > 0L
                ? recorded
                : Profiler.GetTotalReservedMemoryLong();
        }

        private static ProfilerRecorder StartRecorder(
            ProfilerCategory category,
            string statName)
        {
            try
            {
                return ProfilerRecorder.StartNew(
                    category,
                    statName,
                    1);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "[S17-P3] Profiler counter unavailable: " +
                    statName + ". " + exception.Message);
                return default(ProfilerRecorder);
            }
        }

        private static long ReadRecorderValue(
            ProfilerRecorder recorder)
        {
            return recorder.Valid
                ? Math.Max(0L, recorder.LastValue)
                : 0L;
        }

        private static void DisposeRecorder(
            ref ProfilerRecorder recorder)
        {
            if (recorder.Valid)
            {
                recorder.Dispose();
            }
            recorder = default(ProfilerRecorder);
        }

        private static string CreateReportDirectory()
        {
            string stamp = DateTime.UtcNow.ToString(
                "yyyyMMdd_HHmmss_fff",
                CultureInfo.InvariantCulture);
            return Path.Combine(
                global::UnityEngine.Application.persistentDataPath,
                ReportFolderName,
                stamp);
        }

        private static string ReadQualityName()
        {
            int index = QualitySettings.GetQualityLevel();
            string[] names = QualitySettings.names;
            return index >= 0 && index < names.Length
                ? names[index]
                : "Unknown";
        }

        private static double BytesToMegabytes(long bytes)
        {
            return bytes / (1024d * 1024d);
        }

        private static string Csv(string value)
        {
            string safe = value ?? string.Empty;
            return "\"" + safe.Replace("\"", "\"\"") + "\"";
        }

        private void FlushWriters()
        {
            _sampleWriter?.Flush();
            _operationWriter?.Flush();
        }

        private void EnsureGuiStyles()
        {
            if (_labelStyle != null)
            {
                return;
            }

            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                wordWrap = true
            };
            _labelStyle.normal.textColor = Color.white;

            _headerStyle = new GUIStyle(_labelStyle)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold
            };
            _headerStyle.normal.textColor =
                new Color(0.35f, 1f, 0.62f, 1f);
        }

        private sealed class ActiveOperation
        {
            public int Id { get; }
            public string Name { get; }
            public DateTime StartedUtc { get; }
            public long StartTimestamp { get; }
            public string SceneName { get; }
            public long StartUsedMemoryBytes { get; }
            public long StartManagedMemoryBytes { get; }
            public double MaximumObservedFrameMilliseconds { get; set; }
            public long MaximumObservedGcAllocatedBytes { get; set; }

            public ActiveOperation(
                int id,
                string name,
                DateTime startedUtc,
                long startTimestamp,
                string sceneName,
                long startUsedMemoryBytes,
                long startManagedMemoryBytes)
            {
                Id = id;
                Name = name;
                StartedUtc = startedUtc;
                StartTimestamp = startTimestamp;
                SceneName = sceneName;
                StartUsedMemoryBytes =
                    startUsedMemoryBytes;
                StartManagedMemoryBytes =
                    startManagedMemoryBytes;
            }
        }

        private sealed class OperationScope : IDisposable
        {
            private Sprint17Phase3PerformanceCapture _capture;
            private readonly int _operationId;

            public OperationScope(
                Sprint17Phase3PerformanceCapture capture,
                int operationId)
            {
                _capture = capture;
                _operationId = operationId;
            }

            public void Dispose()
            {
                Sprint17Phase3PerformanceCapture capture =
                    _capture;
                if (capture == null)
                {
                    return;
                }

                _capture = null;
                capture.EndOperation(
                    _operationId,
                    true,
                    string.Empty);
            }
        }

        private sealed class NoOperationScope : IDisposable
        {
            public static readonly NoOperationScope Instance =
                new NoOperationScope();

            public void Dispose()
            {
            }
        }

        private struct CaptureLoad
        {
            private static int _maximumObservedCustomers;
            private static int _maximumObservedQueueEntries;
            private static int _maximumObservedDeliveryRuns;
            private static int _maximumObservedActiveOrders;

            public int CurrentDay;
            public string DayState;
            public float SimulationSpeed;
            public int ActiveCustomers;
            public int QueueEntries;
            public int ActiveDeliveryRuns;
            public int ActiveOrders;

            public static int MaximumObservedCustomers =>
                _maximumObservedCustomers;
            public static int MaximumObservedQueueEntries =>
                _maximumObservedQueueEntries;
            public static int MaximumObservedDeliveryRuns =>
                _maximumObservedDeliveryRuns;
            public static int MaximumObservedActiveOrders =>
                _maximumObservedActiveOrders;

            public static void ResetMaximums()
            {
                _maximumObservedCustomers = 0;
                _maximumObservedQueueEntries = 0;
                _maximumObservedDeliveryRuns = 0;
                _maximumObservedActiveOrders = 0;
            }

            public static CaptureLoad Read()
            {
                CaptureLoad load = new CaptureLoad
                {
                    DayState = string.Empty,
                    SimulationSpeed = 1f
                };
                UIRuntimeCompositionRoot root =
                    UIRuntimeCompositionRoot.Instance;

                if (root == null ||
                    root.ActiveSession == null ||
                    !root.ActiveSession.HasActiveSession)
                {
                    return load;
                }

                IntegratedGameStateSnapshot snapshot =
                    root.ActiveSession.Snapshot;
                load.CurrentDay = snapshot.CurrentDay;
                load.DayState = snapshot.DayCycle.State;
                load.SimulationSpeed =
                    snapshot.DayCycle.SimulationSpeedMultiplier;

                if (!string.Equals(
                        SceneManager.GetActiveScene().name,
                        "StoreInitial",
                        StringComparison.Ordinal))
                {
                    return load;
                }

                foreach (CustomerSaveRecord customer
                         in snapshot.Customers)
                {
                    if (PerformanceCaptureValueSanitizer
                        .IsActiveCustomerState(customer.State))
                    {
                        load.ActiveCustomers++;
                    }
                }

                foreach (CheckoutQueueEntrySaveRecord entry
                         in snapshot.QueueEntries)
                {
                    if (PerformanceCaptureValueSanitizer
                        .IsActiveQueueState(entry.State))
                    {
                        load.QueueEntries++;
                    }
                }

                StoreOperationsState state =
                    root.StoreManagementStateProvider == null
                        ? null
                        : root.StoreManagementStateProvider
                            .ManagementState;
                if (state != null)
                {
                    foreach (StoreOrderRecord order
                             in state.Orders)
                    {
                        if (PerformanceCaptureValueSanitizer
                            .IsActiveOrderState(order.State))
                        {
                            load.ActiveOrders++;
                        }
                    }

                    foreach (StoreDeliveryRunRecord run
                             in state.DeliveryRuns)
                    {
                        if (run.Status ==
                            StoreDeliveryRunStatus.InTransit)
                        {
                            load.ActiveDeliveryRuns++;
                        }
                    }
                }

                _maximumObservedCustomers = Math.Max(
                    _maximumObservedCustomers,
                    load.ActiveCustomers);
                _maximumObservedQueueEntries = Math.Max(
                    _maximumObservedQueueEntries,
                    load.QueueEntries);
                _maximumObservedDeliveryRuns = Math.Max(
                    _maximumObservedDeliveryRuns,
                    load.ActiveDeliveryRuns);
                _maximumObservedActiveOrders = Math.Max(
                    _maximumObservedActiveOrders,
                    load.ActiveOrders);
                return load;
            }
        }

        public static class PerformanceCaptureValueSanitizer
        {
            private const double MaximumGpuFrameMilliseconds =
                10000d;

            public static double SanitizeGpuMilliseconds(
                double value)
            {
                if (double.IsNaN(value) ||
                    double.IsInfinity(value) ||
                    value <= 0d ||
                    value > MaximumGpuFrameMilliseconds)
                {
                    return 0d;
                }

                return value;
            }

            public static bool IsActiveCustomerState(
                string state)
            {
                return !string.IsNullOrWhiteSpace(state) &&
                       !string.Equals(
                           state,
                           CustomerState.Despawned.ToString(),
                           StringComparison.Ordinal);
            }

            public static bool IsActiveQueueState(
                string state)
            {
                return string.Equals(
                           state,
                           CheckoutQueueEntryState.Waiting
                               .ToString(),
                           StringComparison.Ordinal) ||
                       string.Equals(
                           state,
                           CheckoutQueueEntryState.Called
                               .ToString(),
                           StringComparison.Ordinal) ||
                       string.Equals(
                           state,
                           CheckoutQueueEntryState.Processing
                               .ToString(),
                           StringComparison.Ordinal);
            }

            public static bool IsActiveOrderState(
                StoreOrderStatus state)
            {
                return state == StoreOrderStatus.Reserved ||
                       state == StoreOrderStatus.InTransit;
            }
        }

        [Serializable]
        private sealed class SessionInformation
        {
            public string captureVersion;
            public string startedUtc;
            public string buildGuid;
            public string applicationVersion;
            public string unityVersion;
            public string platform;
            public string operatingSystem;
            public string processor;
            public int processorCount;
            public int processorFrequencyMhz;
            public int systemMemoryMb;
            public string graphicsDevice;
            public string graphicsDeviceType;
            public int graphicsMemoryMb;
            public string graphicsApiVersion;
            public bool graphicsMultiThreaded;
            public int screenWidth;
            public int screenHeight;
            public string refreshRateRatio;
            public string fullScreenMode;
            public int qualityLevel;
            public string qualityName;
            public string renderPipeline;
            public int vSyncCount;
            public int targetFrameRate;
            public double measurementWarmupSeconds;
            public string overlayHotkeys;
            public string reportDirectory;
            public string notes;
        }

        [Serializable]
        private sealed class SummaryInformation
        {
            public string startedUtc;
            public string updatedUtc;
            public string activeScene;
            public int currentDay;
            public string dayState;
            public int maximumObservedCustomers;
            public int maximumObservedQueueEntries;
            public int maximumObservedDeliveryRuns;
            public int maximumObservedActiveOrders;
            public double measurementWarmupSeconds;
            public long excludedWarmupFrameCount;
            public long excludedOverlayFrameCount;
            public long validGpuTimingSampleCount;
            public long invalidGpuTimingSampleCount;
            public bool gpuTimingReliable;
            public long captureStartupUsedMemoryBytes;
            public long captureStartupReservedMemoryBytes;
            public long frameCount;
            public double averageFps;
            public double averageFrameMilliseconds;
            public double minimumFrameMilliseconds;
            public double maximumFrameMilliseconds;
            public double p50FrameMilliseconds;
            public double p95FrameMilliseconds;
            public double p99FrameMilliseconds;
            public long framesOver16Milliseconds;
            public long framesOver33Milliseconds;
            public long framesOver50Milliseconds;
            public long totalGcAllocatedBytes;
            public long maximumGcAllocatedBytesPerFrame;
            public long framesWithGcAllocations;
            public double averageMainThreadMilliseconds;
            public double maximumMainThreadMilliseconds;
            public double averageGpuMilliseconds;
            public double maximumGpuMilliseconds;
            public long initialUsedMemoryBytes;
            public long maximumUsedMemoryBytes;
            public long usedMemoryGrowthBytes;
            public long initialReservedMemoryBytes;
            public long maximumReservedMemoryBytes;
            public long reservedMemoryGrowthBytes;
            public long maximumManagedMemoryBytes;
            public int warningCount;
            public int errorCount;
            public int activeOperationCount;
            public double targetFrameBudgetMilliseconds;
            public double importantSpikeMilliseconds;
            public double severeSpikeMilliseconds;
        }
    }
}
