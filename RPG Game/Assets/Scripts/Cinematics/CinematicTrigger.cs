using RPG.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField] private EventService eventService;
        bool alreadyTriggered = false;
        [SerializeField] private PlayableDirector playableDirector;

        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.gameObject.tag == "Player")
            {
                alreadyTriggered = true;
                playableDirector.Play();
                eventService.OnCutSceneStarted.InvokeEvent(false);
                Invoke("PlayingCutScenes", (float)playableDirector.duration);
            }
        }

        private void PlayingCutScenes()
        {
            eventService.OnCutSceneEnded.InvokeEvent(true);
        }

    }
}