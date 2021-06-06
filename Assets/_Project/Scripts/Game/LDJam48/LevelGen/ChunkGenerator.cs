using UnityEngine;

namespace LDJam48.LevelGen
{
    public abstract class ChunkGenerator : ScriptableObject
    {
        public abstract LevelChunk GenerateNext();
    }
}