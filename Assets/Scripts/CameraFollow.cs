using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector2 offset = new Vector2(0f, 1.5f);

    [Header("Smooth")]
    [Range(0.01f, 1f)]
    public float smoothTime = 0.15f;

    [Header("Limits (optional)")]
    public bool useLimits = false;
    public Vector2 minLimit;
    public Vector2 maxLimit;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        Vector3 smoothPosition = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        if (useLimits)
        {
            smoothPosition.x = Mathf.Clamp(smoothPosition.x, minLimit.x, maxLimit.x);
            smoothPosition.y = Mathf.Clamp(smoothPosition.y, minLimit.y, maxLimit.y);
        }

        transform.position = smoothPosition;
    }
}
