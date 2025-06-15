using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarCena : MonoBehaviour
{
    public string cenaParaCarregar;

    void Start()
    {
        SceneManager.LoadScene(cenaParaCarregar);
    }
}
