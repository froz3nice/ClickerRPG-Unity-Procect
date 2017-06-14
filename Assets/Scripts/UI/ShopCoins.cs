using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCoins : MonoBehaviour {
    
    private Text coinText;
	// Use this for initialization
	void Start () {
        coinText = GetComponentInChildren<Text>();
        coinText.text = string.Format("C: {0, 3}", GameManager.instance.GetCoins().ToString());
	}
	
	// Update is called once per frame
	void Update ()
    {
        coinText.text = string.Format("C: {0, 3}", GameManager.instance.GetCoins().ToString());

    }
}
