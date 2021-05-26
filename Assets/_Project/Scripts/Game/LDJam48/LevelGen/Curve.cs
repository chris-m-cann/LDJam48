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
            var t = p / range;

            var interval = End - Start;

            var normalizedPoint = _interpolate(t);

            return Start + normalizedPoint * interval;
        }
    }
}