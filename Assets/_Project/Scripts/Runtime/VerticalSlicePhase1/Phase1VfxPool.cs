using System.Collections.Generic;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1VfxPool :
        MonoBehaviour
    {
        private readonly Queue<ParticleSystem>
            _available =
                new Queue<ParticleSystem>();

        private int _capacity;

        public void Configure(int capacity)
        {
            _capacity = Mathf.Max(1, capacity);

            while (_available.Count < _capacity)
            {
                _available.Enqueue(
                    CreateParticleSystem(
                        _available.Count));
            }
        }

        public void Play(
            Vector3 position,
            Color color)
        {
            if (_available.Count == 0)
            {
                Configure(
                    Mathf.Max(1, _capacity));
            }

            ParticleSystem system =
                _available.Dequeue();

            system.transform.position = position;

            ParticleSystem.MainModule main =
                system.main;
            main.startColor = color;

            system.gameObject.SetActive(true);
            system.Clear(true);
            system.Play(true);

            StartCoroutine(
                ReturnAfter(
                    system,
                    1.2f));
        }

        private System.Collections.IEnumerator
            ReturnAfter(
                ParticleSystem system,
                float seconds)
        {
            yield return new WaitForSecondsRealtime(
                seconds);

            if (system != null)
            {
                system.Stop(
                    true,
                    ParticleSystemStopBehavior
                        .StopEmittingAndClear);
                system.gameObject.SetActive(false);
                _available.Enqueue(system);
            }
        }

        private ParticleSystem CreateParticleSystem(
            int index)
        {
            GameObject gameObject =
                new GameObject(
                    "PooledVfx_" + index);

            gameObject.SetActive(false);
            gameObject.transform.SetParent(
                transform,
                false);

            ParticleSystem system =
                gameObject.AddComponent<
                    ParticleSystem>();

            system.Stop(
                true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear);

            ParticleSystem.MainModule main =
                system.main;
            main.playOnAwake = false;
            main.duration = 0.45f;
            main.loop = false;
            main.startLifetime = 0.55f;
            main.startSpeed = 1.1f;
            main.startSize = 0.08f;
            main.maxParticles = 24;

            ParticleSystem.EmissionModule
                emission = system.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(
                new[]
                {
                    new ParticleSystem.Burst(
                        0f,
                        12)
                });

            ParticleSystem.ShapeModule shape =
                system.shape;
            shape.shapeType =
                ParticleSystemShapeType.Sphere;
            shape.radius = 0.18f;

            system.Clear(true);
            return system;
        }
    }
}
