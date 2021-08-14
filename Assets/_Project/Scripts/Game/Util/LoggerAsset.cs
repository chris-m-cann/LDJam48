using UnityEngine;

namespace Util
{
    // simple logging SO to adding to unity event chains to check they are firing
    [CreateAssetMenu(menuName = "Custom/Logger")]
    public class LoggerAsset: ScriptableObject
    {
        public void Log(string message) => Debug.Log(message);
    }
}