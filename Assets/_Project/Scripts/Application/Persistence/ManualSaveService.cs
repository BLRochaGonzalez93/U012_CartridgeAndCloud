using System;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public enum ManualSaveStatus
    {
        Ready = 0,
        Saved = 1,
        NoActiveSession = 2,
        PauseRequired = 3,
        StateNotAllowed = 4,
        UnsafeSnapshot = 5,
        MutationPending = 6,
        ParticipantUnavailable = 7,
        ParticipantRejected = 8,
        Saving = 9,
        CheckpointFailure = 10,
        RepositoryFailure = 11
    }

    public sealed class ManualSaveEvaluation
    {
        public ManualSaveStatus Status { get; }

        public string Detail { get; }

        public bool Allowed =>
            Status == ManualSaveStatus.Ready;

        public ManualSaveEvaluation(
            ManualSaveStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }
    }

    public sealed class ManualSaveResult
    {
        public ManualSaveStatus Status { get; }

        public string Detail { get; }

        public bool Succeeded =>
            Status == ManualSaveStatus.Saved;

        public ManualSaveResult(
            ManualSaveStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }
    }

    public interface IManualSaveCheckpoint :
        IDisposable
    {
        void Commit();
    }

    public interface IManualSaveCheckpointParticipant
    {
        bool CanCheckpoint(out string reason);

        IManualSaveCheckpoint BeginCheckpoint();
    }

    public sealed class ManualSaveService
    {
        private readonly IIntegratedSaveRepository
            _repository;
        private readonly ActiveGameSessionService
            _activeSession;
        private readonly IPauseService _pauseService;
        private readonly ISaveMutationRegistry
            _mutations;
        private readonly IUtcClock _clock;

        private IManualSaveCheckpointParticipant
            _participant;
        private bool _saving;

        public ManualSaveStatus CurrentStatus {
            get;
            private set;
        } = ManualSaveStatus.StateNotAllowed;

        public event Action<ManualSaveResult> Completed;

        public ManualSaveService(
            IIntegratedSaveRepository repository,
            ActiveGameSessionService activeSession,
            IPauseService pauseService,
            ISaveMutationRegistry mutations,
            IUtcClock clock)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _activeSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
            _pauseService = pauseService ??
                throw new ArgumentNullException(
                    nameof(pauseService));
            _mutations = mutations ??
                throw new ArgumentNullException(
                    nameof(mutations));
            _clock = clock ??
                throw new ArgumentNullException(
                    nameof(clock));
        }

        public void RegisterCheckpointParticipant(
            IManualSaveCheckpointParticipant participant)
        {
            _participant = participant ??
                throw new ArgumentNullException(
                    nameof(participant));
        }

        public void UnregisterCheckpointParticipant(
            IManualSaveCheckpointParticipant participant)
        {
            if (ReferenceEquals(
                    _participant,
                    participant))
            {
                _participant = null;
            }
        }

        public ManualSaveEvaluation Evaluate()
        {
            if (!_activeSession.HasActiveSession)
            {
                return Evaluation(
                    ManualSaveStatus.NoActiveSession,
                    "No active session can be saved.");
            }

            if (!_pauseService.IsPaused)
            {
                return Evaluation(
                    ManualSaveStatus.PauseRequired,
                    "Open the pause menu before saving.");
            }

            if (_saving)
            {
                return Evaluation(
                    ManualSaveStatus.Saving,
                    "A manual save is already running.");
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (!IsAllowedState(
                    snapshot.DayCycle.State))
            {
                return Evaluation(
                    ManualSaveStatus.StateNotAllowed,
                    "Manual saving is available only in " +
                    "BeforeOpen, Closed, or Results.");
            }

            if (!IsSnapshotStable(
                    snapshot,
                    out string stabilityReason))
            {
                return Evaluation(
                    ManualSaveStatus.UnsafeSnapshot,
                    stabilityReason);
            }

            if (_mutations.HasPendingMutations)
            {
                return Evaluation(
                    ManualSaveStatus.MutationPending,
                    _mutations.DescribePendingMutations());
            }

            if (_participant == null)
            {
                return Evaluation(
                    ManualSaveStatus.ParticipantUnavailable,
                    "Store persistence is still initializing.");
            }

            try
            {
                if (!_participant.CanCheckpoint(
                        out string reason))
                {
                    return Evaluation(
                        ManualSaveStatus.ParticipantRejected,
                        string.IsNullOrWhiteSpace(reason)
                            ? "Store state is not ready to save."
                            : reason);
                }
            }
            catch (Exception exception)
            {
                return Evaluation(
                    ManualSaveStatus.ParticipantRejected,
                    exception.Message);
            }

            return Evaluation(
                ManualSaveStatus.Ready,
                "The current state can be saved safely.");
        }

        public ManualSaveResult Save()
        {
            ManualSaveEvaluation evaluation =
                Evaluate();

            if (!evaluation.Allowed)
            {
                return Finish(
                    evaluation.Status,
                    evaluation.Detail);
            }

            _saving = true;
            CurrentStatus = ManualSaveStatus.Saving;

            IManualSaveCheckpoint checkpoint = null;
            IDisposable mutation = null;

            try
            {
                mutation = _mutations.Begin(
                    "ManualSave");
                checkpoint =
                    _participant.BeginCheckpoint();

                IntegratedGameStateSnapshot snapshot =
                    IntegratedSnapshotStoreOperationsMutator
                        .Clone(
                            _activeSession.Snapshot,
                            _clock.UtcNow);

                IntegratedSaveRepositoryResult result =
                    _repository.Save(snapshot);

                if (!result.Succeeded)
                {
                    return Finish(
                        ManualSaveStatus.RepositoryFailure,
                        result.Detail);
                }

                checkpoint.Commit();
                _activeSession.Replace(snapshot);

                return Finish(
                    ManualSaveStatus.Saved,
                    "Manual save completed.");
            }
            catch (Exception exception)
            {
                return Finish(
                    checkpoint == null
                        ? ManualSaveStatus.CheckpointFailure
                        : ManualSaveStatus.RepositoryFailure,
                    exception.Message);
            }
            finally
            {
                checkpoint?.Dispose();
                mutation?.Dispose();
                _saving = false;
            }
        }

        private ManualSaveEvaluation Evaluation(
            ManualSaveStatus status,
            string detail)
        {
            CurrentStatus = status;
            return new ManualSaveEvaluation(
                status,
                detail);
        }

        private ManualSaveResult Finish(
            ManualSaveStatus status,
            string detail)
        {
            CurrentStatus = status;
            ManualSaveResult result =
                new ManualSaveResult(
                    status,
                    detail);
            Completed?.Invoke(result);
            return result;
        }

        private static bool IsAllowedState(
            string state)
        {
            return string.Equals(
                       state,
                       "BeforeOpen",
                       StringComparison.Ordinal) ||
                   string.Equals(
                       state,
                       "Closed",
                       StringComparison.Ordinal) ||
                   string.Equals(
                       state,
                       "Results",
                       StringComparison.Ordinal);
        }

        private static bool IsSnapshotStable(
            IntegratedGameStateSnapshot snapshot,
            out string reason)
        {
            if (string.Equals(
                    snapshot.CheckoutStation.State,
                    "Busy",
                    StringComparison.Ordinal))
            {
                reason =
                    "Wait for checkout processing to finish.";
                return false;
            }

            if (snapshot.QueueEntries.Count > 0)
            {
                reason =
                    "Wait for the checkout queue to drain.";
                return false;
            }

            foreach (CustomerSaveRecord customer in
                     snapshot.Customers)
            {
                if (!string.Equals(
                        customer.State,
                        "Despawned",
                        StringComparison.Ordinal))
                {
                    reason =
                        "Wait for active customers to leave.";
                    return false;
                }
            }

            foreach (ReservationSaveRecord reservation in
                     snapshot.Reservations)
            {
                if (string.Equals(
                        reservation.State,
                        "Active",
                        StringComparison.Ordinal))
                {
                    reason =
                        "Resolve active product reservations " +
                        "before saving.";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }
    }
}
