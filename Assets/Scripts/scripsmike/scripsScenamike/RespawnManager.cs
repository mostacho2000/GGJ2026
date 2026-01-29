using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager I;

    [Header("Setup")]
    public Transform startSpawnPoint;
    public float respawnDelay = 0.2f;

    private Transform currentCheckpoint;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        currentCheckpoint = startSpawnPoint;
    }

    public void SetCheckpoint(Transform t)
    {
        currentCheckpoint = t;
        Debug.Log("Checkpoint set: " + t.name);
    }

    public void Respawn(GameObject playerRoot)
    {
        StartCoroutine(RespawnRoutine(playerRoot));
    }

    private IEnumerator RespawnRoutine(GameObject playerRoot)
    {
        yield return new WaitForSeconds(respawnDelay);

        var rb = playerRoot.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Teleport
        if (currentCheckpoint != null)
            playerRoot.transform.position = currentCheckpoint.position;
        else if (startSpawnPoint != null)
            playerRoot.transform.position = startSpawnPoint.position;

        // Reset velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // Reset HP (si existe)
        var hp = playerRoot.GetComponent<PlayerHealth>();
        if (hp != null) hp.ResetAfterRespawn();

        Debug.Log("Respawned!");
    }
}
