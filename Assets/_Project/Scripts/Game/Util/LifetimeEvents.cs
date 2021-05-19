using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class LifetimeEvents : MonoBehaviour
    {
        [SerializeField] private UnityEvent onAwake;
        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onStart;
        [SerializeField] private UnityEvent onApplicationQuit;
        [SerializeField] private UnityEvent onDisable;
        [SerializeField] private UnityEvent onDestroy;


        private void Awake()
        {
            onAwake?.Invoke();
        }

        private void OnEnable() => onEnable?.Invoke();
        private void Start() => onStart?.Invoke();
        private void OnApplicationQuit() => onApplicationQuit?.Invoke();
        private void OnDisable() => onDisable?.Invoke();
        private void OnDestroy() => onDestroy?.Invoke();

    }
}