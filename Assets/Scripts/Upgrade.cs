using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be attached to the powerups and will be added accordingly
/// </summary>
public class Upgrade : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PickedUpPowerUp = true;
            Destroy(gameObject);
            SpawnManager.Instance.DestroyPowerUps();
        }
    }
}
