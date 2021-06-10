using UnityEngine;

namespace LDJam48.LevelGen
{
    public class Curve
    {
        public delegate float Interpolate(float t);

        public float Start { get; set; }
        public float End { get; set; }

        private readonly Interpolate _interpolate;



        public Curve(float start, float end, Interpolate interpolate)
        {
            Start = start;
            End = end;
            _interpolate = interpolate;
        }

        public float GetPoint(float p, float range)
        {
            if (range <= 0)
            {
                // todo(chris) log error here? is it an error or a feature if a range is 0 then always return Start or End?
                return Start;
            }

            var t = p / range;

            var interval = End - Start;

            var normalizedPoint = _interpolate(t);

            return Start + normalizedPoint * interval;
        }

        public static Curve Linear(float start, float end) => new Curve(start, end, t => t);
        public static Curve Squared(float start, float end) => new Curve(start, end, t => t * t);
        public static Curve Floor(float start, float end) => new Curve(start, end, t => 0);
    }
}