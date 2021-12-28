using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LDJam48
{
    public class PlayerContacts : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D leftCollider;
        [SerializeField] private BoxCollider2D rightCollider;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private BoxCollider2D floorCollider;
        [SerializeField] private LayerMask floorLayer;

        [SerializeField] private bool trackYourself;
        [SerializeField] private bool normalRay;
        

        public ContactDetails ContactDetails;

        private void FixedUpdate()
        {
            if (!trackYourself) return;
            ContactDetails = DetectContacts(ContactDetails);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube((Vector2) transform.position + leftCollider.offset, leftCollider.size);
            Gizmos.DrawWireCube((Vector2) transform.position + rightCollider.offset, rightCollider.size);
        }
        
        public ContactDetails DetectContacts(ContactDetails contact)
        {
            contact.WasOnLeftWall = contact.IsOnLeftWall;
            contact.WasOnRightWall = contact.IsOnRightWall;
            contact.WasOnFloor = contact.IsOnFloor;

            contact.IsOnLeftWall = Physics2D.OverlapBox((Vector2) transform.position + leftCollider.offset,
                leftCollider.size, 0, wallLayer);

            contact.IsOnRightWall = Physics2D.OverlapBox((Vector2) transform.position + rightCollider.offset,
                rightCollider.size, 0, wallLayer);

            var floorCollision = Physics2D.OverlapBox((Vector2) transform.position + floorCollider.offset,
                floorCollider.size, 0, floorLayer);

            contact.IsOnFloor = false;
            if (floorCollision != null)
            {
                var contacts = new ContactPoint2D[1];
                if (floorCollision.GetContacts(contacts) > 0)
                {
                    var item = contacts[0];

                    contact.IsOnFloor = item.normal.y < -.5;
                    
                    if (normalRay)
                    {
                        Debug.Log($"Ray normal = {item.normal}");
                        Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
                    }
                }
            }
            
            return contact;
        }

        public void UpdateContactDetails()
        {
            ContactDetails = DetectContacts(ContactDetails);
        }
    }
    [Serializable]
    public struct ContactDetails
    {
        public bool WasOnLeftWall;
        public bool IsOnLeftWall;

        public bool WasOnRightWall;
        public bool IsOnRightWall;

        public bool WasOnFloor;
        public bool IsOnFloor;

        public bool HitLeftWallThisTurn => !WasOnLeftWall && IsOnLeftWall;
        public bool HitRightWallThisTurn => !WasOnRightWall && IsOnRightWall;
        public bool HitFloorThisTurn => !WasOnFloor && IsOnFloor;


        public bool LeftLeftWallThisTurn => WasOnLeftWall && !IsOnLeftWall;
        public bool LeftRightWallThisTurn => WasOnRightWall && !IsOnRightWall;
        public bool LeftFloorThisTurn => WasOnFloor && !IsOnFloor;
    }

}