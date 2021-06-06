using System.Linq;
using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/IntensityBasedChunkGenerator")]
    public class IntensityBasedChunkGenerator: ChunkGenerator
    {
        public float Intensity;
        public ChunkGroup Group;
        public int IntensityFuzz;

        public override LevelChunk GenerateNext()
        {
            Range allowed = new Range{
                Start = Intensity - IntensityFuzz,
                End = Intensity + IntensityFuzz

            };

            Debug.Log($"Picking Chunk between {allowed.Start} and {allowed.End}");

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
    }
}