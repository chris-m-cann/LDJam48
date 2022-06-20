using System;
using LDJam48.Tut;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

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
        [SerializeField] private FloatReference dashPressTime;
        [SerializeField] private FloatReference slamPressTime;
        [EnumToggleButtons] 
        [SerializeField] private PlayerInputs initialAllowedInputs;

        [SerializeField] private TutorialSaveableSO isTutorialEnabled;
        
        

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
                        dashPressTime.Value = Time.time;
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
                        dashPressTime.Value = Time.time;
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
                        slamPressTime.Value = Time.time;
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

        private void Start()
        {
            // AllowedInputs = (PlayerInputs)0xFF; // all
            AllowedInputs = isTutorialEnabled.Data.TutorialRequired ? initialAllowedInputs : (PlayerInputs)0xFF;
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