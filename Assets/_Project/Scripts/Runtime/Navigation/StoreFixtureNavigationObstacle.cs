using UnityEngine;
using UnityEngine.AI;

namespace VRMGames.CartridgeAndCloud.Runtime.Navigation
{
    /// <summary>
    /// Synchronizes the serialized NavMeshObstacle with the authored BoxCollider.
    /// It never derives dimensions from renderers or creates missing components.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider), typeof(NavMeshObstacle))]
    public sealed class StoreFixtureNavigationObstacle : MonoBehaviour
    {
        [SerializeField]
        private bool _carveOnlyStationary = true;

        private void Awake()
        {
            ApplyAuthoredShape();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyAuthoredShape();
        }
#endif

        private void ApplyAuthoredShape()
        {
            BoxCollider box = GetComponent<BoxCollider>();
            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            if (box == null || obstacle == null)
            {
                return;
            }

            obstacle.shape = NavMeshObstacleShape.Box;
            obstacle.center = box.center;
            obstacle.size = box.size;
            obstacle.carving = true;
            obstacle.carveOnlyStationary = _carveOnlyStationary;
        }
    }
}
