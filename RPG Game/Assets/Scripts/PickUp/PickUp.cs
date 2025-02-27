using RPG.Control;
using System.Collections;
using System.Numerics;
using UnityEngine;

namespace RPG.PickUp
{
    public class PickUp : MonoBehaviour, IRayCastable
    {
        [SerializeField] private float pickUpRespawnTime = 1f;
        [SerializeField] private Collider pickUpCollider = null;
        [SerializeField] private IPickable pickableEffect = null;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }
        private void Pickup(GameObject player)
        {
            if (pickableEffect is IPickable effect)
            {
                effect.ApplyEffect(player);
                StartCoroutine(HideForSeconds(pickUpRespawnTime));
            }
        }
        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }
        private void ShowPickUp(bool shouldShow)
        {
            pickUpCollider.enabled = shouldShow;
            foreach (Transform child in transform) 
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.PICKUP;
        }
    }
}