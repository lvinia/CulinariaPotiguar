using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Fragmentos")]
    public GameObject[] fragmentPrefabs; // Array com os 6 prefabs diferentes de fragmentos
    public Transform[] fragmentSpawnPoints;
    
    [Header("Configurações de Aceleração")]
    public float fragmentSpawnInicial = 8f; // Tempo inicial entre spawns
    public float fragmentSpawnMinimo = 2f;  // Tempo mínimo (velocidade máxima)
    public float fragmentTempoParaMaximo = 60f; // Tempo em segundos para atingir velocidade máxima
    
    private float fragmentTimer;
    private float tempoDecorrido;
    private float timeBetweenFragmentSpawns;
    
    void Start()
    {
        ResetSpawner();
    }
    
    void OnEnable()
    {
        // Reseta quando o objeto é reativado
        ResetSpawner();
    }
    
    void Update()
    {
        // Atualiza o tempo decorrido
        tempoDecorrido += Time.deltaTime;
        
        // Calcula a aceleração gradual
        float progressoFragment = Mathf.Clamp01(tempoDecorrido / fragmentTempoParaMaximo);
        timeBetweenFragmentSpawns = Mathf.Lerp(fragmentSpawnInicial, fragmentSpawnMinimo, progressoFragment);
        
        // Spawn dos fragmentos
        fragmentTimer += Time.deltaTime;
        if (fragmentTimer > timeBetweenFragmentSpawns)
        {
            fragmentTimer = 0;
            SpawnFragment();
        }
    }
    
    void SpawnFragment()
    {
        // Escolhe um ponto de spawn aleatório
        int randPoint = Random.Range(0, fragmentSpawnPoints.Length);
        
        // Escolhe um prefab aleatório entre os 6
        int randFragment = Random.Range(0, fragmentPrefabs.Length);
        
        // Instancia o fragmento
        Instantiate(fragmentPrefabs[randFragment], fragmentSpawnPoints[randPoint].position, Quaternion.identity);
    }
    
    public void ResetSpawner()
    {
        // Reseta todos os valores para o estado inicial
        timeBetweenFragmentSpawns = fragmentSpawnInicial;
        fragmentTimer = 0;
        tempoDecorrido = 0;
        
        // Verifica se tem 6 prefabs
        if (fragmentPrefabs.Length != 6)
        {
            Debug.LogWarning("Atenção: Você deve adicionar exatamente 6 prefabs de fragmentos!");
        }
    }
}