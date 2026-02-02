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
    
    // NOTA: Ya no necesitamos la variable 'musicaDeFondo' aquí arriba
    // La buscaremos dinámicamente para evitar errores de referencia.

    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuPrincipal"; 
    private bool estaPausado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
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
        Time.timeScale = 0f; 
    }

    public void ContinuarJuego()
    {
        estaPausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void IrAlMenuConEfecto()
    {
        StartCoroutine(SecuenciaSalida());
    }

    IEnumerator SecuenciaSalida()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(true);

        // --- CORRECCIÓN CLAVE AQUÍ ---
        // Buscamos la música JUSTO AHORA, no al inicio.
        // Buscamos el objeto que tenga el script MusicaContinua.
        MusicaContinua musicaSonando = FindAnyObjectByType<MusicaContinua>();

        if (musicaSonando != null)
        {
            Debug.Log("Música encontrada y destruida: " + musicaSonando.name);
            Destroy(musicaSonando.gameObject); 
        }
        else
        {
            Debug.LogWarning("No encontré ninguna música con el script MusicaContinua para destruir.");
        }
        // -----------------------------

        // Reproducimos Game Over
        if (sourceEfectos != null && clipGameOver != null)
        {
            sourceEfectos.PlayOneShot(clipGameOver);
        }

        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}