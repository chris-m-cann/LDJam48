using UnityEngine;

namespace LDJam48.StateMachine.Player.Action
{
    public class PlayerRaycastsBehaviour : MonoBehaviour
    {
        [SerializeField] private  LayerMask wallMask;
        [SerializeField] private  float boxcastSize = .5f;
        [SerializeField] private Transform leftProjectionPoint;
        [SerializeField] private Transform rightProjectionPoint;


        public void StickToWall(bool isLeftWall)
        {
            // cast from farthest point toward wall
            var direction = Vector2.left;
            var projectionPoint = rightProjectionPoint;
            var offset = .5f;

            if (!isLeftWall)
            {
                direction.x *= -1;
                projectionPoint = leftProjectionPoint;
                offset *= -1;
            }
            
            var hit = Physics2D.BoxCast(projectionPoint.position, boxcastSize * Vector2.one, 0f, direction, 10f, wallMask);

            if (hit.collider != null)
            {
                var pos = transform.position;
                pos.x = hit.point.x + offset;
                transform.position = pos;
            }
        }

        public Vector2 FindDashLandPoint(Vector2 direction, float leftWallX, float rightWallX)
        {
            var castPoint = leftProjectionPoint;
            var finalX = leftWallX;
            var offset = .5f;
            if (direction.x > 0)
            {
                castPoint = rightProjectionPoint;
                finalX = rightWallX;
                offset *= -1;
            }

            var hit = Physics2D.BoxCast(castPoint.position, boxcastSize * Vector2.one, 0f, direction, rightWallX - leftWallX, wallMask);
            var hitX = finalX;

            if (hit.collider)
            {
                hitX = hit.point.x;
            }
            Debug.DrawLine(castPoint.position, new Vector3(hitX, castPoint.position.y), Color.red, 1f);

            
            var r = new Vector2(hitX + offset, castPoint.position.y);
            
            Debug.Log($"Dashing from {castPoint.position.x} to {r.x}");
            return r;
        }
    }
}