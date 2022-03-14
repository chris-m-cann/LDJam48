using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class EnableGameObjectsStateAction : StateAction
    {
        public EnableGameObject[] objects = new EnableGameObject[1];
        protected override IStateAction BuildRuntimeImpl()
        {
            return new EnableGameObjectsStateActionRuntime();
        }
    }

    public class EnableGameObjectsStateActionRuntime : BaseStateActionRuntime<EnableGameObjectsStateAction>
    {
        private GameObject[] _gos;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _gos = new GameObject[_source.objects.Length];

            for (int i = 0; i < _gos.Length; i++)
            {
                _gos[i] = _machine.transform.Find(_source.objects[i].GameObjectName)?.gameObject;
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            for (int i = 0; i < _gos.Length; i++)
            {
                if (_gos[i] == null) continue;

                _gos[i].SetActive(_source.objects[i].enabledOnEnter);
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            for (int i = 0; i < _gos.Length; i++)
            {
                if (_gos[i] == null) continue;

                _gos[i].SetActive(_source.objects[i].enabledOnExit);
            }
        }
    }

    [Serializable]
    public struct EnableGameObject
    {
        public string GameObjectName;
        [Tooltip("on enter GameObjectName.active = enabledOnEnter")]
        public bool enabledOnEnter;
        [Tooltip("on exit GameObjectName.active = enabledOnExit")]
        public bool enabledOnExit;
    }
}