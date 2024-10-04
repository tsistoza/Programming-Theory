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
    private Enemy enemyScript;

    // Variables
    private int waveNumber;
    public int WaveNumber { get; private set; }
    private bool pickedUpPowerUp;
    public bool PickedUpPowerUp { get; set; }
    public bool interWave;

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
        interWave = false;
        pickedUpPowerUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnManager.Instance.EnemyCount == 0 && !interWave)
        {
            interWave = true;
            // Start Next Wave
            // Spawn Pickups, Upgrades in the middle of the map
            waveNumber++;
            waveNumberText.text = $"Wave: {waveNumber}";
        }
    }
    
    private void NextWave()
    {
        if (interWave && PickedUpPowerUp)
        {
            Debug.Log("Starting Next Round\n");
            interWave = false;
        }
    }
}
