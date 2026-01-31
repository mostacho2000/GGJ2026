using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;

public class MaskMenu : MonoBehaviour
{
    [Header("Datos de las Máscaras (Opciones)")]
    [SerializeField] private GameObject[] masks;          // Los prefabs de las máscaras
    [SerializeField] private AnimatorController[] animators; // Los animators correspondientes
    [SerializeField] private Sprite[] maskIcons;          // LOS 5 SPRITES DE LAS MÁSCARAS (Iconos)

    [Header("UI Interacción")]
    [SerializeField] private Button[] buttons;            // Los botones para seleccionar

    [Header("UI Visual (HUD In-Game)")]
    [SerializeField] private GameObject panel;            // Panel de selección
    [SerializeField] private GameObject visualCanvas;     // Panel del juego (HUD)
    [SerializeField] private Image[] hudSlots;            // LAS 3 IMÁGENES DEL HUD (Donde se pondrán los iconos)

    private PlayerMaskInteraction playerMaskInteraction;

    private void Awake()
    {
        playerMaskInteraction = FindAnyObjectByType<PlayerMaskInteraction>();
    }

    void Start()
    {
        // Activar todos los botones al inicio
        foreach (var button in buttons)
        {
            button.interactable = true;
        }

        // Asegurarse de que el HUD esté apagado al inicio y los slots limpios
        visualCanvas.SetActive(false);

        // Pausar el juego
        Time.timeScale = 0f;
    }

    // Se llama desde el botón (index es el ID de la máscara, ej: 0 a 4)
    public void SelectMask(int maskIndex)
    {
        // 1. Añadir la lógica al player
        playerMaskInteraction.masks.Add(masks[maskIndex]);
        playerMaskInteraction.maskAnimators.Add(animators[maskIndex]);

        // 2. Determinar en qué "Slot" del HUD va esta máscara
        // Como acabamos de añadir una, el índice del slot es (Count - 1)
        int currentSlotIndex = playerMaskInteraction.masks.Count - 1;

        // 3. Asignar el sprite correcto al Slot correspondiente
        // Tomamos el sprite de la lista de 5 (maskIcons) y lo ponemos en el slot de 3 (hudSlots)
        if (currentSlotIndex < hudSlots.Length)
        {
            hudSlots[currentSlotIndex].sprite = maskIcons[maskIndex];
            // Opcional: Asegurarse de que la imagen sea visible y tenga color base
            hudSlots[currentSlotIndex].enabled = true;
            hudSlots[currentSlotIndex].color = new Color(1, 1, 1, 0.5f); // Empezar semi-transparente
        }

        // 4. Desactivar el botón para que no se pueda elegir la misma máscara dos veces
        buttons[maskIndex].interactable = false;

        // 5. Verificar si ya elegimos las 3
        if (playerMaskInteraction.masks.Count == 3)
        {
            FinalizeSelection();
        }
    }

    private void FinalizeSelection()
    {
        // Ocultar panel de selección
        panel.SetActive(false);

        // Reanudar tiempo
        Time.timeScale = 1f;

        // Equipar la primera máscara (índice 0 del inventario)
        playerMaskInteraction.SwitchMask();

        // Activar el Canvas del juego
        visualCanvas.SetActive(true);

        // Resaltar visualmente el slot 0
        MaskVisual(0);
    }

    // Esta función resalta el slot activo (0, 1, o 2)
    public void MaskVisual(int activeSlotIndex)
    {
        // Recorremos los 3 slots del HUD
        for (int i = 0; i < hudSlots.Length; i++)
        {
            Color slotColor = hudSlots[i].color;

            if (i == activeSlotIndex)
            {
                // Si es el slot activo, opacidad completa (Brillante)
                slotColor.a = 1f;
            }
            else
            {
                // Si no es el activo, opacidad baja (Oscuro/Transparente)
                slotColor.a = 0.3f; // 0.15f a veces es muy poco visible
            }

            hudSlots[i].color = slotColor;
        }
    }
}