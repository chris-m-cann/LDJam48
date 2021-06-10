using System.Linq;
using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    /*
     * Requires:
     *  - ChunkIntensity
     */

    [CreateAssetMenu(menuName = "Custom/Level/IntensityBasedChunkGenerator")]
    public class IntensityBasedChunkGenerator: ChunkGenerator
    {
        public ChunkGroup Group;
        public int IntensityFuzz;

        public override LevelChunk GenerateNext(GenerationData data)
        {
            Range allowed = new Range{
                Start = data.Intensity - IntensityFuzz,
                End = data.Intensity + IntensityFuzz

            };

            var candidates = Group.Chunks.Where(it => allowed.Contains(it.Intensity)).ToArray();

            if (candidates.Length == 0)
            {
                var lessThan = Group.Chunks.TakeWhile(it => it.Intensity < allowed.Start);
                if (lessThan.Any())
                {
                    var last = lessThan.Last();
                    allowed = new Range(last.Intensity - IntensityFuzz, last.Intensity);
                    candidates = Group.Chunks.Where(it => allowed.Contains(it.Intensity)).ToArray();
                }
                else
                {
                    candidates = MinChunks();
                }


                if (candidates.Length == 0)
                {
                    candidates = MaxChunks();
                }
            }


            var chunk = candidates.RandomElement();


            // Debug.Log($"Picked chunk of {chunk.Intensity}");

            return chunk;
        }

        private LevelChunk[] MinChunks()
        {
            var min = Group.Chunks.First().Intensity;
            return Group.Chunks.TakeWhile(it => it.Intensity < min + IntensityFuzz).ToArray();
        }
        private LevelChunk[] MaxChunks()
        {
            var chunks = Group.Chunks.Reverse();
            var max = chunks.First().Intensity;
            return chunks.TakeWhile(it => it.Intensity > max - IntensityFuzz).ToArray();

        }

        public override void Init(GenerationData data)
        {
            HasNext = true;
        }
    }
}