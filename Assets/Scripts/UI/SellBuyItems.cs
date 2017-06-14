using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellBuyItems : MonoBehaviour {

    public GameObject masterPanel;
    public GameObject item;
    public GameObject[] InventorySlots;
    public GameObject[] ShopSlots;

    private GameObject beforeChanges;

    public void BuyOrSell()
    {
        ItemStatus status = item.GetComponent<ItemStatus>();
        if (item.tag == "Sold")
            Sell();
        else Buy();
    }

    void Buy()
    {

        GameObject soldItem = item;
        ItemStats stats = soldItem.GetComponent<ItemStats>();
        
        if (!GameManager.instance.IsEnoughToBuy(stats.coins))
        {
            Text button = GetComponentInChildren<Text>();
            button.text = "Not enough coins :(";
            return;
        }

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if(InventorySlots[i].transform.childCount == 0)
            {
                soldItem = Instantiate(item, InventorySlots[i].transform);
                break;
            }
        }
        if (soldItem.name != item.name)
        {
            soldItem.tag = "Sold";
            soldItem.name = item.name;
        }
        GameManager.instance.boughtItems.Add(soldItem);
        GameManager.instance.SubtractCoins(stats.coins);
    }

    void Sell()
    {
        ItemStats stats = item.GetComponent<ItemStats>();
        GameManager.instance.AddCoins(stats.coins);
        GameManager.instance.boughtItems.Remove(item);
        Destroy(item);
    }
}

