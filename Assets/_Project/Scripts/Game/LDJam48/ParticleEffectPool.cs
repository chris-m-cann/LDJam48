using System;
using UnityEngine;

namespace LDJam48
{
    public class ParticleEffectPool : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectPrefab;
        [SerializeField] private int poolSize = 3;

        private ParticleSystem[] _particles;

        private void Start()
        {
            _particles = new ParticleSystem[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                _particles[i] = Instantiate(effectPrefab, transform);
            }
        }


        public void Play(ParticleEffectRequest request)
        {
            foreach (var particles in _particles)
            {
                if (particles.gameObject.activeSelf) continue;

                Play(request, particles);
                return;
            }

            Play(request, _particles[0]);
        }

        private void Play(ParticleEffectRequest request, ParticleSystem particles)
        {
            particles.transform.position = request.Position;
            particles.transform.rotation = request.Rotation;
            particles.transform.localScale = request.Scale;

            particles.gameObject.SetActive(true);
            particles.Play();
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