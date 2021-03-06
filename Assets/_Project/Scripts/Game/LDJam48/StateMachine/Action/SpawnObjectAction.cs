using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class SpawnObjectAction : OneShotAction
    {
        public GameObject prefab;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new SpawnObjectActionRuntime();
        }
    }

    public class SpawnObjectActionRuntime : BaseOneShotActionRuntime<SpawnObjectAction>
    {
        public override void Execute()
        {
            Object.Instantiate(_source.prefab, _machine.transform.position, Quaternion.identity);
        }
    }
    
    /*
     * SpawnObject (explosion)
     * SetTarget in move to target script / unset
     * SetTarget in homing script / unset
     * DestroySelf
     *
     *
     * TargetInRange (x, y) [detector set off?]
     * hit wall/ anything
     * 
     */
}