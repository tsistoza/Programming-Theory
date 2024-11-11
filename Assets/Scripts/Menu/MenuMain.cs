using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class MenuMain : MonoBehaviour
{
    public static MenuMain Instance { get; private set; }
    public bool paused;

    // UI
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject PlayerMenu;


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
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        return;
    }

}
