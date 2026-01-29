using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 movement;

    public static bool jumpPressed;
    public static bool jumpHeld;
    public static bool jumpReleased;
    public static bool dashPressed;
    public static bool runHeld;
    public static bool maskSwitchPressed;
    public static bool maskActionPressed;
    public static bool maskActionReleased;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction runAction;
    private InputAction switchMask;
    private InputAction maskAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        //runAction = playerInput.actions["Run"];
        switchMask = playerInput.actions["SwitchMask"];
        maskAction = playerInput.actions["MaskAction"];
    }

    // Update is called once per frame
    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();

        jumpPressed = jumpAction.WasPressedThisFrame();
        jumpHeld = jumpAction.IsPressed();
        jumpReleased = jumpAction.WasReleasedThisFrame();

        dashPressed = dashAction.WasPressedThisFrame();

        //runHeld = runAction.IsPressed();

        maskSwitchPressed = switchMask.WasPressedThisFrame();
        maskActionPressed = maskAction.WasPressedThisFrame();
        maskActionReleased = maskAction.WasReleasedThisFrame();
    }

    public void ChangeDashInput(bool valid)
    {
        if (valid)
        {
            dashAction.Enable();
        }
        else
        {
            dashAction.Disable();
        }
    }
}
