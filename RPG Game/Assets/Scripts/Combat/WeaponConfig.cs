using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.PickUp
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] private Weapon equippedWeaponPrefab = null;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageBonus = 0f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private bool isRightHanded = true;
        private Transform handPos;
        [SerializeField] Projectile projectile = null;


        const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);
            Weapon weapon = null;
            if (equippedWeaponPrefab != null) 
            {
                Transform handTransform = GetTransform(rightHandTransform,leftHandTransform);
                weapon = Instantiate(equippedWeaponPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            handPos = isRightHanded ? rightHand : leftHand;
            return handPos;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange() 
        {
            return weaponRange;
        }
        public Weapon GetWeaponPrefab()
        {
            return equippedWeaponPrefab;
        }
        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
    }
}

