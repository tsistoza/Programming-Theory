using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    // Modifiers
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRange;

    // Components
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    // Variables
    private int enemyCount;
    public int EnemyCount
    {
        get { return enemyCount; }
        private set { enemyCount = value; }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemyWave();
    }
    private void SpawnEnemyWave()
    {
        //@TODO Randomize enemies so that it doesnt spawn the same slimes over and over
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
        if (enemyCount <= 0)
        {
            // Wait for user player pickup then spawn
            for (int i = 0; i < 5; i++)
            {
                GameObject enemyPrefab = GetRandomEnemy();
                Instantiate(enemyPrefab, GetSpawnPos(), enemyPrefab.transform.rotation);
            }
            /*for (int i = 0; i < (int)(GameManager.Instance.WaveNumber * spawnRate); i++)
            {
                Instantiate(enemyPrefab, GetSpawnPos(), enemyPrefab.transform.rotation);
            }*/
        }
    }

    private GameObject GetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Count-1)];
    }

    /// <summary>
    /// Generate Random Position around the arena, not near player
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = GameObject.Find("Player").transform.position;
        while (!PlayerIsOutOfRange(spawnPos))
        {
            float xUpper = GameObject.Find("XUpperBound").transform.position.x;
            float xLower = GameObject.Find("XLowerBound").transform.position.x;
            float zUpper = GameObject.Find("ZUpperBound").transform.position.z;
            float zLower = GameObject.Find("ZLowerBound").transform.position.z;
            spawnPos = new Vector3(Random.Range(xLower, xUpper),
                            -155,
                            Random.Range(zLower, zUpper));
        }
        return spawnPos;
    }

    public bool PlayerIsOutOfRange(Vector3 spawnPos)
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        // Check if spawnPos is within the spawnRange from the player
        if (spawnPos.x < playerPos.x - spawnRange || spawnPos.x > playerPos.x + spawnRange)
        {
            return true;
            
        }
        if (spawnPos.z < playerPos.z - spawnRange || spawnPos.z > playerPos.z + spawnRange)
        {
            return true;
        }
        return false;
    }
}
