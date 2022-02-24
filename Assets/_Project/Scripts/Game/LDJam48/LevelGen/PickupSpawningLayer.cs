using System.Collections;
using UnityEngine;

namespace LDJam48.LevelGen
{
    public class PickupSpawningLayer : SpawningLayer
    {
        
        [SerializeField] private SpawnBudget budget;
        [SerializeField] [Range(0, 1)] private float spawnProbablity = 1;

        public override IEnumerator OnBuilt(Parameters spawn)
        {
            if (Random.value > spawnProbablity) yield break;
            if (budget != null && !budget.AllocateBudget(CalculateCost(spawn))) yield break;;

            yield return StartCoroutine(base.OnBuilt(spawn));
        }

        private float CalculateCost(Parameters spawn)
        {
            var height = spawn.Chunk.Height;
            var width = spawn.Chunk.Width;
            var startPos = spawn.ChunkStartPos + new Vector3(-4, -.5f);
            var startCell = _tilemap.WorldToCell(startPos);
            var cell = new Vector3Int(startCell.x, startCell.y, startCell.z);
            int total = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    cell.x = x + startCell.x;
                    cell.y = startCell.y - y;
                    var tile = _tilemap.GetTile(cell);
                    if (tile is PrefabHolderTile holder)
                    {
                        total += holder.Prefab.GetComponent<Pickup>()?.Value ?? 0;
                    }
                }
            }

            return total;
        }
    }
}