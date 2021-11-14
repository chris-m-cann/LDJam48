using UnityEngine;

namespace LDJam48
{
    public abstract class Detector: MonoBehaviour
    {
        // todo more complex? maybe return the target GameObject?
        public bool WasDetected { get; protected set; }
    }
}