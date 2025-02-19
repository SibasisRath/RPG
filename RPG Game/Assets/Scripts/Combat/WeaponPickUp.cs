using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRayCastable
    {
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private float weaponRespawnTime = 1f;
        [SerializeField] private Collider weaponPickUpCollider = null;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }
        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(weaponRespawnTime));
        }
        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }
        private void ShowPickUp(bool shouldShow)
        {
            weaponPickUpCollider.enabled = shouldShow;
            foreach (Transform child in transform) 
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.PICKUP;
        }
    }
}