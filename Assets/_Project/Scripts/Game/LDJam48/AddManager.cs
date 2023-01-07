using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Util.Scenes;
using Util.Var;
using Util.Var.Events;

namespace LDJam48
{
    public class AddManager : MonoBehaviour
    {
        [ScenePath]
        [SerializeField] private string runScene;
        // [SerializeField] private VoidGameEvent onRunStart;
        // [SerializeField] private VoidGameEvent onRunEnd;
        [SerializeField] private IntReference runDistance;
        [SerializeField] private float timeBeforeAd = 20;
        
        
        private AddMediation _mediation = new AddMediation();

        // private float _runStart;
        private float _timeSinceLastAd;

        private void Start()
        {
            _mediation.InitServices();
        }

        // private void OnEnable()
        // {
            // onRunStart.OnEventTrigger += OnRunStart;
            // onRunEnd.OnEventTrigger += OnRunEnd;
        // }
        
        // private void OnDisable()
        // {
            // onRunStart.OnEventTrigger -= OnRunStart;
            // onRunEnd.OnEventTrigger -= OnRunEnd;
        // }

        // private void OnRunStart(Util.Void v)
        // {
        //     _runStart = Time.unscaledTime;
        //     Debug.Log($"RunStarted @ {_runStart}");
        // }
        //
        // private void OnRunEnd(Util.Void v)
        // {
        //     _timeSinceLastAd += Time.unscaledTime - _runStart;
        //     Debug.Log($"Run Ended _timeSinceLastAdd =  {_timeSinceLastAd}");
        // }

        [ContextMenu("Show Add")]
        public void ShowAdd()
        {
            _mediation.ShowAd();
        }

        // public IEnumerator MaybeShowAdd()
        // {
        //     Debug.Log($"Show ad? {_timeSinceLastAd} > {timeBeforeAd}");
        //     if (_timeSinceLastAd > timeBeforeAd)
        //     {
        //         Debug.Log($"Showing ad");
        //         Task task = _mediation.ShowAd();
        //         yield return new WaitUntil(() => task.IsCompleted);
        //         _timeSinceLastAd = 0;
        //     }
        // }


        public IEnumerator OnSceneEnd(string oldScene, string newScene)
        {
            if (oldScene == runScene)
            {
                _timeSinceLastAd += runDistance.Value;
            }

            if (newScene == runScene)
            {
                if (ShouldShowAd())
                {
                    yield return StartCoroutine(CoShowAdd());
                    
                    Reset();
                }   
            }
        }
        
        
        public IEnumerator CoShowAdd()
        {
            Task task = _mediation.ShowAd();
            yield return new WaitUntil(() => task.IsCompleted);
        }
        
        public bool ShouldShowAd()
        {
            return _timeSinceLastAd > timeBeforeAd;
        }

        public void Reset()
        {
            _timeSinceLastAd = 0;
        }
    }
}