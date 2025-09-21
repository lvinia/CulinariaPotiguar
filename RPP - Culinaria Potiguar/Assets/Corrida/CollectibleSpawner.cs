using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] fragments; // Fragmentos
    public GameObject[] powers;    // Poderes colecionáveis

    public Transform[] fragmentSpawnPoints;
    public Transform[] powerSpawnPoints;

    private float fragmentTimer;
    public float timeBetweenFragmentSpawns = 8f;

    private float powerTimer;
    public float timeBetweenPowerSpawns = 12f;

    void Update()
    {
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