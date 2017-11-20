using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] float timeBetweenSpawns = 5f;

    [SerializeField] Enemy enemyToSpawn;

    Area area;
    
    float currentTimeBetweenSpawns;

    void Start()
    {
        area = FindObjectOfType<Area>();
        currentTimeBetweenSpawns = timeBetweenSpawns;

        for (int i = 0; i < 250; i++)
        {
            SpawnEnemyOnNetwork();
        }
    }

    void Update()
    {
        if (!isServer)
            return;

        currentTimeBetweenSpawns += Time.deltaTime;
        if (currentTimeBetweenSpawns > timeBetweenSpawns)
        {
            currentTimeBetweenSpawns = 0f;
            SpawnEnemyOnNetwork();
        }
    }

    void SpawnEnemyOnNetwork()
    {
        Vector3 positionToSpawn = FindFreePosition();
        GameObject newEnemy = Instantiate(enemyToSpawn.gameObject) as GameObject;
        newEnemy.transform.position = positionToSpawn;
        NetworkServer.Spawn(newEnemy);
    }

    Vector3 FindFreePosition()
    {
        float currentSizeOfArea = area.transform.localScale.x;
        int x = Random.Range((int)(-currentSizeOfArea * 0.4f), (int)(currentSizeOfArea * 0.4f));
        int z = Random.Range((int)(-currentSizeOfArea * 0.4f), (int)(currentSizeOfArea * 0.4f));

        return new Vector3(x, 0f, z);
    }
}
