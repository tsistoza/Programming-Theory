using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    [SerializeField] private List<Perk> perks;
    [SerializeField] private GameObject shuriken;

    // Stats - Not Player Stats, but stats of the perks
    public int PoisonDamage = 1;
    public int PoisonDuration = 9;

    public void PerkUpdate(Perk perk)
    {
        perks = gameObject.GetComponent<PlayerController>().perks;
        if (perks.Contains(perk))
        {
            switch(perk)
            {
                case Perk.Shuriken_Perk:
                    Console.WriteLine("Spawning Shuriken");
                    ShurikenSpawn();
                    break;
                case Perk.Poison_Perk:
                    PoisonDuration++;
                    break;
                default: break; // Unknown Perk
            }
        }
    }

    private void ShurikenSpawn()
    {
        Instantiate(shuriken);
    }
}
