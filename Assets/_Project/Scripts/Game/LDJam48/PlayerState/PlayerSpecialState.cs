using UnityEngine;
using Util;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Special")]
    public class PlayerSpecialState : PlayerState
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float duration = .3f;
        [SerializeField] private string anim = "player_slam";
        [SerializeField] private AudioClipAsset sound;



        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(0, -speed);
            machine.Context.Animator.Play(anim);

            machine.Context.MainCollider.enabled = false;
            machine.Context.SlamCollider.enabled = true;


            machine.Context.SfxChannel.Raise(sound);

            _machine.Context.ExecuteAfter(duration, () =>
            {
                machine.Context.MainCollider.enabled = true;
                machine.Context.SlamCollider.enabled = false;
                if (machine.CurrentState == this)
                {
                    machine.CurrentState = machine.States.Falling;
                }
            });
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.Idle;
                return;
            }
        }
    }
}