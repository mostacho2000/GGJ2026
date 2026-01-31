using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem; 

public class PauseManager : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelPausa;
    public GameObject panelGameOver;

    [Header("Audio")]
    public AudioSource sourceEfectos; 
    public AudioClip clipGameOver;    
    
    // Ya no es pública para arrastrar, la buscamos por código
    private AudioSource musicaDeFondo; 

    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuPrincipal"; 
    private bool estaPausado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        // --- CAMBIO IMPORTANTE ---
        // Buscamos el objeto que tenga el script MusicaContinua y cogemos su audio
        MusicaContinua objetoMusica = FindAnyObjectByType<MusicaContinua>();
        if (objetoMusica != null)
        {
            musicaDeFondo = objetoMusica.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (estaPausado) ContinuarJuego();
            else PausarJuego();
        }
    }

    public void PausarJuego()
    {
        estaPausado = true;
        if (panelPausa != null) panelPausa.SetActive(true);
        // Cambia el mapa de acciones si usas el sistema de Input
        InputManager.playerInput.SwitchCurrentActionMap("UI");

        Time.timeScale = 0f;
    }

    public void ContinuarJuego()
    {
        estaPausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        // Vuelve al mapa de acciones del juego
        InputManager.playerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void IrAlMenuConEfecto()
    {
        StartCoroutine(SecuenciaSalida());
    }

    IEnumerator SecuenciaSalida()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(true);

        // 1. Matamos la música para siempre
        // Usamos Destroy en lugar de Stop para que, al llegar al menú, 
        // el objeto desaparezca y no choque con la música del menú.
        if (musicaDeFondo != null)
        {
            Destroy(musicaDeFondo.gameObject); 
        }

        // 2. Sonido de derrota
        if (sourceEfectos != null && clipGameOver != null)
        {
            sourceEfectos.PlayOneShot(clipGameOver);
        }

        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}