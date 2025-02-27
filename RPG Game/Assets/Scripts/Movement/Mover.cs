using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private NavMeshAgent meshAgent;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;


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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
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
            MoverSaveData data = new()
            {
                position = new SerializableVector3(transform.position),
                rotation = new SerializableVector3(transform.eulerAngles)
            };
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

          /*  if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position; // Snap agent to the nearest valid NavMesh position
                transform.eulerAngles = data.rotation.ToVector();
            }*/

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}