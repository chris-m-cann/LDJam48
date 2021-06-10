using UnityEngine;

namespace LDJam48.LevelGen
{
    public abstract class ChunkGenerator : ScriptableObject
    {
        public abstract LevelChunk GenerateNext(GenerationData data);

        public abstract void Init(GenerationData data);
        
        public bool HasNext { get; protected set; }
    }
}