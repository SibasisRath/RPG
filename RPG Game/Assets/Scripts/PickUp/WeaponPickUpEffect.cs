using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PickUp
{
    public class WeaponPickUpEffect : IPickable
    {
        [SerializeField] private WeaponConfig weapon = null;

        public override void ApplyEffect(GameObject player)
        {
            Fighter fighter = player.GetComponent<Fighter>();
            if (fighter != null)
            {
                fighter.EquipWeapon(weapon);
            }
        }
    }
}