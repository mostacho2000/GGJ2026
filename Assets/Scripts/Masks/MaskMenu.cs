using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;

public class MaskMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] masks;

    [SerializeField]
    private AnimatorController[] animators;

    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private Image[] images;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject visualCanvas;

    private PlayerMaskInteraction playerMaskInteraction;

    private void Awake()
    {
        playerMaskInteraction = FindAnyObjectByType<PlayerMaskInteraction>();
    }

    void Start()
    {
        // Enable all buttons at the start
        foreach (var button in buttons)
        {
            button.interactable = true;
        }

        // Pause movement
        Time.timeScale = 0f;
    }

    public void SelectMask(int index)
    {
        playerMaskInteraction.masks.Add(masks[index]);
        playerMaskInteraction.maskAnimators.Add(animators[index]);

        if (playerMaskInteraction.masks.Count == 3)
        {
            //Disable the canvas when three masks have been selected
            panel.SetActive(false);

            //Resume movement
            Time.timeScale = 1f;

            //Equip the first mask
            playerMaskInteraction.SwitchMask();
            MaskVisual(0);

            visualCanvas.SetActive(true);
        }

        // Assign the corresponding sprite to the image
        //images[index].sprite = sprites[index];

        //Disable the button used to select the mask
        buttons[index].interactable = false;
    }

    public void MaskVisual(int index)
    {
        // Remove the alpha from the index image
        Color tempColor = images[index].color;
        tempColor.a = 1f;
        images[index].color = tempColor;

        // Add alpha to other images
        for (int i = 0; i < images.Length; i++)
        {
            if (i != index)
            {
                Color otherColor = images[i].color;
                otherColor.a = 0.15f;
                images[i].color = otherColor;
            }
        }
    }
}
