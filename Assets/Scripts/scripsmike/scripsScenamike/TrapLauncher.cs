using System.Collections;
using UnityEngine;

public class TrapLauncher : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("FIRING")]
    [SerializeField] private float interval = 2f;
    [SerializeField] private Vector2 direction = Vector2.left;

    private void Start()
    {
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Fire();
        }
    }

    private void Fire()
    {
        if (!projectilePrefab || !firePoint) return;

        var go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        var proj = go.GetComponent<Projectile2D>();
        if (proj != null)
        {
            // si por error direction está en (0,0), dispara hacia la derecha
            Vector2 dir = (direction.sqrMagnitude < 0.0001f) ? Vector2.right : direction.normalized;
            proj.Fire(dir);
        }
    }
}
