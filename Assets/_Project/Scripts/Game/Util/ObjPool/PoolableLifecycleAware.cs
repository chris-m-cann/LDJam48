using UnityEngine;

namespace Util.ObjPool
{
    public abstract class PoolableLifecycleAware : MonoBehaviour
    {
        public virtual void OnPush()
        {
        }

        public virtual void OnPop()
        {
            
        }
    }
}