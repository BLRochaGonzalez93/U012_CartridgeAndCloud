using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Interaction
{
    public enum WorldInteractionKind
    {
        Unknown = 0,
        Fixture = 1,
        Display = 2,
        Checkout = 3,
        Container = 4,
        Product = 5,
        Customer = 6,
        Employee = 7,
        Supplier = 8,
        Delivery = 9,
        Door = 10
    }

    public interface IWorldInteractionTarget
    {
        string InteractionId { get; }

        WorldInteractionKind InteractionKind { get; }

        Transform InteractionTransform { get; }

        float InteractionRange { get; }

        int InteractionPriority { get; }

        bool IsInteractionAvailable { get; }

        Vector3 GetInteractionPoint(
            Vector3 actorPosition);
    }

    public enum PlayerInteractionAttemptStatus
    {
        None = 0,
        NoTarget = 1,
        Unavailable = 2,
        OutOfRange = 3,
        Accepted = 4
    }

    public readonly struct PlayerInteractionAttempt
    {
        public PlayerInteractionAttemptStatus Status { get; }

        public string TargetId { get; }

        public WorldInteractionKind TargetKind { get; }

        public float Distance { get; }

        public float InteractionRange { get; }

        public bool Accepted =>
            Status == PlayerInteractionAttemptStatus.Accepted;

        public PlayerInteractionAttempt(
            PlayerInteractionAttemptStatus status,
            string targetId,
            WorldInteractionKind targetKind,
            float distance,
            float interactionRange)
        {
            Status = status;
            TargetId = targetId ?? string.Empty;
            TargetKind = targetKind;
            Distance = Mathf.Max(0f, distance);
            InteractionRange = Mathf.Max(0f, interactionRange);
        }
    }

    public static class WorldInteractionGeometry
    {
        public static Vector3 ResolveClosestPoint(
            Transform targetRoot,
            Vector3 actorPosition)
        {
            if (targetRoot == null)
            {
                return actorPosition;
            }

            Collider[] colliders =
                targetRoot.GetComponentsInChildren<Collider>(false);

            Vector3 bestPoint = targetRoot.position;
            float bestDistance = float.PositiveInfinity;

            foreach (Collider candidate in colliders)
            {
                if (candidate == null || !candidate.enabled)
                {
                    continue;
                }

                Vector3 point =
                    candidate.ClosestPoint(actorPosition);
                float distance =
                    PlanarSqrDistance(actorPosition, point);

                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                bestPoint = point;
            }

            return bestPoint;
        }

        public static float PlanarDistance(
            Vector3 from,
            Vector3 to)
        {
            return Mathf.Sqrt(
                PlanarSqrDistance(from, to));
        }

        private static float PlanarSqrDistance(
            Vector3 from,
            Vector3 to)
        {
            float x = from.x - to.x;
            float z = from.z - to.z;
            return x * x + z * z;
        }
    }
}
