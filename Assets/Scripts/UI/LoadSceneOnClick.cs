using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
    {
        /*if (sceneIndex == 1)
        {
            ItemDatabase database = GameManager.instance.GetComponent<ItemDatabase>();
            database.getItemsFromGameManager();
            database.WriteItemsToJsonFile();
            GameManager.instance.boughtItems.Clear();
        }*/
        SceneManager.LoadScene(sceneIndex);
    }
}
