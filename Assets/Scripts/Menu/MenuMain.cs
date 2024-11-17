using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MenuMain : MonoBehaviour
{
    public static MenuMain Instance { get; private set; }
    public bool paused;

    // UI
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject Enemy;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        score.text = $"Score: {GameManager.Instance.WaveNumber}";
    }

    public void Resume()
    {
        paused = false;
        PlayerMenu.SetActive(false);
        return;
    }

    public void Exit()
    {
        SaveScore();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        return;
    }

    // Save Data
    private void SaveScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (!File.Exists(path))
        {
            SaveData data1 = new SaveData();
            data1.m_MaxName = MenuHandler.Instance.playerName;
            data1.m_MaxWave = GameManager.Instance.WaveNumber;
            string json = JsonUtility.ToJson(data1);
            File.WriteAllText(path, json);
        }
        string file = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(file);
        if (data.m_MaxWave > GameManager.Instance.WaveNumber)
        {
            data.m_MaxName = MenuHandler.Instance.playerName;
            data.m_MaxWave = GameManager.Instance.WaveNumber;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        return;
    }
}
