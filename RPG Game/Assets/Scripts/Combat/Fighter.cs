using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PickUp
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private BaseStats stats;
        [SerializeField] private float timeBetweenAttacks = 0.5f;
        [SerializeField] private Animator animator;
        [SerializeField] private Mover mover;
        [SerializeField] private ActionScheduler actionScheduler;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;
        private WeaponConfig currentWeaponConfig = null;
        private Weapon currentWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Health target;

        private void Start()
        {
            if (currentWeaponConfig == null) { EquipWeapon(defaultWeapon); }
        }
        public void EquipWeapon(WeaponConfig weapon)
        {            
            currentWeaponConfig = weapon;
            currentWeapon = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
         }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
          
            if (target == null)  return;
            if (target.IsDead()) return;
            if (!GetIsInRange(target.transform))
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
            animator.ResetTrigger("StopAttack");
            animator.SetTrigger("Attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform))
            { 
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public void StartAttackAction(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        private void StopAttack()
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("StopAttack");
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
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
            float damage = stats.GetStat(Stat.DAMAGE);

            if (currentWeapon != null)
            {
                currentWeapon.OnHit();
            }           

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.DAMAGE)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.DAMAGE)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }
    }
}