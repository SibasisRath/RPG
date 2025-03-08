using RPG.Events;
using RPG.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterEnum characterType;
        [SerializeField] Progression progression = null;
        private EventService eventService;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);

        }
        private void Start()
        {
            currentLevel.ForceInit();
        }
        public void Init(EventService eventService)
        {
            this.eventService = eventService;
            SubscribeToEvents();
        }
        private void SubscribeToEvents()
        {
            if (eventService == null) 
            {
                if (this.gameObject.CompareTag("Player"))
                {
                    print("no eventservice.");
                }
               
                return;
            }
            eventService.OnGainingExperience.AddListener(UpdatingLevel);
        }
        private void UnsubscribeToEvents()
        {
            if (eventService == null) { return; }
            eventService.OnGainingExperience.RemoveListener(UpdatingLevel);
        }
        public void UpdatingLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                if (levelUpParticleEffect != null)
                {
                    LevelUpEffect();
                }               
                eventService.OnLevelUp.InvokeEvent();
            }
        }

        private void LevelUpEffect()
        {
            GameObject levelUpParticalEffect = Instantiate(levelUpParticleEffect, transform);
            Destroy(levelUpParticalEffect, levelUpParticalEffect.GetComponentInChildren<ParticleSystem>().main.duration);
        }

        public float GetStat(Stat stat)
        { 
            return (BaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        private float BaseStat(Stat stat)
        {
            if (progression != null)
            {
                return progression.GetStat(stat, characterType, GetLevel());
            }
           return 0;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel()
        {
           /* if (currentLevel < 1 && progression != null)
            {
                currentLevel = CalculateLevel();
            }*/
            return currentLevel.value;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePointsPoints();
            int penultimateLevel = progression.GetLevels(Stat.EXPERIENCE_TO_LEVEL_UP, characterType);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.EXPERIENCE_TO_LEVEL_UP, characterType, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void OnDisable()
        {
            UnsubscribeToEvents();
        }

     
    }
}