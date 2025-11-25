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
    private bool isSpawning = false;
    private bool spawnedItems = false;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        spawnedItems = LevelManager.main.GetComponent<Item_Grabber>().itemsSpawned;
    }

}
