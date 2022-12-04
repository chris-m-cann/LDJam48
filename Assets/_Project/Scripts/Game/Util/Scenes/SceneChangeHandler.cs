using System.Collections;
using UnityEngine;

namespace Util.Scenes
{
    public abstract class SceneChangeHandler: MonoBehaviour
    {
        // called as soo as a scene is loaded
        public abstract IEnumerator CoSceneLoaded();
        // called right before a scene is ended, either switching or reloading
        public abstract IEnumerator CoSceneEnding();
    }
}