using System;
using DG.Tweening;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class ShakeAction : StateAction
    {
        public float duration;
        public float positionStrength;
        public int positionVibrato;
        [Range(0, 90)] public float positionRandomness;
        public bool easeOut;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new ShakeActionRuntime();
        }
    }

    public class ShakeActionRuntime : BaseStateActionRuntime<ShakeAction>
    {
        private Tweener _tweener;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _tweener = _machine.transform.DOShakePosition(
                _source.duration,
                _source.positionStrength,
                _source.positionVibrato,
                _source.positionRandomness,
                snapping: false,
                fadeOut: _source.easeOut
            );
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _tweener?.Kill();
            _tweener = null;
        }
    }
}