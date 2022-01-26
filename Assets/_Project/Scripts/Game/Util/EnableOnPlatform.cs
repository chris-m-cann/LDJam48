using System;
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
            if (Array.IndexOf(platforms, Application.platform) != -1)
            {
                foreach (var target in targets)
                {
                    target.SetActive(enableTargets);
                }
            } else {
                foreach (var target in targets)
                {
                    target.SetActive(!enableTargets);
                }
}

#endif
        }
    }
}