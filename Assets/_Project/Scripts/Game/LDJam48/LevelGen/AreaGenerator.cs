using System;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48.LevelGen
{
    /*
     * Requires:
     *  - BaseSprintsPerArea
     *  - BaseStartIntensity
     *  - BaseChunksPerSprint
     *
     *
     * Area Controls:
     *  - Delta SpA: rough number of Sprints to modify the base by
     *  - Delta cpS: rough number of Chunks to modify the base by     ???
     *  - Delta i: rough number to modify the base intensity by
     *  - i Window size: how high up it ramps in intensity
     *  - c Window size: how high up it ramps number of chunks per sprint by ???
     *  - Curve for how it ramps up intensity per sprint
     *  - Curve for how it ramps up chunks per sprint  ???
     */

    [CreateAssetMenu(menuName = "Custom/Level/AreaGenerator")]
    public class AreaGenerator : ChunkGenerator
    {
        // this is a long area with these types of sprints
        // short intense area
        // the longer it goes the more sprints I have
        // the longer it goes the more chunks I have per sprint
        // the longer it goes on the more intense the sprints are

        // configure type of sprints
        // say how many sprints in it
        // increase that based on how far through we are
        // increase how intense the sprints are based on where we are??


        public ChunkGenerator[] SprintGenerators;
        public ChunkGenerator RestGenerator;
        public int NumSprintsFuzz;

        public float IntensityWindow;
        public float ChunksPerSprintWindow;
        // todo(chris) add curve type

        [NonSerialized] private int _sprintIdx;
        [NonSerialized] private Curve _curve;
        [NonSerialized] private int _sprintsPerArea;

        [NonSerialized] private Curve _chunksPerSprintCurve;


        [NonSerialized] private ChunkGenerator _currentGenerator;
        public override LevelChunk GenerateNext(GenerationData data)
        {
            if (_currentGenerator == null) Init(data);

            if (_currentGenerator.HasNext)
            {
                return _currentGenerator.GenerateNext(data);
            }
            else
            {
                return NextGenerator(data);

            }
        }

        public override void Init(GenerationData data)
        {
            Reset();
            HasNext = true;
            _curve = Curve.Linear(data.Intensity, data.Intensity + IntensityWindow);
            _sprintsPerArea = Mathf.RoundToInt(Random.Range(data.SprintsPerArea - NumSprintsFuzz, data.SprintsPerArea + NumSprintsFuzz));

            _chunksPerSprintCurve = Curve.Linear(data.ChunksPerSprint, data.ChunksPerSprint + ChunksPerSprintWindow);

            SetUpGenerator(data);
        }

        private LevelChunk NextGenerator(GenerationData data)
        {
            ++_sprintIdx;

            if (_sprintIdx >= _sprintsPerArea)
            {
                HasNext = false;
                Reset();
                return RestGenerator.GenerateNext(data);
            }
            else
            {
                SetUpGenerator(data);
                var chunk = _currentGenerator.GenerateNext(data);

                return chunk;
            }

        }

        private void SetUpGenerator(GenerationData data)
        {
            _currentGenerator = SprintGenerators.RandomElement();
            // -1 here so last index is == IntensityEnd
            var intensity = _curve.GetPoint(_sprintIdx, _sprintsPerArea - 1);
            data.Intensity = intensity;


            var chunks = _chunksPerSprintCurve.GetPoint(_sprintIdx, _sprintsPerArea - 1);
            data.ChunksPerSprint = Mathf.RoundToInt(chunks);

            _currentGenerator.Init(data);
        }

        private void Reset()
        {
            _sprintIdx = 0;
            _currentGenerator = null;
        }
    }
}