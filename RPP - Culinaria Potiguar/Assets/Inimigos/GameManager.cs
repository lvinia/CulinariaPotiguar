using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acesso fácil
    
    [Header("Obstáculos Voadores")]
    public GameObject[] flyingObstacles;
    public GameObject[] flyingSpawnPoints;
    private float flyingTimer;
    public float flyingSpawnInicial = 3f;      // Tempo inicial entre spawns
    public float flyingSpawnMinimo = 0.8f;     // Tempo mínimo (velocidade máxima)
    public float flyingTempoParaMaximo = 60f;  // Tempo para atingir velocidade máxima
    
    [Header("Obstáculos Terrestres")]
    public GameObject[] groundObstacles;
    public GameObject[] groundSpawnPoints;
    private float groundTimer;
    public float groundSpawnInicial = 2f;
    public float groundSpawnMinimo = 0.5f;
    public float groundTempoParaMaximo = 60f;
    
    [Header("Velocidade do Jogo")]
    public float speedBase = 5f;
    public float speedMultiplier = 1f;
    public float speedIncreaseRate = 0.1f;
    
    [Header("UI")]
    public Text distanceUI;
    private float distance;
    
    private float tempoDecorrido;
    private float timeBetweenFlyingSpawns;
    private float timeBetweenGroundSpawns;
    
    void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        // Inicializa com os valores iniciais
        timeBetweenFlyingSpawns = flyingSpawnInicial;
        timeBetweenGroundSpawns = groundSpawnInicial;
    }
    
    void Update()
    {
        distanceUI.text = "Distance: " + distance.ToString("F2");
        
        // Atualiza o tempo decorrido
        tempoDecorrido += Time.deltaTime;
        
        // Calcula a aceleração gradual dos spawns
        float progressoFlying = Mathf.Clamp01(tempoDecorrido / flyingTempoParaMaximo);
        timeBetweenFlyingSpawns = Mathf.Lerp(flyingSpawnInicial, flyingSpawnMinimo, progressoFlying);
        
        float progressoGround = Mathf.Clamp01(tempoDecorrido / groundTempoParaMaximo);
        timeBetweenGroundSpawns = Mathf.Lerp(groundSpawnInicial, groundSpawnMinimo, progressoGround);
        
        // Aumentar velocidade gradualmente
        speedMultiplier += Time.deltaTime * speedIncreaseRate;
        distance += Time.deltaTime * 0.8f;
        
        // Timer dos voadores
        flyingTimer += Time.deltaTime;
        if (flyingTimer > timeBetweenFlyingSpawns)
        {
            flyingTimer = 0;
            int randPoint = Random.Range(0, flyingSpawnPoints.Length);
            int randObstacle = Random.Range(0, flyingObstacles.Length);
            Instantiate(flyingObstacles[randObstacle], flyingSpawnPoints[randPoint].transform.position, Quaternion.identity);
        }
        
        // Timer dos terrestres
        groundTimer += Time.deltaTime;
        if (groundTimer > timeBetweenGroundSpawns)
        {
            groundTimer = 0;
            int randPoint = Random.Range(0, groundSpawnPoints.Length);
            int randObstacle = Random.Range(0, groundObstacles.Length);
            Instantiate(groundObstacles[randObstacle], groundSpawnPoints[randPoint].transform.position, Quaternion.identity);
        }
    }
}