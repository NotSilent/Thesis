using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] int maxNumberOfEnemies = 5;
    [SerializeField] float timeBetweenSpawns = 5f;

    [SerializeField] Enemy enemyToSpawn;

    int currentNumberOfEnemies;
    float currentTimeBetweenSpawns;
    
    void Start()
    {
        currentNumberOfEnemies = 0;
        currentTimeBetweenSpawns = timeBetweenSpawns;
    }

    void Update()
    {
        if (!isServer)
            return;

        currentTimeBetweenSpawns += Time.deltaTime;
        if (currentTimeBetweenSpawns > timeBetweenSpawns && currentNumberOfEnemies < maxNumberOfEnemies)
        {
            currentNumberOfEnemies++;
            currentTimeBetweenSpawns = 0f;
            SpawnEnemyOnNetwork();
        }
    }

    void SpawnEnemyOnNetwork()
    {
        GameObject newEnemy = Instantiate(enemyToSpawn.gameObject) as GameObject;
        NetworkServer.Spawn(newEnemy);
    }
}
