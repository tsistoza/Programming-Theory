using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    [SerializeField] private List<Perk> perks;
    [SerializeField] private GameObject shuriken;
    [SerializeField] private Transform parent;

    void Start()
    {
        perks = gameObject.GetComponent<PlayerController>().perks;
    }

    public void PerkUpdate(Perk perk)
    {
        if(perks.Contains(perk))
        {
            switch(perk)
            {
                case Perk.Shuriken_Perk:
                    ShurikenSpawn();
                    break;
                default: break; // Unknown Perk
            }
        }
    }

    private void ShurikenSpawn()
    {
        Instantiate(shuriken, parent, true);
    }

}
