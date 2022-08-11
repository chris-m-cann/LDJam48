using System;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;
using Void = Util.Void;

namespace LDJam48
{
    public class SideMarkerPlacement : MonoBehaviour
    {
        [SerializeField] private SideMarker markerPrefab;
        [SerializeField] private IntReference placementPoint;
        [SerializeField] private GameObjectReference player;
        [SerializeField] private VoidGameEvent onRunStarted;
        [SerializeField] private float spawnDistance;
        [SerializeField] private float offsetY;
        [SerializeField] private float spawnPointX;
        
        
        private bool _spawned;
        private bool _passed;
        private SideMarker _marker;
        private Action<int> _onDistanceChanged;
        private bool _running = false;
        private float _startY = 0f;

        private void OnEnable()
        {
            onRunStarted.OnEventTrigger += RunStart;
            _onDistanceChanged = TrySpawnMarker;
        }

        private void OnDisable()
        {
            onRunStarted.OnEventTrigger -= RunStart;
        }

        private void Update()
        {
            if (!_running || player.Value == null) return;

            OnDistanceChanged((int)Mathf.Abs(player.Value.transform.position.y - _startY));
        }

        private void RunStart(Void v)
        {
            if (placementPoint.Value <= 0) return;
            
            _startY = player.Value.transform.position.y;
            _running = true;
        }
        
        private void OnDistanceChanged(int d)
        {
            _onDistanceChanged(d);
        }

        private void TrySpawnMarker(int distance)
        {
            if (Math.Abs(placementPoint.Value - distance) < spawnDistance)
            {
                var position = new Vector3(spawnPointX,  _startY- placementPoint.Value, 0f);
                _marker = Instantiate(markerPrefab, position, Quaternion.identity);
                

                _onDistanceChanged = CheckIfPassedMarker;
            }
        }

        private void CheckIfPassedMarker(int distance)
        {
            if (distance > placementPoint.Value)
            {
                _marker.OnPassedMarker();

                _onDistanceChanged = NullOp.Fun;
                _running = false;
            }
        }
    }
}