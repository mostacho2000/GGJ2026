using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 'true' mantiene la posición/rotación/escala en el mundo
            // evitando que el jugador se deforme si la plataforma está escalada
            collision.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);

            // Opcional: Si el jugador mantiene escala extraña al salir
            // collision.transform.localScale = Vector3.one; 
        }
    }
}