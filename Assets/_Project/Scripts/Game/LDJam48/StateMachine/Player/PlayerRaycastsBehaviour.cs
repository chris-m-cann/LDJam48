using UnityEngine;

namespace LDJam48.StateMachine.Player.Action
{
    [RequireComponent(typeof(PlayerColliders))]
    public class PlayerRaycastsBehaviour : MonoBehaviour
    {
        [SerializeField] private  LayerMask dashWallMask;
        [SerializeField] private  LayerMask stickWallMask;
        [SerializeField] private  float boxcastDepth = .5f;

        [SerializeField] private bool debugLogs;
        
        private PlayerColliders _colliders;


        private void Awake()
        {
            _colliders = GetComponent<PlayerColliders>();
        }

        public void StickToWall(bool isLeftWall)
        {

            // cast from farthest point toward wall
            var direction = Vector2.left;
            var projectionPoint = _colliders.rightProjectionPoint + Vector2.left * (boxcastDepth / 2);
            var offset = .6f;

            if (!isLeftWall)
            {
                direction.x *= -1;
                projectionPoint = _colliders.leftProjectionPoint + Vector2.right * (boxcastDepth / 2);
                offset *= -1;
            }
            
            var hit = Physics2D.BoxCast(projectionPoint, new Vector2(boxcastDepth, _colliders.horizontalDetectorSize.y), 0f, direction, 1f, stickWallMask);

            if (hit.collider != null)
            {
                Debug.DrawLine(projectionPoint, hit.point);

                var pos = transform.position;
                pos.x = hit.point.x + offset;

                if (debugLogs)
                {
                    Debug.Log($"StickToWall(isLeftWall={isLeftWall}) -> hit point = {hit.point}, final pos = {pos}");
                }

                transform.position = pos;
            }
        }

        public Vector2 FindDashLandPoint(Vector2 direction, float leftWallX, float rightWallX)
        {
            var castPoint = _colliders.leftProjectionPoint + Vector2.right * (boxcastDepth / 2);
            var finalX = leftWallX;
            var offset = .5f;
            if (direction.x > 0)
            {
                castPoint = _colliders.rightProjectionPoint + Vector2.left * (boxcastDepth / 2);
                finalX = rightWallX;
                offset *= -1;
            }

            var castBoxSize = new Vector2(boxcastDepth, _colliders.horizontalDetectorSize.y);
            var hit = Physics2D.BoxCast(castPoint, castBoxSize, 0f, direction, rightWallX - leftWallX, dashWallMask);
            var hitX = finalX;

            if (hit.collider)
            {
                hitX = hit.point.x;
            }

            if (debugLogs)
            {
                Debug.Log($"Casting from {castPoint} in direction {direction}, distance {rightWallX - leftWallX}, hit {hit.collider}, hitx {hitX}, cast size {castBoxSize}");
                Debug.DrawLine(castPoint, new Vector3(hitX + offset, castPoint.y), Color.red, 1f);
            }


            var r = new Vector2(hitX + offset, transform.position.y);
            
            return r;
        }
        
        public Vector2 FindSlamLandPoint(float maxDistance)
        {
            var direction = Vector2.down;
            var castPoint = _colliders.bottomProjectionPoint + direction * (boxcastDepth / 2);

            var finalY = castPoint.y - maxDistance;
            var offset = .5f;

            var hit = Physics2D.BoxCast(castPoint, new Vector2(_colliders.verticalDetectorSize.x, boxcastDepth), 0f, direction, maxDistance, dashWallMask);
            var hitY = finalY;

            if (hit.collider)
            {
                hitY = hit.point.y;

                if (debugLogs)
                {
                    Debug.Log($"FindSlamLandPoint, hit point = {hit.point}");
                }
            }

            var r = new Vector2(transform.position.x, hitY + offset);

            if (debugLogs)
            {
                Debug.DrawLine(castPoint, r, Color.blue, 1f);
                Debug.Log($"FindSlamLandPoint, final point = {r}");
            }

            return r;
        }
    }
}