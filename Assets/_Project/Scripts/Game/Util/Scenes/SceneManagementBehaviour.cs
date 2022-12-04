using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Util.Scenes
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

        public void HandleRequest(SceneManagementRequest request)
        {
            Debug.Log($"Handling request {request.name}");
            request.Perform(this);
        }

        private Dictionary<string, AsyncOperation> _pendingSceneLoads = new();
        
        public void LoadInBackground(string sceneName, bool waitForCompletion)
        {
            StartCoroutine(CoLoadInBackground(sceneName, waitForCompletion, 30));
        }

        private IEnumerator CoLoadInBackground(string sceneName, bool waitForCompletion, float maxWait)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = !waitForCompletion;

            if (waitForCompletion)
            {
                _pendingSceneLoads[sceneName] = asyncOperation;
            }
            
            float end = Time.unscaledTime + maxWait;
            
            while (!asyncOperation.isDone || Time.unscaledTime > end)
            {
                yield return null;
            }
        }
        
        public void CompleteSceneLoad(string sceneName)
        {
            if (_pendingSceneLoads.ContainsKey(sceneName))
            {
                _pendingSceneLoads[sceneName].allowSceneActivation = true;
                _pendingSceneLoads.Remove(sceneName);
            }
        }
    }
}