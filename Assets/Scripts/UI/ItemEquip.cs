using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquip : MonoBehaviour {

    public GameObject masterPanel;
    public GameObject item;
    public GameObject[] InventorySlots;
    public GameObject weaponSlot;
    public GameObject ArmorSlot;
    public GameObject[] PotionSlots;

    public int firstEquippedConsumableSlotIndex = 0;
    public int takenConsumableSlotCount = 0;

    private GameObject beforeChanges;

    public void EquipOrUnEquip()
    {
        ItemStatus status = item.GetComponent<ItemStatus>();
        if (status.IsEquipped)
            Unequip();
        else Equip();
    }

    void Equip()
    {

        ItemStatus status = item.GetComponent<ItemStatus>();
        status.ownedByHero = masterPanel.GetComponent<Selection>().selection;
        if (status.Type == "Weapon" && weaponSlot.transform.childCount == 0)
            item.transform.SetParent(weaponSlot.transform);
        else if (status.Type == "Weapon" && weaponSlot.transform.childCount != 0)
        {
            Unequip(weaponSlot.transform.GetChild(0).gameObject);
            item.transform.SetParent(weaponSlot.transform);
        }
        else if (status.Type == "Armor" && ArmorSlot.transform.childCount == 0)
            item.transform.SetParent(ArmorSlot.transform);
        else if (status.Type == "Armor" && ArmorSlot.transform.childCount != 0)
        {
            Unequip(ArmorSlot.transform.GetChild(0).gameObject);
            item.transform.SetParent(ArmorSlot.transform);

        }
        else if (status.Type == "Consumable")
        {
            GameObject[] items = GameManager.instance.items;
            ItemStats consumable= null;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == item.name)
                    consumable = items[i].GetComponent<ItemStats>();
            }
            bool added = false;
            foreach (GameObject g in PotionSlots)
            {
                if (g.transform.childCount == 0)
                {
                    item.transform.SetParent(g.transform);
                    takenConsumableSlotCount++;

                    for (int i = 0; i < GameManager.instance.heroes.Length; i++)
                    {
                        if (GameManager.instance.heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
                            GameManager.instance.heroes[i].GetComponent<HeroStateMachine>().playerStats.Consumables.Add(consumable);
                    }
                    added = true;
                    break;
                }
            }
            if (takenConsumableSlotCount == PotionSlots.Length && !added)
                {
                    Unequip(PotionSlots[firstEquippedConsumableSlotIndex].transform.GetChild(0).gameObject);
                    item.transform.SetParent(PotionSlots[firstEquippedConsumableSlotIndex].transform);
                    takenConsumableSlotCount++;
                    firstEquippedConsumableSlotIndex++;
                }
            if (firstEquippedConsumableSlotIndex >= PotionSlots.Length)
                firstEquippedConsumableSlotIndex = 0;
        }

        GameObject[] heroes = GameManager.instance.heroes;

        for (int i = 0; i < heroes.Length; i++) {
            PlayerStats stats = heroes[i].GetComponent<HeroStateMachine>().playerStats;
            ItemStats itemStats = item.GetComponent<ItemStats>();
            if (heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
            {
                stats.stamina += itemStats.stamina;
                stats.agility += itemStats.agility;
                stats.intellect += itemStats.intellect;
                stats.dexterity += itemStats.dexterity;
                //HP
                stats.baseHP += stats.stamina * 5;
                stats.curHP = stats.baseHP;
                //MP
                stats.baseMP += stats.intellect * 2;
                stats.curMP = stats.baseMP;
                //Defense
                stats.baseDEF += stats.agility * 2;
                stats.curDEF = stats.baseDEF;
                //DMG
                stats.baseATK += stats.dexterity * 2;
                stats.curATK = stats.baseATK;
            }
        }
        status.IsEquipped = true;
        GetComponentInChildren<Text>().text = "Unequip";

    }

    void Unequip()
    {
        beforeChanges = item;

        ItemStatus status = item.GetComponent<ItemStatus>();
        status.ownedByHero = null;
        for (int i = 0; i < InventorySlots.Length; i++)
            if (InventorySlots[i].transform.childCount == 0)
            {
                item.transform.SetParent(InventorySlots[i].transform);
                break;
            }
        if (status.Type == "Consumable")
        {
            takenConsumableSlotCount--;

            for (int i = 0; i < GameManager.instance.heroes.Length; i++)
            {
                if (GameManager.instance.heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
                    GameManager.instance.heroes[i].GetComponent<HeroStateMachine>().playerStats.Consumables.Remove(item.GetComponent<ItemStats>());
            }
        }

        GameObject[] heroes = GameManager.instance.heroes;

        for (int i = 0; i < heroes.Length; i++)
        {
            PlayerStats stats = heroes[i].GetComponent<HeroStateMachine>().playerStats;
            ItemStats itemStats = item.GetComponent<ItemStats>();
            if (heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
            {
                //HP
                stats.baseHP -= stats.stamina * 5;
                stats.curHP = stats.baseHP;
                //MP
                stats.baseMP -= stats.intellect * 2;
                stats.curMP = stats.baseMP;
                //Defense
                stats.baseDEF -= stats.agility * 2;
                stats.curDEF = stats.baseDEF;
                //DMG
                stats.baseATK -= stats.dexterity * 2;
                stats.curATK = stats.baseATK;
                stats.stamina -= itemStats.stamina;
                stats.agility -= itemStats.agility;
                stats.intellect -= itemStats.intellect;
                stats.dexterity -= itemStats.dexterity;

            }
        }
        status.IsEquipped = false;
        GetComponentInChildren<Text>().text = "Equip";
    }
    
    void Unequip(GameObject itemToUnequip)
    {
        beforeChanges = item;

        ItemStatus status = itemToUnequip.GetComponent<ItemStatus>();
        status.ownedByHero = null;
        for (int i = 0; i < InventorySlots.Length; i++)
            if (InventorySlots[i].transform.childCount == 0)
            {
                itemToUnequip.transform.SetParent(InventorySlots[i].transform);
                break;
            }
        if (status.Type == "Consumable")
        {
            takenConsumableSlotCount--;

            for (int i = 0; i < GameManager.instance.heroes.Length; i++)
            {
                if (GameManager.instance.heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
                    GameManager.instance.heroes[i].GetComponent<HeroStateMachine>().playerStats.Consumables.Remove(item.GetComponent<ItemStats>());
            }
        }

        GameObject[] heroes = GameManager.instance.heroes;

        for (int i = 0; i < heroes.Length; i++)
        {
            PlayerStats stats = heroes[i].GetComponent<HeroStateMachine>().playerStats;
            ItemStats itemStats = item.GetComponent<ItemStats>();
            if (heroes[i].name == masterPanel.GetComponent<Selection>().selection.name)
            {
                stats.stamina -= itemStats.stamina;
                stats.agility -= itemStats.agility;
                stats.intellect -= itemStats.intellect;
                stats.dexterity -= itemStats.dexterity;
            }
        }
        status.IsEquipped = false;
        GetComponentInChildren<Text>().text = "Equip";
    }
}
