using System.Collections.Generic;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1WallOcclusionController :
        MonoBehaviour
    {
        public const string PlayerPrefsKey =
            "CC_S16_P1_HideOccludingWalls";

        private readonly HashSet<Renderer>
            _hidden =
                new HashSet<Renderer>();

        private Camera _camera;
        private Transform _target;

        public bool HideOccludingWalls {
            get;
            private set;
        }

        public void Configure(
            Camera camera,
            Transform target,
            bool defaultValue)
        {
            _camera = camera;
            _target = target;

            HideOccludingWalls =
                PlayerPrefs.GetInt(
                    PlayerPrefsKey,
                    defaultValue ? 1 : 0) == 1;
        }

        public void SetEnabled(bool enabled)
        {
            HideOccludingWalls = enabled;

            PlayerPrefs.SetInt(
                PlayerPrefsKey,
                enabled ? 1 : 0);
            PlayerPrefs.Save();

            if (!enabled)
            {
                RestoreAll();
            }
        }

        private void LateUpdate()
        {
            if (!HideOccludingWalls ||
                _camera == null ||
                _target == null)
            {
                RestoreAll();
                return;
            }

            RestoreAll();

            Vector3 direction =
                _target.position -
                _camera.transform.position;

            float distance =
                direction.magnitude;

            if (distance <= 0.01f)
            {
                return;
            }

            RaycastHit[] hits =
                Physics.RaycastAll(
                    _camera.transform.position,
                    direction.normalized,
                    distance);

            foreach (RaycastHit hit in hits)
            {
                Phase1OccludableWall wall =
                    hit.collider
                        .GetComponentInParent<
                            Phase1OccludableWall>();

                if (wall == null)
                {
                    continue;
                }

                Renderer[] renderers =
                    wall.GetComponentsInChildren<
                        Renderer>(true);

                foreach (Renderer renderer
                         in renderers)
                {
                    if (renderer == null ||
                        !renderer.enabled)
                    {
                        continue;
                    }

                    renderer.enabled = false;
                    _hidden.Add(renderer);
                }
            }
        }

        private void OnDisable()
        {
            RestoreAll();
        }

        private void RestoreAll()
        {
            foreach (Renderer renderer in _hidden)
            {
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }

            _hidden.Clear();
        }
    }
}
