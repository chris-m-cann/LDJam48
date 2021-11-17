using UnityEngine;
using Util;

namespace LDJam48
{
    public class Detectors : MonoBehaviour
    {
        [SerializeField] private Detector[] detectors;

        public Detector GetDetector(int idx)
        {
            if (idx < 0 || idx >= detectors.Length)
            {
                return null;
            }

            return detectors[idx];
        }

        public void EnableDetector(int idx)
        {
            if (!detectors.HasIndex(idx))
            {
                Debug.LogError($"{name}: Detectors {idx} doesnt exist");
                return;
            }

            detectors[idx].enabled = true;
        }
        
        public void DisableDetector(int idx)
        {
            if (!detectors.HasIndex(idx))
            {
                Debug.LogError($"{name}: Detectors {idx} doesnt exist");
                return;
            }

            detectors[idx].enabled = false;
        }
    }
}