using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Similar to the upgrade class in that it will handle the perks
/// Note: Upgrades are primarily stats driven upgrades
/// And Perks are things outside of stats such as equipments.
/// </summary>
public class Perks : MonoBehaviour
{
    [SerializeField] private List<Perk> playerPerks;
    [SerializeField] private Perk obj;
    [SerializeField] private PerkHandler handler;

    void Start()
    {
        playerPerks = GameObject.Find("Player").GetComponent<PlayerController>().perks;
        handler = GameObject.Find("Player").GetComponent<PerkHandler>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        playerPerks.Add(obj);
        handler.PerkUpdate(obj);
        Destroy(gameObject);
        SpawnManager.Instance.DestroyPowerUps();
    }
}
