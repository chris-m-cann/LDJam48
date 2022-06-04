using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48.StateMachine.Player.Action
{
    [System.Serializable]
    public class PlayerOnWallStateAction : StateAction
    {
        public float minWallSpeed = 5;
        public float maxWallSpeed = 10;
        public string anim = "player_wall_land";
        public Vector2Reference maxVelocity;

        public LayerMask wallMask;
        public float boxcastSize = .5f;

        public FloatVariable CarriedYVel;
        public int LeftWallEffectIndex;
        public int RightWallEffectIndex;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerOnWallStateActionRuntime();
        }
    }
    
    public class PlayerOnWallStateActionRuntime : BaseStateActionRuntime<PlayerOnWallStateAction> {

    private bool _isLeft = false;

    private Rigidbody2D _rigidbody;
    private PlayerContacts _contacts;
    private SpriteRenderer _sprite;
    private ParticlesBehaviour _particles;
    private SoundsBehaviour _sounds;
    private PlayerRaycastsBehaviour _raycasts;
    
    public override void OnAwake(StateMachineBehaviour machine)
    {
        base.OnAwake(machine);
        _rigidbody = machine.GetComponent<Rigidbody2D>();
        _contacts = machine.GetComponent<PlayerContacts>();
        _sprite = machine.GetComponent<SpriteRenderer>();
        _particles = machine.GetComponent<ParticlesBehaviour>();
        _sounds = machine.GetComponent<SoundsBehaviour>();
        _raycasts = machine.GetComponent<PlayerRaycastsBehaviour>();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log($"updating on contacts on sliding");
        _contacts.UpdateContactDetails(true);
        _isLeft = _contacts.ContactDetails.IsOnLeftWall;
        
        _source.maxVelocity.Value = new Vector2(0, _source.maxWallSpeed);

        var wallSpeed = Mathf.Min(-_source.minWallSpeed, _source.CarriedYVel.Value);

        _rigidbody.velocity = new Vector2(0, wallSpeed);

        _sprite.flipX = !_isLeft;

        _raycasts.StickToWall(_isLeft);

        EnableWallParticles();
    }


        private void EnableWallParticles()
        {
            var effectIndex = _isLeft ? _source.LeftWallEffectIndex : _source.RightWallEffectIndex;
            _particles.PlayEffect(effectIndex);
        }
        
        private void DisableWallParticles()
        {
            var effectIndex = _isLeft ? _source.LeftWallEffectIndex : _source.RightWallEffectIndex;
            _particles.StopEffect(effectIndex);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            _source.CarriedYVel.Value = _rigidbody.velocity.y;

            DisableWallParticles();

            _source.maxVelocity.Value = new Vector2(100, _source.maxWallSpeed);
        }
    }
    
    // on enter anim
}