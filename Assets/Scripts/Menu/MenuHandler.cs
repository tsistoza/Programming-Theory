using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TextMeshProUGUI highScoreText;
    public static MenuHandler Instance {  get; private set; }

    public string playerName;
    public int highScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Play()
    {
        SceneManager.LoadScene(1);
        return;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        return;
    }

    public void SetPlayerName()
    {
        playerName = nameField.text;
        return;
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string file = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(file);
            highScoreText.text = $"High Score: {data.m_MaxWave}\nName: {data.m_MaxName}";
            highScore = data.m_MaxWave;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string m_MaxName;
    public int m_MaxWave;
}
