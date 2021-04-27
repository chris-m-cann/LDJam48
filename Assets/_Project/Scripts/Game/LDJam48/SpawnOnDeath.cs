using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace LDJam48
{
    public class SpawnOnDeath: MonoBehaviour
    {
        public List<Pair<GameObject, Vector2>> ThingsToSpawn;


        private bool _isQuitting;
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }


        public void SpawnObjects()
        {
            if (_isQuitting) return;

            foreach (var thing in ThingsToSpawn)
            {
                Instantiate(thing.First, transform.position, Quaternion.identity);
            }

        }
    }
}