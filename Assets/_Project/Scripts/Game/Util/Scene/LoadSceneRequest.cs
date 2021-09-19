using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Util.Scene
{
    [CreateAssetMenu(menuName = "Custom/Scenes/LoadSceneRequest")]
    public class LoadSceneRequest : SceneManagementRequest
    {
        [SerializeField] private string sceneName;
        
        #if UNITY_EDITOR
        [SerializeField] private SceneAsset scene;

        private void OnValidate()
        {
            sceneName = scene.name;
        }
        #endif

        public override void Perform(SceneManagementBehaviour manager)
        {
            manager.LoadScene(sceneName);
        }
    }
}