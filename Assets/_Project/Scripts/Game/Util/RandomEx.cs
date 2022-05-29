using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public static class RandomEx
    {
        #region ranges

        /*
         *  Ranges:
         *   [ - includes
         *   ( - excludes
         *
         *   [start, end] - Closed Range
         *   (start, end) - Open Range
         *   [start, end) - Half-Open Range
         *   (start, end] - half-Open Range (I'll call it half-closed to differentiate)
         *
         *   minDiff - the closest you can get to a point without being "equal" to it
         */

        public static float Range(Range range) => ClosedRange(range);
        public static float ClosedRange(Range range) => Random.Range(range.Start, range.End);
        public static float OpenRange(Range range, float minDiff = float.Epsilon) => Random.Range(range.Start + minDiff, range.End - minDiff);
        public static float HalfOpenRange(Range range, float minDiff = float.Epsilon) => Random.Range(range.Start, range.End - minDiff);
        public static float HalfClosedRange(Range range, float minDiff = float.Epsilon) => Random.Range(range.Start + minDiff, range.End);

        #endregion


        public static T RandomElement<T>(this T[] self) => self[Random.Range(0, self.Length)];
        public static T RandomElement<T>(this List<T> self) => self[Random.Range(0, self.Count)];


        public static T RandomElement<T>(this T[] self, Predicate<T> predicate, int attemptCount = 100)
        {
            for (int i = 0; i < attemptCount; i++)
            {
                T choice = self[Random.Range(0, self.Length)];
                if (predicate(choice))
                {
                    return choice;
                }
            }

            foreach (var element in self)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            Debug.LogError("Predicate was false for all attempts");
            return RandomElement(self);
        }
        public static T RandomElement<T>(this List<T> self, Predicate<T> predicate, int attemptCount = 10)
        {
            for (int i = 0; i < attemptCount; i++)
            {
                T choice = self[Random.Range(0, self.Count)];
                if (predicate(choice))
                {
                    return choice;
                }
            }

            throw new ArgumentException("Predicate was false for all attempts");
        }
    }
}