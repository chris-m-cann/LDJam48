using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/RandomChunkGenerator")]
    public class RandomChunkGenerator : ChunkGenerator
    {
        [SerializeField] private ChunkGroup chunks;

        public override LevelChunk GenerateNext()
        {
            return chunks.Chunks.RandomElement();
        }
    }
}