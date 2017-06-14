using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateEquippedItems : MonoBehaviour {

    public GameObject weaponSlot;
    public GameObject armorSlot;
    public GameObject[] potionSlot;
    public GameObject masterPanel;
    public Text selectedHeroText;

    private GameObject selection;

	// Use this for initialization
	void Start () {
        selection = masterPanel.GetComponent<Selection>().selection;
        GenerateEquipped();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateEquipped()
    {
        selection = masterPanel.GetComponent<Selection>().selection;
        selectedHeroText.text = selection.GetComponent<HeroStateMachine>().playerStats.theName;
        weaponSlot.transform.DetachChildren();
        armorSlot.transform.DetachChildren();
        foreach (GameObject slot in potionSlot)
        {
            slot.transform.DetachChildren();
        }

        int consumableSlotIndex = 0;
        List<GameObject> items = GameManager.instance.boughtItems;
        for (int i = 0; i < items.Count; i++)
            if (items[i].GetComponent<ItemStatus>().ownedByHero != null)
                if(items[i].GetComponent<ItemStatus>().ownedByHero.name == selection.name)
                {
                    if (items[i].GetComponent<ItemStatus>().Type == "Weapon")
                        items[i].transform.SetParent(weaponSlot.transform);
                    else if (items[i].GetComponent<ItemStatus>().Type == "Armor")
                        items[i].transform.SetParent(armorSlot.transform);
                    else if (items[i].GetComponent<ItemStatus>().Type == "Consumable")
                    {
                        items[i].transform.SetParent(potionSlot[consumableSlotIndex].transform);
                        consumableSlotIndex++;
                    }
                }
    }
}
