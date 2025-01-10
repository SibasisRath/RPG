using RPG.Combat;
using RPG.Resources;
using RPG.Events;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;
        private Camera playerCamera;
        [SerializeField] private Fighter fighter;
        private bool hasHit;
        private Health health;
        [SerializeField] private EventService eventService;

        private bool hasControl;
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
            health = GetComponent<Health>();
            playerCamera = Camera.main;
            SubscribingToEvents();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (!hasControl) return;
            if (InteractWithCombat()) return ;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) 
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                if (Input.GetMouseButton(0))
                {
                    fighter.StartAttackAction(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit raycastHit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(raycastHit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private Ray GetMouseRay() => playerCamera.ScreenPointToRay(Input.mousePosition);

        private void OnDisable()
        {
            UnsubscribingToEvents();
        }
    }
}