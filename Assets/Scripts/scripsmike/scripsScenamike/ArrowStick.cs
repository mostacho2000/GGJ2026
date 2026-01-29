using UnityEngine;

public class ArrowStick : MonoBehaviour
{
    public LayerMask stickTo;        // Ground
    public bool becomePlatform = true;

    private bool stuck;
    [SerializeField]
    private Collider2D col;

    public void Stick(string reason)
    {
        if (stuck) return;
        stuck = true;

        Debug.Log($"ARROW STUCK! reason={reason} obj={name}", this);

        // Detener movimiento
        var proj = GetComponent<Projectile2D>();
        if (proj != null) proj.enabled = false;

        // Volverse sólido
        if (becomePlatform && col != null)
        {
            // deshabilitar el collider
            col.enabled = false;

            gameObject.tag = "Platform";
        }
    }

    private bool InMask(int layer)
    {
        return (stickTo.value & (1 << layer)) != 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Arrow TriggerEnter with {other.name} layer={LayerMask.LayerToName(other.gameObject.layer)}", this);

        if (stuck) return;
        if (other.transform.root.CompareTag("Player")) return;

        if (InMask(other.gameObject.layer))
            Stick("TriggerEnter + LayerMask");
        else
            Debug.Log("NOT in stickTo mask", this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Backup: a veces Enter no cae por timings raros, pero Stay sí
        if (stuck) return;
        if (other.transform.root.CompareTag("Player")) return;

        if (InMask(other.gameObject.layer))
            Stick("TriggerStay + LayerMask");
    }

    // Extra backup: si por alguna razón no estuviera en trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Arrow CollisionEnter with {collision.collider.name}", this);

        if (stuck) return;
        if (collision.transform.root.CompareTag("Player")) return;

        if (InMask(collision.gameObject.layer))
            Stick("CollisionEnter + LayerMask");
    }
}
