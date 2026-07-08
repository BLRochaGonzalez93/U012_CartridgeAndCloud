using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Declares the physical responsibility of an authored wrapper prefab.
    /// The root convention is base-centre, identity rotation/scale and +Z front.
    /// A direct GroundAnchor child can be used when the imported visual pivot
    /// cannot satisfy that convention without modifying the source mesh.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrefabPhysicalContractAuthoring : MonoBehaviour
    {
        public const string GroundAnchorName = "GroundAnchor";

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

        public Transform GroundAnchor =>
            transform.Find(GroundAnchorName);

        public bool HasGroundReference =>
            GroundAnchor != null ||
            _pivotAtBaseCentre;

        public bool TryGetGroundAnchor(
            out Transform groundAnchor)
        {
            groundAnchor = GroundAnchor;

            if (groundAnchor != null)
            {
                return true;
            }

            if (_pivotAtBaseCentre)
            {
                groundAnchor = transform;
                return true;
            }

            return false;
        }

        public bool TryValidate(out string report)
        {
            if (!_positiveZIsFront)
            {
                report =
                    $"'{name}' does not use the +Z front convention.";
                return false;
            }

            if (!IsIdentity(transform))
            {
                report =
                    $"'{name}' root transform must remain at identity.";
                return false;
            }

            Transform groundAnchor = GroundAnchor;
            if (groundAnchor != null)
            {
                if (groundAnchor.parent != transform)
                {
                    report =
                        $"'{name}' GroundAnchor must be a direct child of the prefab root.";
                    return false;
                }

                if (!Approximately(
                        groundAnchor.localRotation,
                        Quaternion.identity) ||
                    !Approximately(
                        groundAnchor.localScale,
                        Vector3.one))
                {
                    report =
                        $"'{name}' GroundAnchor must keep identity rotation and scale.";
                    return false;
                }
            }
            else if (!_pivotAtBaseCentre)
            {
                report =
                    $"'{name}' requires either a base-centre pivot or a direct GroundAnchor child.";
                return false;
            }

            if (_role == PhysicalRole.InteractivePhysical &&
                GetComponentInChildren<Collider>(true) == null)
            {
                report =
                    $"Interactive prefab '{name}' requires authored collision.";
                return false;
            }

            report =
                $"Physical contract for '{name}' is valid ({_role}).";
            return true;
        }

        public void ValidateOrThrow()
        {
            if (!TryValidate(out string report))
            {
                throw new InvalidOperationException(report);
            }
        }

        private static bool IsIdentity(Transform value)
        {
            return Approximately(
                       value.localPosition,
                       Vector3.zero) &&
                   Approximately(
                       value.localRotation,
                       Quaternion.identity) &&
                   Approximately(
                       value.localScale,
                       Vector3.one);
        }

        private static bool Approximately(
            Vector3 left,
            Vector3 right)
        {
            return Mathf.Approximately(left.x, right.x) &&
                   Mathf.Approximately(left.y, right.y) &&
                   Mathf.Approximately(left.z, right.z);
        }

        private static bool Approximately(
            Quaternion left,
            Quaternion right)
        {
            return Mathf.Abs(
                       Quaternion.Dot(left, right)) >=
                   0.99999f;
        }
    }
}
