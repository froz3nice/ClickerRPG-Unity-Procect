using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoad : MonoBehaviour {


	// Use this for initialization
	void Awake ()
    {
            ItemDatabase database = GameManager.instance.GetComponent<ItemDatabase>();
            database.ConstrutItemDatabase();
            database.AddItemsFromDataBaseToGameManager();
        
    }
}
