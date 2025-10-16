using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public int vidas = 3;
    public float invulnerabilidadeTempo = 0.3f;

    public TextMeshProUGUI textoVidas;
    public GameObject personagemNormal;
    public GameObject personagemVermelho;
    public GameObject telaGameOver;

    private bool estaInvulneravel = false;

    void Start()
    {
        AtualizarTextoVidas();
        telaGameOver.SetActive(false);
        personagemNormal.SetActive(true);
        personagemVermelho.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Dano") && !estaInvulneravel)
        {
            TomarDano();
        }
    }

    void TomarDano()
    {
        vidas--;
        AtualizarTextoVidas();

        if (vidas <= 0)
        {
            GameOver();
            FragmentManager.Instance.ResetFragments();
        }
        else
        {
            StartCoroutine(InvulnerabilidadeTemporaria());
        }
    }

    void AtualizarTextoVidas()
    {
        textoVidas.text = "Vidas: " + vidas;
    }

    System.Collections.IEnumerator InvulnerabilidadeTemporaria()
    {
        estaInvulneravel = true;
        personagemNormal.SetActive(false);
        personagemVermelho.SetActive(true);

        yield return new WaitForSeconds(invulnerabilidadeTempo);

        personagemNormal.SetActive(true);
        personagemVermelho.SetActive(false);
        estaInvulneravel = false;
    }

    void GameOver()
    {
        telaGameOver.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        if (transform.position.y < -10f && !estaInvulneravel)
        {
            GameOver();
        }
    }
}
