using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Grounding
{
    public static class GroundingUtility
    {
        private const float DefaultRayHeight = 4f;
        private const float DefaultRayDistance = 10f;

        public static bool AlignVisualRootToGround(
            Transform characterRoot,
            string visualRootName = "VisualRoot")
        {
            if (characterRoot == null)
            {
                return false;
            }

            Transform visualRoot = FindNamedChild(
                characterRoot,
                visualRootName);
            if (visualRoot == null)
            {
                visualRoot = characterRoot;
            }

            if (!TryGetRendererBounds(
                    visualRoot,
                    out Bounds bounds))
            {
                return false;
            }

            float delta = characterRoot.position.y - bounds.min.y;
            if (Mathf.Abs(delta) <= 0.0001f)
            {
                return true;
            }

            visualRoot.position += Vector3.up * delta;
            return true;
        }

        public static bool AlignChildrenToRootBase(
            Transform root,
            Renderer ignoredRootRenderer = null)
        {
            if (root == null)
            {
                return false;
            }

            Renderer[] renderers =
                root.GetComponentsInChildren<Renderer>(true);

            bool found = false;
            Bounds bounds = default;
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null ||
                    renderer == ignoredRootRenderer ||
                    !renderer.enabled)
                {
                    continue;
                }

                if (!found)
                {
                    bounds = renderer.bounds;
                    found = true;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            if (!found)
            {
                return false;
            }

            float rootHeight = Mathf.Abs(root.lossyScale.y);
            float desiredMinY = root.position.y - rootHeight * 0.5f;
            float delta = desiredMinY - bounds.min.y;
            if (Mathf.Abs(delta) <= 0.0001f)
            {
                return true;
            }

            for (int index = 0; index < root.childCount; index++)
            {
                Transform child = root.GetChild(index);
                child.position += Vector3.up * delta;
            }

            return true;
        }

        public static bool TrySnapRootToGround(
            Transform root,
            float rayHeight = DefaultRayHeight,
            float rayDistance = DefaultRayDistance)
        {
            if (root == null)
            {
                return false;
            }

            Vector3 origin = root.position + Vector3.up * rayHeight;
            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                Vector3.down,
                rayDistance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

            Array.Sort(
                hits,
                (left, right) => left.distance.CompareTo(right.distance));

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == root ||
                    hit.transform.IsChildOf(root))
                {
                    continue;
                }

                Vector3 position = root.position;
                position.y = hit.point.y;
                root.position = position;
                return true;
            }

            return false;
        }

        private static bool TryGetRendererBounds(
            Transform root,
            out Bounds bounds)
        {
            Renderer[] renderers =
                root.GetComponentsInChildren<Renderer>(true);

            bool found = false;
            bounds = default;
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null || !renderer.enabled)
                {
                    continue;
                }

                if (!found)
                {
                    bounds = renderer.bounds;
                    found = true;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            return found;
        }

        private static Transform FindNamedChild(
            Transform root,
            string childName)
        {
            foreach (Transform child in
                     root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(
                        child.name,
                        childName,
                        StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }
    }
}
