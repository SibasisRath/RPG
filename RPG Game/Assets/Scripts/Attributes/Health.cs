using RPG.Core;
using RPG.Events;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPoints;
        [SerializeField] private Animator animator;
        [SerializeField] private float regenerationPercentage = 70;
        private EventService eventService;
        [SerializeField] private GameObject healthGainEffect; // instead of instantiating keep it disable and enable it when reqyuired.
        [SerializeField] private BaseStats baseStats = null;
        private bool wasDeadLastFrame = false;

        [System.Serializable]
        public class TakedamageEvent : UnityEvent<float>
        { }

        public UnityEvent onDie = null;

        [SerializeField] private TakedamageEvent takeDamageEvent = null;
        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }

        // start can be called sometimes after we have restored the save state.
        private void Start()
        {
            healthPoints.ForceInit();
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
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
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
            return healthPoints.value;
        }

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }
        public float GetHealthPoints()
        {
            return healthPoints.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }
        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            UpdateState();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamageEvent.Invoke(damage);
            if (IsDead())
            {
                onDie.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                takeDamageEvent.Invoke(damage);
            }
            UpdateState();
        }

        public void Heal(float healingAmount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healingAmount, GetMaxHealthPoints());
            UpdateState();
        }

     

        private void AwardExperience(GameObject instigator)
        {
            instigator.TryGetComponent<Experience>(out Experience experience);
            if (experience == null) return;
            float exp = GetComponent<BaseStats>().GetStat(Stat.EXPERIENCE_REWARD);
            experience.GainExperience(exp);
            print("experience reward. " + GetComponent<BaseStats>().GetStat(Stat.EXPERIENCE_REWARD));
        }
        private void OnDisable()
        {
            UnsubscribeToEvent();
        }

        internal float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.HEALTH);
        }
        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("Die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public void Init(EventService eventService)
        {
            this.eventService = eventService;
        }
    }
}