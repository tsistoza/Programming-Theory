using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    // Components
    [SerializeField] private TextMeshProUGUI waveNumberText;

    // Variables
    private int waveNumber;
    private Enemy enemyScript;

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
        enemyScript = GameObject.Find("Enemy").GetComponent<Enemy>();
        waveNumber = 1;
        waveNumberText.text = $"Wave: {waveNumber}";
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnManager.Instance.EnemyCount == 0)
        {
            // Start Next Wave
            // Spawn Pickups, Upgrades in the middle of the map
            waveNumber++;
            waveNumberText.text = $"Wave: {waveNumber}";
        }
    }
}
