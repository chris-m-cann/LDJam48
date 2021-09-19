using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Util.Var;

namespace Util.UI
{
    [RequireComponent(typeof(TweenBehaviour))]
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private bool startOpen;
        
        [SerializeField] private Menu[] menus;
        [SerializeField] private int defaultMenu = 0;

        [SerializeField] private Transition onOpenTransition;
        [SerializeField] private Transition onCloseTransition;

        

        private bool _isOpen;
        private int _currentMenu;
        private TweenBehaviour _tweenBehaviour;
        private readonly Stack<SwapMenuAction> _backstack = new Stack<SwapMenuAction>();

        private void Awake()
        {
            _tweenBehaviour = GetComponent<TweenBehaviour>();
            for (var index = 0; index < menus.Length; index++)
            {
                var menu = menus[index];
                menu.Attach(this, index);
            }

            _isOpen = startOpen;
        }

        public void OpenMenu() => OpenMenuTo(defaultMenu);

        public void OpenMenuTo(int menuIdx) => StartCoroutine(CoOpenMenuTo(menuIdx));

        private IEnumerator CoOpenMenuTo(int menuIdx)
        {
            _backstack.Clear();
            onOpenTransition.OnTransitionStart?.Invoke();
            foreach (var menu in menus)
            {
                menu.gameObject.SetActive(false);
            }

            menus[menuIdx].gameObject.SetActive(true);

            yield return null;
            // so in new system are we doing this or should we just make it a coroutine? that way we can wait for it like the others???
            var openCoroutine = StartCoroutine(CoEnableMenu(menuIdx));

            if (menus[menuIdx].WaitForOpen)
            {
                yield return openCoroutine;
            }

            yield return StartCoroutine(onOpenTransition.CoTween(this, _tweenBehaviour));
            
            
            onOpenTransition.OnTransitionComplete?.Invoke();
        }

        private IEnumerator CoEnableMenu(int menuIdx)
        {
            yield return StartCoroutine(menus[menuIdx].CoOnMenuEnabled());
            _currentMenu = menuIdx;
            _isOpen = true;
            // onOpen?.Invoke();
        }


        public void ToggleMenu()
        {
            if (_isOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        public void ToggleMenu(InputAction.CallbackContext ctx)
        {
            Debug.Log($"Toggleing menu phase = {ctx.phase}");
            if (_isOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        public void SwitchMenu(SwapMenuAction action)
        {
            if (!_isOpen)
            {
                Debug.LogError($"cannot switch to menu {action.Destination} as menu is not open");
                return;
            }

            if (action.Destination >= menus.Length || action.Destination < 0)
            {
                Debug.LogError($"menu {action.Destination} is out of range. only {menus.Length} configured");
                return;
            }
            
            Debug.Log($"MenuManager, switching to {action.Destination}");
            
            _backstack.Push(action);
            StartCoroutine(CoSwitchMenu(action));
        }

        private IEnumerator CoSwitchMenu(SwapMenuAction action)
        {
            yield return StartCoroutine(menus[action.SourceMenu].CoRunTransition(action.OnExitTransition));
            
            menus[action.SourceMenu].gameObject.SetActive(false);
            menus[action.Destination].gameObject.SetActive(true);
            
            yield return StartCoroutine(menus[action.Destination].CoRunTransition(action.OnEnterTransition));
        }

        public void CloseMenu()
        {
            StartCoroutine(CoCloseMenu());
        }

        private IEnumerator CoCloseMenu()
        {
            _backstack.Clear();
            onCloseTransition.OnTransitionStart?.Invoke();
            
            var coroutine = StartCoroutine(CoDisableMenu());

            if (menus[_currentMenu].WaitForClose)
            {
                yield return coroutine;
            }

            yield return StartCoroutine(onCloseTransition.CoTween(this, _tweenBehaviour));

            onCloseTransition.OnTransitionComplete?.Invoke();
        }

        private IEnumerator CoDisableMenu()
        {
            yield return StartCoroutine(menus[_currentMenu].CoOnMenuDisabled());
            _isOpen = false;
        }

        public void PopBackstack()
        {
            if (_backstack.Count == 0) CloseMenu();

            SwapMenuAction action = _backstack.Pop();

            StartCoroutine(CoPopBackstack(action));
        }

        private IEnumerator CoPopBackstack(SwapMenuAction action)
        {
            var poppedTo = menus[action.SourceMenu];
            var poppedFrom = menus[action.Destination];
            yield return StartCoroutine(poppedFrom.CoRunTransition(action.OnPopFromTransition));
            poppedFrom.gameObject.SetActive(false);
            poppedTo.gameObject.SetActive(true);
            yield return StartCoroutine(poppedTo.CoRunTransition(action.OnPopBackToTransition));
        }
    }

    [Serializable]
    public struct Transition
    {
        public TweenDescriptionReference[] Tweens;
        public UnityEvent OnTransitionStart;
        public UnityEvent OnTransitionComplete;
        
        public IEnumerator CoTween(MonoBehaviour behaviour, TweenBehaviour tweener)
        {
            var coroutines = Tweens.Select(it =>
            {
                if (it.IsBehaviouIndex)
                {
                    return behaviour.StartCoroutine(it.BehaviourIndex.First.CoPlay(it.Value));
                }
                else
                {
                    return behaviour.StartCoroutine(tweener.CoPlay(it.Value));
                }
            });

            yield return behaviour.WaitForAll(coroutines);
        }
    }
}