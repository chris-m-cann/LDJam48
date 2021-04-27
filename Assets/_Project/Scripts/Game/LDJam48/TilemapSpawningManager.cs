using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48
{
    public class TilemapSpawningManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Vector2 topLeft;
        [SerializeField] private Vector2 bottomRight;
        [SerializeField] private int enemyQuota;
        [SerializeField] private int gemQuota;


        private int _enemies;
        private int _gems;

        private void Start()
        {
            SpawnObjects();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere((Vector3)topLeft + transform.position, .5f);


            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector3)bottomRight + transform.position, .5f);

            Gizmos.color = Color.white;
        }

        private void SpawnObjects()
        {
            var start = tilemap.WorldToCell((Vector3)topLeft + transform.position);
            var end = tilemap.WorldToCell((Vector3)bottomRight + transform.position);

            if (start.x > end.x || start.y < end.y)
            {
                Debug.LogError($"Start and end wrong start ={start}, end={end}");
            }

            for (int x = start.x; x <= end.x; ++x)
            {
                for (int y = start.y; y >= end.y; --y)
                {
                    var cell = new Vector3Int(x, y, start.z);
                    var tile = tilemap.GetTile(cell);
                    if (tile is ProbabilityTile probTile)
                    {
                        SpawnObjects2(probTile, tilemap.CellToWorld(cell));
                    }
                }
            }
        }

        private void SpawnObjects2(ProbabilityTile tile, Vector3 position)
        {
            // position will be in bottom left corner of the tile
            position.x += tilemap.cellSize.x / 2;
            position.y += tilemap.cellSize.y / 2;

            foreach (var obj in tile.Spawnables)
            {
                if (OverQuota(obj)) continue;

                var shouldSpawn = Random.value < obj.Probability;

                if (shouldSpawn)
                {
                    UpdateQuota(obj);
                    Instantiate(obj.Object, position, Quaternion.identity);
                }
            }
        }

        private bool OverQuota(SpawnDetails spawn)
        {
            switch (spawn.Type)
            {
                case SpawnType.Enemy: return _enemies >= enemyQuota;
                case SpawnType.Gem: return _gems >= gemQuota;
                default: return false;
            }
        }

        private void UpdateQuota(SpawnDetails spawn)
        {
            switch (spawn.Type)
            {
                case SpawnType.Enemy:
                    ++_enemies;
                    break;
                case SpawnType.Gem:
                    ++_gems;
                    break;
                default:
                    break;
            }
        }
    }

    public enum SpawnType
    {
        Enemy, Gem
    }

    [Serializable]
    public struct SpawnDetails
    {
        public GameObject Object;
        [Range(0, 1)]
        public float Probability;
        public SpawnType Type;
    }
}