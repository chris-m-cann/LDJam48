using System;
using System.Collections;
using LDJam48.Var.Observe;
using UnityEngine;
using Util;
using Util.Scenes;
using Util.Var.Events;
using Void = Util.Void;

namespace LDJam48
{
    public class ScreenWipeController : SceneChangeHandler
    {
        [SerializeField] private ScreenWipe[] screenWipes;
        [SerializeField] private ObservableScreenWipeVariable wipe;
        [SerializeField] private VoidGameEvent onWipeComplete;
        [SerializeField] private int test = -1;

        private ScrabbleBag<ScreenWipe> _wipes;
        private bool _wiping;

        private void Awake()
        {
            _wipes = new ScrabbleBag<ScreenWipe>(screenWipes, true);
        }

        private void OnEnable()
        {
            onWipeComplete.OnEventTrigger += SetWipingComplete;
        }

        private void OnDisable()
        {
            onWipeComplete.OnEventTrigger -= SetWipingComplete;
        }

        private void SetWipingComplete(Void v)
        {
            _wiping = false;
        }


        public override IEnumerator CoSceneLoaded()
        {
            yield break;
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {
            if (wipe.ActiveObservers == 0) yield break;
            
            _wiping = true;
            wipe.Value = (test >= 0 ? screenWipes[test] : _wipes.GetRandomElement());
            // wipe.SetAndRaise(test >= 0 ? screenWipes[test] : _wipes.GetRandomElement(), true);

            while (_wiping)
            {
                yield return null;
            }
        }
    }
}