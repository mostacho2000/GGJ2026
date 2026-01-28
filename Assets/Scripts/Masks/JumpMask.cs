using UnityEngine;

public class JumpMask : Mask
{
    public override void OnEquip()
    {
        playerMovementStats.maxJumpNumber = 2;
    }

    public override void Activate()
    {
        InputManager.jumpPressed = true;
        if(playerMovement.numberOfJumps == 1) playerMovement.JumpChecks();
    }
    public override void Deactivate() 
    { 

    }
}
