using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    [SerializeField] private List<Perk> perks;
    [SerializeField] private GameObject shuriken;

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
                default: break; // Unknown Perk
            }
        }
    }

    private void ShurikenSpawn()
    {
        Instantiate(shuriken);
    }

}
