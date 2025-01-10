using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private int sceneToLoad = 1;
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
            DontDestroyOnLoad(gameObject);
            Fade fader = FindObjectOfType<Fade>();

            yield return fader.FadeOut(fadeOutTime);
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            wrapper.Load();
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.playerSpawnPosition.position);
            player.transform.rotation = otherPortal.playerSpawnPosition.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.portalId != this.otherPortalId) continue;
                return portal;
            }

            return null;
        }
    }
}