using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    private bool areItemsSpawned = false;

    public static EnemySpawner main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject playerPrefab;
    [Tooltip("Set this to the game object displaying the current wave")] [SerializeField] private TMP_Text waveText;


    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 20;
    [SerializeField] private float enemySpawnRate = 0.5f;
    [SerializeField] private float difficultyScaling = 0.5f;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float defaultCoinPerWave = 5;
    [SerializeField] private LayerMask _groundLayer;


    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private bool _newRecord = false;
    private string _defaultWaveText = "Wave: ";
    private int enemiesAlive;
    public int currentWave = 1;
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

    private void Start()
    {
        main.StartWave();
    }

    private void Update()
    {
        spawnedItems = LevelManager.main.GetComponent<Item_Grabber>().itemsSpawned;

        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemySpawnRate) && enemiesLeftToSpawn > 0)
        {
            timeSinceLastSpawn = 0f;
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    public void EnemyDestroyed() 
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

    [ContextMenu("End Wave")]
    private void EndWave()
    {
        currentWave++;
        enemyPrefabIndex = 0;
        if (currentWave > SaveDataController.Instance.current.waveRecorded || _newRecord)
        {
            SaveDataController.Instance.current.waveRecorded = currentWave;
            _newRecord = true;
            waveText.text = $"<color=#FF0000>W</color><color=#FF7600>a</color><color=#FFE800>v</color><color=#00FF00>e</color><color=#0000FF>:</color> <color=#FF0000>{currentWave.ToString()}</color>";
        }
        else
        {
            waveText.text = _defaultWaveText + currentWave.ToString() + "</color>"; 
        }
        main.isSpawning = false;
        SaveDataController.Instance.current.coins += Mathf.RoundToInt(defaultCoinPerWave + (1.3f * ((float) currentWave - 1f)));
        LevelManager.main.gameObject.GetComponent<Item_Grabber>().Trigger();
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(main.baseEnemies * Mathf.Pow(main.currentWave, main.difficultyScaling));
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabIndex <= 2) {
            enemyPrefabIndex++;
            prefabtoSpawn = main.enemyPrefabs[0];
        }
        else if (enemyPrefabIndex <= 5)
        {
            enemyPrefabIndex++;
            //add rTFCount if statement later
            prefabtoSpawn = main.enemyPrefabs[1];
        }
        else
        {
            enemyPrefabIndex = 0;
            prefabtoSpawn = main.enemyPrefabs[2];
            timeSinceLastSpawn = -10f;
        }

        float angle = Random.Range(-180f, 180f);
        Vector2 position = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * spawnDistance;

        newEnemy = Instantiate(prefabtoSpawn, position + (Vector2)Camera.main.transform.position, Quaternion.identity);
        Vector2 boxCastSize = new Vector2(2f, 2f);

        RaycastHit2D check = Physics2D.BoxCast(newEnemy.transform.position, boxCastSize, 0f, Vector2.zero, _groundLayer);
        if (check) 
        { 
            newEnemy.transform.position = new Vector3(0f, 0f, 0f);
            
        }
        aliveEnemies.Add(newEnemy);
    }
}
