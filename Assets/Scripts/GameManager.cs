using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Game Manager is used to control the games state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    // Components
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI highScoreText;

    // Variables
    private int waveNumber;
    public int WaveNumber {
        get { return waveNumber; } 
        private set {  waveNumber = value; }
    }
    private bool pickedUpPowerUp;
    public bool PickedUpPowerUp
    {
        get { return pickedUpPowerUp; }
        set { pickedUpPowerUp = value; }
    }
    public bool spawnedPowerUps;
    public bool m_gameOver;
    private string Name;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Name = MenuHandler.Instance.playerName;
        highScoreText.text = $"High Score: {MenuHandler.Instance.highScore}";
        WaveNumber = 1;
        waveNumberText.text = $"Wave: {WaveNumber}";
    }

    // Start is called before the first frame update
    void Start()
    {
        pickedUpPowerUp = false;
        spawnedPowerUps = false;
        m_gameOver = false;
        SpawnManager.Instance.SpawnEnemyWave();
    }

    // Update is called once per frame
    void Update()
    {
        TransitionHandler();
    }

    private void TransitionHandler()
    {
        if (SpawnManager.Instance.EnemyCount == 0 && isInUpgradeWave() == 0)
        {
            SpawnManager.Instance.SpawnPowerUps();
            waveNumber++;
            waveNumberText.text = $"Wave: {waveNumber}";
        } else if (SpawnManager.Instance.EnemyCount == 0 && isInUpgradeWave() == 2)
        {
            spawnedPowerUps = false;
            pickedUpPowerUp = false;
            SpawnManager.Instance.SpawnEnemyWave();
        }
    }

    private int isInUpgradeWave()
    {
        if (!spawnedPowerUps && !PickedUpPowerUp) { return 0; } // You are in an attack wave
        // else if(spawnedPowerUps && !PickedUpPowerUp) { return 1; } // You are in an upgrade wave
        else if(spawnedPowerUps && PickedUpPowerUp) // You are exiting an upgrade wave set conditions to default false
        {
            return 2;
        }
        return -1; // Unknown Wave
    }

    public void GameOver ()
    {
        gameOver.SetActive(true);
        // Pause Background
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
