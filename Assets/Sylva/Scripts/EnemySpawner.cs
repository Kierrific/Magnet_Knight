using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private bool areItemsSpawned = false;

    public static EnemySpawner main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject playerPrefab;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 20;
    [SerializeField] private float enemySpawnRate = 0.5f;
    [SerializeField] private float difficultyScaling = 0.5f;
    [SerializeField] private float spawnDistance;

    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int enemiesAlive;
    private int currentWave = 1;
    private int enemiesLeftToSpawn;
    private float timeSinceLastSpawn;
    private bool isSpawning = false;
    private bool spawnedItems = false;
    private int enemyPrefabIndex = 0;
    //private int rTFCount = 0;
    private GameObject newEnemy;
    private GameObject prefabtoSpawn;

    private List<GameObject> aliveEnemies = new();

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Update()
    {
        spawnedItems = LevelManager.main.GetComponent<Item_Grabber>().itemsSpawned;

        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemySpawnRate) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    public void StartWave()
    {
        if (!spawnedItems && !isSpawning)
        {
            main.isSpawning = true;
            main.enemiesLeftToSpawn = main.EnemiesPerWave();
        }
    }

    private void EndWave()
    {
        main.isSpawning = false;
        LevelManager.main.gameObject.GetComponent<Item_Grabber>().Trigger();
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(main.baseEnemies * Mathf.Pow(main.currentWave, main.difficultyScaling));
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabIndex == 0) {
            enemyPrefabIndex++;
            prefabtoSpawn = main.enemyPrefabs[0];
        }
        else if (enemyPrefabIndex == 1)
        {
            enemyPrefabIndex--;
            //add rTFCount if statement later
            prefabtoSpawn = main.enemyPrefabs[1];
        }

        float angle = Random.Range(-180f, 180f);
        Vector2 position = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * spawnDistance;

        newEnemy = Instantiate(prefabtoSpawn, position + (Vector2)Camera.main.transform.position, Quaternion.identity);
        aliveEnemies.Add(newEnemy);
    }
}
