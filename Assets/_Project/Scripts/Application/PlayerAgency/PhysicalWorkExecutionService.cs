using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.PlayerAgency;

namespace VRMGames.CartridgeAndCloud.Application.PlayerAgency
{
    public readonly struct PhysicalWorkPreflightResult
    {
        public bool Allowed { get; }
        public string Reason { get; }

        private PhysicalWorkPreflightResult(
            bool allowed,
            string reason)
        {
            Allowed = allowed;
            Reason = reason ?? string.Empty;
        }

        public static PhysicalWorkPreflightResult Pass()
        {
            return new PhysicalWorkPreflightResult(
                true,
                string.Empty);
        }

        public static PhysicalWorkPreflightResult Reject(
            string reason)
        {
            return new PhysicalWorkPreflightResult(
                false,
                string.IsNullOrWhiteSpace(reason)
                    ? "Physical work preflight rejected."
                    : reason.Trim());
        }
    }

    public readonly struct PhysicalWorkBeginResult
    {
        public bool Started { get; }
        public PhysicalWorkExecution Execution { get; }
        public string Reason { get; }

        public PhysicalWorkBeginResult(
            bool started,
            PhysicalWorkExecution execution,
            string reason)
        {
            Started = started;
            Execution = execution;
            Reason = reason ?? string.Empty;
        }
    }

    public readonly struct PhysicalWorkTransitionResult
    {
        public bool Succeeded { get; }
        public PhysicalWorkState State { get; }
        public string Reason { get; }

        public PhysicalWorkTransitionResult(
            bool succeeded,
            PhysicalWorkState state,
            string reason)
        {
            Succeeded = succeeded;
            State = state;
            Reason = reason ?? string.Empty;
        }
    }

    public interface IPhysicalWorkHandler
    {
        PhysicalWorkKind Kind { get; }

        PhysicalWorkPreflightResult Preflight(
            PhysicalWorkRequest request);

        PhysicalWorkTransitionResult Begin(
            PhysicalWorkExecution execution);

        PhysicalWorkTransitionResult Complete(
            PhysicalWorkExecution execution);

        void Cancel(PhysicalWorkExecution execution);
    }

    /// <summary>
    /// Session-scoped lifecycle authority for physical work. This Phase
    /// deliberately serializes work per actor but does not reserve targets;
    /// cross-actor target/resource reservations belong to S20G.
    /// </summary>
    public sealed class PhysicalWorkExecutionService
    {
        private readonly Dictionary<PhysicalWorkKind, IPhysicalWorkHandler>
            _handlers =
                new Dictionary<PhysicalWorkKind, IPhysicalWorkHandler>();

        private readonly Dictionary<PhysicalWorkId, PhysicalWorkExecution>
            _activeById =
                new Dictionary<PhysicalWorkId, PhysicalWorkExecution>();

        private readonly Dictionary<PhysicalWorkActorRef, PhysicalWorkId>
            _activeByActor =
                new Dictionary<PhysicalWorkActorRef, PhysicalWorkId>();

        public int ActiveCount => _activeById.Count;

        public void RegisterHandler(IPhysicalWorkHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (handler.Kind == PhysicalWorkKind.None)
            {
                throw new ArgumentException(
                    "Physical work handler kind cannot be None.",
                    nameof(handler));
            }

            if (_handlers.ContainsKey(handler.Kind))
            {
                throw new InvalidOperationException(
                    "A physical work handler is already registered for " +
                    handler.Kind + ".");
            }

            _handlers.Add(handler.Kind, handler);
        }

        public PhysicalWorkPreflightResult Preflight(
            PhysicalWorkRequest request)
        {
            if (!_handlers.TryGetValue(
                    request.Kind,
                    out IPhysicalWorkHandler handler))
            {
                return PhysicalWorkPreflightResult.Reject(
                    "No handler is registered for " +
                    request.Kind + ".");
            }

            if (_activeByActor.ContainsKey(request.Actor))
            {
                return PhysicalWorkPreflightResult.Reject(
                    "Actor already has an active physical work item.");
            }

            return handler.Preflight(request);
        }

