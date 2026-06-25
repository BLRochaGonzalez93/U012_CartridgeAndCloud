using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    public sealed class PlacedObjectView : MonoBehaviour
    {
        private PlacementInstanceId _id;

        public PlacementInstanceId Id => _id;

        public void Configure(PlacementInstanceId id)
        {
            _id = id;
            name = $"Placed_{id.Value}";
        }
    }
}
