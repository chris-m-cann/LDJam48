using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48
{
    [RequireComponent(typeof(SpawnOnDeath))]
    public class GemWorth : MonoBehaviour
    {
        [SerializeField] private Vector2Int worth;
        [SerializeField] private List<Pickup> gems;
        [SerializeField] private float offsetMax = 1;

        private SpawnOnDeath _spawner;

        private void Awake()
        {
            _spawner = GetComponent<SpawnOnDeath>();
        }

        private void Start()
        {
            gems = gems.OrderByDescending(gem => gem.Value).ToList();


            var amount = Random.Range(worth.x, worth.y);
            var lastAmount = 0;
            while (amount > 0 && amount != lastAmount)
            {
                lastAmount = amount;
                foreach (var gem in gems)
                {
                    if (gem.Value <= amount)
                    {
                        var offset = new Vector2(
                            Random.Range(0, offsetMax),
                            Random.Range(0, offsetMax)
                        );

                        _spawner.ThingsToSpawn.Add(PairEx.Make(gem.gameObject, Vector2.zero));
                        // _spawner.ThingsToSpawn.Add(PairEx.Make(gem.gameObject, offset));

                        amount -= gem.Value;
                        break;
                    }
                }
            }

        }


    }
}