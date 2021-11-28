using System;
using System.Linq;
using Util.Var.Observe;

namespace LDJam48.StateMachine.Player.Action
{
    [Serializable]
    public class PerformActionBasedOnDirection : OneShotAction
    {
        public ObservableVector2Variable Direction;
        public OneShotAction[] LeftActions = new OneShotAction[0];
        public OneShotAction[] RightAction = new OneShotAction[0];
        public OneShotAction[] ZeroAction = new OneShotAction[0];

        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new PerformActionBasedOnDirectionRuntime(
                leftActions: LeftActions.Select(it => it.BuildRuntime()).ToArray(),
                rightActions: RightAction.Select(it => it.BuildRuntime()).ToArray(),
                zeroActions: ZeroAction.Select(it => it.BuildRuntime()).ToArray()
            );
        }
    }
    
    public class PerformActionBasedOnDirectionRuntime : BaseOneShotActionRuntime<PerformActionBasedOnDirection>
    {
        private readonly IOneShotAction[] _leftActions;
        private readonly IOneShotAction[] _rightActions;
        private readonly IOneShotAction[] _zeroActions;

        public PerformActionBasedOnDirectionRuntime(IOneShotAction[] leftActions, IOneShotAction[] rightActions, IOneShotAction[] zeroActions)
        {
            _leftActions = leftActions;
            _rightActions = rightActions;
            _zeroActions = zeroActions;
        }

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            
            _leftActions.OnAwake(machine);
            _rightActions.OnAwake(machine);
            _zeroActions.OnAwake(machine);
        }

        public override void Execute()
        {
            switch (_source.Direction.Value.x.CompareTo(0))
            {
                case -1 :
                    _leftActions.Execute();
                    break;
                case 0:
                    _zeroActions.Execute();
                    break;
                default:
                    _rightActions.Execute();
                    break;
            }
        }
    }
}