using System;
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
        [ScenePath] [SerializeField] private string firstScene;
        private Dictionary<string, AsyncOperation> _pendingSceneLoads = new();
        [SerializeField] private UnityEvent sceneUnloaded;
        

        private void Start()
        {
            if (SceneManager.sceneCount == 1)
            {
                StartCoroutine(CoLoadScene(firstScene));
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; ++i)
                {
                    var s = SceneManager.GetSceneAt(i);
                    if (s != gameObject.scene)
                    {
                        SceneManager.SetActiveScene(s);
                        break;
                    }
                }
            }
        }
        
        public void HandleRequest(SceneManagementRequest request) => request.Perform(this);
        public void ReplaceActiveScene(string sceneName) => StartCoroutine(CoReplaceActiveScene(sceneName));
        
        public void LoadInBackground(string sceneName, bool waitForCompletion) => StartCoroutine(CoLoadInBackground(sceneName, waitForCompletion, 30));
        
        public void CompleteLoadInBackground(string sceneName) => StartCoroutine(CoCompleteSceneLoad(sceneName));
        
        public void ReloadActiveScene() => StartCoroutine(CoReloadScene());
        

        private IEnumerator CoReplaceActiveScene(string sceneName)
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

        private IEnumerator CoLoadScene(string scenePath)
        {
            yield return StartCoroutine(CoNotifyEnding(SceneManager.GetActiveScene().path, scenePath));


            var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

            op.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePath));
                StartCoroutine(CoNotifyLoaded());
            };
            while (!op.isDone)
            {
                yield return null;
            }

        }

        private IEnumerator CoNotifyEnding(string currentScenePath, string nextScenePath)
        {
            var handlers = FindObjectsOfType<SceneChangeHandler>()
                .Select(h => StartCoroutine(h.CoSceneEnding(currentScenePath, nextScenePath)));

            yield return this.WaitForAll(handlers);
        }

        private IEnumerator CoNotifyLoaded()
        {
            var handlers = FindObjectsOfType<SceneChangeHandler>()
                .Select(h => StartCoroutine(h.CoSceneLoaded()));

            yield return this.WaitForAll(handlers);
        }


        public IEnumerator CoReloadScene()
        {
            yield return StartCoroutine(CoReplaceScene(SceneManager.GetActiveScene(),
                SceneManager.GetActiveScene().path));
        }

        public IEnumerator CoReplaceScene(Scene unload, string loadPath)
        {
            yield return StartCoroutine(CoNotifyEnding(unload.path, loadPath));

            var unloadOp = SceneManager.UnloadSceneAsync(unload);
            while (unloadOp.isDone)
            {
                yield return null;
            }

            var op = SceneManager.LoadSceneAsync(loadPath, LoadSceneMode.Additive);

            op.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(loadPath));
                StartCoroutine(CoNotifyLoaded());
                
            };
            while (!op.isDone)
            {
                yield return null;
            }
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
        
        private IEnumerator CoCompleteSceneLoad(string sceneName)
        {
            if (_pendingSceneLoads.ContainsKey(sceneName))
            {
                
                yield return StartCoroutine(CoNotifyEnding(SceneManager.GetActiveScene().path, sceneName));
                
                Debug.Log($"unloading {SceneManager.GetActiveScene().name}");
                var unloadTask = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                
                
                _pendingSceneLoads[sceneName].completed += operation =>
                {
                    Debug.Log($"loaded {sceneName}");
                    SceneManager.SetActiveScene(SceneManager.GetSceneByPath(sceneName));
                    
                    StartCoroutine(CoNotifyLoaded());
                };
                _pendingSceneLoads[sceneName].allowSceneActivation = true;
                _pendingSceneLoads.Remove(sceneName);
                
                // weird unity nuance that you cant unload on scene async while loading another so need set other scene allowed to load first
                unloadTask.allowSceneActivation = true;
                while (!unloadTask.isDone)
                {
                    yield return null;
                }
                
                sceneUnloaded?.Invoke();
                Debug.Log($"unloaded done");
            }
        }
    }
}