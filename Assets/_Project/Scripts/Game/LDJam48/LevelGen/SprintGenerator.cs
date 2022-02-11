using System;
using System.Linq;
using UnityEngine;
using Util;
using Range = Util.Range;

namespace LDJam48.LevelGen
{
    public class SprintGenerator : MonoBehaviour
    {
        public ChunkGroup Group;
        public int IntensityFuzz;

        public LevelChunk[] GenerateSprint(int numChunks, Curve curve)
        {
            Group.Chunks = Group.Chunks.OrderBy(it => it.Intensity).ToArray();

            var chunks = new LevelChunk[numChunks];

            for (int i = 0; i < numChunks; i++)
            {
                var intensity = curve.GetPoint(i, numChunks - 1);
                chunks[i] = PickChunk(intensity);
            }

            return chunks;
        }

        public LevelChunk[] GenerateSprint(int startIntensity, float intensityGradient, float peakGradient, int peaks, int chunksPerPeak)
        {
            var chunks = new LevelChunk[peaks * chunksPerPeak];

            var chunkIdx = 0;
            for (int peak = 0; peak < peaks; ++peak)
            {
                var start = startIntensity + peak * intensityGradient;

                for (int c = 0; c < chunksPerPeak; ++c)
                {
                    var intensity = start + c * peakGradient;

                    chunks[chunkIdx] = PickChunk((int)intensity);
                    ++chunkIdx;
                }

            }

            return chunks;
        }

        private LevelChunk PickChunk(float intensity)
        {
            Range allowed = new Range{
                Start = intensity - IntensityFuzz,
                End = intensity + IntensityFuzz

            };

            // Debug.Log($"Picking Chunk between {allowed.Start} and {allowed.End}");

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

    public static class RangeEx
    {
        public static bool Contains(this Range self, float candidate)
        {
            return candidate >= self.Start && candidate <= self.End;
        }
    }
}