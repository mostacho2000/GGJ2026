using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    private int hp;
    private bool dead;

    private void Awake() => hp = maxHP;

    public void TakeDamage(int dmg)
    {
        if (dead) return;
        hp -= dmg;
        if (hp <= 0)
        {
            dead = true;
            RespawnManager.I.Respawn(gameObject);
        }
    }

    public void ResetAfterRespawn()
    {
        hp = maxHP;
        dead = false;
    }
}
