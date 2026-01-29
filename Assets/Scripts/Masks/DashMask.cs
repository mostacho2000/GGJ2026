using UnityEngine;

public class DashMask : Mask
{
    public override void OnEquip()
    {
        playerMovementStats.numberOfDashes = 1;

        inputManager.ChangeDashInput(false);
    }

    public override void Activate()
    {
        InputManager.dashPressed = true;
        playerMovement.DashCheck();
    }

    public override void Deactivate()
    {

    }
}
