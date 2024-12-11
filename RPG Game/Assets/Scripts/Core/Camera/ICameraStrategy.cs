using UnityEngine;

namespace RPG.Core
{
    public interface ICameraStrategy
    {
        void UpdateCamera(Transform cameraTransform, Transform targetTransform);
    }
}