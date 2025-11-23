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
    
    [Header("Ajuste de Escala")]
    public float multiplicadorAltura = 1.5f;
    public float offsetVertical = 0f; // NOVO - Ajusta a posição Y
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (backgrounds.Count > 0)
        {
            spriteRenderer.sprite = backgrounds[0];
        }
        
        AjustarTamanhoBackground();
        baseSize = spriteRenderer.size;
        
        CentralizarBackground();
        
        if (playerSpeedController == null && usarVelocidadeDoPlayer)
        {
            playerSpeedController = FindObjectOfType<PlayerSpeedController>();
        }
        
        if (wallTransition == null)
        {
            wallTransition = FindObjectOfType<WallTransition>();
        }
        
        if (wallTransition != null)
        {
            wallTransition.OnCobriuTela += TrocarBackground;
        }
    }
    
    void CentralizarBackground()
    {
        Camera cam = Camera.main;
        Vector3 posicao = transform.position;
        posicao.y = cam.transform.position.y + offsetVertical; // MODIFICADO
        posicao.z = 0;
        transform.position = posicao;
    }
    
    void AjustarTamanhoBackground()
    {
        Camera cam = Camera.main;
        
        // Calcula o tamanho exato da câmera em world units
        float alturaCamera = cam.orthographicSize * 2f;
        float larguraCamera = alturaCamera * cam.aspect;
        
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null) return;
        
        // Calcula proporção da sprite original
        float proporcaoSprite = sprite.bounds.size.x / sprite.bounds.size.y;
        
        // SOLUÇÃO DEFINITIVA: Sempre usa a altura da câmera multiplicada
        // e ajusta a largura proporcionalmente
        float alturaFinal = alturaCamera * multiplicadorAltura;
        float larguraFinal = alturaFinal * proporcaoSprite;
        
        // Garante que também cobre a largura
        if (larguraFinal < larguraCamera)
        {
            larguraFinal = larguraCamera * 1.2f;
            alturaFinal = larguraFinal / proporcaoSprite;
        }
        
        spriteRenderer.size = new Vector2(larguraFinal, alturaFinal);
        
        Debug.Log($"Background ajustado: {larguraFinal} x {alturaFinal} | Câmera: {larguraCamera} x {alturaCamera}");
    }
    
    void Update()
    {
        float currentSpeed = speedToSizeMultiplier * (baseSpeed + GameManager.Instance.speedBase) * GameManager.Instance.speedMultiplier;
        
        if (usarVelocidadeDoPlayer && playerSpeedController != null)
        {
            float playerSpeed = playerSpeedController.GetVelocidadeAtual();
            currentSpeed *= playerSpeed;
        }
        
        spriteRenderer.size = new Vector2(spriteRenderer.size.x + currentSpeed, baseSize.y);
        
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
        
        AjustarTamanhoBackground();
        baseSize = spriteRenderer.size;
        CentralizarBackground();
        
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