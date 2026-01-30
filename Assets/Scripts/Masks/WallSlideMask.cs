using UnityEngine;

public class WallSlideMask : Mask
{
    public override void OnEquip()
    {

    }

    public override void Activate()
    {
        playerMovementStats.wallSlideSpeed = 0.01f;
        playerMovementStats.wallJumpPostBufferTime = 0.125f;
    }

    public override void Deactivate()
    {
        playerMovementStats.wallSlideSpeed = 50f;
    }
}
