using System;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    public bool isInitialized;

    [SerializeField] float timeBetweenSpawns = 5f;

    [SerializeField] Enemy enemyToSpawn;
    [SerializeField] Enemy bossEnemy;
    [SerializeField] WavesConfig wavesConfig;

    Area area;

    float currentTimeBetweenSpawns;

    void Start()
    {
        isInitialized = false;
        area = FindObjectOfType<Area>();
        currentTimeBetweenSpawns = timeBetweenSpawns;
    }

    void Update()
    {
        if (!isServer || !isInitialized)
            return;

        currentTimeBetweenSpawns += Time.deltaTime;
        //if (currentTimeBetweenSpawns > timeBetweenSpawns)
        //{
        //    currentTimeBetweenSpawns = 0f;
        //    SpawnEnemyOnNetwork(enemyToSpawn.gameObject, FindFreePosition());
        //}
    }

    void SpawnEnemyOnNetwork(GameObject enemy, Vector3 positionToSpawn, WavesConfig.Wave wave = null)
    {
        GameObject newEnemy = Instantiate(enemy) as GameObject;
        newEnemy.transform.position = positionToSpawn;

        if (wave != null)
        {
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.speed = wave.speed;
            enemyComponent.timeBetweenShoots = wave.rateOfFire;
            newEnemy.GetComponent<Weapon>().SetBulletDamage(wave.damage);
        }

        NetworkServer.Spawn(newEnemy);
    }

    Vector3 FindFreePosition()
    {
        float currentSizeOfArea = area.transform.localScale.x;
        int x = UnityEngine.Random.Range((int)(-currentSizeOfArea * 0.4f), (int)(currentSizeOfArea * 0.4f));
        int z = UnityEngine.Random.Range((int)(-currentSizeOfArea * 0.4f), (int)(currentSizeOfArea * 0.4f));

        return new Vector3(x, 0f, z);
    }

    public void DeleteAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void SpawnWave(int level)
    {
        WavesConfig.Wave wave = wavesConfig.waves[Mathf.Clamp(level, 0, wavesConfig.waves.Length)];
        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            SpawnEnemyOnNetwork(enemyToSpawn.gameObject, FindFreePosition(), wave);
        }
        if (bossEnemy)
            SpawnEnemyOnNetwork(bossEnemy.gameObject, Vector3.zero);
    }
}
