using UnityEngine;

public class Sair : MonoBehaviour
{
    // Método para sair do jogo
    public void Saindo()
    {
        Debug.Log("Saindo do jogo");
        
#if UNITY_EDITOR
        // Se estiver no Editor, para o Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Se for o jogo compilado, fecha a aplicação
            Application.Quit();
#endif
    }
}