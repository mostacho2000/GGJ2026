using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats playerMovementStats;
    public PlayerMovement playerMovement;

    public abstract void OnEquip();
    public abstract void Activate();
    public abstract void Deactivate();

    public virtual void ResetMovementStats()
    {
        playerMovementStats.numberOfDashes = 0;
        playerMovementStats.maxJumpNumber = 1;
    }
}
