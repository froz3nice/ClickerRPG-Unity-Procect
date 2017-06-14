using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInventory : MonoBehaviour {

    public GameObject gridWithGenerateInventoryScript;

    void OnEnable()
    {
        gridWithGenerateInventoryScript.GetComponent<GenerateInventoryList>().GenerateInventory();
    }
}
