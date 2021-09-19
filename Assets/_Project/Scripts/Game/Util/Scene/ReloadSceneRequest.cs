using LDJam48.LevelGen;
using UnityEngine;

namespace Util.Scene
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