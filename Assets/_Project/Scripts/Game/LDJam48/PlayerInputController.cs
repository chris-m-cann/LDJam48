using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48
{
    public class PlayerInputController : MonoBehaviour
    {

        [Flags]
        public enum PlayerInputs
        {
            DashLeft = 1,
            DashRight = 1 << 1,
            Slam = 1 << 2
        }

        
        [SerializeField] private Vector2GameEvent dashAction;
        [SerializeField] private VoidGameEvent dashLeftAction;
        [SerializeField] private VoidGameEvent dashRightAction;
        [SerializeField] private VoidGameEvent slamAction;

        public event Action<PlayerInputs> OnPlayerInput;

        public PlayerInputs AllowedInputs
        {
            get => _allowedActions;
            set
            {
                _allowedActions = value;
                if ((_allowedActions & PlayerInputs.DashLeft) > 0)
                { 
                    _onDashLeft = () =>
                    {
                        OnPlayerInput?.Invoke(PlayerInputs.DashLeft);
                        dashAction?.Raise(Vector2.left);
                        dashLeftAction?.Raise();
                    };
                }
                else
                {
                    _onDashLeft = NullOp.Fun;
                }
                
                if ((_allowedActions & PlayerInputs.DashRight) > 0)
                { 
                    _onDashRight = () =>
                    {
                        OnPlayerInput?.Invoke(PlayerInputs.DashRight);
                        dashAction?.Raise(Vector2.right);
                        dashRightAction?.Raise();
                    };
                }
                else
                {
                    _onDashRight = NullOp.Fun;
                }
                
                if ((_allowedActions & PlayerInputs.Slam) > 0)
                { 
                    _onSlam = () =>
                    {
                        OnPlayerInput?.Invoke(PlayerInputs.Slam);
                        slamAction?.Raise();
                    };
                }
                else
                {
                    _onSlam = NullOp.Fun;
                }
            }
        }


        private PlayerInputs _allowedActions;
        private Action _onDashLeft;
        private Action _onDashRight;
        private Action _onSlam;

        private void Awake()
        {
            AllowedInputs = (PlayerInputs)0xFF; // all
        }

        public void OnDash(Vector2 direction)
        {
            if (direction == Vector2.left)
            {
                OnDashLeft();
            }
            
            
            if (direction == Vector2.right)
            {
                OnDashRight();
            }
            
        }

        public void OnDashLeft()
        {
            _onDashLeft();
        }
        
        public void OnDashRight()
        {
            _onDashRight();
        }

        public void OnSlam()
        {
            _onSlam();
        }
    }
}