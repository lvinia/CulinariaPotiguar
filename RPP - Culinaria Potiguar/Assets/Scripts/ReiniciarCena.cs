using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarCena : MonoBehaviour
{
    public void Reiniciar()
    {
        Time.timeScale = 1f; // Garante que o jogo não está pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}