using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// SE ELIMINÓ: using UnityEditor.Animations; <--- Esto causaba el error

public class PlayerMaskInteraction : MonoBehaviour
{
    public List<GameObject> masks;

    // CAMBIO: Se usa RuntimeAnimatorController para que funcione en el juego final (.exe)
    public List<RuntimeAnimatorController> maskAnimators;

    private GameObject currentMask;
    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private MaskMenu maskMenu;
    public Transform maskHoldPoint;

    private Animator animator;

    private int currentMaskIndex;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        //Find the InputManager in the scene
        inputManager = FindAnyObjectByType<InputManager>();
        maskMenu = FindAnyObjectByType<MaskMenu>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //Empty the list of masks at the start
        masks = new List<GameObject>(); // Aseguramos que la lista esté inicializada
        masks.Clear();
        
        // También aseguramos que la lista de animadores esté limpia si es necesario
        // maskAnimators.Clear(); // (Opcional, depende de tu lógica)

        currentMaskIndex = -1;
        playerMovement.projectileProtected = false;
    }

    private void Update()
    {
        // Verificar que currentMask no sea nulo antes de acceder a sus componentes
        if (currentMask != null)
        {
            if (InputManager.maskActionPressed)
            {
                currentMask.GetComponent<Mask>()?.Activate();
            }

            if (InputManager.maskActionReleased)
            {
                currentMask.GetComponent<Mask>()?.Deactivate();
            }
        }

        if (InputManager.maskSwitchPressed)
        {
            // Solo intentamos cambiar si tenemos máscaras disponibles
            if (masks.Count > 0)
            {
                SwitchMask();
            }
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
        
        // Instanciamos la nueva máscara
        currentMask = Instantiate(masks[currentMaskIndex], maskHoldPoint.position, Quaternion.identity);
        currentMask.transform.SetParent(maskHoldPoint);
        
        // Asignamos el animador correspondiente
        if (currentMaskIndex < maskAnimators.Count)
        {
            animator.runtimeAnimatorController = maskAnimators[currentMaskIndex];
        }

        // Configuramos la máscara
        Mask maskComponent = currentMask.GetComponent<Mask>();
        if (maskComponent != null)
        {
            maskComponent.playerMovement = playerMovement;
            maskComponent.inputManager = inputManager;
            maskComponent.ResetMovementStats();
            maskComponent.OnEquip();
        }

        // Actualizamos el visual del menú
        if (maskMenu != null)
        {
            maskMenu.MaskVisual(currentMaskIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            // If the player is projectile protected, make the projectile stick
            if (!playerMovement.projectileProtected)
            {
                animator.SetTrigger("Dead");
                if (RespawnManager.I != null)
                {
                    RespawnManager.I.Respawn(transform.root.gameObject);
                }
            }
        }
    }
}