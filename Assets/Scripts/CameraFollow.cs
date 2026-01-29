using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Configuración de Offset")]
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Header("Suavizado")]
    [Range(0.01f, 1f)]
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [Header("Límites de la Cámara")]
    [Tooltip("Activa esto para que la cámara no salga de la zona")]
    public bool enableLimits = true;

    [Tooltip("La esquina inferior izquierda del nivel (X mínima, Y mínima)")]
    public Vector2 minLimit;

    [Tooltip("La esquina superior derecha del nivel (X máxima, Y máxima)")]
    public Vector2 maxLimit;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculamos la posición deseada con el suavizado habitual
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // 2. Aplicamos los límites (Clamping)
        if (enableLimits)
        {
            // Restringimos la X entre el mínimo y el máximo
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minLimit.x, maxLimit.x);

            // Restringimos la Y entre el mínimo y el máximo
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minLimit.y, maxLimit.y);
        }

        // 3. Mantenemos el Z fijo (muy importante en 2D)
        smoothedPosition.z = -10f;

        // 4. Aplicamos la posición final
        transform.position = smoothedPosition;
    }


    private void OnDrawGizmos()
    {
        if (enableLimits)
        {
            Gizmos.color = Color.red;
            // Dibuja un cuadro que representa el área permitida
            Vector3 center = new Vector3((minLimit.x + maxLimit.x) / 2, (minLimit.y + maxLimit.y) / 2, 0);
            Vector3 size = new Vector3(maxLimit.x - minLimit.x, maxLimit.y - minLimit.y, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}