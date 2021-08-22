using System;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    public class SideMarkerPlacement : MonoBehaviour
    {
        [SerializeField] private SideMarker markerPrefab;
        [SerializeField] private IntReference placementPoint;
        [SerializeField] private ObservableIntVariable distanceTravelled;
        [SerializeField] private float spawnDistance;
        [SerializeField] private float offsetY;
        [SerializeField] private float spawnPointX;
        
        
        private bool _spawned;
        private bool _passed;
        private SideMarker _marker;
        private Action<int> _onDistanceChanged;

        private void OnEnable()
        {
            _onDistanceChanged = TrySpawnMarker;
            distanceTravelled.OnValueChanged += OnDistanceChanged;
        }
        
        
        private void OnDisable()
        {
            distanceTravelled.OnValueChanged -= OnDistanceChanged;
        }

        private void OnDistanceChanged(int d)
        {
            _onDistanceChanged(d);
        }

        private void TrySpawnMarker(int distance)
        {
            var v = Math.Abs(placementPoint.Value - distance);
                
            if (Math.Abs(placementPoint.Value - distance) < spawnDistance)
            {
                var position = new Vector3(spawnPointX, offsetY - placementPoint.Value, 0f);
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
            }
        }
    }
}