using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    // Modifiers
    [SerializeField] private float spawnRate;

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
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
        SpawnEnemyWave(GetRandomEnemy());
    }
    private void SpawnEnemyWave(GameObject enemyPrefab)
    {
        //@TODO Randomize enemies so that it doesnt spawn the same slimes over and over
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Count();
        if (enemyCount <= 0)
        {
            // Wait for user player pickup then spawn
            Instantiate(enemyPrefab);
            /*for (int i = 0; i < (int)(GameManager.Instance.waveNumber * spawnRate); i++)
            {
            }*/
        }
    }

    private GameObject GetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Count-1)];
    }
}
