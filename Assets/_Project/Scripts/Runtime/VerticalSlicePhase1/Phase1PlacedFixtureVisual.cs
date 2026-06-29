using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1PlacedFixtureVisual :
        MonoBehaviour
    {
        public string DefinitionId {
            get;
            private set;
        }

        public string InstanceId {
            get;
            private set;
        }

        public void Configure(
            string definitionId,
            string instanceId)
        {
            DefinitionId =
                definitionId ?? string.Empty;
            InstanceId =
                instanceId ?? string.Empty;
        }
    }
}
