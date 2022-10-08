using System;

namespace LDJam48.LevelGen
{
    [Serializable]
    public class GenerationData
    {
        public float Intensity;
        public float SprintsPerArea;
        public float ChunksPerSprint;


        public int AreasComplete;
        public int SprintsCompleteInArea;
        public int ChunksCompleteInSprint;

    }
}