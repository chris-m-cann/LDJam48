using UnityEngine;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/LevelGen/ChunkGroup")]
    public class ChunkGroup : ScriptableObject
    {
        public string Description;
        public LevelChunk[] Chunks;
    }
}