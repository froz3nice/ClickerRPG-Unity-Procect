using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateShopList : MonoBehaviour {

    public GameObject[] Slots;
    public GameObject masterPanel;
    
    void Awake()
    {
        GenerateInventory();
    }

	public void GenerateInventory()
    {

        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].transform.childCount >= 0)
            {
                for (int j = 0; j < Slots[i].transform.childCount; j++)
                    Destroy(Slots[i].transform.GetChild(j).gameObject);
                Slots[i].transform.DetachChildren();
            }

        GameObject[] items = GameManager.instance.items;
        for (int i = 0; i < items.Length; i++)
            for (int j = 0; j < Slots.Length; j++)
                if (items[i].tag == masterPanel.GetComponent<Selection>().selection.GetComponent<ItemStatus>().Type && Slots[j].transform.childCount <= 0)
                {
                    GameObject item = Instantiate(items[i], Slots[j].transform);
                    item.transform.name = items[i].transform.name;
                    break;
                }
    }
}
