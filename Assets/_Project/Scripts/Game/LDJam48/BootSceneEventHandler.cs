using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util;
using Util.Scenes;
using Util.Var;
using Util.Var.Events;

namespace LDJam48
{
    public class BootSceneEventHandler : SceneChangeHandler
    {
        [SerializeField] private VoidGameEvent raiseOnLoad;
        [SerializeField] [ScenePath] private string fistScene;
        [SerializeField] private GameObject sceneObject;
        [SerializeField] private UnityEvent onInitialSceneLoad;
        
        
        
        
        
        private bool _firstLoad = true;

        private void Awake()
        {
            if (SceneManager.sceneCount == 1)
            {
                sceneObject.gameObject.SetActive(true);
            }
        }


        public override IEnumerator CoSceneLoaded()
        {
            if (_firstLoad)
            {
                for (int i = 0; i < SceneManager.sceneCount; ++i)
                {
                    if (SceneManager.GetSceneAt(i).path == fistScene)
                    {
                        onInitialSceneLoad?.Invoke();
                        break;
                    }
                }
                _firstLoad = false;
            }
            
            raiseOnLoad?.Raise();
            yield break;
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {
            yield break;
        }
    }
}