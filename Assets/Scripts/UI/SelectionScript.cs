using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour {

    public bool SelectHeroesOrEnemies;

    private GameObject selectedUnit;

	// Use this for initialization
	void Start () {
        selectedUnit = transform.parent.GetComponent<Selection>().selection;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Select()
    {
        if (SelectHeroesOrEnemies)
            AddToArray(GameManager.instance.selectedHeroes, selectedUnit);
        else if (!SelectHeroesOrEnemies)
            AddToArray(GameManager.instance.selectedEnemies, selectedUnit);
    }

    private void AddToArray(GameObject[] array, GameObject unit)
    {
        if (!IsUnitInArray(array, unit))
        {
            for (int i = 1; i < array.Length; i++)
                array[i] = array[i - 1];
            array[0] = unit;
        }
    }

    private bool IsUnitInArray(GameObject[] array, GameObject unit)
    {
        for (int i = 0; i < array.Length; i++)
            if (unit.name == array[i].name)
                return true;
        return false;
    }
}
