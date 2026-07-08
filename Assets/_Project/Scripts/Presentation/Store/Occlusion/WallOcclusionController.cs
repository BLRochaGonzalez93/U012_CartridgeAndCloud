using System.Collections.Generic;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion
{
    public sealed class WallOcclusionController :
        MonoBehaviour
    {
        public const string PlayerPrefsKey =
            "CC_S16_P1_HideOccludingWalls";

        private readonly HashSet<Renderer>
            _hidden =
                new HashSet<Renderer>();

        private UnityCamera _camera;
        private Transform _target;

        public bool HideOccludingWalls {
            get;
            private set;
        }

        public bool CanChangeVisibility {
            get;
            private set;
        }

        public void Configure(
            UnityCamera camera,
            Transform target,
            bool defaultValue)
        {
            Configure(
                camera,
                target,
                defaultValue,
                allowUserToggle: true);
        }

        public void Configure(
            UnityCamera camera,
            Transform target,
            bool defaultValue,
            bool allowUserToggle)
        {
            _camera = camera;
            _target = target;
            CanChangeVisibility =
                allowUserToggle;

            if (!CanChangeVisibility)
            {
                HideOccludingWalls = false;
                PlayerPrefs.SetInt(
                    PlayerPrefsKey,
                    0);
                PlayerPrefs.Save();
                RestoreAll();
                return;
            }

            HideOccludingWalls =
                PlayerPrefs.GetInt(
                    PlayerPrefsKey,
                    defaultValue ? 1 : 0) == 1;
        }

        public void SetEnabled(bool enabled)
        {
            if (!CanChangeVisibility)
            {
                HideOccludingWalls = false;
                RestoreAll();
                return;
            }

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
                OccludableWall wall =
                    hit.collider
                        .GetComponentInParent<
                            OccludableWall>();

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
