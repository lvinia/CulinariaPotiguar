using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Configurações de Rolagem")]
    public float scrollSpeed = 5.0f; // A mesma velocidade de rolagem do seu jogo (em unidades/segundo)

    // Se você estiver usando um SpriteRenderer, use este.
    private Renderer backgroundRenderer;

    // A posição X do offset da textura.
    private float currentOffset = 0f;

    private void Awake()
    {
        backgroundRenderer = GetComponent<Renderer>();
        if (backgroundRenderer == null)
        {
            Debug.LogError("O componente Renderer (SpriteRenderer, MeshRenderer, etc.) é obrigatório neste objeto.");
            enabled = false;
        }

        // Importante: Para rolagem suave do material, você deve usar um material
        // com um shader que suporte o deslocamento de textura (como Unlit/Texture).
        // Se estiver usando o shader padrão Sprite/Default, talvez precise criar 
        // um Material customizado para que a mudança do offset funcione.
    }

    private void Update()
    {
        // 1. Calcula o novo offset baseado no tempo e velocidade
        // O valor do offset vai de 0 a 1 (representando 0% a 100% da textura)
        currentOffset += scrollSpeed * Time.deltaTime;
        
        // 2. Mantém o offset no intervalo de [0, 1] para criar o loop
        // % (mod) garante que a rolagem volte ao início da textura (loop infinito)
        if (currentOffset > 1.0f)
        {
            currentOffset -= 1.0f; 
        }
        
        // 3. Aplica o offset ao material para rolar a textura
        // '_MainTex' é o nome padrão para a textura principal na maioria dos shaders
        backgroundRenderer.material.SetTextureOffset("_MainTex", new Vector2(currentOffset, 0));
    }
}