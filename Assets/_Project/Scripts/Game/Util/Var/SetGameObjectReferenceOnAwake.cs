using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Util.Var
{
    public class SetGameObjectReferenceOnAwake : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable variable;
        [SerializeField] private bool assignSelf = true;

        [HideIf("$assignSelf")] [SerializeField]
        private GameObject objectToAssign;
        

        private void Awake()
        {
            if (variable == null) return;
            variable.Value = assignSelf ? gameObject : objectToAssign;
        }
    }
}