using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    public static class StaffOrientationUtility
    {
        public static bool TryResolveHorizontalRotation(
            Vector3 position,
            Vector3 target,
            out Quaternion rotation)
        {
            Vector3 direction = target - position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                rotation = Quaternion.identity;
                return false;
            }

            rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            return true;
        }
    }
}
