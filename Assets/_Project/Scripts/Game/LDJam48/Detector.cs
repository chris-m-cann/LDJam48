using System;
using UnityEngine;

namespace LDJam48
{
    public abstract class Detector: MonoBehaviour
    {
        public event Action<GameObject> OnDetected;

        protected virtual void InvokeOnDetected(GameObject obj)
        {
            OnDetected?.Invoke(obj);
        }
    }
}