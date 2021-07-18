using Cinemachine;
using UnityEngine;
using Util;

namespace LDJam48
{
    [CreateAssetMenu(menuName = "Custom/ShakeDefinition")]
    public class ShakeDefinition : ScriptableObject
    {
        public CinemachineImpulseDefinition Definition;
    }
}