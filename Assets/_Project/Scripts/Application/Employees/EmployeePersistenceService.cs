using System;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public sealed class EmployeePersistenceService : IDisposable
    {
        private readonly EmployeeHiringService _hiring;
        private readonly EmployeeScheduleService _schedule;
        private readonly EmployeePayrollService _payroll;
        private readonly IActiveGameSession _activeSession;
        private readonly ISaveMutationRegistry _mutations;
        private readonly IUtcClock _clock;
        private bool _synchronizing;
        private bool _disposed;

        public EmployeePersistenceService(
            EmployeeHiringService hiring,
            EmployeeScheduleService schedule,
            EmployeePayrollService payroll,
            IActiveGameSession activeSession,
            ISaveMutationRegistry mutations,
            IUtcClock clock)
        {
            _hiring = hiring ??
                throw new ArgumentNullException(nameof(hiring));
            _schedule = schedule ??
                throw new ArgumentNullException(nameof(schedule));
            _payroll = payroll ??
                throw new ArgumentNullException(nameof(payroll));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _mutations = mutations ??
                throw new ArgumentNullException(nameof(mutations));
            _clock = clock ??
                throw new ArgumentNullException(nameof(clock));

            _hiring.PersistenceChanged += HandleStateChanged;
            _schedule.StateChanged += HandleStateChanged;
            _payroll.StateChanged += HandleStateChanged;
        }

        public void RestoreFromActiveSnapshot()
        {
            ThrowIfDisposed();

            if (!_activeSession.HasActiveSession)
            {
                return;
            }

            EmployeeSystemSaveRecord state =
                _activeSession.Snapshot.EmployeeSystem ??
                EmployeeSystemSaveRecord.Empty();

            _synchronizing = true;
            try
            {
                _hiring.Restore(state);
                _schedule.RestoreSchedules(state.Schedules);
                _payroll.RestoreOutstandingObligations(
                    state.OutstandingSalaries);
            }
            finally
            {
                _synchronizing = false;
            }
        }

        public void SynchronizeSnapshot()
        {
            ThrowIfDisposed();

            if (_synchronizing || !_activeSession.HasActiveSession)
            {
                return;
            }

            _synchronizing = true;
            try
            {
                EmployeeSystemSaveRecord state = Capture();
                IntegratedGameStateSnapshot current =
                    _activeSession.Snapshot;
                IntegratedGameStateSnapshot next =
                    IntegratedSnapshotStoreOperationsMutator.Clone(
                        current,
                        ResolveUpdatedUtc(current),
                        employeeSystem: state);

                using (_mutations.Begin("employee-state-sync"))
                {
                    _activeSession.Replace(next);
                }
            }
            finally
            {
                _synchronizing = false;
            }
        }

        public EmployeeSystemSaveRecord Capture()
        {
            ThrowIfDisposed();

            return new EmployeeSystemSaveRecord(
                _hiring.PostingSequence,
                _hiring.HasPublishedAnyPosting,
                _hiring.CapturePostings(),
                _hiring.CaptureEmployees(),
                _schedule.CaptureSchedules(),
                _payroll.CaptureOutstandingObligations());
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _hiring.PersistenceChanged -= HandleStateChanged;
            _schedule.StateChanged -= HandleStateChanged;
            _payroll.StateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged()
        {
            SynchronizeSnapshot();
        }

        private DateTime ResolveUpdatedUtc(
            IntegratedGameStateSnapshot source)
        {
            DateTime now = _clock.UtcNow;
            return now >= source.UpdatedUtc
                ? now
                : source.UpdatedUtc;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    nameof(EmployeePersistenceService));
            }
        }
    }
}
