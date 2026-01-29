using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMaskInteraction : MonoBehaviour
{
    public List<GameObject> masks;

    private GameObject currentMask;
    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private MaskMenu maskMenu;
    public Transform maskHoldPoint;

    private int currentMaskIndex;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        //Find the InputManager in the scene
        inputManager = FindAnyObjectByType<InputManager>();
        maskMenu = FindAnyObjectByType<MaskMenu>();
    }

    private void Start()
    {
        //Empty the list of masks at the start
        masks.Clear();

        currentMaskIndex = -1;
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

    public void SwitchMask()
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
        currentMask.GetComponent<Mask>().inputManager = inputManager;
        currentMask.GetComponent<Mask>()?.ResetMovementStats();
        currentMask.GetComponent<Mask>()?.OnEquip();

        maskMenu.MaskVisual(currentMaskIndex);
    }
}
