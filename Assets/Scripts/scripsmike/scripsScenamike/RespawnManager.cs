using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager I;

    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private float respawnDelay = 0.25f;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
    }

    public void SetCheckpoint(Transform t) => currentCheckpoint = t;

    public void Respawn(GameObject player) => StartCoroutine(RespawnRoutine(player));

    private IEnumerator RespawnRoutine(GameObject player)
    {
        yield return new WaitForSeconds(respawnDelay);

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        player.transform.position = currentCheckpoint.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // Si agregas vida/estado, aquí lo reseteas
        var hp = player.GetComponent<PlayerHealth>();
        if (hp != null) hp.ResetAfterRespawn();
    }
}
