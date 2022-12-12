using UnityEngine;

namespace Util.Scenes
{
    [CreateAssetMenu(menuName = "Custom/Scenes/LoadSceneRequest")]
    public class LoadSceneRequest : SceneManagementRequest
    {
        [ScenePath]
        [SerializeField] private string sceneName;


        public override void Perform(SceneManagementBehaviour manager)
        {
            manager.ReplaceActiveScene(sceneName);
        }
    }
}