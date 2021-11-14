using UnityEngine;

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
    }
}