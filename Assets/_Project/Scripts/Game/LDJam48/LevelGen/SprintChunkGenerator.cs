using System;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48.LevelGen
{
    /*
     * Requires:
     *  - BaseChunksPerSprint
     *  - BaseStartIntensity
     *
     * Handles:
     *  - Delta CpS: rough number of chunks to modify the base by
     *  - Delta i: rough number to modify the base intensity by
     *  - Window size: how high up it ramps in intensity
     */

    [CreateAssetMenu(menuName = "Custom/Level/SprintChunkGenerator")]
    public class SprintChunkGenerator : ChunkGenerator
    {
        public ChunkGenerator[] ChunkGenerators;
        public ChunkGenerator RestGenerator;
        public int NumChunksFuzz;

        public float IntensityWindow;
        // todo(chris) add curve type

        [NonSerialized] private int _chunkIdx;
        [NonSerialized] private Curve _curve;
        [NonSerialized] private int _chunksThisSprint;
        [NonSerialized] private ChunkGenerator _currentGenerator;

        public override LevelChunk GenerateNext(GenerationData data)
        {
            // if not started sprint yet the set up
            if (_currentGenerator == null) Init(data);

            // if generated all the chunks for this sprint then add on the end of sprint rest
            if (_chunkIdx == _chunksThisSprint)
            {
                HasNext = false;
                Reset();
                return RestGenerator.GenerateNext(data);
            }


            // -1 here so last index is == IntensityEnd
            var intensity = _curve.GetPoint(_chunkIdx, _chunksThisSprint - 1);
            data.Intensity = intensity;

            var chunk = _currentGenerator.GenerateNext(data);

            ++_chunkIdx;
            return chunk;
        }

        public override void Init(GenerationData data)
        {
            HasNext = true;
            _chunkIdx = 0;

            _curve = Curve.Linear(data.Intensity, data.Intensity + IntensityWindow);
            _chunksThisSprint = Mathf.RoundToInt(Random.Range(data.ChunksPerSprint - NumChunksFuzz, data.ChunksPerSprint + NumChunksFuzz));

            _currentGenerator = ChunkGenerators.RandomElement();
            _currentGenerator.Init(data);
            RestGenerator.Init(data);
        }

        private void Reset()
        {
            _chunkIdx = 0;
        }

    }
}