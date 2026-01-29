using UnityEngine;

public class MusicaContinua : MonoBehaviour
{
    public static MusicaContinua instancia;

    void Awake()
    {
        // El patrón Singleton: Asegura que solo haya UNA música sonando
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // <--- LA MAGIA: Esto hace que sobreviva al cambio de escena
        }
        else
        {
            // Si ya existe una música viniendo del nivel anterior, destruimos esta nueva
            // para que no haya dos canciones sonando a la vez.
            Destroy(gameObject);
        }
    }
}