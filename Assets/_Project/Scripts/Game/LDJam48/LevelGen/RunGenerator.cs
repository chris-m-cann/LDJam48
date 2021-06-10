using System;
using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/RunGenerator")]
    public class RunGenerator : ChunkGenerator
    {
        public ChunkGenerator[] AreaGenerators;
        [SerializeField] private float AreasToMaxIntensity;
        [SerializeField] private float IntensityStart;
        [SerializeField] private float IntensityMax;
        [SerializeField] private float AreasToMaxSprintsPerArea;
        [SerializeField] private float SprintsPerAreaStart;
        [SerializeField] private float SprintsPerAreaMax;
        [SerializeField] private float AreasToMaxChunksPerSprint;
        [SerializeField] private float ChunksPerSprintStart;
        [SerializeField] private float ChunksPerSprintMax;

        [NonSerialized] private ChunkGenerator _currentGenerator;
        [NonSerialized] private Curve _intensityCurve;
        [NonSerialized] private Curve _sprintsPerAreaCurve;
        [NonSerialized] private Curve _chunksPerSprintCurve;
        [NonSerialized] private int _areasComplete;

        public override LevelChunk GenerateNext(GenerationData data)
        {
            if (_currentGenerator == null) Init(data);


            if (!_currentGenerator.HasNext)
            {
                ++_areasComplete;
                data = UpdateBaseValues(data);
                NextGenerator(data);
            }

            return _currentGenerator.GenerateNext(data);
        }

        private GenerationData UpdateBaseValues(GenerationData data)
        {

            data.Intensity = Mathf.Clamp(
                _intensityCurve.GetPoint(_areasComplete, AreasToMaxIntensity - 1),
                IntensityStart,
                IntensityMax
            );

            data.SprintsPerArea = Mathf.Clamp(
                _sprintsPerAreaCurve.GetPoint(_areasComplete, AreasToMaxSprintsPerArea - 1),
                SprintsPerAreaStart,
                SprintsPerAreaMax
            );

            data.ChunksPerSprint = Mathf.Clamp(
                _chunksPerSprintCurve.GetPoint(_areasComplete, AreasToMaxChunksPerSprint - 1),
                ChunksPerSprintStart,
                ChunksPerSprintMax
            );


            return data;
        }

        private void NextGenerator(GenerationData data)
        {
            _currentGenerator = AreaGenerators.RandomElement();
            _currentGenerator.Init(data);
        }

        public override void Init(GenerationData data)
        {
            HasNext = true;

            // linear curve slowly ramping up the base difficulties
            _intensityCurve = Curve.Linear(IntensityStart, IntensityMax);
            _sprintsPerAreaCurve = Curve.Linear(SprintsPerAreaStart, SprintsPerAreaMax);
            _chunksPerSprintCurve = Curve.Linear(ChunksPerSprintStart, ChunksPerSprintMax);
            _areasComplete = 0;

            UpdateBaseValues(data);

            NextGenerator(data);
        }
    }
}