using UnityEngine;

namespace Util.Scenes
{
    public abstract class SceneManagementRequest : ScriptableObject
    {
        public abstract void Perform(SceneManagementBehaviour manager);
    }
}