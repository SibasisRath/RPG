using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "AI/Configuration")]
public class AIConfig : ScriptableObject
{
    [Header(" Patrol Settings")]
    [Tooltip("Speed while patrolling")]
    public float patrolSpeedFraction = 0.2f;  // Speed while patrolling
    [Tooltip("How close AI must get to a waypoint")]
    public float waypointTolerance = 1.2f;   // How close AI must get to a waypoint
    [Tooltip("Time spent at a waypoint before moving")]
    public float waypointDwellTime = 3f;     // Time spent at a waypoint before moving

    [Header(" Combat Settings")]
    [Tooltip("AI detection range")]
    public float chaseDistance = 5f;        // AI detection range
    [Tooltip("Distance required to attack")]
    public float attackRange = 2f;          // Distance required to attack
    [Tooltip("Cooldown time between attacks")]
    public float timeBetweenAttacks = 0.5f; // Cooldown time between attacks

    [Header(" Suspicion Settings")]
    [Tooltip("Time AI waits before returning to patrol")]
    public float suspicionTime = 3f; // Time AI waits before returning to patrol

    [Header(" Aggro Settings")]
    [Tooltip("Time AI stays aggressive after losing sight")]
    public float agroCooldownTime = 5f;  // Time AI stays aggressive after losing sight
    [Tooltip("How far AI alerts nearby enemies")]
    public float shoutDistance = 5f;     // How far AI alerts nearby enemies

    [Header(" Flee Settings")]
    [Tooltip("AI flees if health falls below this fraction (e.g., 30%)")]
    public float fleeHealthThreshold = 0.3f; // AI flees if health falls below this fraction (e.g., 30%)
    [Tooltip("How far AI runs when fleeing")]
    public float fleeDistance = 10f;        // How far AI runs when fleeing
    [Tooltip("Time AI waits before returning to normal behavior")]
    public float fleeTime = 3f;             // Time AI waits before returning to normal behavior
}