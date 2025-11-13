using UnityEngine;
using System;

public class WallTransition : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeMovimento = 10f; // Velocidade que o muro atravessa a tela
    
    [Header("Posições")]
    public float posicaoInicialX = 19.71f; // Posição inicial do muro (fora da tela à direita)
    public float posicaoFinalX = -19.71f; // Posição final (fora da tela à esquerda)
    
    private bool estaAtivo = false;
    private bool jaTrocouBackground = false;
    
    // Evento que será chamado quando o muro cobrir a tela
    public event Action OnCobriuTela;
    
    void Start()
    {
        // Começa invisível/desativado fora da tela
        transform.position = new Vector3(posicaoInicialX, 0.21f, 0f);
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (estaAtivo)
        {
            // Move o muro da direita para a esquerda
            transform.position += Vector3.left * velocidadeMovimento * Time.deltaTime;
            
            // Verifica se está no centro da tela (cobrindo tudo) e ainda não trocou
            if (!jaTrocouBackground && transform.position.x <= 0f)
            {
                jaTrocouBackground = true;
                OnCobriuTela?.Invoke(); // Notifica que está cobrindo a tela
                Debug.Log("Muro cobriu a tela! Trocando background...");
            }
            
            // Se saiu da tela pela esquerda, desativa e reseta
            if (transform.position.x <= posicaoFinalX)
            {
                DesativarMuro();
            }
        }
    }
    
    // Método para ativar o muro e começar a animação
    public void AtivarMuro()
    {
        gameObject.SetActive(true);
        estaAtivo = true;
        jaTrocouBackground = false;
        transform.position = new Vector3(posicaoInicialX, 0.21f, 0f);
        Debug.Log("Muro ativado! Iniciando transição...");
    }
    
    // Método para desativar o muro
    private void DesativarMuro()
    {
        estaAtivo = false;
        gameObject.SetActive(false);
        Debug.Log("Muro desativado! Transição completa.");
    }
}