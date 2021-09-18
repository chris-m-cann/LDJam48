using System;
using UnityEngine;
using Util;

namespace LDJam48
{
    // next thing to do is have parent objects to the particle system so that they can have their own rotations and offsets
    public class ParticleEffectPool : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectPrefab;
        [SerializeField] private int poolSize = 3;

        private Pair<Transform, ParticleSystem>[] _particles;

        private void Start()
        {
            _particles = new Pair<Transform, ParticleSystem>[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                var parent = new GameObject($"{effectPrefab.name} Parent {i}");
                parent.transform.SetParent(transform);
                
                var particles = Instantiate(effectPrefab, parent.transform);
                particles.gameObject.SetActive(false);

                _particles[i] = new Pair<Transform, ParticleSystem>(parent.transform, particles);
            }
        }


        public void Play(ParticleEffectRequest request)
        {
            foreach (var particles in _particles)
            {
                if (particles.Second.gameObject.activeSelf) continue;

                Play(request, particles);
                return;
            }

            Play(request, _particles[0]);
        }

        private void Play(ParticleEffectRequest request, Pair<Transform, ParticleSystem> particles)
        {
            particles.First.position = request.Position;
            particles.First.rotation =  request.Rotation;
            particles.First.localScale = request.Scale;

            particles.Second.gameObject.SetActive(true);
            particles.Second.Play();
        }
    }

    [Serializable]
    public struct ParticleEffectRequest
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}