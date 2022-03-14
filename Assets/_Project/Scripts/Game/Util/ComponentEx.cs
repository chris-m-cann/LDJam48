using LDJam48.StateMachine;
using Unity.VisualScripting;
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

        public static Component GetComponentInChildren(this Component self, string typename)
        {
            var component = self.GetComponent(typename);
            if (component != null) return component;

            for (int i = 0, childcount = self.transform.childCount; i < childcount; i++)
            {
                var child = self.transform.GetChild(i);

                component = child.GetComponentInChildren(typename);
                
                if (component != null) return component;
            }

            return null;
        }
    }
}