using System;
using UnityEditor;
using UnityEngine;
using Util;
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


        [SerializeField] private Vector2 horizontalDetectorOffset;
        [SerializeField] private Vector2 horizontalDetectorSize;
        [SerializeField] private Vector2 verticalDetectorOffset;
        [SerializeField] private Vector2 verticalDetectorSize;

        public ContactDetails ContactDetails;

        private void FixedUpdate()
        {
            if (!trackYourself) return;
            ContactDetails = DetectContacts(ContactDetails);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + horizontalDetectorOffset, horizontalDetectorSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube((Vector2)transform.position + verticalDetectorOffset, verticalDetectorSize);
        }

        public ContactDetails DetectContacts(ContactDetails contact)
        {
            return DetectContactsNew(contact);
        }

        public ContactDetails DetectContactsOld(ContactDetails contact)
        {
            contact.WasOnLeftWall = contact.IsOnLeftWall;
            contact.WasOnRightWall = contact.IsOnRightWall;
            contact.WasOnFloor = contact.IsOnFloor;

            contact.IsOnLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftCollider.offset,
                leftCollider.size, 0, wallLayer);

            contact.IsOnRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightCollider.offset,
                rightCollider.size, 0, wallLayer);

            var floorCollision = Physics2D.OverlapBox((Vector2)transform.position + floorCollider.offset,
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

        public ContactDetails DetectContactsNew(ContactDetails contact)
        {
            contact.WasOnLeftWall = contact.IsOnLeftWall;
            contact.WasOnRightWall = contact.IsOnRightWall;
            contact.WasOnFloor = contact.IsOnFloor;

            contact.IsOnFloor = false;
            contact.IsOnLeftWall = false;
            contact.IsOnRightWall = false;

            var filer = new ContactFilter2D();
            filer.layerMask = wallLayer;
            var overlaps = new RaycastHit2D[4];

            int results = Physics2D.BoxCastNonAlloc(
                (Vector2)transform.position + verticalDetectorOffset,
                verticalDetectorSize,
                0,
                Vector2.down,
                overlaps,
                0f,
                wallLayer
            );

            for (int i = 0; i < results; ++i)
            {
                RaycastHit2D hit = overlaps[i];


                // todo(chris) this is triggering for the parts between levels
                // add max x difference between this and hit point?
                if (hit.normal.y > .5f)
                {
                    contact.IsOnFloor = true;
                    if (normalRay)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
                    }
                }
            }

            results = Physics2D.BoxCastNonAlloc(
                (Vector2)transform.position + horizontalDetectorOffset,
                horizontalDetectorSize,
                0,
                Vector2.down,
                overlaps,
                0f,
                wallLayer
            );

            for (int i = 0; i < results; ++i)
            {
                RaycastHit2D hit = overlaps[i];

                if (hit.normal.x > .5f)
                {
                    contact.IsOnLeftWall = true;

                    if (normalRay)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}, collider name = {hit.collider.name}, in {hit.collider.transform.parent.parent.gameObject.name}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
                    }
                }

                if (hit.normal.x < -.5f)
                {
                    contact.IsOnRightWall = true;

                    if (normalRay)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}, collider name = {hit.collider.name}, in {hit.collider.transform.parent.parent.gameObject.name}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
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