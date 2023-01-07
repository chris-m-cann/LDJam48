using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Util.Scenes;
using Util.Var;

namespace LDJam48
{
    public class AddManager : SceneChangeHandler
    {
        [ScenePath]
        [SerializeField] private string runScene;
        [SerializeField] private IntReference runDistance;
        [SerializeField] private float distanceBeforeAd = 20;


#if UNITY_ANDROID
        
        private AddMediation _mediation = new AddMediation();
        private float _distanceSinceLastAd;

        private void Start()
        {
            _mediation.InitServices();
        }


        [ContextMenu("Show Add")]
        public void ShowAdd()
        {
            _mediation.ShowAd();
        }

        private IEnumerator CoShowAdd()
        {
            Task task = _mediation.ShowAd();
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private bool ShouldShowAd()
        {
            return _distanceSinceLastAd > distanceBeforeAd;
        }

        private void Reset()
        {
            _distanceSinceLastAd = 0;
        }

        public override IEnumerator CoSceneLoaded()
        {
            yield break;
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {
            if (currentScenePath == runScene)
            {
                _distanceSinceLastAd += runDistance.Value;
            }

            if (nextScenePath == runScene)
            {
                if (ShouldShowAd())
                {
                    yield return StartCoroutine(CoShowAdd());
                    
                    Reset();
                }   
            }
        }
        
#else
        public override IEnumerator CoSceneLoaded()
        {
            yield break;
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {

            yield break;
        }
#endif
    }
}