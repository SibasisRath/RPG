using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float timeBetweenAttacks = 0.5f;
        [SerializeField] private Animator animator;
        [SerializeField] private Mover mover;
        [SerializeField] private ActionScheduler actionScheduler;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private string defaultWeaponName = "Unarmed";
        private Weapon currentWeapon = null;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Health target;


        private void Start()
        {
            if (currentWeapon == null) { EquipWeapon(defaultWeapon); }
        }
        public void EquipWeapon(Weapon weapon)
        {            
            currentWeapon = weapon;
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
         }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
          
            if (target == null)  return;
            if (target.IsDead()) return;
            if (!IsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
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

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
        }

        public void StartAttackAction(GameObject combatTarget)
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

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
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
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, currentWeapon.GetWeaponDamage());
            }
        }
        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}