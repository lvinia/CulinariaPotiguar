using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public int vidas = 3;
    public float invulnerabilidadeTempo = 0.3f;
    
    public TextMeshProUGUI textoVidas;
    public SpriteRenderer spriteRenderer;
    public GameObject telaGameOver;
    
    // NOVO - Variáveis para o som
    public AudioClip damageSound;
    private AudioSource audioSource;
    
    private bool estaInvulneravel = false;
    private Color corNormal;
    
    void Start()
    {
        AtualizarTextoVidas();
        telaGameOver.SetActive(false);
        
        if (spriteRenderer != null)
            corNormal = spriteRenderer.color;
        
        // NOVO - Pega ou adiciona o AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        
        // NOVO - Toca o som de dano
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
        
        if (vidas <= 0)
        {
            GameOver();
            if (FragmentManager.Instance != null)
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
        
        float elapsed = 0f;
        while (elapsed < invulnerabilidadeTempo)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = corNormal;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }
        
        spriteRenderer.color = corNormal;
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
    
    public int getVida()
    {
        return vidas;
    }
}