using RPG.Core;
using RPG.Events;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float healthPoints = -1f;
        [SerializeField] private Animator animator;
        [SerializeField] private float regenerationPercentage = 70;
        [SerializeField] private EventService eventService = null;
        [SerializeField] private GameObject healthGainEffect; // instead of instantiating keep it disable and enable it when reqyuired.
        [SerializeField] private BaseStats baseStats = null;
        private bool isDead = false;

        [System.Serializable]
        public class TakedamageEvent : UnityEvent<float>
        { }

        [SerializeField] private TakedamageEvent damageTextDisplayEvent = null;

        // start can be called sometimes after we have restored the save state.
        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = baseStats.GetStat(Stat.HEALTH);
            }
            SubscribeToEvent();
        }
        public void SubscribeToEvent()
        {
            if (eventService == null) { return; }
            eventService.OnLevelUp.AddListener(RegenerateHealth); // because here first we are gaining exp. then we are going to next level and then we are getting more health. Later when health generation potions will be added we will use this for that too.
        }
        public void UnsubscribeToEvent()
        {
            if (eventService == null) { return; }
            eventService.OnLevelUp.RemoveListener(RegenerateHealth);
        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.HEALTH) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
            if (healthGainEffect != null)
            {
                HealthUpEffect();
            }
        }
        private void HealthUpEffect()
        {
            GameObject healthUpParticalEffect = Instantiate(healthGainEffect, transform);
            Destroy(healthUpParticalEffect, healthUpParticalEffect.GetComponentInChildren<ParticleSystem>().main.duration);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }
        public float GetHealthPoints()
        {
            return healthPoints;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }
        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                Death();
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            damageTextDisplayEvent.Invoke(damage);
            if (healthPoints == 0)
            {
                Death();
                AwardExperience(instigator);
            }

        }

        private void Death()
        {
            if (isDead) return;
            isDead = true;
            print("in death method.");
            animator.SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            if (!instigator.TryGetComponent<Experience>(out var experience)) return;
            experience.GainExperience(baseStats.GetStat(Stat.EXPERIENCE_REWARD));
        }
        private void OnDisable()
        {
            UnsubscribeToEvent();
        }
    }
}