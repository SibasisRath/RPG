using RPG.Core;
using RPG.Events;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private NavMeshAgent meshAgent;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private float maxSpeed = 6f;


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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            meshAgent.destination = destination;
            meshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            meshAgent.isStopped = false;
        }

        public void Cancel()
        {
            meshAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}