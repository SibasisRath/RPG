using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Events
{
    public class EventService : MonoBehaviour
    {
        /*    public EventController<ResultType> OnPlayerDied { get; private set; }

            public EventController OnPlayerTriggerCalamity { get; private set; }*/


        public EventController<bool> OnCutSceneStarted { get; private set; }
        public EventController<bool> OnCutSceneEnded { get; private set; }


        public EventService()
        {
            OnCutSceneStarted = new();
            OnCutSceneEnded = new();
        }
    }
}