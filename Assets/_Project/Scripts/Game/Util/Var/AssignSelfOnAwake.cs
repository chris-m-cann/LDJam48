using System;
using UnityEngine;

namespace Util.Var
{
    public class AssignSelfOnAwake : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable variable;

        private void Awake()
        {
            if (variable == null) return;
            variable.Value = gameObject;
        }
    }
}