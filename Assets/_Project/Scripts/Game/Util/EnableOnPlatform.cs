using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class EnableOnPlatform : MonoBehaviour
    {
        [SerializeField] private GameObject[] targets;
        [SerializeField] private RuntimePlatform[] platforms;

        [SerializeField] private bool enableTargets;

        [SerializeField] private bool enabledInEditor;

        private void Awake()
        {

#if UNITY_EDITOR
            foreach (var target in targets)
            {
                target.SetActive(enabledInEditor);
            }


#else

            Debug.Log($"EnableOnPlatform: platform is {Application.platform}");

            var validPlatform = Array.IndexOf(platforms, Application.platform) != -1;
            foreach (var target in targets)
            {
                var active = validPlatform ? enableTargets : !enableTargets;
                target.SetActive(active);
            }

#endif
        }
    }
}