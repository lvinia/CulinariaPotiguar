using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acesso fácil

    public GameObject[] flyingObstacles;
    public GameObject[] flyingSpawnPoints;
    public float flyingTimer;
    public float timeBetweenFlyingSpawns = 3f;

    public GameObject[] groundObstacles;
    public GameObject[] groundSpawnPoints;
    public float groundTimer;
    public float timeBetweenGroundSpawns = 2f;

    public float speedMultiplier = 1f; // Valor inicial importante!
    public float speedIncreaseRate = 0.1f; // Taxa de aumento da velocidade

    public Text distanceUI;
    private float distance;

    void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        distanceUI.text = "Distance: " + distance.ToString("F2");
        
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