        public PhysicalWorkBeginResult TryBegin(
            PhysicalWorkRequest request)
        {
            PhysicalWorkPreflightResult preflight =
                Preflight(request);

            if (!preflight.Allowed)
            {
                return new PhysicalWorkBeginResult(
                    false,
                    null,
                    preflight.Reason);
            }

            IPhysicalWorkHandler handler =
                _handlers[request.Kind];

            PhysicalWorkExecution execution =
                new PhysicalWorkExecution(
                    PhysicalWorkId.New(),
                    request);

            PhysicalWorkTransitionResult begin =
                handler.Begin(execution);

            if (!begin.Succeeded)
            {
                return new PhysicalWorkBeginResult(
                    false,
                    null,
                    begin.Reason);
            }

            execution.State = PhysicalWorkState.Executing;
            _activeById.Add(execution.WorkId, execution);
            _activeByActor.Add(
                execution.Request.Actor,
                execution.WorkId);

            return new PhysicalWorkBeginResult(
                true,
                execution,
                string.Empty);
        }

        public PhysicalWorkTransitionResult TryComplete(
            PhysicalWorkId workId)
        {
            if (!_activeById.TryGetValue(
                    workId,
                    out PhysicalWorkExecution execution))
            {
                return new PhysicalWorkTransitionResult(
                    false,
                    PhysicalWorkState.None,
                    "Physical work is not active.");
            }

            IPhysicalWorkHandler handler =
                _handlers[execution.Request.Kind];

            PhysicalWorkTransitionResult completion =
                handler.Complete(execution);

            if (!completion.Succeeded)
            {
                return completion;
            }

            execution.State = PhysicalWorkState.Completed;
            RemoveActive(execution);

            return new PhysicalWorkTransitionResult(
                true,
                PhysicalWorkState.Completed,
                string.Empty);
        }

        public PhysicalWorkTransitionResult TryCancel(
            PhysicalWorkId workId)
        {
            if (!_activeById.TryGetValue(
                    workId,
                    out PhysicalWorkExecution execution))
            {
                return new PhysicalWorkTransitionResult(
                    false,
                    PhysicalWorkState.None,
                    "Physical work is not active.");
            }

            IPhysicalWorkHandler handler =
                _handlers[execution.Request.Kind];

            handler.Cancel(execution);
            execution.State = PhysicalWorkState.Cancelled;
            RemoveActive(execution);

            return new PhysicalWorkTransitionResult(
                true,
                PhysicalWorkState.Cancelled,
                string.Empty);
        }

        public bool TryGetActiveForActor(
            PhysicalWorkActorRef actor,
            out PhysicalWorkExecution execution)
        {
            execution = null;

            if (!_activeByActor.TryGetValue(
                    actor,
                    out PhysicalWorkId workId))
            {
                return false;
            }

            return _activeById.TryGetValue(
                workId,
                out execution);
        }

        private void RemoveActive(
            PhysicalWorkExecution execution)
        {
            _activeById.Remove(execution.WorkId);
            _activeByActor.Remove(execution.Request.Actor);
        }
    }

    /// <summary>
    /// Lightweight adapter for work whose business side effects are owned by
    /// the composition layer or a later concrete handler. It is useful for
    /// routing already-existing interactions through the common lifecycle
    /// without duplicating their authority.
    /// </summary>
    public sealed class DelegatePhysicalWorkHandler :
        IPhysicalWorkHandler
    {
        private readonly Func<
            PhysicalWorkRequest,
            PhysicalWorkPreflightResult> _preflight;

        public PhysicalWorkKind Kind { get; }

        public DelegatePhysicalWorkHandler(
            PhysicalWorkKind kind,
            Func<PhysicalWorkRequest, PhysicalWorkPreflightResult>
                preflight = null)
        {
            if (kind == PhysicalWorkKind.None)
            {
                throw new ArgumentOutOfRangeException(nameof(kind));
            }

            Kind = kind;
            _preflight = preflight;
        }

        public PhysicalWorkPreflightResult Preflight(
            PhysicalWorkRequest request)
        {
            return _preflight == null
                ? PhysicalWorkPreflightResult.Pass()
                : _preflight(request);
        }

        public PhysicalWorkTransitionResult Begin(
            PhysicalWorkExecution execution)
        {
            return new PhysicalWorkTransitionResult(
                true,
                PhysicalWorkState.Executing,
                string.Empty);
        }

        public PhysicalWorkTransitionResult Complete(
            PhysicalWorkExecution execution)
        {
            return new PhysicalWorkTransitionResult(
                true,
                PhysicalWorkState.Completed,
                string.Empty);
        }

        public void Cancel(PhysicalWorkExecution execution)
        {
        }
    }
}
