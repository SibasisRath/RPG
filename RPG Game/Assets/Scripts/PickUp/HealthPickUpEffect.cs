using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PickUp
{
    public class HealthPickUpEffect : IPickable
    {
        [SerializeField] private float healthAmount = 50f;

        public override void ApplyEffect(GameObject player)
        {
            Health health = player.GetComponent<Health>();
            if (health != null)
            {
                //print("health up.");
                health.AdditionalHealth(healthAmount);
            }
        }
    }
}