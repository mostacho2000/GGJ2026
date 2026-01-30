using UnityEngine;
using System.Collections.Generic;

public class PlatformMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The external script containing Speed and Wait Time lists.")]
    public PlatformStats platformStats;
    public GameObject platform;

    [Tooltip("The physical objects marking the path.")]
    public List<Transform> waypoints;

    // Internal State
    private int _targetIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    private void Start()
    {
        // Safety Checks
        if (platformStats == null)
        {
            Debug.LogError("PlatformStats is missing! Please assign it in the Inspector.");
            return;
        }

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            return;
        }

        // Validate List Sizes
        if (platformStats.platformSpeeds.Count != waypoints.Count ||
            platformStats.timesBeforeNextMove.Count != waypoints.Count)
        {
            Debug.LogWarning("Warning: Waypoints, Speeds, and Wait Times lists have different sizes. " +
                             "This may cause errors during the loop.");
        }

        // Setup Starting Position
        // We snap the platform to the first waypoint (Index 0)
        if (waypoints[0] != null)
        {
            platform.transform.position = waypoints[0].position;
        }

        // Set the first target to be the SECOND waypoint (Index 1)
        // If there is only 1 waypoint, we just stay there (index 0).
        _targetIndex = (waypoints.Count > 1) ? 1 : 0;

        _isWaiting = false;
    }

    private void FixedUpdate()
    {
        if (waypoints.Count < 2) return;

        if (_isWaiting)
        {
            HandleWaitTimer();
        }
        else
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        Transform targetWaypoint = waypoints[_targetIndex];

        if (targetWaypoint == null) return;

        // Fetch speed from the external stats script
        // We use the index of the TARGET to determine the speed getting there
        float currentSpeed = platformStats.platformSpeeds[_targetIndex];

        // Move the platform (No Rigidbody)
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetWaypoint.position, currentSpeed * Time.fixedDeltaTime);

        // Check for arrival
        if (Vector3.Distance(platform.transform.position, targetWaypoint.position) < 0.01f)
        {
            // Snap to exact position
            platform.transform.position = targetWaypoint.position;

            // Fetch wait time from the external stats script
            float waitDuration = platformStats.timesBeforeNextMove[_targetIndex];
            StartWait(waitDuration);
        }
    }

    private void StartWait(float duration)
    {
        _waitTimer = duration;
        _isWaiting = true;
    }

    private void HandleWaitTimer()
    {
        _waitTimer -= Time.fixedDeltaTime;

        if (_waitTimer <= 0f)
        {
            _isWaiting = false;
            UpdateWaypointIndex();
        }
    }

    private void UpdateWaypointIndex()
    {
        // Increment index and loop back to 0 if we hit the end
        _targetIndex++;

        if (_targetIndex >= waypoints.Count)
        {
            _targetIndex = 0;
        }
    }

    // --- Visualization ---

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                // Draw line to previous node
                if (i > 0 && waypoints[i - 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
                }
            }
        }

        // Draw loop line (Last -> First)
        if (waypoints.Count > 1)
        {
            if (waypoints[waypoints.Count - 1] != null && waypoints[0] != null)
            {
                Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
    }
}
