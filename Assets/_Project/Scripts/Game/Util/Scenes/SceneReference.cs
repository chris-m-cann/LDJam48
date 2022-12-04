using UnityEngine;

namespace Util.Scenes
{
    public class SceneReference : ScriptableObject
    {
        [SceneName] [SerializeField]
        public string scene;

        [SerializeField] public SceneReference[] requires;
    }
}