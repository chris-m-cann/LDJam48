using System;
using LDJam48.StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace LDJam48.Save
{
    public class OnSaveEvents : MonoBehaviour
    {
        [SerializeField] private SaveableSO saveable;
        [SerializeField] private UnityEvent onSaveRequested;
        [SerializeField] private UnityEvent onLoadComplete;

        private void OnEnable()
        {
            saveable.OnRequestSave += OnSaveRequested;
            saveable.OnLoadComplete += OnLoadComplete;
        }
        
        private void OnDisable()
        {
            saveable.OnRequestSave -= OnSaveRequested;
            saveable.OnLoadComplete -= OnLoadComplete;
        }

        private void OnSaveRequested() => onSaveRequested?.Invoke();
        private void OnLoadComplete() => onLoadComplete?.Invoke();
    }
}