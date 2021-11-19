using System;
using System.Collections.Generic;
using System.Linq;
using Util.Var.Observe;

namespace Util
{
    public static class TypeEx
    {
        public static bool BaseIsObservable(this Type self) => HasBaseWhere(self,
            it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(ObservableVariable<>));

        public static bool HasBase(this Type self, Type test) => HasBaseWhere(self, t => t == test);
        public static bool HasBase<T>(this Type self) => HasBase(self, typeof(T));

        public static bool HasBaseWhere(this Type self, Predicate<Type> pred)
        {
            while (self != null && self != typeof(object))
            {
                if (pred(self)) return true;
                self = self.BaseType;
            }


            return false;
        }
        
        public static IEnumerable<Type> GetTypeList<T>()
        {
            return typeof(T).Assembly.GetTypes()
                .Where(it => !it.IsAbstract)
                .Where(it => !it.IsGenericTypeDefinition)
                .Where(it => typeof(T).IsAssignableFrom(it));
        }
    }
}