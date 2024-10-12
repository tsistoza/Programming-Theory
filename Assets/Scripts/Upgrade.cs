using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be attached to the powerups and will be added accordingly
/// </summary>
public class Upgrade : MonoBehaviour
{
    [SerializeField] private PlayerController playerScript;
    [SerializeField] private int upgradeId;

    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void SetUpgrade()
    {
        switch (upgradeId)
        {
            case 0: playerScript.DamagePerBullet++; break;
            case 1: playerScript.PlayerMaxHp++; break;
            case 2: playerScript.moveSpd++; break;
            default: Debug.Log("Unknown Upgrade"); break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PickedUpPowerUp = true;
            SetUpgrade();
            Destroy(gameObject);
            SpawnManager.Instance.DestroyPowerUps(); // Destroy extra powerups
        }
    }
}
