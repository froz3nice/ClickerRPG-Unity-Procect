using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChangeHeroesNames : MonoBehaviour {

    public GameObject Selections;
    public GameObject heroNamePlace;
    
    private Text placeName;
    private bool Created;
    public Text[] selectedHeroes;

	// Use this for initialization
	void Awake ()
    {
        placeName = heroNamePlace.GetComponentInChildren<Text>();
        CreateHeroNamePlace();
        selectedHeroes = GetNameSpaceText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        selectedHeroes = GetNameSpaceText();
        if (IsSelectedChanged())
            ChangeHeroNameSpaces();
    }

    void ChangeHeroNameSpaces()
    {
        for (int i = 0; i < GameManager.instance.selectedHeroes.Length; i++)
        {
            selectedHeroes[i].text = GameManager.instance.selectedHeroes[i].name;
        }
    }

    void CreateHeroNamePlace()
    {
        for (int i = 0; i < GameManager.instance.selectedHeroes.Length; i++)
        {
            placeName.text = GameManager.instance.selectedHeroes[i].name;
            GameObject changeSpace = Instantiate(heroNamePlace, Selections.transform, true);
        }
        Destroy(heroNamePlace);
    }

    bool IsSelectedChanged()
    {
        int count = 0;
        for(int i = 0; i < selectedHeroes.Length; i++)
            for(int j = 0; j < GameManager.instance.selectedHeroes.Length; j++)
                if (selectedHeroes[i].text == GameManager.instance.selectedHeroes[j].name)
                    count++;
        if (count == selectedHeroes.Length)
            return false;
        return true;
    }

    Text[] GetNameSpaceText()
    {
        Text[] texts = new Text[GameManager.instance.selectedHeroes.Length];
        var Images = Selections.GetComponentsInChildren<Image>();
        for(int i = 1; i < GameManager.instance.selectedHeroes.Length+1; i++)
        {
            Text text = Images[i].GetComponentInChildren<Text>();
            texts[i-1] = text;
        }
        return texts;
    }
}
