using System;
using Cinemachine;
using UnityEngine;

namespace LDJam48
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraShakeManager : MonoBehaviour
    {
        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(ShakeDefinition shake)
        {
            _impulseSource.m_ImpulseDefinition = shake.Definition;
            _impulseSource.GenerateImpulse();
        }

    }
}