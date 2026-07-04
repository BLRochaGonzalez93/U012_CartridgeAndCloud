using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

using VRMGames.CartridgeAndCloud.Runtime.Composition;
namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    public sealed class StoreUiStateBridge :
        MonoBehaviour
    {
        public bool HasRuntime =>
            UIRuntimeCompositionRoot.Instance != null;

        public void Publish(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;

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

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;

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
