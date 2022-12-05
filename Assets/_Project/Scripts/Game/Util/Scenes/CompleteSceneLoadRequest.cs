using UnityEngine;

namespace Util.Scenes
{
    [CreateAssetMenu(menuName = "Custom/Scenes/CompleteSceneLoadRequest")]
    public class CompleteSceneLoadRequest : SceneManagementRequest
    {
        [SceneName]
        [SerializeField] private string sceneName;
        

        public override void Perform(SceneManagementBehaviour manager)
        {
            Debug.Log($"completeing scene start: {sceneName}");
            manager.CompleteLoadInBackground(sceneName);
        }
    }
}