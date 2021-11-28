using LDJam48.StateMachine;
using UnityEngine;

namespace Util
{
    public static class ComponentEx
    {
        public static void SetComponent<T>(this Component self, ref T component)
        {
            component = self.GetComponent<T>();
        }
        
        public static void SetComponent<T1, T2>(this Component self, ref T1 component1, ref T2 component2)
        {
            self.SetComponent(ref component1);
            self.SetComponent(ref component2);
        }
        
        public static void SetComponent<T1, T2, T3>(this Component self, ref T1 component1, ref T2 component2, ref T3 component3)
        {
            self.SetComponent(ref component1);
            self.SetComponent(ref component2, ref component3);
        }
        
        public static void SetComponent<T1, T2, T3, T4>(this Component self, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4)
        {
            self.SetComponent(ref component1);
            self.SetComponent(ref component2, ref component3, ref component4);
        }
    }
}