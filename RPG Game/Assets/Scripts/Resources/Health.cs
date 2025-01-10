using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;
        [SerializeField] private Animator animator;
        private bool isDead = false;

        // start can be called sometimes after we have restored the save state.
        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public float GetPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetHealth());
        }

        public object CaptureState()
        {
            return health;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health == 0)
            {
                Death();
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                Death();
                AwardExperience(instigator);
            }
        }

        private void Death()
        {
            if (isDead) return;
            isDead = true;
            animator.SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }
    }
}