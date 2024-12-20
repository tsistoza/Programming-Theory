using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

/// <summary>
/// Spawn Manager is in charge of spawning objects (Enemies, Powerups)
/// </summary>
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    // Modifiers
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private float spawnRange = 9;

    // Components
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> powerUps = new List<GameObject>();
    [SerializeField] private GameObject powerUpSpawn;
    [SerializeField] private List<GameObject> spawnedPowerUps = new List<GameObject>();

    // Variables
    private int enemyCount;
    public int EnemyCount
    {
        get { return enemyCount; }
        set { enemyCount = value; }
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

    private void Update()
    {
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
    }


    public void SpawnEnemyWave()
    {
        //@TODO Randomize enemies so that it doesnt spawn the same slimes over and over
        if (enemyCount <= 0 && !GameManager.Instance.PickedUpPowerUp)
        {
            // Wait for user player pickup then spawn
            for (int i = 0; i < (int)(GameManager.Instance.WaveNumber * spawnRate) ; i++)
            {
                (int index, GameObject enemyPrefab) = GetRandomEnemy();
                Instantiate(enemyPrefab, GetSpawnPos(), enemyPrefab.transform.rotation);
            }
        }
    }

    private (int index, GameObject enemy) GetRandomEnemy()
    {
        int index = Random.Range(0, enemies.Count);
        return (index, enemies[index]);
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

    public void SpawnPowerUps()
    {
        for (int i=0; i<3; i++)
        {
            GameObject clone = Instantiate(powerUps[Random.Range(0, powerUps.Count-1)],
                                           powerUpSpawn.transform.position + Vector3.right * i * 10,
                                           powerUps[i].transform.rotation);
            spawnedPowerUps.Add(clone);
        }
        GameManager.Instance.spawnedPowerUps = true;
    }

    public void DestroyPowerUps()
    {
        foreach(GameObject powerUp in spawnedPowerUps)
        {
            Destroy(powerUp);
        }
    }
}
