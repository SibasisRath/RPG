using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        [SerializeField] private Animator animator;
        private bool isDead = false;
        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                
                Death();
            }
        }

        private void Death()
        {
            if (isDead) return;
            isDead = true;
            animator.SetTrigger("Die");
        }
    }
}