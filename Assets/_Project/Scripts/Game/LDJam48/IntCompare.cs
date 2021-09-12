using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Util.Var;

namespace LDJam48
{
    public class IntCompare : MonoBehaviour
    {
        [SerializeField] private IntReference a;
        [SerializeField] private IntReference b;

        [SerializeField] private UnityEvent aGreaterThanB;
        [SerializeField] private UnityEvent aLessThanB;
        [SerializeField] private UnityEvent aEqualsB;

        public void RunCompare()
        {
            if (a.Value > b.Value)
            {
                aGreaterThanB?.Invoke();
            } else if (a.Value < b.Value)
            {
                aLessThanB?.Invoke();
            }
            else
            {
                aEqualsB?.Invoke();
            }
        }
        
    }
}