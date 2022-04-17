using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LDJam48.StateMachine.Player.Action;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Action
{
    [System.Serializable]
    public class DashAction : StateAction
    {
        public float leftWallX;
        public float rightWallX;
        [DrawWithUnity]
        public AnimationCurve dashCurve;
        public float dashTime;
        public float dashStartDelay;
        public Vector2 defaultDirection;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new DashActionRuntime();
        }
    }

    public class DashActionRuntime : BaseStateActionRuntime<DashAction>
    {
        private PlayerRaycastsBehaviour _raycasts;
        private SpriteRenderer _sprite;
        private Tweener _dashCoroutine;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _machine.TryGetComponent(out _raycasts);
            _machine.TryGetComponent(out _sprite);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _machine.ExecuteAfter(_source.dashStartDelay, StartDash);
        }

        private void StartDash()
        {
            Vector2 direction = _source.defaultDirection;

            if (_sprite.flipX)
            {
                direction *= -1;
            }

            var end = _raycasts.FindDashLandPoint(direction, rightWallX:_source.rightWallX, leftWallX:_source.leftWallX);

            var maxDistance = Mathf.Abs(_source.rightWallX - _source.leftWallX);
            var actualDistance = Mathf.Abs(((Vector2)_machine.transform.position - end).x);
            var factor = (float)actualDistance / maxDistance;
            var time = _source.dashTime * factor;
            
            _dashCoroutine = _machine.transform.DOMoveX(end.x, time).SetEase(_source.dashCurve)
                .SetUpdate(UpdateType.Fixed, isIndependentUpdate:false)
                .OnComplete(() =>
                {
                    _sprite.flipX = !_sprite.flipX;
                    _machine.StateComplete(0);
                });
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _dashCoroutine.Kill();
        }
    }
}