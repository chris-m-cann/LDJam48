using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D leftCollider;
        [SerializeField] private BoxCollider2D rightCollider;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private BoxCollider2D floorCollider;
        [SerializeField] private LayerMask floorLayer;


        [SerializeField] private float wallSpeed;
        [SerializeField] private float fallSpeed;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashFallSpeed;
        [SerializeField] private float slashTime;
        [SerializeField] private float slamSpeed;
        [SerializeField] private float slamTime;

        [SerializeField] private float bounceVel;
        [SerializeField] private float bounceTime;

        [SerializeField] private Collider2D mainCollider;
        [SerializeField] private Collider2D slashCollider;
        [SerializeField] private Collider2D slamCollider;

        [SerializeField] private Variable<bool> isPaused;

        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset dashClip;
        [SerializeField] private AudioClipAsset slashClip;
        [SerializeField] private AudioClipAsset landClip;
        [SerializeField] private AudioClipAsset slamClip;


        [SerializeField] private ObservableIntVariable input;



        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _sprite;

        private bool _isDashing;
        private ContactDetails _contacts;
        private bool _isSlashing;
        private bool _isSlamming;

        public bool IsAttacking => _isSlamming || _isSlashing;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            EnterFreeFall();
        }

        private void OnLeftWallChanged(bool onWall)
        {
            if (onWall)
            {
                _rb.velocity = new Vector2(0, -wallSpeed);
                _isDashing = false;
                _animator.Play("player_slide");
                _sprite.flipX = false;
            }
        }

        private void OnRightWallChanged(bool onWall)
        {
            if (onWall)
            {
                _rb.velocity = new Vector2(0, -wallSpeed);
                _isDashing = false;
                _animator.Play("player_slide");
                _sprite.flipX = true;
            }
        }

        private void OnFloorChanged(bool onFloor)
        {
            if (onFloor)
            {
                sfxChannel.Raise(landClip);
                _rb.velocity = Vector2.zero;
                _isDashing = false;
                _animator.Play("player_idle");
            }
        }

        private void Update()
        {
            if (isPaused?.Value == true) return;
            // no input when doing these moves
            if (_isSlamming || _isSlashing) return;

            if (input.Value != 0)
            {

            }

            var isLeftPressed = Input.GetKeyDown(KeyCode.LeftArrow);
            var isRightPressed = Input.GetKeyDown(KeyCode.RightArrow);

            if (input.Value != 0)
            {
                isLeftPressed = input.Value == -1;
                isRightPressed = input.Value == 1;
            }

            input.Value = 0;

            if (_isDashing)
            {
                if (_rb.velocity.x > 0)
                {
                    if (isLeftPressed)
                    {
                        Slam();
                    } else if (isRightPressed)
                    {
                        Slash();
                    }
                }
                else
                {
                    if (isLeftPressed)
                    {
                        Slash();
                    } else if (isRightPressed)
                    {
                        Slam();
                    }
                }

                return;
            }

            if (_contacts.IsOnFloor)
            {
                if (!_contacts.IsOnRightWall && isRightPressed)
                {
                    DashRight();
                } else if (!_contacts.IsOnLeftWall && isLeftPressed)
                {
                    DashLeft();
                }
            }
            else
            {
                if (_contacts.IsOnLeftWall)
                {
                    if (isRightPressed)
                    {
                        DashRight();
                    }
                } else if (_contacts.IsOnRightWall)
                {
                    if (isLeftPressed)
                    {
                        DashLeft();
                    }
                } else // free fall
                {
                    if (isLeftPressed)
                    {
                        DashLeft();
                    }
                    else if (isRightPressed)
                    {
                        DashRight();
                    }
                }
            }
        }

        private void DashRight()
        {
            _rb.velocity = new Vector2(dashSpeed, -dashFallSpeed);
            _isDashing = true;
            _animator.Play("player_dash");
            _sprite.flipX = false;

            sfxChannel.Raise(dashClip);
        }


        private void DashLeft()
        {
            _rb.velocity = new Vector2(-dashSpeed, -dashFallSpeed);
            _isDashing = true;
            _animator.Play("player_dash");
            _sprite.flipX = true;

            sfxChannel.Raise(dashClip);
        }

        private void Slash()
        {
            _isSlashing = true;
            _animator.Play("player_slice");
            mainCollider.enabled = false;
            slashCollider.enabled = true;


            sfxChannel.Raise(slashClip);

            this.ExecuteAfter(slashTime, () =>
            {
                mainCollider.enabled = true;
                slashCollider.enabled = false;
                _isSlashing = false;
                if (_isDashing)
                {
                    _animator.Play("player_dash");
                }
            });
        }

        private void Slam()
        {
            _isSlamming = true;
            _isDashing = false;
            _animator.Play("player_slam");
            _rb.velocity = Vector2.down * slamSpeed;
            mainCollider.enabled = false;
            slamCollider.enabled = true;

            sfxChannel.Raise(slamClip);

            this.ExecuteAfter(slashTime, () =>
            {
                mainCollider.enabled = true;
                slamCollider.enabled = false;
                _isSlamming = false;

                if (_contacts.IsOnFloor)
                {
                    OnFloorChanged(true);
                } else if (
                    !_contacts.IsOnLeftWall &&
                    !_contacts.IsOnRightWall)
                {
                    EnterFreeFall();
                }

            });
        }




        private void FixedUpdate()
        {
            _contacts = DetectContacts(_contacts);
            if (_contacts.HitLeftWallThisTurn)
            {
                OnLeftWallChanged(true);
            }
            if (_contacts.HitRightWallThisTurn)
            {
                OnRightWallChanged(true);
            }

            if (_contacts.HitFloorThisTurn)
            {
                OnFloorChanged(true);
            }

            if (!_contacts.IsOnFloor && (_contacts.LeftLeftWallThisTurn || _contacts.LeftRightWallThisTurn) && !_isDashing)
            {
                EnterFreeFall();
            }
        }

        private void EnterFreeFall()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeed);
            _animator.Play("player_fall");
        }

        private ContactDetails DetectContacts(ContactDetails contact)
        {
            contact.WasOnLeftWall = contact.IsOnLeftWall;
            contact.WasOnRightWall = contact.IsOnRightWall;
            contact.WasOnFloor = contact.IsOnFloor;

            contact.IsOnLeftWall = Physics2D.OverlapBox((Vector2) transform.position + leftCollider.offset,
                leftCollider.size, 0, wallLayer);

            contact.IsOnRightWall = Physics2D.OverlapBox((Vector2) transform.position + rightCollider.offset,
                rightCollider.size, 0, wallLayer);

            contact.IsOnFloor = Physics2D.OverlapBox((Vector2) transform.position + floorCollider.offset,
                floorCollider.size, 0, wallLayer);

            return contact;
        }





        public void Bounce()
        {
            _isDashing = false;
            _isSlamming = false;
            _isSlashing = false;

            _rb.velocity = new Vector2(_rb.velocity.x, bounceVel);

            this.ExecuteAfter(bounceTime, () =>
            {
                if (!_isDashing)
                {
                    EnterFreeFall();
                }
            });
        }
    }

    #region StateBaseContoller

    // class PlayerControllerStateBased : MonoBehaviour
    // {
    //            [SerializeField] private BoxCollider2D leftCollider;
    //     [SerializeField] private BoxCollider2D rightCollider;
    //     [SerializeField] private LayerMask wallLayer;
    //     [SerializeField] private BoxCollider2D floorCollider;
    //     [SerializeField] private LayerMask floorLayer;
    //
    //
    //     [SerializeField] private float wallSpeed;
    //     [SerializeField] private float fallSpeed;
    //     [SerializeField] private float dashSpeed;
    //     [SerializeField] private float dashFallSpeed;
    //
    //
    //     private Rigidbody2D _rb;
    //     private Animator _animator;
    //     private SpriteRenderer _sprite;
    //
    //     private bool _isDashing;
    //     private bool _onLeftWall, _onRightWall, _isOnFloor;
    //
    //     private bool _isLeftPressed, _isRightPressed;
    //
    //     private ContactDetails _contacts;
    //
    //     private IState _state = new FreeFall();
    //
    //     private void Awake()
    //     {
    //         _sprite = GetComponent<SpriteRenderer>();
    //         _animator = GetComponent<Animator>();
    //         _rb = GetComponent<Rigidbody2D>();
    //     }
    //
    //     private void OnEnable()
    //     {
    //         _rb.velocity = Vector2.down * fallSpeed;
    //         _animator.Play("player_fall");
    //         // isOnLeftWall.OnValueChanged += OnLeftWallChanged;
    //         // isOnRightWall.OnValueChanged += OnRightWallChanged;
    //         // isOnFloor.OnValueChanged += OnFloorChanged;
    //     }
    //
    //     private void OnDisable()
    //     {
    //         // isOnLeftWall.OnValueChanged -= OnLeftWallChanged;
    //         // isOnRightWall.OnValueChanged -= OnRightWallChanged;
    //         // isOnFloor.OnValueChanged -= OnFloorChanged;
    //     }
    //
    //     private void OnLeftWallChanged(bool onWall)
    //     {
    //         if (onWall)
    //         {
    //             Debug.Log("OnLeftWall");
    //             _rb.velocity = new Vector2(0, -wallSpeed);
    //             _isDashing = false;
    //             _animator.Play("player_slide");
    //             _sprite.flipX = false;
    //         }
    //     }
    //
    //     private void OnRightWallChanged(bool onWall)
    //     {
    //         if (onWall)
    //         {
    //             _rb.velocity = new Vector2(0, -wallSpeed);
    //             _isDashing = false;
    //             _animator.Play("player_slide");
    //             _sprite.flipX = true;
    //         }
    //     }
    //
    //     private void OnFloorChanged(bool onFloor)
    //     {
    //         if (onFloor)
    //         {
    //             _rb.velocity = Vector2.zero;
    //             _isDashing = false;
    //             _animator.Play("player_idle");
    //         }
    //     }
    //
    //     public void LeftPressed(bool v)
    //     {
    //         _isLeftPressed = v;
    //     }
    //
    //     public void RightPressed(bool v)
    //     {
    //         _isRightPressed = v;
    //     }
    //
    //     private bool ConsumeLeftPress()
    //     {
    //         return _isLeftPressed;
    //         var tmp = _isLeftPressed;
    //         _isLeftPressed = false;
    //         return tmp;
    //     }
    //
    //
    //     private bool ConsumeRightPress()
    //     {
    //         return _isRightPressed;
    //         var tmp = _isRightPressed;
    //         _isRightPressed = false;
    //         return tmp;
    //     }
    //     private void Update()
    //     {
    //         // var horizontal = Input.GetAxisRaw("Horizontal");
    //         // LeftPressed(horizontal < 0);
    //         // RightPressed(horizontal > 0);
    //         LeftPressed(Input.GetKeyDown(KeyCode.LeftArrow));
    //         RightPressed(Input.GetKeyDown(KeyCode.RightArrow));
    //     }
    //
    //     private void DashRight()
    //     {
    //         _rb.velocity = new Vector2(dashSpeed, -dashFallSpeed);
    //         _isDashing = true;
    //         _animator.Play("player_dash");
    //         _sprite.flipX = false;
    //     }
    //
    //
    //     private void DashLeft()
    //     {
    //         _rb.velocity = new Vector2(-dashSpeed, -dashFallSpeed);
    //         _isDashing = true;
    //         _animator.Play("player_dash");
    //         _sprite.flipX = true;
    //     }
    //
    //     private void Slash()
    //     {
    //
    //         _animator.Play("player_slice");
    //     }
    //
    //     private void FixedUpdate()
    //     {
    //
    //         _contacts = DetectContacts(_contacts);
    //
    //         _state.FixedUpdate(this);
    //     }
    //
    //
    //     private ContactDetails DetectContacts(ContactDetails contact)
    //     {
    //         contact.WasOnLeftWall = contact.IsOnLeftWall;
    //         contact.WasOnRightWall = contact.IsOnRightWall;
    //         contact.WasOnFloor = contact.IsOnFloor;
    //
    //         contact.IsOnLeftWall = Physics2D.OverlapBox((Vector2) transform.position + leftCollider.offset,
    //             leftCollider.size, 0, wallLayer);
    //
    //         contact.IsOnRightWall = Physics2D.OverlapBox((Vector2) transform.position + rightCollider.offset,
    //             rightCollider.size, 0, wallLayer);
    //
    //         contact.IsOnFloor = Physics2D.OverlapBox((Vector2) transform.position + floorCollider.offset,
    //             floorCollider.size, 0, wallLayer);
    //
    //         return contact;
    //     }
    //
    //     private void ChangeState(IState state)
    //     {
    //         _state.OnExit(this);
    //         _state = state;
    //         _state.OnEnter(this);
    //     }
    //
    //
    //     public struct ContactDetails
    //     {
    //         public bool WasOnLeftWall;
    //         public bool IsOnLeftWall;
    //
    //         public bool WasOnRightWall;
    //         public bool IsOnRightWall;
    //
    //         public bool WasOnFloor;
    //         public bool IsOnFloor;
    //
    //         public bool HitLeftWallThisTurn => !WasOnLeftWall && IsOnLeftWall;
    //         public bool HitRightWallThisTurn => !WasOnRightWall && IsOnRightWall;
    //         public bool HitFloorThisTurn => !WasOnFloor && IsOnFloor;
    //
    //
    //         public bool LeftLeftWallThisTurn => WasOnLeftWall && !IsOnLeftWall;
    //         public bool LeftRightWallThisTurn => WasOnRightWall && !IsOnRightWall;
    //         public bool LeftFloorThisTurn => WasOnFloor && !IsOnFloor;
    //     }
    //
    //     interface IState
    //     {
    //         void OnEnter(PlayerControllerStateBased controller);
    //         void OnExit(PlayerControllerStateBased controller);
    //         void Update(PlayerControllerStateBased controller);
    //         void FixedUpdate(PlayerControllerStateBased controller);
    //     }
    //
    //     abstract class BaseState : IState
    //     {
    //         public virtual void OnEnter(PlayerControllerStateBased controller){}
    //         public virtual void OnExit(PlayerControllerStateBased controller){}
    //         public virtual  void Update(PlayerControllerStateBased controller){}
    //         public virtual  void FixedUpdate(PlayerControllerStateBased controller){}
    //     }
    //
    //     class FreeFall : BaseState
    //     {
    //         private bool _hasDashed = false;
    //
    //         public override void OnEnter(PlayerControllerStateBased controller)
    //         {
    //             base.OnEnter(controller);
    //
    //             controller._rb.velocity = Vector2.down * controller.fallSpeed;
    //             controller._animator.Play("player_fall");
    //         }
    //
    //         public override void FixedUpdate(PlayerControllerStateBased controller)
    //         {
    //             base.FixedUpdate(controller);
    //
    //             var next = HandleInput(controller);
    //
    //             if (next != null)
    //             {
    //                 controller.ChangeState(next);
    //                 return;
    //             }
    //
    //             if (controller._contacts.HitFloorThisTurn)
    //             {
    //                 controller.ChangeState(new Idle());
    //                 return;
    //             }
    //
    //             if (controller._contacts.HitLeftWallThisTurn)
    //             {
    //                 controller.ChangeState(new OnWall(true));
    //                 return;
    //             }
    //
    //
    //             if (controller._contacts.HitRightWallThisTurn)
    //             {
    //                 controller.ChangeState(new OnWall(false));
    //                 return;
    //             }
    //         }
    //
    //         private IState HandleInput(PlayerControllerStateBased controller)
    //         {
    //             if (controller.ConsumeLeftPress())
    //             {
    //                 controller.DashLeft();
    //                 return new Dashing(true);
    //             }
    //
    //             if (controller.ConsumeRightPress())
    //             {
    //                 controller.DashRight();
    //                 return new Dashing(false);
    //             }
    //
    //
    //             return null;
    //         }
    //     }
    //
    //     class OnWall : BaseState
    //     {
    //         private bool _isLeftWall;
    //
    //         public OnWall(bool isLeftWall)
    //         {
    //             _isLeftWall = isLeftWall;
    //         }
    //
    //         public override void OnEnter(PlayerControllerStateBased controller)
    //         {
    //             base.OnEnter(controller);
    //
    //             if (_isLeftWall)
    //             {
    //                 controller.OnLeftWallChanged(true);
    //             }
    //             else
    //             {
    //                 controller.OnRightWallChanged(true);
    //             }
    //         }
    //
    //         public override void FixedUpdate(PlayerControllerStateBased controller)
    //         {
    //             base.FixedUpdate(controller);
    //
    //
    //             var next = HandleInput(controller);
    //
    //             if (next != null)
    //             {
    //                 controller.ChangeState(next);
    //                 return;
    //             }
    //
    //             if (controller._contacts.HitFloorThisTurn)
    //             {
    //                 controller.ChangeState(new Idle());
    //                 return;
    //             }
    //
    //             if (controller._contacts.LeftLeftWallThisTurn)
    //             {
    //                 controller.ChangeState(new FreeFall());
    //                 return;
    //             }
    //         }
    //
    //         private IState HandleInput(PlayerControllerStateBased controller)
    //         {
    //             if (_isLeftWall)
    //             {
    //                 if (controller.ConsumeRightPress())
    //                 {
    //                     controller.DashRight();
    //                     return new Dashing(false);
    //                 }
    //             }
    //             else
    //             {
    //                 if (controller.ConsumeLeftPress())
    //                 {
    //                     controller.DashLeft();
    //                     return new Dashing(true);
    //                 }
    //             }
    //
    //             return null;
    //         }
    //     }
    //
    //     class Dashing : BaseState
    //     {
    //         private bool _left;
    //
    //         public Dashing(bool left)
    //         {
    //             _left = left;
    //         }
    //
    //         public override void FixedUpdate(PlayerControllerStateBased controller)
    //         {
    //             base.FixedUpdate(controller);
    //
    //
    //             var next = HandleInput(controller);
    //
    //             if (next != null)
    //             {
    //                 controller.ChangeState(next);
    //                 return;
    //             }
    //
    //             if (controller._contacts.HitFloorThisTurn)
    //             {
    //                 controller.ChangeState(new Idle());
    //                 return;
    //             }
    //
    //             if (controller._contacts.HitLeftWallThisTurn)
    //             {
    //                 controller.ChangeState(new OnWall(true));
    //                 return;
    //             }
    //
    //
    //             if (controller._contacts.HitRightWallThisTurn)
    //             {
    //                 controller.ChangeState(new OnWall(false));
    //                 return;
    //             }
    //         }
    //
    //         private IState HandleInput(PlayerControllerStateBased controller)
    //         {
    //             if (_left)
    //             {
    //                 if (controller.ConsumeLeftPress())
    //                 {
    //                     controller.Slash();
    //                     return null;
    //                 }
    //             }
    //             else
    //             {
    //                 if (controller.ConsumeRightPress())
    //                 {
    //                     controller.Slash();
    //                     return null;
    //                 }
    //             }
    //
    //             return null;
    //         }
    //     }
    //
    //     class Idle : BaseState
    //     {
    //         public override void OnEnter(PlayerControllerStateBased controller)
    //         {
    //             base.OnEnter(controller);
    //             controller.OnFloorChanged(true);
    //         }
    //
    //         public override void FixedUpdate(PlayerControllerStateBased controller)
    //         {
    //             base.FixedUpdate(controller);
    //
    //             var next = HandleInput(controller);
    //
    //             if (next != null)
    //             {
    //                 controller.ChangeState(next);
    //                 return;
    //             }
    //
    //             if (controller._contacts.LeftFloorThisTurn)
    //             {
    //                 controller.ChangeState(new FreeFall());
    //                 return;
    //             }
    //         }
    //
    //         private IState HandleInput(PlayerControllerStateBased controller)
    //         {
    //             if (controller.ConsumeLeftPress())
    //             {
    //                 controller.DashLeft();
    //                 return new Dashing(true);
    //             }
    //
    //             if (controller.ConsumeRightPress())
    //             {
    //                 controller.DashRight();
    //                 return new Dashing(false);
    //             }
    //
    //
    //             return null;
    //         }
    //     }
    //
    //
    // }
    //

    #endregion
}
