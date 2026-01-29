using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem; // Necesario para el nuevo sistema

public class PauseManager : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelPausa;
    public GameObject panelGameOver;

    [Header("Audio")]
    public AudioSource sourceAudio;
    public AudioClip musicaGameOver;

    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuPrincipal";
    private bool estaPausado = false;

    void Start()
    {
        // Debug inicial para saber si el script arrancó
        Debug.Log("PauseManager iniciado correctamente.");

        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
    }

    void Update()
    {
        // 1. Verificamos si Unity detecta un teclado siquiera
        if (Keyboard.current == null)
        {
            // Si esto sale en consola, Unity no reconoce tu teclado
            return; 
        }

        // 2. Verificamos si se presionó la tecla
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // ESTE ES EL MENSAJE QUE PEDISTE
            Debug.Log(">> ¡TECLA ESCAPE PRESIONADA! <<"); 

            if (estaPausado)
            {
                Debug.Log("Estado actual: Pausado. Ejecutando ContinuarJuego()...");
                ContinuarJuego();
            }
            else
            {
                Debug.Log("Estado actual: Jugando. Ejecutando PausarJuego()...");
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        estaPausado = true;
        if (panelPausa != null) 
        {
            panelPausa.SetActive(true);
            Debug.Log("Panel de Pausa activado.");
        }
        else
        {
            Debug.LogError("ERROR: No has asignado el 'Panel Pausa' en el Inspector.");
        }
        Time.timeScale = 0f;
    }

    public void ContinuarJuego()
    {
        estaPausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Juego reanudado.");
    }

    public void IrAlMenuConEfecto()
    {
        Debug.Log("Iniciando secuencia de salida...");
        StartCoroutine(SecuenciaSalida());
    }

    IEnumerator SecuenciaSalida()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) 
        {
            panelGameOver.SetActive(true);
            Debug.Log("Panel Game Over mostrado.");
        }
        else
        {
            Debug.LogError("ERROR: No has asignado el 'Panel Game Over' en el Inspector.");
        }

        if (sourceAudio != null && musicaGameOver != null)
        {
            sourceAudio.PlayOneShot(musicaGameOver);
            Debug.Log("Reproduciendo música...");
        }

        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;
        Debug.Log("Cargando escena de menú: " + nombreEscenaMenu);
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}