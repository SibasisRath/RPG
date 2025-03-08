using RPG.Events;
using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;
        private EventService eventService;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            if (eventService == null) { print("no event"); }
            eventService.OnGainingExperience.InvokeEvent();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
            GetComponent<BaseStats>()?.UpdatingLevel();
        }

        public float GetExperiencePointsPoints()
        {
            return experiencePoints;
        }

        public void Init(EventService eventService)
        {
            this.eventService = eventService;
        }
    }
}