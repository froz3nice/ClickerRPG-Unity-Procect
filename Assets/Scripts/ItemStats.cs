
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {

    public int coins;
    public int stamina;
    public int intellect;
    public int agility;
    public int dexterity;
    public string slug;

    public int amountOfHpToGive;
    public int amountOfMpToGive;

    public string ConsumableDescription;


    void Start()
    {
        ConsumableDescription = string.Format("Coins: {2}\n\nThis consumables gives {0} health points and {1} mana points.", amountOfHpToGive, amountOfMpToGive, coins);
    }
}
