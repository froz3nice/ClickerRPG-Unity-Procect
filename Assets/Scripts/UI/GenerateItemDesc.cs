using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemStats), typeof(ItemStatus))]
public class GenerateItemDesc : MonoBehaviour {

    private Text text;
    private Button button;

    void Start()
    {
        GameObject parent = transform.parent.parent.parent.parent.parent.gameObject;
        Text[] allTexts = parent.GetComponentsInChildren<Text>();
        Button[] allButtons = parent.GetComponentsInChildren<Button>();
        foreach (Text t in allTexts)
            if (t.transform.name == "ItemDescription")
                text = t;
        foreach (Button b in allButtons)
            if (b.transform.name == "ItemEquip")
                button = b;
    }

    public void GenerateDescription()
    {
        GameObject parent = transform.parent.parent.parent.parent.parent.gameObject;
        Text[] allTexts = parent.GetComponentsInChildren<Text>();
        Button[] allButtons = parent.GetComponentsInChildren<Button>();
        foreach (Text t in allTexts)
            if (t.transform.name == "ItemDescription")
                text = t;
        foreach (Button b in allButtons)
            if (b.transform.name == "ItemEquip")
                button = b;

        button.interactable = true;
        ItemStatus status = GetComponent<ItemStatus>();
        ItemStats stats = GetComponent<ItemStats>();

        if (status.Type == "Consumable")
            text.text = stats.ConsumableDescription;
        else
            text.text = string.Format("Name - {0}\nType - {1}\n  {2, -11} {6, 3}\n  {3, -11} {7, 3}\n  {4, -11} {8, 3}\n  {5, -11} {9, 3}\n\n{10, -11} {11, 3}",
                transform.name, status.Type, "Intellect:", "Agility:", "Dexterity:", "Stamina:", stats.intellect, stats.agility, stats.dexterity, stats.stamina, "Cost:", stats.coins);
        if (button.GetComponent<ItemEquip>() != null)
        {
            ItemEquip equip = button.GetComponent<ItemEquip>();
            equip.item = gameObject;

            if (status.IsEquipped)
                button.GetComponentInChildren<Text>().text = "Unequip";
            else
                button.GetComponentInChildren<Text>().text = "Equip";
        }
        else
        {
            SellBuyItems buySell = button.GetComponent<SellBuyItems>();
            buySell.item = gameObject;

            if (tag == "Sold")
                button.GetComponentInChildren<Text>().text = "Sell";
            else
                button.GetComponentInChildren<Text>().text = "Buy";
        }
    }
}
