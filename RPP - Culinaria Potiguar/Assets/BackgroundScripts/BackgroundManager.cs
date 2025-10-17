using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [Header("Configurações de Background")]
    [SerializeField] private GameObject[] backgroundPrefabs; // Suas 6 fases
    [SerializeField] private GameObject muroPrefab; // Prefab do muro especial
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float spawnPositionX = 20f; // Posição inicial de spawn
    
    [Header("Configurações de Pool")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private int maxPoolSize = 15;
    
    [Header("Configurações de Muro")]
    [SerializeField] private int backgroundsBeforeWall = 3; // Quantos backgrounds antes de spawnar um muro
    
    private Camera mainCamera;
    private ObjectPool<GameObject> backgroundPool;
    private ObjectPool<GameObject> wallPool;
    
    private float nextSpawnPositionX;
    private float lastBackgroundWidth;
    private int backgroundCounter = 0;
    private List<GameObject> activeBackgrounds = new List<GameObject>();
    
    private void Awake()
    {
        mainCamera = Camera.main;
        
        // Inicializa o pool de backgrounds
        backgroundPool = new ObjectPool<GameObject>(
            createFunc: CreateBackground,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: poolSize,
            maxSize: maxPoolSize
        );
        
        // Inicializa o pool de muros
        wallPool = new ObjectPool<GameObject>(
            createFunc: CreateWall,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 10
        );
        
        // Calcula posição inicial
        CalculateInitialSpawnPosition();
    }
    
    private void Start()
    {
        // Spawna os primeiros backgrounds para preencher a tela
        SpawnInitialBackgrounds();
    }
    
    private void Update()
    {
        // Verifica se precisa spawnar novo background
        if (nextSpawnPositionX < mainCamera.transform.position.x + GetCameraWidth())
        {
            SpawnNextBackground();
        }
    }
    
    private void CalculateInitialSpawnPosition()
    {
        float cameraWidth = GetCameraWidth();
        nextSpawnPositionX = mainCamera.transform.position.x + (cameraWidth / 2f);
    }
    
    private void SpawnInitialBackgrounds()
    {
        // Spawna backgrounds suficientes para cobrir a tela inteira + área extra
        float cameraWidth = GetCameraWidth();
        float totalWidth = cameraWidth * 1.5f;
        float currentX = nextSpawnPositionX;
        
        while (currentX < mainCamera.transform.position.x + totalWidth)
        {
            SpawnNextBackground();
            currentX = nextSpawnPositionX;
        }
    }
    
    private void SpawnNextBackground()
    {
        backgroundCounter++;
        
        GameObject bg;
        
        // Decide se spawna um muro ou um background normal
        if (backgroundCounter % backgroundsBeforeWall == 0)
        {
            bg = wallPool.Get();
        }
        else
        {
            bg = backgroundPool.Get();
        }
        
        // Posiciona o background
        SpriteRenderer spriteRenderer = bg.GetComponent<SpriteRenderer>();
        float bgWidth = spriteRenderer.bounds.size.x;
        
        bg.transform.position = new Vector3(nextSpawnPositionX + (bgWidth / 2f), mainCamera.transform.position.y, 0f);
        
        // Atualiza a próxima posição de spawn
        nextSpawnPositionX += bgWidth;
        lastBackgroundWidth = bgWidth;
        
        activeBackgrounds.Add(bg);
    }
    
    public void ReturnBackgroundToPool(GameObject bg)
    {
        activeBackgrounds.Remove(bg);
        
        // Verifica se é um muro ou background normal
        if (bg.CompareTag("Wall") || bg.name.Contains("Muro"))
        {
            wallPool.Release(bg);
        }
        else
        {
            backgroundPool.Release(bg);
        }
    }
    
    // Métodos do Object Pool para Backgrounds
    private GameObject CreateBackground()
    {
        // Seleciona um prefab aleatório das 6 fases
        GameObject prefab = backgroundPrefabs[Random.Range(0, backgroundPrefabs.Length)];
        GameObject bg = Instantiate(prefab);
        
        // Adiciona o script de scroll
        BackgroundScroller scroller = bg.GetComponent<BackgroundScroller>();
        if (scroller == null)
            scroller = bg.AddComponent<BackgroundScroller>();
        
        scroller.Initialize(this, scrollSpeed);
        scroller.StretchToFitCamera();
        
        return bg;
    }
    
    private GameObject CreateWall()
    {
        GameObject wall = Instantiate(muroPrefab);
        
        // Adiciona o script de scroll
        BackgroundScroller scroller = wall.GetComponent<BackgroundScroller>();
        if (scroller == null)
            scroller = wall.AddComponent<BackgroundScroller>();
        
        scroller.Initialize(this, scrollSpeed);
        scroller.StretchToFitCamera();
        
        // Marca como muro para identificação
        if (!wall.CompareTag("Wall"))
            wall.tag = "Wall";
        
        return wall;
    }
    
    private void OnGetFromPool(GameObject bg)
    {
        bg.SetActive(true);
    }
    
    private void OnReleaseToPool(GameObject bg)
    {
        bg.SetActive(false);
    }
    
    private void OnDestroyPoolObject(GameObject bg)
    {
        Destroy(bg);
    }
    
    private float GetCameraWidth()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        return cameraHeight * mainCamera.aspect;
    }
    
    // Método público para alterar velocidade em runtime
    public void SetScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
        
        // Atualiza velocidade de todos backgrounds ativos
        foreach (GameObject bg in activeBackgrounds)
        {
            BackgroundScroller scroller = bg.GetComponent<BackgroundScroller>();
            if (scroller != null)
                scroller.Initialize(this, scrollSpeed);
        }
    }
    
    private void OnDestroy()
    {
        backgroundPool?.Dispose();
        wallPool?.Dispose();
    }
}