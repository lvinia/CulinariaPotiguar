using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public List<Sprite> backgrounds;
    
    private SpriteRenderer spriteRenderer;
    public float baseSpeed = 0f;
    public float speedToSizeMultiplier = 1f;
    private Vector2 baseSize;
    
    [Header("Sincronização com Player")]
    public PlayerSpeedController playerSpeedController;
    public bool usarVelocidadeDoPlayer = true;
    
    [Header("Troca de Backgrounds")]
    public float tempoParaTrocar = 30f;
    private int indiceAtual = 0;
    private float tempoDecorrido = 0f;
    
    [Header("Transição com Muro")]
    public WallTransition wallTransition;
    private bool aguardandoTroca = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSize = spriteRenderer.size;
        
        // Define o primeiro background
        if (backgrounds.Count > 0)
        {
            spriteRenderer.sprite = backgrounds[0];
        }
        
        // Procura o player
        if (playerSpeedController == null && usarVelocidadeDoPlayer)
        {
            playerSpeedController = FindObjectOfType<PlayerSpeedController>();
        }
        
        // Procura o muro
        if (wallTransition == null)
        {
            wallTransition = FindObjectOfType<WallTransition>();
        }
        
        // Inscreve no evento do muro
        if (wallTransition != null)
        {
            wallTransition.OnCobriuTela += TrocarBackground;
        }
    }
    
    void Update()
    {
        // Movimento do background (como estava antes)
        float currentSpeed = speedToSizeMultiplier * (baseSpeed + GameManager.Instance.speedBase) * GameManager.Instance.speedMultiplier;
        
        if (usarVelocidadeDoPlayer && playerSpeedController != null)
        {
            float playerSpeed = playerSpeedController.GetVelocidadeAtual();
            currentSpeed *= playerSpeed;
        }
        
        spriteRenderer.size = new Vector2(spriteRenderer.size.x + currentSpeed, baseSize.y);
        
        // Controle de tempo para ativar o muro
        tempoDecorrido += Time.deltaTime;
        
        if (tempoDecorrido >= tempoParaTrocar && !aguardandoTroca && backgrounds.Count > 0)
        {
            if (wallTransition != null)
            {
                aguardandoTroca = true;
                wallTransition.AtivarMuro();
            }
            else
            {
                // Se não tem muro, troca direto
                TrocarBackground();
            }
        }
    }
    
    private void TrocarBackground()
    {
        if (backgrounds.Count == 0) return;
        
        indiceAtual++;
        
        if (indiceAtual >= backgrounds.Count)
        {
            indiceAtual = 0;
        }
        
        spriteRenderer.sprite = backgrounds[indiceAtual];
        
        tempoDecorrido = 0f;
        aguardandoTroca = false;
    }
    
    void OnDestroy()
    {
        if (wallTransition != null)
        {
            wallTransition.OnCobriuTela -= TrocarBackground;
        }
    }
}