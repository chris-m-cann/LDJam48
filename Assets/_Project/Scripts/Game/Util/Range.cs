namespace Util
{
    [System.Serializable]
    public struct Range
    {
        public float Start;
        public float End;


        public Range(float start, float end)
        {
            Start = start;
            End = end;
        }
    }
}