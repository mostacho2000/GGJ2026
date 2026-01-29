using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 1;  // Jam: 1 hit = respawn (rápido)
    private int hp;
    private bool dead;

    private void Awake()
    {
        hp = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;
        hp -= dmg;
        if (hp <= 0) Die();
    }

    private void Die()
    {
        dead = true;
        RespawnManager.I.Respawn(gameObject);
    }

    public void ResetAfterRespawn()
    {
        hp = maxHP;
        dead = false;
    }
}
