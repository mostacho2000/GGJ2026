using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))] // Forza a que exista un RB
public class PlatformMovement : MonoBehaviour
{
    [Header("Dependencies")]
    public PlatformStats platformStats;
    // Ya no necesitamos la variable pública 'platform' si este script está en el objeto padre
    // Si el script está en el mismo objeto que se mueve, usamos 'transform' directamente.

    public List<Transform> waypoints;

    // Internal State
    private int _targetIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Configuración de seguridad para la física
        _rb.bodyType = RigidbodyType2D.Kinematic; // Importante: Kinematic para que no caiga por gravedad
        _rb.useFullKinematicContacts = true; // Importante: Permite colisiones completas siendo Kinematic

        if (platformStats == null || waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Faltan referencias en PlatformMovement");
            return;
        }

        // Posición inicial
        if (waypoints[0] != null) transform.position = waypoints[0].position;

        _targetIndex = (waypoints.Count > 1) ? 1 : 0;
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

        float currentSpeed = platformStats.platformSpeeds[_targetIndex];

        // CAMBIO CLAVE: Mover usando el Rigidbody, no el Transform
        Vector2 newPosition = Vector2.MoveTowards(_rb.position, targetWaypoint.position, currentSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(newPosition);

        // Chequear llegada
        if (Vector2.Distance(_rb.position, targetWaypoint.position) < 0.05f)
        {
            // Ajuste final
            _rb.MovePosition(targetWaypoint.position);

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
        _targetIndex++;
        if (_targetIndex >= waypoints.Count) _targetIndex = 0;
    }

    // (Tu código de Gizmos se mantiene igual, es correcto)
    private void OnDrawGizmos()
    {
        // ... (Mismo código de gizmos que ya tenías)
    }
}