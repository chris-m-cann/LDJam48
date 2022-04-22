using System;
using UnityEngine;

namespace Util
{
    public class SetGlobalShaderProperties : MonoBehaviour
    {
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

        private void Update()
        {
            Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
        }
    }
}