using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateHeroStats : MonoBehaviour {

    public Text heroDescription;
    public GameObject MasterPanel;

    private GameObject selection;
	// Use this for initialization
	void Start () {
        selection = MasterPanel.GetComponent<Selection>().selection;
        GenerateHeroDesription();
	}
	
	public void GenerateHeroDesription()
    {
        selection = MasterPanel.GetComponent<Selection>().selection;
        int plusStamina = 0;
        int plusAgility = 0;
        int plusIntellect = 0;
        int plusDexterity = 0;
        PlayerStats stats = selection.GetComponent<HeroStateMachine>().playerStats;
        List<GameObject> items = GameManager.instance.boughtItems;
        for(int i = 0; i < items.Count; i++)
            if(items[i].GetComponent<ItemStatus>().ownedByHero != null)
                if(items[i].GetComponent<ItemStatus>().ownedByHero.name == selection.name)
                {
                    plusStamina += items[i].GetComponent<ItemStats>().stamina;
                    plusAgility += items[i].GetComponent<ItemStats>().agility;
                    plusIntellect += items[i].GetComponent<ItemStats>().intellect;
                    plusDexterity += items[i].GetComponent<ItemStats>().dexterity;
                }
        heroDescription.text = string.Format("{12}\n\n{0, -11}: {1, 3} + {2}\n{3, -11}: {4, 3} + {5}\n{6, -11}: {7, 3} + {8}\n{9, -11}: {10, 3} + {11}\n",
            "Stamina", stats.stamina - plusStamina, plusStamina, "Agility", stats.agility - plusAgility, plusAgility, "Intellect",
            stats.intellect - plusIntellect, plusIntellect, "Dexterity", stats.dexterity - plusDexterity, plusDexterity, stats.theName);
    }
}
