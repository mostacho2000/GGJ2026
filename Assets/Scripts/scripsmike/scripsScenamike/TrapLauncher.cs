using UnityEngine;

public class TrapLauncher : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("FIRING")]
    public float interval = 1.2f;
    public Vector2 direction = Vector2.right;

    private bool playerInside;
    private float timer;

    private void Update()
    {
        if (!playerInside) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            Debug.Log("DISPARANDO");
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("NO HAY PROJECTILE PREFAB");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("NO HAY FIRE POINT");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log("PROYECTIL INSTANCIADO");

        Projectile2D p = proj.GetComponent<Projectile2D>();
        if (p != null)
        {
            p.Fire(direction);
        }
        else
        {
            Debug.LogError("EL PREFAB NO TIENE Projectile2D");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ALGO ENTRO AL TRIGGER: " + other.name);

        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTADO DENTRO DE LA ZONA");
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("PLAYER SALIO DE LA ZONA");
            playerInside = false;
        }
    }
}
