using System;
using LDJam48.StateMachine.Player;
using UnityEditor;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48
{
    [RequireComponent(typeof(PlayerColliders))]
    public class PlayerContacts : MonoBehaviour
    {
        [SerializeField] private LayerMask wallLayer;

        [SerializeField] private bool trackYourself;
        [SerializeField] private bool debug;

        public ContactDetails ContactDetails;

        private PlayerColliders _colliders;
        
        private void Awake()
        {
            _colliders = GetComponent<PlayerColliders>();
        }

        private void FixedUpdate()
        {
            if (!trackYourself) return;
            UpdateContactDetails();
        }
        

        private ContactDetails DetectContacts(ContactDetails contact, bool log = false)
        {
            var next = DetectContactsNew(contact, log);
            return next;
        }
        public ContactDetails DetectContactsNew(ContactDetails contact, bool log = false)
        {
            contact.WasOnLeftWall = contact.IsOnLeftWall;
            contact.WasOnRightWall = contact.IsOnRightWall;
            contact.WasOnFloor = contact.IsOnFloor;

            contact.IsOnFloor = false;
            contact.IsOnLeftWall = false;
            contact.IsOnRightWall = false;
            contact.FloorCollider = null;
            contact.LeftWallCollider = null;
            contact.RightWallCollider = null;

            var filer = new ContactFilter2D();
            filer.layerMask = wallLayer;
            var overlaps = new RaycastHit2D[4];

            int results = Physics2D.BoxCastNonAlloc(
                (Vector2)transform.position + _colliders.verticalDetectorOffset,
                _colliders.verticalDetectorSize,
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
                    contact.FloorCollider = hit.collider;
                    if (debug && !contact.WasOnFloor)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}, hit p={hit.point}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
                    }
                }
            }

            results = Physics2D.BoxCastNonAlloc(
                (Vector2)transform.position + _colliders.horizontalDetectorOffset,
                _colliders.horizontalDetectorSize,
                0,
                Vector2.down,
                overlaps,
                0f,
                wallLayer
            );

            if (log)
            {
                Debug.Log($"horizontal detection, results = {results}");
            }

            for (int i = 0; i < results; ++i)
            {
                RaycastHit2D hit = overlaps[i];
                
                if (log)
                {
                    Debug.Log($"horizontal detection hit {hit.collider.gameObject.name}, normal = {hit.normal}");
                }

                if (hit.normal.x > .5f)
                {
                    contact.IsOnLeftWall = true;
                    contact.LeftWallCollider = hit.collider;

                    if (debug && !contact.WasOnLeftWall)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}, hit p={hit.point}, collider name = {hit.collider.name}, in {hit.collider.transform.parent.parent.gameObject.name}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
                    }
                }

                if (hit.normal.x < -.5f)
                {
                    contact.IsOnRightWall = true;
                    contact.RightWallCollider = hit.collider;

                    if (debug && !contact.WasOnRightWall)
                    {
                        Debug.LogWarning($"Ray normal = {hit.normal}, hit p={hit.point}, collider name = {hit.collider.name}, in {hit.collider.transform.parent.parent.gameObject.name}");
                        Debug.DrawRay(hit.point, hit.normal * 3, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2f);
                    }
                }
            }

            if (debug)
            {
                if (contact.WasOnFloor && !contact.IsOnFloor)
                {
                    Debug.Log("Contacts no longer on floor");
                }
                if (contact.WasOnLeftWall && !contact.IsOnLeftWall)
                {
                    Debug.Log("Contacts no longer on LeftWall");
                }
                if (contact.WasOnRightWall && !contact.IsOnRightWall)
                {
                    Debug.Log("Contacts no longer on RightWall");
                }
            }

            return contact;
        }

        public ContactDetails UpdateContactDetails(bool log = false)
        {
            ContactDetails = DetectContacts(ContactDetails, log);
            return ContactDetails;
        }

        public void DLog(string source)
        {
            Debug.Log($"{source}: contacts -> wf({ContactDetails.WasOnFloor}), wl({ContactDetails.WasOnLeftWall}, wr({ContactDetails.WasOnRightWall}), if({ContactDetails.IsOnFloor}), il({ContactDetails.IsOnLeftWall}, ir({ContactDetails.IsOnRightWall})");
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

        public Collider2D FloorCollider;
        public Collider2D LeftWallCollider;
        public Collider2D RightWallCollider;

        public bool HitLeftWallThisTurn => !WasOnLeftWall && IsOnLeftWall;
        public bool HitRightWallThisTurn => !WasOnRightWall && IsOnRightWall;
        public bool HitFloorThisTurn => !WasOnFloor && IsOnFloor;


        public bool LeftLeftWallThisTurn => WasOnLeftWall && !IsOnLeftWall;
        public bool LeftRightWallThisTurn => WasOnRightWall && !IsOnRightWall;
        public bool LeftFloorThisTurn => WasOnFloor && !IsOnFloor;
    }
}