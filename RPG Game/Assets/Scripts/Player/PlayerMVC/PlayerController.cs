using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Fighter fighter;
        private bool hasHit;


        void Update()
        {
            if(InteractWithCombat()) return ;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) 
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (!GetComponent<Fighter>().CanAttack(target))
                {
                    continue;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    fighter.StartAttackAction(target);
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
                    mover.StartMoveAction(raycastHit.point);
                }
                return true;
            }
            return false;
        }

        private Ray GetMouseRay() => playerCamera.ScreenPointToRay(Input.mousePosition);
    }
}