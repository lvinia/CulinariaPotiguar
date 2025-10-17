using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float scrollSpeed = 5f;
    
    [Header("Referências")]
    [SerializeField] private Camera mainCamera;
    
    private float destroyPositionX;
    private BackgroundManager manager;
    
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        // Calcula a posição X onde o background deve ser destruído/retornado ao pool
        // Usa a borda esquerda da câmera menos uma margem
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        destroyPositionX = mainCamera.transform.position.x - (cameraWidth / 2f) - 5f;
    }
    
    public void Initialize(BackgroundManager bgManager, float speed)
    {
        manager = bgManager;
        scrollSpeed = speed;
    }
    
    private void Update()
    {
        // Move o background para a esquerda
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        
        // Verifica se saiu da tela
        if (transform.position.x < destroyPositionX)
        {
            if (manager != null)
            {
                manager.ReturnBackgroundToPool(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    // Método para ajustar a escala do sprite mantendo proporções
    public void StretchToFitCamera()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;
        
        // Calcula o tamanho da câmera em unidades do mundo
        float cameraHeight = 2f * mainCamera.orthographicSize;
        
        // Pega o tamanho original do sprite
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        
        // Calcula a escala necessária para cobrir a altura da câmera
        float scaleY = cameraHeight / spriteHeight;
        
        // Aplica a mesma escala em X e Y para manter proporções
        transform.localScale = new Vector3(scaleY, scaleY, 1f);
    }
}