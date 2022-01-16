using System;
using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/InOrderChunkGenerator")]
    public class InOrderChunkGenerator : ChunkGenerator
    {
        [SerializeField] private ChunkGroup chunks;

        [NonSerialized] private int _idx = 0;
        public override LevelChunk GenerateNext(GenerationData data)
        {
            try
            {
                if (chunks.Chunks.Length > _idx)
                {
                    LevelChunk chunk = chunks.Chunks[_idx];
                    ++_idx;
                    return chunk;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                HasNext = chunks.Chunks.Length > _idx;
            }
        }

        public override void Init(GenerationData data)
        {
            HasNext = chunks.Chunks.Length > 0;
            _idx = 0;
        }
    }
}