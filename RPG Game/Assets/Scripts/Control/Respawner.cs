using RPG.Attributes;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = 0.2f;
        [SerializeField] float healthRegenPercentage = 20;

        private void Awake()
        {
            GetComponent<Health>().onDie.AddListener(Respawn);
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            Fade fader = FindObjectOfType<Fade>();
            yield return fader.FadeOut(fadeTime);
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            yield return fader.FadeIn(fadeTime);
        }
    }
}