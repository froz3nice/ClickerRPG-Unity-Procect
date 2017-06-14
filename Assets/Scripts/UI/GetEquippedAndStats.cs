using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEquippedAndStats : MonoBehaviour {
    
    public GameObject heroInventoryPanel;
    public GameObject heroStatsPanel;

    void OnEnable()
    {
        heroInventoryPanel.GetComponent<GenerateEquippedItems>().GenerateEquipped();
        heroStatsPanel.GetComponent<GenerateHeroStats>().GenerateHeroDesription();
    }
}
