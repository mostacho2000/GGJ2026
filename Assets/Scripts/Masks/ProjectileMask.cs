using UnityEngine;

public class ProjectileMask : Mask
{
    public override void OnEquip()
    {
        playerMovement.projectileProtected = true;
    }

    public override void Activate()
    {

    }

    public override void Deactivate()
    {

    }
}
