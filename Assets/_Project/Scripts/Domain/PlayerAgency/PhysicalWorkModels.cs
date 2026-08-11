using System;

namespace VRMGames.CartridgeAndCloud.Domain.PlayerAgency
{
    public enum PhysicalWorkKind
    {
        None = 0,
        InspectTarget = 1
    }

    public enum PhysicalWorkActorKind
    {
        Player = 1,
        Employee = 2
    }

    public enum PhysicalWorkState
    {
        None = 0,
        Executing = 1,
        Completed = 2,
        Cancelled = 3
    }

    public readonly struct PhysicalWorkId :
        IEquatable<PhysicalWorkId>
    {
        public Guid Value { get; }

        public bool IsEmpty => Value == Guid.Empty;

        public PhysicalWorkId(Guid value)
        {
            Value = value;
        }

        public static PhysicalWorkId New()
        {
            return new PhysicalWorkId(Guid.NewGuid());
        }

        public bool Equals(PhysicalWorkId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is PhysicalWorkId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString("N");
        }

        public static bool operator ==(
            PhysicalWorkId left,
            PhysicalWorkId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            PhysicalWorkId left,
            PhysicalWorkId right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct PhysicalWorkActorRef :
        IEquatable<PhysicalWorkActorRef>
    {
        public const string PlayerActorId = "player-owner";

        public PhysicalWorkActorKind Kind { get; }
        public string ActorId { get; }

        public PhysicalWorkActorRef(
            PhysicalWorkActorKind kind,
            string actorId)
        {
            if (kind == PhysicalWorkActorKind.Player)
            {
                actorId = string.IsNullOrWhiteSpace(actorId)
                    ? PlayerActorId
                    : actorId.Trim();
            }
            else if (string.IsNullOrWhiteSpace(actorId))
            {
                throw new ArgumentException(
                    "Employee work actor requires a stable id.",
                    nameof(actorId));
            }

            Kind = kind;
            ActorId = actorId?.Trim() ?? string.Empty;
        }

        public static PhysicalWorkActorRef Player =>
            new PhysicalWorkActorRef(
                PhysicalWorkActorKind.Player,
                PlayerActorId);

        public bool Equals(PhysicalWorkActorRef other)
        {
            return Kind == other.Kind &&
                   string.Equals(
                       ActorId,
                       other.ActorId,
                       StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PhysicalWorkActorRef other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Kind * 397) ^
                       StringComparer.Ordinal.GetHashCode(
                           ActorId ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return Kind + ":" + ActorId;
        }
    }

    public readonly struct PhysicalWorkTargetRef
    {
        public string TargetKind { get; }
        public string TargetId { get; }

        public PhysicalWorkTargetRef(
            string targetKind,
            string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetKind))
            {
                throw new ArgumentException(
                    "Physical work target kind is required.",
                    nameof(targetKind));
            }

            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Physical work target id is required.",
                    nameof(targetId));
            }

            TargetKind = targetKind.Trim();
            TargetId = targetId.Trim();
        }

        public override string ToString()
        {
            return TargetKind + ":" + TargetId;
        }
    }

    public readonly struct PhysicalWorkRequest
    {
        public PhysicalWorkKind Kind { get; }
        public PhysicalWorkActorRef Actor { get; }
        public PhysicalWorkTargetRef Target { get; }

        public PhysicalWorkRequest(
            PhysicalWorkKind kind,
            PhysicalWorkActorRef actor,
            PhysicalWorkTargetRef target)
        {
            if (kind == PhysicalWorkKind.None)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(kind));
            }

            Kind = kind;
            Actor = actor;
            Target = target;
        }
    }

    public sealed class PhysicalWorkExecution
    {
        public PhysicalWorkId WorkId { get; }
        public PhysicalWorkRequest Request { get; }
        public PhysicalWorkState State { get; internal set; }

        public PhysicalWorkExecution(
            PhysicalWorkId workId,
            PhysicalWorkRequest request)
        {
            if (workId.IsEmpty)
            {
                throw new ArgumentException(
                    "Physical work id cannot be empty.",
                    nameof(workId));
            }

            WorkId = workId;
            Request = request;
            State = PhysicalWorkState.Executing;
        }
    }
}
