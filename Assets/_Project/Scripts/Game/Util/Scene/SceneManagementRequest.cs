using System;
using UnityEditor;
using UnityEngine;

namespace Util.Scene
{
    public abstract class SceneManagementRequest : ScriptableObject
    {
        public abstract void Perform(SceneManagementBehaviour manager);
    }
}