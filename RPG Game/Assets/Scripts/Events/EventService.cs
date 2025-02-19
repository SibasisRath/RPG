using UnityEngine;

namespace RPG.Events
{
    public class EventService : MonoBehaviour
    {
        public EventController<bool> OnCutSceneStarted { get; private set; }
        public EventController<bool> OnCutSceneEnded { get; private set; }

        //public EventController<float> OnDealingDamage { get; private set; }

        public EventController OnGainingExperience { get; private set; }
        public EventController OnLevelUp { get; private set; }  


        public EventService()
        {
            OnCutSceneStarted = new();
            OnCutSceneEnded = new();
           // OnDealingDamage = new();
            OnGainingExperience = new();
            OnLevelUp = new();
        }
    }
}