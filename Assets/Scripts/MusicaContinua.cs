using UnityEngine;
using UnityEngine.SceneManagement; // <--- 1. IMPORTANTE: Necesario para leer los nombres de las escenas

public class MusicaContinua : MonoBehaviour
{
    public static MusicaContinua instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

            // 2. Nos "suscribimos" al sistema de escenas.
            // Esto significa: "Avísame cada vez que cargue una escena nueva"
            SceneManager.sceneLoaded += VerificarEscena;
        }
        else
        {
            // Si ya existe otra música (ej. volviste al nivel 1), borramos esta nueva
            Destroy(gameObject);
        }
    }

    // 3. Importante: Si este objeto se destruye, dejamos de escuchar para evitar errores
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= VerificarEscena;
    }

    // 4. Esta es la función que se ejecuta automáticamente al cambiar de nivel
    void VerificarEscena(Scene scene, LoadSceneMode mode)
    {
        // AQUÍ ESTÁ EL FILTRO:
        // Si la escena nueva se llama "Winner" (o como le hayas puesto), la música se apaga.
        // También agregué "MenuPrincipal" por seguridad, por si vuelves al menú de otra forma.
        if (scene.name == "Winner" || scene.name == "MenuPrincipal")
        {
            // Rompemos el ciclo Singleton para que esta música muera y deje sonar la del Winner
            Destroy(gameObject);
        }
    }
}