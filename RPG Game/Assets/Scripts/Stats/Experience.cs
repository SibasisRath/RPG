using RPG.Events;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;
        [SerializeField] private EventService eventService = null;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            eventService.OnGainingExperience.InvokeEvent();
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public float GetExperiencePointsPoints()
        {
            return experiencePoints;
        }
    }
}