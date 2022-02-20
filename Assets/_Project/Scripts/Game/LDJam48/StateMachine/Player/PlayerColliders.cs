using System;
using UnityEngine;

namespace LDJam48.StateMachine.Player
{
    public class PlayerColliders : MonoBehaviour
    {
        public Collider2D Main;
        public Collider2D Slash;
        public Collider2D Slam;

        [Header("Detectors")]
        public Vector2 horizontalDetectorOffset;
        public Vector2 horizontalDetectorSize;
        public Vector2 verticalDetectorOffset;
        public Vector2 verticalDetectorSize;

        [Header("RaycastOffsets")] 
        public float leftCastOffset;
        public float rightCastOffset;
        public float bottomCastOffset;

        [Space] [SerializeField] private bool drawGizmos;
        
        
        public Vector2 rightProjectionPoint => (Vector2)transform.position + horizontalDetectorOffset + rightCastOffset * Vector2.right;
        public Vector2 leftProjectionPoint => (Vector2)transform.position + horizontalDetectorOffset + leftCastOffset * Vector2.left;
        public Vector2 bottomProjectionPoint => (Vector2)transform.position + verticalDetectorOffset + bottomCastOffset * Vector2.up;


        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            Gizmos.color = Color.red;
            Vector2 horizontalCenter = (Vector2)transform.position + horizontalDetectorOffset;
            Gizmos.DrawWireCube(horizontalCenter, horizontalDetectorSize);


            Gizmos.color = Color.blue;
            Vector2 verticalCenter = (Vector2)transform.position + verticalDetectorOffset;
            Gizmos.DrawWireCube(verticalCenter, verticalDetectorSize);


            Gizmos.color = Color.yellow;
            float horizontalHalfHeight = horizontalDetectorSize.y / 2;
            Gizmos.DrawLine(
                new Vector2(leftProjectionPoint.x, horizontalCenter.y + horizontalHalfHeight),
                new Vector2(leftProjectionPoint.x, horizontalCenter.y - horizontalHalfHeight));

            Gizmos.DrawLine(
                new Vector2(rightProjectionPoint.x, horizontalCenter.y + horizontalHalfHeight),
                new Vector2(rightProjectionPoint.x, horizontalCenter.y - horizontalHalfHeight));

            float verticalHalfWidth = verticalDetectorSize.x / 2;
            
            Gizmos.DrawLine(
                new Vector2(horizontalCenter.x + verticalHalfWidth, bottomProjectionPoint.y),
                new Vector2(horizontalCenter.x - verticalHalfWidth, bottomProjectionPoint.y));
        }
    }
}