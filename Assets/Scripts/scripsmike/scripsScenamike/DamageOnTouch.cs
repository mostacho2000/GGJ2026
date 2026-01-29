using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform root = other.transform.root;

        if (!root.CompareTag("Player")) return;

        Debug.Log("TRAMPA DAÑO AL PLAYER: " + gameObject.name);

        var hp = root.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(damage);
        else
            Debug.LogError("PlayerHealth NO encontrado en Player");
    }
}
