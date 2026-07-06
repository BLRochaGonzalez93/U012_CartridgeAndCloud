using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Executes authored actor movement exclusively through a complete NavMesh
    /// path. It never falls back to direct Transform movement through geometry.
    /// </summary>
    internal static class NavMeshActorMovement
    {
        private const float StartSampleRadius = 3f;
        private const float DestinationSampleRadius = 1.25f;
        private const float ArrivalTolerance = 0.08f;
        private const float StuckMovementThreshold = 0.0025f;
        private const float StuckRetrySeconds = 2f;
        private const float MaximumTravelSeconds = 35f;
        private const int MaximumRepathAttempts = 3;

        internal sealed class Result
        {
            public bool Succeeded { get; private set; }
            public string FailureReason { get; private set; } = string.Empty;

            internal void Complete()
            {
                Succeeded = true;
                FailureReason = string.Empty;
            }

            internal void Fail(string reason)
            {
                Succeeded = false;
                FailureReason = reason ?? string.Empty;
            }
        }

        public static IEnumerator MoveTo(
            Transform actor,
            Vector3 destination,
            float speed,
            Result result)
        {
            if (result == null)
            {
                yield break;
            }

            result.Fail("Movement did not start.");

            if (actor == null)
            {
                result.Fail("The actor no longer exists.");
                yield break;
            }

            NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
            if (agent == null || !agent.enabled)
            {
                result.Fail("The actor has no enabled NavMeshAgent.");
                yield break;
            }

            if (!TryPlaceAgentOnNavMesh(agent, actor.position))
            {
                result.Fail("The actor could not be placed on the authored NavMesh.");
                yield break;
            }

            if (!NavMesh.SamplePosition(
                    destination,
                    out NavMeshHit destinationHit,
                    DestinationSampleRadius,
                    agent.areaMask))
            {
                result.Fail("The destination is outside the authored NavMesh.");
                yield break;
            }

            agent.speed = Mathf.Max(0.1f, speed);
            agent.stoppingDistance = Mathf.Max(0.1f, agent.stoppingDistance);
            agent.autoBraking = true;
            agent.autoRepath = true;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;

            if (!TryAssignCompletePath(agent, destinationHit.position))
            {
                StopAgent(agent);
                result.Fail("No complete NavMesh path exists to the destination.");
                yield break;
            }

            Vector3 previousPosition = actor.position;
            float stagnantSeconds = 0f;
            float travelSeconds = 0f;
            int repathAttempts = 0;

            while (actor != null && agent != null)
            {
                float deltaTime = Time.deltaTime;
                if (deltaTime <= 0f)
                {
                    yield return null;
                    continue;
                }

                travelSeconds += deltaTime;

                if (!agent.enabled || !agent.isOnNavMesh)
                {
                    result.Fail("The actor left the authored NavMesh while moving.");
                    yield break;
                }

                if (!agent.pathPending)
                {
                    if (agent.pathStatus != NavMeshPathStatus.PathComplete)
                    {
                        StopAgent(agent);
                        result.Fail("The NavMesh path became incomplete while moving.");
                        yield break;
                    }

                    float arrivalDistance =
                        agent.stoppingDistance + ArrivalTolerance;
                    if (agent.remainingDistance <= arrivalDistance)
                    {
                        StopAgent(agent);
                        result.Complete();
                        yield break;
                    }
                }

                Vector3 planarDelta = actor.position - previousPosition;
                planarDelta.y = 0f;
                previousPosition = actor.position;

                bool shouldBeMoving =
                    !agent.pathPending &&
                    agent.remainingDistance >
                    agent.stoppingDistance + ArrivalTolerance;

                if (shouldBeMoving &&
                    planarDelta.sqrMagnitude <
                    StuckMovementThreshold * StuckMovementThreshold)
                {
                    stagnantSeconds += deltaTime;
                }
                else
                {
                    stagnantSeconds = 0f;
                }

                if (stagnantSeconds >= StuckRetrySeconds)
                {
                    if (repathAttempts >= MaximumRepathAttempts ||
                        !TryAssignCompletePath(agent, destinationHit.position))
                    {
                        StopAgent(agent);
                        result.Fail("The actor remained blocked after repeated NavMesh repaths.");
                        yield break;
                    }

                    repathAttempts++;
                    stagnantSeconds = 0f;
                }

                if (travelSeconds >= MaximumTravelSeconds)
                {
                    StopAgent(agent);
                    result.Fail("The actor exceeded the maximum NavMesh travel time.");
                    yield break;
                }

                yield return null;
            }

            result.Fail("The actor was destroyed before reaching the destination.");
        }

        private static bool TryPlaceAgentOnNavMesh(
            NavMeshAgent agent,
            Vector3 requestedPosition)
        {
            if (agent.isOnNavMesh)
            {
                return true;
            }

            return NavMesh.SamplePosition(
                       requestedPosition,
                       out NavMeshHit startHit,
                       StartSampleRadius,
                       agent.areaMask) &&
                   agent.Warp(startHit.position);
        }

        private static bool TryAssignCompletePath(
            NavMeshAgent agent,
            Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            if (!agent.CalculatePath(destination, path) ||
                path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            agent.ResetPath();
            agent.isStopped = false;
            return agent.SetPath(path);
        }

        private static void StopAgent(NavMeshAgent agent)
        {
            if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            {
                return;
            }

            agent.isStopped = true;
            agent.ResetPath();
        }
    }
}
