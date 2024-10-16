using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be attached to the powerups and will be added accordingly
/// </summary>
public class Upgrade : MonoBehaviour
{
    [SerializeField] private PlayerController playerScript;
    [SerializeField] private GunHandler gunScript;
    [SerializeField] private int upgradeId;

    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gunScript = GameObject.Find("Player").GetComponent<GunHandler>();
    }

    private void SetUpgrade()
    {
        switch (upgradeId)
        {
            case 0: gunScript.DamagePerBullet++; break;
            case 1: playerScript.PlayerMaxHp++; break;
            case 2: playerScript.moveSpd+=5; break;
            default: Debug.Log("Unknown Upgrade"); break;
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
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
