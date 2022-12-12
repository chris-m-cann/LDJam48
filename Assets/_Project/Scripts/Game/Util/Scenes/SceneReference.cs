using UnityEngine;

namespace Util.Scenes
{
    public class SceneReference : ScriptableObject
    {
        [ScenePath] [SerializeField]
        public string scene;

        [SerializeField] public SceneReference[] requires;
    }
}