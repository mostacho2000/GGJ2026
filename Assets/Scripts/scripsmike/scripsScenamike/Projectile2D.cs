using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool rotateToVelocity = true;

    private Vector2 dir;

    public void Fire(Vector2 direction)
    {
        dir = direction.normalized;

        if (rotateToVelocity)
        {
            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, ang);
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }
}
