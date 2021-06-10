using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/RandomChunkGenerator")]
    public class RandomChunkGenerator : ChunkGenerator
    {
        [SerializeField] private ChunkGroup chunks;

        public override LevelChunk GenerateNext(GenerationData data)
        {
            return chunks.Chunks.RandomElement();
        }

        public override void Init(GenerationData data)
        {
            HasNext = true;
        }
    }
}