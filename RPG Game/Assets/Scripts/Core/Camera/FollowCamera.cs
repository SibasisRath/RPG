using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        private Transform cameraTransform;

        private ICameraStrategy cameraStrategy;

        private void Start()
        {
            cameraTransform = transform;

            // Default strategy: Static follow
            cameraStrategy = new StaticFollowStrategy();
        }

        private void LateUpdate()
        {
            cameraStrategy.UpdateCamera(cameraTransform, targetTransform);
        }

        public void SetCameraStrategy(ICameraStrategy newStrategy)
        {
            cameraStrategy = newStrategy;
        }
    }
}