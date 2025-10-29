using UnityEngine;

public class PlayerSpeedController : MonoBehaviour
{
    [Header("Componentes")]
    private Animator animator;
    
    [Header("Configurações de Velocidade")]
    public float velocidadeInicial = 0.5f; // Começa devagar (50% da velocidade da animação)
    public float velocidadeMaxima = 2f;    // Velocidade máxima (200% da velocidade)
    public float tempoParaAtingirMaximo = 60f; // Tempo em segundos para atingir velocidade máxima
    
    private float velocidadeAtual;
    private float tempoDecorrido;

    void Start()
    {
        animator = GetComponent<Animator>();
        velocidadeAtual = velocidadeInicial;
        
        // Define a velocidade inicial da animação
        if (animator != null)
        {
            animator.speed = velocidadeAtual;
        }
    }

    void Update()
    {
        // Aumenta o tempo decorrido
        tempoDecorrido += Time.deltaTime;
        
        // Calcula o progresso (0 a 1)
        float progresso = Mathf.Clamp01(tempoDecorrido / tempoParaAtingirMaximo);
        
        // Interpola suavemente entre velocidade inicial e máxima
        velocidadeAtual = Mathf.Lerp(velocidadeInicial, velocidadeMaxima, progresso);
        
        // Aplica a velocidade no Animator
        if (animator != null)
        {
            animator.speed = velocidadeAtual;
        }
    }
    
    // Função para pegar a velocidade atual (útil para outros scripts)
    public float GetVelocidadeAtual()
    {
        return velocidadeAtual;
    }
}