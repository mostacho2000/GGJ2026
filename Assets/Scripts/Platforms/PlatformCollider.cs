using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    // --- Player Parenting (No Rigidbody) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Note: The Player must have a Rigidbody2D for this collision to be detected
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
