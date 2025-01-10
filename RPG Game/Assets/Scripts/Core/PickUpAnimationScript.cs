using UnityEngine;

public class PickUpAnimationScript : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [Tooltip("How high/low the object moves from its start point.")]
    [SerializeField] private float amplitude = 0.5f;

    [Tooltip("How quickly it bobs up and down.")]
    [SerializeField] private float bobSpeed = 2f;

    [Header("Rotation Settings")]
    [Tooltip("Degrees per second to rotate around the Y axis (360 makes one full rotation per second).")]
    [SerializeField] private float rotationSpeed = 90f;

    // We'll store the initial position to offset from
    private Vector3 startPosition;

    void Start()
    {
        // Record the object's starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // 1) Bobbing (Up & Down)
        float yOffset = amplitude * Mathf.Sin(Time.time * bobSpeed);
        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + yOffset,
            startPosition.z
        );

        // 2) Continuous 360 Rotation
        // Rotate around the Y-axis by 'rotationSpeed' degrees/second
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
