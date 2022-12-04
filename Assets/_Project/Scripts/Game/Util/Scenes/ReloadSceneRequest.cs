using UnityEngine;

namespace Util.Scenes
{
    [CreateAssetMenu(menuName = "Custom/Scenes/ReloadSceneRequest")]
    public class ReloadSceneRequest : SceneManagementRequest
    {
        public override void Perform(SceneManagementBehaviour manager)
        {
            manager.ReloadScene();
        }
    }
}