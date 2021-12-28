using System;

namespace Util
{
    public enum CompareOp
    {
        LessThan = -1, Equals = 0, GreaterThan = 1
    }

    public static class CompareOpEx
    {
        public static bool Compare<T>(this CompareOp self, T lhs, T rhs) where T : IComparable
        {
            int result = lhs.CompareTo(rhs);
            
            switch (self)
            {
                case CompareOp.LessThan:
                    return result < 0;
                case CompareOp.Equals:
                    return result == 0;
                case CompareOp.GreaterThan:
                    return result > 0;
                default:
                    return false;
            }
        }
    }
}