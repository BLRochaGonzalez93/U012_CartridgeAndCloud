using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.Characters;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Doors
{
    /// <summary>
    /// Trigger-backed sensor owned by the automatic-door prefab. A kinematic
    /// rigidbody on the sensor object guarantees trigger messages without
    /// requiring physics bodies on NavMesh-driven characters.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public sealed class AutomaticDoorSensor : MonoBehaviour
    {
        private readonly HashSet<CharacterPresence> _characters =
            new HashSet<CharacterPresence>();

        public bool HasCharacter => _characters.Count > 0;

        public void Configure(float depth)
        {
            if (depth <= 0f)
            {
                return;
            }

            BoxCollider trigger = GetComponent<BoxCollider>();
            trigger.isTrigger = true;
            Vector3 size = trigger.size;
            size.z = Mathf.Max(size.z, depth * 2f);
            trigger.size = size;
        }

        private void OnTriggerEnter(Collider other)
        {
            CharacterPresence presence =
                other.GetComponentInParent<CharacterPresence>();
            if (presence != null)
            {
                _characters.Add(presence);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterPresence presence =
                other.GetComponentInParent<CharacterPresence>();
            if (presence != null)
            {
                _characters.Remove(presence);
            }
        }

        private void OnDisable()
        {
            _characters.Clear();
        }
    }
}
