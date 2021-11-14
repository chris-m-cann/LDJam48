using UnityEngine;

namespace Util
{
    public static class ArrayEx
    {
        public static void Fill<T>(this T[] self, T v)
        {
            for (int i = 0; i < self.Length; i++)
            {
                self[i] = v;
            }
        }

        public static void Swap<T>(this T[] self, int i, int j)
        {
            (self[i], self[j]) = (self[j], self[i]);
        }

        public static bool HasIndex<T>(this T[] self, int idx)
        {
            return idx > -1 && idx < self.Length;
        }
    }
}
