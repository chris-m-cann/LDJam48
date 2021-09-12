using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Util.UI
{
    [RequireComponent(typeof(TweenBehaviour))]
    public class Menu : MonoBehaviour
    {
        public bool WaitForOpen = false;
        public bool WaitForClose = true;

        [SerializeField] private Transition onMenuEnabledTransition;
        [SerializeField] private Transition onMenuDisabledTransition;
        [SerializeField] private Transition onSwitchedToTransition;
        [SerializeField] private Transition onSwitchedFromTransition;
        
        
        [SerializeField] private Transition onEnter;
        [SerializeField] private Transition onExit;
        [SerializeField] private Transition onPoppedTo;
        [SerializeField] private Transition onPoppedFrom;

        [SerializeField] private Transition[] transitions;
        [SerializeField] private SwapMenuAction[] actions;
        
        
        
        private TweenBehaviour _tweenBehaviour;
        private IMenuManager _manager;
        private int _menuIndex;

        private void Awake()
        {
            _tweenBehaviour = GetComponent<TweenBehaviour>();
        }

        public void CloseParent() => _manager?.CloseMenu();

        public void SwitchToMenu(int actionIndex)
        {
            var action = actions[actionIndex];
            action.SourceMenu = _menuIndex;
            _manager?.SwitchMenu(action);
        }

        public void PopBackstack()
        {
            _manager?.PopBackstack();
        }

        public void Attach(IMenuManager menuManager, int menuIndex)
        {
            _manager = menuManager;
            _menuIndex = menuIndex;
        }
        public IEnumerator CoOnMenuEnabled()
        {
            yield return StartCoroutine(CoRunTransition(onMenuEnabledTransition));
        }
        
        public IEnumerator CoOnMenuDisabled()
        {
            yield return StartCoroutine(CoRunTransition(onMenuDisabledTransition));
        }

        public IEnumerator CoRunTransition(int transitionIdx)
        {
            yield return StartCoroutine(CoRunTransition(transitions[transitionIdx]));
        }

        public IEnumerator CoOnEnter()
        {
            yield return StartCoroutine(CoRunTransition(onEnter));
        }

        public IEnumerator CoOnExit()
        {
            yield return StartCoroutine(CoRunTransition(onExit));
        }

        
        public IEnumerator CoPoppedTo()
        {
            yield return StartCoroutine(CoRunTransition(onPoppedTo));
        }

        public IEnumerator CoPoppedFrom()
        {
            yield return StartCoroutine(CoRunTransition(onPoppedFrom));
        }

        private IEnumerator CoRunTransition(Transition transition)
        {
            transition.OnTransitionStart?.Invoke();

            yield return StartCoroutine(transition.CoTween(this, _tweenBehaviour));
            
            transition.OnTransitionComplete?.Invoke();
        }
    }
}
