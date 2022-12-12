using UnityEngine;

namespace Util.Scenes
{
    [CreateAssetMenu(menuName = "Custom/Scenes/StartBackgroundSceneLoad")]
    public class StartBackgroundSceneLoad : SceneManagementRequest
    {
        [ScenePath]
        [SerializeField] private string sceneName;
        [SerializeField] private bool awaitCompletion;
        
        public override void Perform(SceneManagementBehaviour manager)
        {
            Debug.Log($"starting in background: {sceneName}");
            manager.LoadInBackground(sceneName, awaitCompletion);
        }
    }
}