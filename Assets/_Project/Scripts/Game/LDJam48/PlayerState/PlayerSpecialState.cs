using UnityEngine;
using Util;
using Util.Var;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Special")]
    public class PlayerSpecialState : PlayerState
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float duration = .3f;
        [SerializeField] private string anim = "player_slam";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private Vector2Reference maxVelocity;



        public override PlayerState OnEnter()
        {
            maxVelocity.Value = new Vector2(maxVelocity.Value.x, speed);
            _machine.Context.Rigidbody2D.velocity = new Vector2(0, -speed);
            _machine.Context.Animator.Play(anim);

            _machine.Context.MainCollider.enabled = false;
            _machine.Context.SlamCollider.enabled = true;


            _machine.Context.SfxChannel.Raise(sound);

            _machine.Context.ExecuteAfter(duration, () =>
            {
                _machine.Context.MainCollider.enabled = true;
                _machine.Context.SlamCollider.enabled = false;
                if (_machine.CurrentState == this)
                {
                    _machine.CurrentState = _machine.States.Falling;
                }
            });

            return null;
        }

        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                return _machine.States.Idle;
            }

            return null;
        }
    }
}