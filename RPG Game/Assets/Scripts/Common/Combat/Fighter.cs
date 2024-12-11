using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 0.5f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private Animator animator;
        [SerializeField] private Mover mover;
        [SerializeField] private ActionScheduler actionScheduler;
        private float timeSinceLastAttack = 0f;
        private Health target;
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
          
            if (target == null)  return;
            if (target.IsDead()) return;
            if (!IsInRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void StartAttackAction(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
        }
        private void AttackBehaviour()
        {
            transform.LookAt(transform.position);
            if (timeBetweenAttacks < timeSinceLastAttack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
           
        }

        //animation event
        private void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }
    }
}