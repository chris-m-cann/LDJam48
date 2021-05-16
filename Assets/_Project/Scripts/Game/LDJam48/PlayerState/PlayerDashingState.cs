using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Dashing")]
    public class PlayerDashingState : PlayerState
    {
        [SerializeField] private float dashSpeed = 15;
        [SerializeField] private string anim = "player_slice";
        [SerializeField] private string actionMap = "Dashing";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private ObservableStringVariable activeActionMap;



        private Vector2 _direction;
        private string _prevActionMap;

        public PlayerDashingState Init(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            _machine.Context.Sprite.flipX = _direction. x < 0;


            _machine.Context.Rigidbody2D.velocity = dashSpeed * _direction;
            _machine.Context.Animator.Play(anim);

            machine.Context.MainCollider.enabled = false;
            machine.Context.SlashCollider.enabled = true;

            _machine.Context.SfxChannel.Raise(sound);


            _machine.Context.OnSlamInput += OnSlam;

            _prevActionMap = activeActionMap.Value;
            activeActionMap.Value = actionMap;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.MainCollider.enabled = true;
            _machine.Context.SlashCollider.enabled = false;
            _machine.Context.OnSlamInput -= OnSlam;
            activeActionMap.Value = _prevActionMap;
        }

        private void OnSlam() => _machine.CurrentState = _machine.States.Slamming;

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitLeftWallThisTurn ||
                _machine.Context.Contacts.HitRightWallThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.OnWall;
            }
        }
    }
}