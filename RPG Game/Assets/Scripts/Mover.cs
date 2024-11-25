using System;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator playerAnimator;
    private Transform playerTransform;
    private bool hasHit;
    Vector3 velocity;
    Vector3 localVelocity;


    private Ray lastRay;

    void Start()
    {
        playerTransform = transform;
        if (!meshAgent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not placed on a NavMesh!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        UpdateAnimation();

    }

    private void UpdateAnimation()
    {
        velocity = meshAgent.velocity;
        localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        playerAnimator.SetFloat("ForwardSpeed", speed);
    }

    private void MoveToCursor()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        hasHit = Physics.Raycast(ray, out RaycastHit raycastHit);

        if (hasHit)
        {
            meshAgent.destination = raycastHit.point;
        }
    }
}

