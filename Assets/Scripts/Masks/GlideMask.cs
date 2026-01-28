using UnityEngine;

public class GlideMask : Mask
{
    public override void OnEquip()
    {

    }

    public override void Activate()
    {
        playerMovementStats.maxFallSpeed = 7f;
    }

    public override void Deactivate()
    {
        playerMovementStats.maxFallSpeed = 26f;
    }
}
