using UnityEngine;

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

    private int enemiesAlive;
    private int currentWave = 1;
    private int enemiesLeftToSpawn;
    private float timeSinceLastSpawn;
    private bool isSpawning = false;
    private bool spawnedItems = false;
    private GameObject newEnemy;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        spawnedItems = LevelManager.main.GetComponent<Item_Grabber>().itemsSpawned;

        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemySpawnRate) && enemiesLeftToSpawn > 0)
        {
            //SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
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

    //private void SpawnEnemy()
    //{
        //GameObject prefabtoSpawn = main.enemyPrefabs[0];
        //newEnemy = Instantiate(prefabtoSpawn, WIP, Quaternion.identity);
    //}
}
