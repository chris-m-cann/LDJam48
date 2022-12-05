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

        private IEnumerator CoLoadScene(int sceneIndex)
        {
            yield return StartCoroutine(CoNotifyEnding());

            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);

            yield return StartCoroutine(CoNotifyLoaded());

            onSceneLoadingComplete?.Invoke();
        }

        private IEnumerator CoLoadScene(string sceneName)
        {
            Scene active = SceneManager.GetActiveScene();
            Scene load = SceneManager.GetSceneByPath(sceneName);
            if (load.isLoaded)
            {
                SceneManager.SetActiveScene(load);
                yield break;
            }

            yield return StartCoroutine(CoReplaceScene(active, sceneName));
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

        public void ReloadActiveScene()
        {
            StartCoroutine(CoReloadScene());
        }

        public IEnumerator CoReloadScene()
        {
            yield return StartCoroutine(CoReplaceScene(SceneManager.GetActiveScene(), SceneManager.GetActiveScene().path));
        }

        public IEnumerator CoReplaceScene(Scene unload, string loadPath)
        {
            yield return StartCoroutine(CoNotifyEnding());

            var unloadOp = SceneManager.UnloadSceneAsync(unload);
            while (unloadOp.isDone)
            {
                yield return null;
            }


            var op = SceneManager.LoadSceneAsync(loadPath, LoadSceneMode.Additive);

            op.completed += operation => { SceneManager.SetActiveScene(SceneManager.GetSceneByPath(loadPath)); };
            while (!op.isDone)
            {
                yield return null;
            }

            yield return StartCoroutine(CoNotifyLoaded());
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
            if (SceneManager.GetSceneByPath(sceneName).isLoaded) yield break;
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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

            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(sceneName));
        }

        public void CompleteSceneLoad(string sceneName)
        {
            StartCoroutine(CoCompleteSceneLoad(sceneName));
        }

        private IEnumerator CoCompleteSceneLoad(string sceneName)
        {
            if (_pendingSceneLoads.ContainsKey(sceneName))
            {
                var unloadTask = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

                Debug.Log($"unloaded menu");
                _pendingSceneLoads[sceneName].completed += operation =>
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByPath(sceneName));
                };
                _pendingSceneLoads[sceneName].allowSceneActivation = true;
                _pendingSceneLoads.Remove(sceneName);

                unloadTask.allowSceneActivation = true;
                while (!unloadTask.isDone)
                {
                    yield return null;
                }
            }
        }


        public void SetActiveScene(string scenePath)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePath));
        }
    }
}