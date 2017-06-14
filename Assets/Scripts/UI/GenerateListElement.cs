using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GenerateListElement : MonoBehaviour {

    public GameObject element;
    public GameObject Grid;
    public string ButtonText;
    public string SelectHeroesOrEnmeiesOrItems; // "Enemies", "Items", "Heroes"
    public string ItemTag; // just for items

    private Selection Selection;
    private Text elementName;
    private Text buttonName;

	// Use this for initialization
	public void Start () {
        
        element.SetActive(true);

        for (int i = 0; i < Grid.transform.childCount; i++)
        {
            Transform child = Grid.transform.GetChild(i);
            if (child.name != element.transform.name)
                Destroy(child.gameObject);
        }

        Button button = element.GetComponentInChildren<Button>();

        elementName = element.GetComponentInChildren<Text>();
        buttonName = button.GetComponentInChildren<Text>();
        Selection = element.GetComponent<Selection>();

        if (SelectHeroesOrEnmeiesOrItems == "Heroes")
            CreateElementList(GameManager.instance.heroes);
        else if (SelectHeroesOrEnmeiesOrItems == "Enemies")
            CreateElementList(GameManager.instance.enemies);
        else if (SelectHeroesOrEnmeiesOrItems == "Items")
            CreateItemList(GameManager.instance.items);
	}

    void CreateElementList(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (SelectHeroesOrEnmeiesOrItems == "Enemies") { 
                if (array[i].GetComponent<EnemyStateMachine>().EnemyStats.timesDefeated > 0)
                    elementName.text = array[i].name + " - defeated " + array[i].GetComponent<EnemyStateMachine>().EnemyStats.timesDefeated;
                else
                    elementName.text = array[i].name;
            }
                else
                    elementName.text = array[i].name;
            buttonName.text = ButtonText;
            Selection.selection = array[i];
            GameObject changedElement = Instantiate(element, Grid.transform, true);
        }
        Destroy(element);
    }

    void CreateItemList(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].tag == ItemTag)
            {
                elementName.text = array[i].name;
                buttonName.text = ButtonText;
                Selection.selection = array[i];
                GameObject changedElement = Instantiate(element, Grid.transform, true);
            }
        }
        element.SetActive(false);
    }

    public void SetItemTag(string newTag)
    {
        ItemTag = newTag;
    }

    public void SetButtonName(string newButtonName)
    {
        ButtonText = newButtonName;
    }
}
