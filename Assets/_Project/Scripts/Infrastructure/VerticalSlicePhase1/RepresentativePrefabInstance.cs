using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class RepresentativePrefabInstance :
        MonoBehaviour
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private string _family;

        [SerializeField]
        private bool _conceptual;

        public string Id =>
            _id;

        public string Family =>
            _family;

        public bool Conceptual =>
            _conceptual;

        public void Configure(
            string id,
            string family,
            bool conceptual)
        {
            _id = id ?? string.Empty;
            _family =
                family ?? string.Empty;
            _conceptual = conceptual;
        }
    }
}
