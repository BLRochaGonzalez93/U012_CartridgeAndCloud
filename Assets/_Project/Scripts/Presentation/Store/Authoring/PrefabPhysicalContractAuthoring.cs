using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Declares the physical responsibility of an authored wrapper prefab.
    /// The root convention is base-centre, identity rotation/scale and +Z front.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrefabPhysicalContractAuthoring : MonoBehaviour
    {
        public enum PhysicalRole
        {
            Decorative = 0,
            WalkableVisual = 1,
            StaticCollisionDelegatedToEnvironment = 2,
            InteractivePhysical = 3,
            DisplayOnly = 4,
            FutureContent = 5
        }

        [SerializeField]
        private PhysicalRole _role;

        [SerializeField]
        private bool _pivotAtBaseCentre = true;

        [SerializeField]
        private bool _positiveZIsFront = true;

        public PhysicalRole Role => _role;
        public bool PivotAtBaseCentre => _pivotAtBaseCentre;
        public bool PositiveZIsFront => _positiveZIsFront;

        public bool TryValidate(out string report)
        {
            if (!_pivotAtBaseCentre || !_positiveZIsFront)
            {
                report = $"'{name}' does not use the base-centre/+Z prefab convention.";
                return false;
            }

            if (transform.localPosition != Vector3.zero ||
                transform.localRotation != Quaternion.identity ||
                transform.localScale != Vector3.one)
            {
                report = $"'{name}' root transform must remain at identity.";
                return false;
            }

            if (_role == PhysicalRole.InteractivePhysical &&
                GetComponentInChildren<Collider>(true) == null)
            {
                report = $"Interactive prefab '{name}' requires authored collision.";
                return false;
            }

            report = $"Physical contract for '{name}' is valid ({_role}).";
            return true;
        }

        public void ValidateOrThrow()
        {
            if (!TryValidate(out string report))
            {
                throw new InvalidOperationException(report);
            }
        }
    }
}
