using RPG.Control;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public enum PortalId
    {
        SceneOnePortalOne,
        SceneOnePortalTwo,
        SceneTwoPortalOne,
        SceneTwoPortalTwo,
    } 

    public class Portal : MonoBehaviour
    {
        [SerializeField] PortalId portalId;
        [SerializeField] PortalId otherPortalId;
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform playerSpawnPosition;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fade fader = FindObjectOfType<Fade>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;


            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // Disable physics

            // Temporarily disable agent to avoid unwanted movement
            agent.enabled = false;

            // Force the player's position exactly where needed
            player.transform.position = otherPortal.playerSpawnPosition.position;
            player.transform.rotation = otherPortal.playerSpawnPosition.rotation;

            // Manually update agent's position to match the transform
            agent.Warp(otherPortal.playerSpawnPosition.position);

           
            if (rb != null) rb.isKinematic = false; // Disable physics

            // Re-enable the agent
            agent.enabled = true;
        }


        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.portalId != otherPortalId) continue;

                return portal;
            }

            return null;
        }
    }
}