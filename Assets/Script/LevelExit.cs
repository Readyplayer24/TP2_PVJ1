using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Configuración de Salida")]
    [Tooltip("Menu Principal")]
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("¡Nivel superado! Cargando siguiente escena...");

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("¡Atención! No escribiste el nombre de la escena de destino en el Inspector.");
            }
        }
    }
}