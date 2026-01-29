using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Trigger respawn to the parent object
        
        RespawnManager.I.Respawn(other.transform.root.gameObject);
    }
}
