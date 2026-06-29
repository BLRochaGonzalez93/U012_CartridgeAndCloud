using System;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public enum DailyAutosaveStatus
    {
        NotClosed = 0,
        AlreadySaved = 1,
        Saving = 2,
        Saved = 3,
        Failed = 4
    }

    public sealed class DailyAutosaveResult
    {
        public DailyAutosaveStatus Status { get; }
        public string DayId { get; }
        public string Detail { get; }

        public bool Succeeded =>
            Status == DailyAutosaveStatus.Saved ||
            Status ==
                DailyAutosaveStatus.AlreadySaved;

        public DailyAutosaveResult(
            DailyAutosaveStatus status,
            string dayId,
            string detail)
        {
            Status = status;
            DayId = dayId ?? string.Empty;
            Detail = detail ?? string.Empty;
        }
    }

    public sealed class DailyAutosaveService
    {
        private readonly IIntegratedSaveRepository
            _repository;
        private readonly IAutosaveMarkerRepository
            _markerRepository;
        private readonly ActiveGameSessionService
            _activeSession;

        private bool _saving;

        public DailyAutosaveStatus CurrentStatus {
            get;
            private set;
        } = DailyAutosaveStatus.NotClosed;

        public event Action<DailyAutosaveResult>
            Completed;

        public DailyAutosaveService(
            IIntegratedSaveRepository repository,
            IAutosaveMarkerRepository markerRepository,
            ActiveGameSessionService activeSession)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _markerRepository =
                markerRepository ??
                throw new ArgumentNullException(
                    nameof(markerRepository));
            _activeSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
        }

        public DailyAutosaveResult TryAutosave()
        {
            if (!_activeSession.HasActiveSession)
            {
                return Finish(
                    DailyAutosaveStatus.Failed,
                    string.Empty,
                    "No active session.");
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            string dayId = snapshot.DayCycle.DayId;

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                CurrentStatus =
                    DailyAutosaveStatus.NotClosed;
                return new DailyAutosaveResult(
                    CurrentStatus,
                    dayId,
                    "The store day is not closed.");
            }

            if (_saving)
            {
                return new DailyAutosaveResult(
                    DailyAutosaveStatus.Saving,
                    dayId,
                    "Autosave is already running.");
            }

            string lastSavedDay =
                _markerRepository.LoadLastSavedDay(
                    snapshot.SlotId);

            if (string.Equals(
                    lastSavedDay,
                    dayId,
                    StringComparison.Ordinal))
            {
                return Finish(
                    DailyAutosaveStatus.AlreadySaved,
                    dayId,
                    "This day has already been autosaved.");
            }

            _saving = true;
            CurrentStatus =
                DailyAutosaveStatus.Saving;

            try
            {
                IntegratedSaveRepositoryResult result =
                    _repository.Save(snapshot);

                if (!result.Succeeded)
                {
                    return Finish(
                        DailyAutosaveStatus.Failed,
                        dayId,
                        result.Detail);
                }

                _markerRepository.SaveLastSavedDay(
                    snapshot.SlotId,
                    dayId);

                return Finish(
                    DailyAutosaveStatus.Saved,
                    dayId,
                    "Autosave completed.");
            }
            catch (Exception exception)
            {
                return Finish(
                    DailyAutosaveStatus.Failed,
                    dayId,
                    exception.Message);
            }
            finally
            {
                _saving = false;
            }
        }

        private DailyAutosaveResult Finish(
            DailyAutosaveStatus status,
            string dayId,
            string detail)
        {
            CurrentStatus = status;

            DailyAutosaveResult result =
                new DailyAutosaveResult(
                    status,
                    dayId,
                    detail);

            Completed?.Invoke(result);
            return result;
        }
    }
}
