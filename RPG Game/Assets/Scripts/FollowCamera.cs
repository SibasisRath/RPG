using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform targetTrasnform;
    [SerializeField] private Vector3 offSet;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cameraTransform.position = targetTrasnform.position + offSet;  
    }
}
