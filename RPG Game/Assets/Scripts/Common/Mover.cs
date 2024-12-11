using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private NavMeshAgent meshAgent;
        [SerializeField] private Animator playerAnimator;

        Vector3 velocity;
        Vector3 localVelocity;
        private Transform playerTransform;

        private void Start()
        {
            playerTransform = transform;
        }
        // Update is called once per frame
        private void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            velocity = meshAgent.velocity;
            localVelocity = playerTransform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            playerAnimator.SetFloat("ForwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            meshAgent.destination = destination;
            meshAgent.isStopped = false;
        }

        public void Cancel()
        {
            Debug.Log(" canceled from mover.");
            meshAgent.isStopped = true;
        }
    }
}