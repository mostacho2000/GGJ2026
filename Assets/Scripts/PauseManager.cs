using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para las Corrutinas

public class PauseManager : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelPausa;
    public GameObject panelGameOver;

    [Header("Audio")]
    public AudioSource sourceAudio; // El componente que emite el sonido
    public AudioClip musicaGameOver; // El archivo de audio de 6 segundos

    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuPrincipal"; // Pon aquí el nombre exacto de tu escena de menú
    private bool estaPausado = false;

    void Start()
    {
        // Asegurarnos que los paneles esten apagados al iniciar el nivel
        panelPausa.SetActive(false);
        panelGameOver.SetActive(false);
    }

    void Update()
    {
        // Detectar la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estaPausado)
            {
                ContinuarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    // --- FUNCIONES DEL MENÚ DE PAUSA ---

    public void PausarJuego()
    {
        estaPausado = true;
        panelPausa.SetActive(true);
        Time.timeScale = 0f; // ESTO CONGELA EL TIEMPO
    }

    public void ContinuarJuego()
    {
        estaPausado = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // ESTO DESCONGELA EL TIEMPO
    }

    // Esta función la conectas al botón "Menu" del panel de pausa
    public void IrAlMenuConEfecto()
    {
        StartCoroutine(SecuenciaSalida());
    }

    // --- LA SECUENCIA CHINGONA (Corrutina) ---
    IEnumerator SecuenciaSalida()
    {
        // 1. Ocultamos el menú de pausa y mostramos el de Game Over
        panelPausa.SetActive(false);
        panelGameOver.SetActive(true);

        // 2. Reproducimos la canción
        if (sourceAudio != null && musicaGameOver != null)
        {
            sourceAudio.PlayOneShot(musicaGameOver);
        }

        // 3. Esperamos 6 segundos REALES (ignorando que el juego está en pausa)
        yield return new WaitForSecondsRealtime(6f);

        // 4. Importante: Devolver el tiempo a la normalidad antes de cambiar de escena
        // Si no haces esto, el menú principal o el siguiente juego iniciarán congelados.
        Time.timeScale = 1f;

        // 5. Cargar el menú
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}