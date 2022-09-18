using UnityEngine;

namespace Util.ObjPool
{
    public abstract class PoolableLifecycleAwareBehaviour : MonoBehaviour
    {
        public virtual void OnPush()
        {
        }

        public virtual void OnPop()
        {
            
        }
    }
    
    
}