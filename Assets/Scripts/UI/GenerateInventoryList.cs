using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateInventoryList : MonoBehaviour {

    public GameObject[] Slots;
    public bool shopMode = false;

    void Start()
    {
        GenerateInventory();
    }

    void Awake()
    {
        GenerateInventory();
    }

    public void GenerateInventory()
    {

        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].transform.childCount >= 0)
                Slots[i].transform.DetachChildren();



        List<GameObject> items = GameManager.instance.boughtItems;
        for (int i = 0; i < items.Count; i++)
            for (int j = 0; j < Slots.Length; j++)
                if (Slots[j].transform.childCount <= 0)
                {
                    if(shopMode)
                    items[i].transform.SetParent(Slots[i].transform);
                    else if(items[i].GetComponent<ItemStatus>().ownedByHero == null)
                        items[i].transform.SetParent(Slots[i].transform);
                    break;
                }
    }
}
