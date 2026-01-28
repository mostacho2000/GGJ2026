using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMaskInteraction : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> masks;

    private GameObject currentMask;
    private PlayerMovement playerMovement;
    public Transform maskHoldPoint;

    private int currentMaskIndex = 0;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (InputManager.maskActionPressed)
        {
            currentMask.GetComponent<Mask>()?.Activate();
        }

        if (InputManager.maskSwitchPressed)
        {
            SwitchMask();
        }

        if (InputManager.maskActionReleased) 
        {
            currentMask.GetComponent<Mask>()?.Deactivate();
        }
    }

    private void SwitchMask()
    {
        if (currentMask != null)
        {
            Destroy(currentMask);
        }
        currentMaskIndex++;
        if (currentMaskIndex >= masks.Count)
        {
            currentMaskIndex = 0;
        }
        currentMask = Instantiate(masks[currentMaskIndex], maskHoldPoint.position, Quaternion.identity);
        currentMask.transform.SetParent(maskHoldPoint);

        currentMask.GetComponent<Mask>().playerMovement = playerMovement;
        currentMask.GetComponent<Mask>()?.ResetMovementStats();
        currentMask.GetComponent<Mask>()?.OnEquip();
    }
}
