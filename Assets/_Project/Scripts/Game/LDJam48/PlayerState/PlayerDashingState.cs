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
        [SerializeField] private string onExitActionMap = "NotDashing";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private ObservableStringVariable activeActionMap;



        private Vector2 _direction;
        private float _prevGravity = 1f;

        public PlayerDashingState Init(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public override PlayerState OnEnter()
        {
            _machine.Context.Sprite.flipX = _direction. x < 0;

            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;

            _machine.Context.Rigidbody2D.velocity = dashSpeed * _direction;
            _prevGravity = _machine.Context.Rigidbody2D.gravityScale;
            _machine.Context.Rigidbody2D.gravityScale = 0f;
            _machine.Context.Animator.Play(anim);

            _machine.Context.MainCollider.enabled = false;
            _machine.Context.SlashCollider.enabled = true;

            _machine.Context.SfxChannel.Raise(sound);


            _machine.Context.OnSlamInput += OnSlam;

            Debug.Log($"OnEnter Setting actionMap = {actionMap}");
            activeActionMap.Value = actionMap;

            return null;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.MainCollider.enabled = true;
            _machine.Context.SlashCollider.enabled = false;
            _machine.Context.OnSlamInput -= OnSlam;
            Debug.Log($"On Exit Setting actionMap = {onExitActionMap}");
            activeActionMap.Value = onExitActionMap;

            _machine.Context.Rigidbody2D.gravityScale = _prevGravity;
        }

        private void OnSlam() => _machine.CurrentState = _machine.States.Slamming;

        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.HitLeftWallThisTurn ||
                _machine.Context.Contacts.HitRightWallThisTurn)
            {
                return _machine.States.OnWall;
            }

            return null;
        }
    }
}