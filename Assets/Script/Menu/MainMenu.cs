using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void JugarNuevaPartida()
    {
        SceneManager.LoadScene("main");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }
}