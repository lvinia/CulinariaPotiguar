using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] fragments; // Fragmentos
    public GameObject[] powers;    // Poderes colecionáveis

    public Transform[] fragmentSpawnPoints;
    public Transform[] powerSpawnPoints;

    [Header("Configurações de Aceleração - Fragmentos")]
    public float fragmentSpawnInicial = 8f; // Tempo inicial entre spawns
    public float fragmentSpawnMinimo = 2f;  // Tempo mínimo (velocidade máxima)
    public float fragmentTempoParaMaximo = 60f; // Tempo em segundos para atingir velocidade máxima

    [Header("Configurações de Aceleração - Poderes")]
    public float powerSpawnInicial = 12f;
    public float powerSpawnMinimo = 4f;
    public float powerTempoParaMaximo = 60f;

    private float fragmentTimer;
    private float powerTimer;
    private float tempoDecorrido;

    private float timeBetweenFragmentSpawns;
    private float timeBetweenPowerSpawns;

    void Start()
    {
        // Inicializa com os valores iniciais
        timeBetweenFragmentSpawns = fragmentSpawnInicial;
        timeBetweenPowerSpawns = powerSpawnInicial;
    }

    void Update()
    {
        // Atualiza o tempo decorrido
        tempoDecorrido += Time.deltaTime;

        // Calcula a aceleração gradual
        float progressoFragment = Mathf.Clamp01(tempoDecorrido / fragmentTempoParaMaximo);
        timeBetweenFragmentSpawns = Mathf.Lerp(fragmentSpawnInicial, fragmentSpawnMinimo, progressoFragment);

        float progressoPower = Mathf.Clamp01(tempoDecorrido / powerTempoParaMaximo);
        timeBetweenPowerSpawns = Mathf.Lerp(powerSpawnInicial, powerSpawnMinimo, progressoPower);

        // Spawn dos fragmentos
        fragmentTimer += Time.deltaTime;
        if (fragmentTimer > timeBetweenFragmentSpawns)
        {
            fragmentTimer = 0;
            int randPoint = Random.Range(0, fragmentSpawnPoints.Length);
            int randFragment = Random.Range(0, fragments.Length);
            Instantiate(fragments[randFragment], fragmentSpawnPoints[randPoint].position, Quaternion.identity);
        }

        // Spawn dos poderes
        powerTimer += Time.deltaTime;
        if (powerTimer > timeBetweenPowerSpawns)
        {
            powerTimer = 0;
            int randPoint = Random.Range(0, powerSpawnPoints.Length);
            int randPower = Random.Range(0, powers.Length);
            Instantiate(powers[randPower], powerSpawnPoints[randPoint].position, Quaternion.identity);
        }
    }
}