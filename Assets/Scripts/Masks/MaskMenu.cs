using UnityEngine;
using TMPro;
using UnityEngine.UI;
// BORRADA LA LÍNEA: using UnityEditor.Animations; <--- Ya no la necesitamos

public class MaskMenu : MonoBehaviour
{
    [Header("Datos de las Máscaras (Opciones)")]
    [SerializeField] private GameObject[] masks;
    
    // CAMBIO IMPORTANTE AQUÍ ABAJO:
    // Antes: private AnimatorController[] animators;
    // Ahora: private RuntimeAnimatorController[] animators;
    // Esto acepta exactamente los mismos archivos, pero permite exportar el juego.
    [SerializeField] private RuntimeAnimatorController[] animators; 

    [SerializeField] private Sprite[] maskIcons;

    [Header("UI Interacción")]
    [SerializeField] private Button[] buttons;

    [Header("UI Visual (HUD In-Game)")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject visualCanvas;
    [SerializeField] private Image[] hudSlots;

    private PlayerMaskInteraction playerMaskInteraction;

    private void Awake()
    {
        playerMaskInteraction = FindAnyObjectByType<PlayerMaskInteraction>();
    }

    void Start()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }

        visualCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    public void SelectMask(int maskIndex)
    {
        playerMaskInteraction.masks.Add(masks[maskIndex]);
        playerMaskInteraction.maskAnimators.Add(animators[maskIndex]); // Esto funcionará si actualizas el otro script también

        int currentSlotIndex = playerMaskInteraction.masks.Count - 1;

        if (currentSlotIndex < hudSlots.Length)
        {
            hudSlots[currentSlotIndex].sprite = maskIcons[maskIndex];
            hudSlots[currentSlotIndex].enabled = true;
            hudSlots[currentSlotIndex].color = new Color(1, 1, 1, 0.5f);
        }

        buttons[maskIndex].interactable = false;

        if (playerMaskInteraction.masks.Count == 3)
        {
            FinalizeSelection();
        }
    }

    private void FinalizeSelection()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        playerMaskInteraction.SwitchMask();
        visualCanvas.SetActive(true);
        MaskVisual(0);
    }

    public void MaskVisual(int activeSlotIndex)
    {
        for (int i = 0; i < hudSlots.Length; i++)
        {
            Color slotColor = hudSlots[i].color;

            if (i == activeSlotIndex)
            {
                slotColor.a = 1f;
            }
            else
            {
                slotColor.a = 0.3f;
            }

            hudSlots[i].color = slotColor;
        }
    }
}