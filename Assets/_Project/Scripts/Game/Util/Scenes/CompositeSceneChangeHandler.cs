using System;
using System.Collections;
using UnityEngine;

namespace Util.Scenes
{
    public class CompositeSceneChangeHandler : SceneChangeHandler
    {
        [SerializeField] private SceneChangeHandler[] delegates;
        
        public override IEnumerator CoSceneLoaded()
        {
            foreach (var dgt in delegates)
            {
                yield return StartCoroutine(dgt.CoSceneLoaded());
            }
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {
            foreach (var dgt in delegates)
            {
                yield return StartCoroutine(dgt.CoSceneEnding(currentScenePath, nextScenePath));
            }
        }
    }
}