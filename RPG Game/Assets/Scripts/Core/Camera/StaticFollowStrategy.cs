using UnityEngine;

namespace RPG.Core
{
    public class StaticFollowStrategy : ICameraStrategy
    {
        public void UpdateCamera(Transform cameraTransform, Transform targetTransform)
        {
            cameraTransform.position = targetTransform.position;
        }
    }
}