using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    // Components
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private GameObject powerUp;

    // Variables
    private int waveNumber;
    public int WaveNumber { get; private set; }
    private bool pickedUpPowerUp;
    public bool PickedUpPowerUp
    {
        get { return pickedUpPowerUp; }
        set { pickedUpPowerUp = value; }
    }
    public bool spawnedPowerUps;

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
        waveNumber = 1;
        waveNumberText.text = $"Wave: {waveNumber}";
        pickedUpPowerUp = false;
        spawnedPowerUps = false;
        SpawnManager.Instance.SpawnEnemyWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnManager.Instance.EnemyCount == 0 && !spawnedPowerUps)
        {
            // Start Next Wave
            // Spawn Pickups, Upgrades in the middle of the map
            SpawnManager.Instance.SpawnPowerUps();
            waveNumber++;
            waveNumberText.text = $"Wave: {waveNumber}";
        }
        if (SpawnManager.Instance.EnemyCount == 0 && PickedUpPowerUp)
        {
            Debug.Log("Spawn Enemy Wave");
            PickedUpPowerUp = false;
            SpawnManager.Instance.SpawnEnemyWave();
        }
    }
}
