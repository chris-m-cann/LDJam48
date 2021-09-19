using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Util.Scene
{
    public class SceneManagementBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityEvent onSceneLoadingComplete;
        
        public void LoadNextScene()
        {
            var current = SceneManager.GetActiveScene().buildIndex;
            var next = current + 1;

            if (next >= SceneManager.sceneCount)
            {
                next = 0;
            }

            StartCoroutine(CoLoadScene(next));
        }

        private IEnumerator CoLoadScene(int sceneIndex)
        {
            yield return StartCoroutine(CoNotifyEnding());

            SceneManager.LoadScene(sceneIndex);

            yield return StartCoroutine(CoNotifyLoaded());
            
            onSceneLoadingComplete?.Invoke();
        }

        private IEnumerator CoLoadScene(string sceneName)
        {
            yield return StartCoroutine(CoNotifyEnding());

            SceneManager.LoadScene(sceneName);

            yield return StartCoroutine(CoNotifyLoaded());
        }

        private IEnumerator CoNotifyEnding()
        {
            var handlers = FindObjectsOfType<SceneChangeHandler>()
                .Select(h => StartCoroutine(h.CoSceneEnding()));

            yield return this.WaitForAll(handlers);
        }

        private IEnumerator CoNotifyLoaded()
        {
            var handlers = FindObjectsOfType<SceneChangeHandler>()
                .Select(h => StartCoroutine(h.CoSceneLoaded()));

            yield return this.WaitForAll(handlers);
        }

        public void LoadScene(string sceneName) => StartCoroutine(CoLoadScene(sceneName));

        public void ReloadScene()
        {
            var current = SceneManager.GetActiveScene().buildIndex;

            StartCoroutine(CoLoadScene(current));
        }

        public void HandleRequest(SceneManagementRequest request) => request.Perform(this);
    }
}