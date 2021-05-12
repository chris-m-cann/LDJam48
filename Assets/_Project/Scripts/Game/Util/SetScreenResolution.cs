using System;
using UnityEngine;

namespace Util
{
    public class SetScreenResolution : MonoBehaviour
    {
        [SerializeField] private int width = 900;
        [SerializeField] private int height = 1600;

        private void Awake()
        {
#if UNITY_ANDROID
                Screen.SetResolution(width, height, true);
#endif
        }
    }
}