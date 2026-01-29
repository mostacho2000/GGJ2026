using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats playerMovementStats;
    public PlayerMovement playerMovement;
    public InputManager inputManager;

    public abstract void OnEquip();
    public abstract void Activate();
    public abstract void Deactivate();

    public virtual void ResetMovementStats()
    {
        playerMovementStats.numberOfDashes = 0;
        playerMovementStats.maxJumpNumber = 1;
        playerMovementStats.maxFallSpeed = 26f;
        playerMovementStats.wallJumpPostBufferTime = 0f;
        inputManager.ChangeDashInput(true);
        playerMovement.projectileProtected = false;
    }
}
