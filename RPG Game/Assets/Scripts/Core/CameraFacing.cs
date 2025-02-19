using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private Camera cam;
        private void Start()
        {
            cam = Camera.main;
        }
        private void LateUpdate()
        {
            transform.forward = cam.transform.forward;
        }
    }
}