using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainUIHandler is used to handle the UI, not the wavenumber but the upgrades, the guns, perks, stats...
/// </summary>
public class MainUIHandler : MonoBehaviour
{
    // Components
    private PlayerController playerScript;
    public Slider hpSlider;

    // Variables
    private int maxHealth;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        maxHealth = playerScript.PlayerMaxHp;
    }

    void Update()
    {
        updateHpBar();
    }

    public void updateHpBar()
    {
        maxHealth = playerScript.PlayerMaxHp;
        hpSlider.value = (float) playerScript.PlayerHitPoints / maxHealth;
    }
}
