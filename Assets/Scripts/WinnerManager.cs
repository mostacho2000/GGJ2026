using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerManager : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuPrincipal"; // Nombre de tu menú
    public AudioSource musicaVictoria;

    void Start()
    {
        // Configuración de seguridad para el audio
        if (musicaVictoria != null)
        {
            musicaVictoria.loop = false; // Asegura que NO se repita
            musicaVictoria.Play(); // Reproduce la canción una vez
        }
    }

    public void RegresarAlMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}