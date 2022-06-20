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

        private void OnEnable()
        {
            gems = gems.OrderByDescending(gem => gem.Value).ToList();


            var amount = Random.Range(worth.x, worth.y);
            var lastAmount = 0;
            while (amount > 0 && amount != lastAmount)
            {
                lastAmount = amount;
                foreach (var gem in gems)
                {
                    while (gem.Value <= amount)
                    {
                        var offset = new Vector2(
                            Random.Range(0, offsetMax),
                            Random.Range(0, offsetMax)
                        );

                        _spawner.Add(gem.gameObject, offset);

                        amount -= gem.Value;
                    }
                }
            }

        }


    }
}