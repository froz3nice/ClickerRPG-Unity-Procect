using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : MonoBehaviour {

    public int id;
    public GameObject ownedByHero;
    public string Type;
    public bool IsEquipped = false;

    public void SetOwner(GameObject owner)
    {
        ownedByHero = owner;
    }

    public void setIsEquipped(bool b)
    {
        IsEquipped = b;
    }
	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
