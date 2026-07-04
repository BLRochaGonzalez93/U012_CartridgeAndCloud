using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.Placement
{
    public sealed class PlacedFixtureVisual :
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
