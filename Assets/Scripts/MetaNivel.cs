using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaNivel : MonoBehaviour
{
    [Header("Configuración de Viaje")]
    [Tooltip("Escribe aquí EXACTAMENTE el nombre de la escena a la que quieres ir.")]
    public string nombreSiguienteEscena; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo el jugador puede activar el cambio de nivel
        if (other.CompareTag("Player"))
        {
            // Verificamos que no se te haya olvidado poner el nombre
            if (!string.IsNullOrEmpty(nombreSiguienteEscena))
            {
                Debug.Log("Viajando a: " + nombreSiguienteEscena);
                SceneManager.LoadScene(nombreSiguienteEscena);
            }
            else
            {
                Debug.LogError("¡ERROR! Se te olvidó escribir el nombre de la escena en el Inspector.");
            }
        }
    }
}