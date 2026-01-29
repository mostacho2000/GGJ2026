using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color activeColor = Color.green;
    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (!other.transform.root.CompareTag("Player")) return;

        RespawnManager.I.SetCheckpoint(transform);
        activated = true;

        if (sr != null) sr.color = activeColor;

        Debug.Log("CHECKPOINT ACTIVADO: " + name);
    }
}
