using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Util.Scenes
{
    public class EventDrivenSceneChangeHandler : SceneChangeHandler
    {
        [SerializeField] private UnityEvent onSceneLoaded;
        [SerializeField] private UnityEvent onSceneEnding;

        private bool _sceneLoadedNotified;
        private bool _sceneEndingNotified;
        public override IEnumerator CoSceneLoaded()
        {
            _sceneLoadedNotified = false;
            onSceneLoaded?.Invoke();

            while (!_sceneLoadedNotified)
            {
                yield return null;
            }
        }

        public override IEnumerator CoSceneEnding()
        {          
            _sceneEndingNotified = false;
            onSceneEnding?.Invoke();

            while (!_sceneEndingNotified)
            {
                yield return null;
            }
        }

        public void SceneLoadingEventProcessingComplete()
        {
            _sceneLoadedNotified = true;
        }
        
        
        public void SceneEndingEventProcessingComplete()
        {
            _sceneEndingNotified = true;
        }
    }
}