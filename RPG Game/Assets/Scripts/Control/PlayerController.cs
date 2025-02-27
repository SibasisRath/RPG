using RPG.PickUp;
using RPG.Events;
using RPG.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Fighter fighter;
        [SerializeField] private Health health;
        [SerializeField] private EventService eventService;
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;


        private Camera playerCamera;
        private bool hasHit;


        private bool hasControl;

        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;


        private void SetControl(bool control) => hasControl = control; 
        private void SubscribingToEvents()
        {
            eventService.OnCutSceneStarted.AddListener(SetControl);
            eventService.OnCutSceneEnded.AddListener(SetControl);
        }
        private void UnsubscribingToEvents()
        {
            eventService.OnCutSceneStarted.RemoveListener(SetControl);
            eventService.OnCutSceneEnded.RemoveListener(SetControl);
        }

        private void Start()
        {
            hasControl = true;
            playerCamera = Camera.main;
            SubscribingToEvents();
        }

        void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.NONE);
                return;
            }
            if (!hasControl) return;
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.NONE);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRayCastable[] raycastables = hit.transform.GetComponents<IRayCastable>();
                foreach (IRayCastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.MOVEMENT);
                return true;
            }
            
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;            
            return true;
        }

        private Ray GetMouseRay() => playerCamera.ScreenPointToRay(Input.mousePosition);

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private void OnDisable()
        {
            UnsubscribingToEvents();
        }
    }
}