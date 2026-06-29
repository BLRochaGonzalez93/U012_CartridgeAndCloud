using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class Sprint15RuntimeStateBridge :
        MonoBehaviour
    {
        public bool HasRuntime =>
            Sprint15RuntimeCompositionRoot.Instance != null;

        public void Publish(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            Sprint15RuntimeCompositionRoot root =
                Sprint15RuntimeCompositionRoot.Instance;

            if (root == null)
            {
                throw new InvalidOperationException(
                    "Sprint 15 runtime is not installed.");
            }

            root.PublishAuthoritativeSnapshot(snapshot);
        }

        public DailyAutosaveResult PublishClosedDay(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "The snapshot day must be Closed.",
                    nameof(snapshot));
            }

            Sprint15RuntimeCompositionRoot root =
                Sprint15RuntimeCompositionRoot.Instance;

            if (root == null)
            {
                throw new InvalidOperationException(
                    "Sprint 15 runtime is not installed.");
            }

            return root.PublishAuthoritativeSnapshot(
                snapshot);
        }
    }
}
