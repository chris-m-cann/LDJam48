using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Util;
using Range = Util.Range;

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
        public Range IntensityFuzz;

        [NonSerialized] private Dictionary<int, ScrabbleBag<LevelChunk>> _chunks;
        public override LevelChunk GenerateNext(GenerationData data)
        {
            Range allowed = new Range{
                Start = data.Intensity - IntensityFuzz.Start,
                End = data.Intensity + IntensityFuzz.End

            };

            var scrabbleBag = PickScrabbleBag(allowed);

            var chunk = scrabbleBag.GetRandomElement();


            // Debug.Log($"Picked chunk of {chunk.Intensity}");

            return chunk;
        }

        private ScrabbleBag<LevelChunk> PickScrabbleBag(Range allowed)
        {
            var candidates = _chunks.Where(it => allowed.Contains(it.Key)).Select(it => it.Value).ToArray();
            if (candidates.Length > 0)
            {
                return candidates.ToArray().RandomElement();
            }

            var lessThan = _chunks.Where(it => it.Key < allowed.Start);
            if (!lessThan.Any())
            {
                return MinChunks();
            }

            var last = lessThan.Max(it => it.Key);
            allowed = new Range(last - IntensityFuzz.Start, last);
            candidates = _chunks.Where(it => allowed.Contains(it.Key)).Select(it => it.Value).ToArray();

            if (candidates.Length == 0)
            {
                return MaxChunks();
            }

            return candidates.RandomElement();;
        }

        private ScrabbleBag<LevelChunk> MinChunks()
        {
            var min = _chunks.Min(it => it.Key);
            return _chunks[min];
        }
        private ScrabbleBag<LevelChunk> MaxChunks()
        {
            var max = _chunks.Max(it => it.Key);
            return _chunks[max];
        }

        public override void Init(GenerationData data)
        {
            HasNext = true;

            if (_chunks != null) return;

            _chunks = Group.Chunks
                .GroupBy(it => it.Intensity)
                .ToDictionary(
                    it => it.Key,
                    it => new ScrabbleBag<LevelChunk>(it.ToArray())
                    );
        }
    }
}