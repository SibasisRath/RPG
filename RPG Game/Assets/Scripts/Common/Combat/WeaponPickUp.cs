using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private ParticleSystem particle = null;
        [SerializeField] private float weaponRespawnTime = 1f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(RespawningWeapon());
                //this can be polished
            }
        }

        private IEnumerator RespawningWeapon()
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(weaponRespawnTime);
            ShowPickUp(true);
        }
        private void ShowPickUp(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform) 
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}