using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.PickUp
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.COMBAT;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                fighter.StartAttackAction(gameObject);
            }
            return true;
        }
    }
}
