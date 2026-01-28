using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI; // Necesario para controlar el Slider

public class MenuManager : MonoBehaviour
{
    [Header("Paneles (Arrastra aquí los GameObjects)")]
    public GameObject panelPrincipal;
    public GameObject panelSonido;
    public GameObject panelTutorial;

    [Header("Configuración de Juego")]
    public string nombreEscenaJuego = "Nivel1"; // Cambia esto por el nombre de tu escena de juego

    // Método que se ejecuta al iniciar
    void Start()
    {
        // Aseguramos que al abrir el juego solo se vea el menú principal
        RegresarAlMenu();
    }

    // --- FUNCIONES DE NAVEGACIÓN ---

    public void AbrirOpcionesSonido()
    {
        panelPrincipal.SetActive(false);
        panelSonido.SetActive(true);
        panelTutorial.SetActive(false);
    }

    public void AbrirTutorial()
    {
        panelPrincipal.SetActive(false);
        panelSonido.SetActive(false);
        panelTutorial.SetActive(true);
    }

    public void RegresarAlMenu()
    {
        panelPrincipal.SetActive(true);
        panelSonido.SetActive(false);
        panelTutorial.SetActive(false);
    }

    // --- FUNCIONES DE LOS BOTONES PRINCIPALES ---

    public void Jugar()
    {
        // Carga la escena del juego
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego..."); // Mensaje para ver en el editor
        Application.Quit();
    }

    // --- CONTROL DE SONIDO ---
    
    // Conecta esta función al evento "On Value Changed" del Slider en el Inspector
    public void ControlarVolumen(float volumen)
    {
        // AudioListener controla el volumen global de todo el juego (0.0 a 1.0)
        AudioListener.volume = volumen;
    }
}
