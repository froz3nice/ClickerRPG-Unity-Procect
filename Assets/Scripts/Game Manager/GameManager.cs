using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject[] enemies;
    public GameObject[] heroes;
    public GameObject[] items;


    public int EnemiesInBattleNumber = 1;
    public int HeroesInBattleNumber = 2;

    public GameObject[] selectedEnemies;
    public GameObject[] selectedHeroes;
    public List<GameObject> boughtItems;
    public List<GameObject> equippedItems;
    
    public GameObject Player;

    public bool firstLoadDone = false;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        selectedEnemies = new GameObject[EnemiesInBattleNumber];
        selectedEnemies[0] = enemies[0];

        selectedHeroes = new GameObject[HeroesInBattleNumber];
		selectedHeroes[0] = heroes[0];
        selectedHeroes[1] = heroes[1];
        loadData();

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void loadData()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            Debug.Log(heroes[i].name);
            if (heroes[i].GetComponent<HeroStateMachine>().playerStats.agility==0 && heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK == 0)
            {
                switch (heroes[i].name)
                {
                    case "Fighter":
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.agility = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP = 100;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel = 1;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp = 0;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = 150;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina = 10;
                        break;
                    case "Goblin":
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.agility = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP = 80;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP = 20;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel = 1;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp = 0;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = 150;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina = 5;
                        break;
                    case "Skeleton":
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.agility = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP = 80;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel = 1;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp = 0;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity = 12;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = 150;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina = 8;
                        break;
                    case "Mummy":
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.agility = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF = 15;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP = 120;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP = 10;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel = 1;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp = 0;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect = 5;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = 150;
                        heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina = 15;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                heroes[i].GetComponent<HeroStateMachine>().playerStats.agility = PlayerPrefs.GetInt(heroes[i].name + "agility");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK = PlayerPrefs.GetFloat(heroes[i].name + "baseATK");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF = PlayerPrefs.GetFloat(heroes[i].name + "baseDEF");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP = PlayerPrefs.GetFloat(heroes[i].name + "baseHP");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP = PlayerPrefs.GetFloat(heroes[i].name + "baseMP");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel = PlayerPrefs.GetInt(heroes[i].name + "CharacterLevel");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.curATK = PlayerPrefs.GetFloat(heroes[i].name + "curATK");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.curDEF = PlayerPrefs.GetFloat(heroes[i].name + "curDEF");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp = PlayerPrefs.GetInt(heroes[i].name + "CurExp");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.curHP = PlayerPrefs.GetFloat(heroes[i].name + "curHP");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.curMP = PlayerPrefs.GetFloat(heroes[i].name + "curMP");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity = PlayerPrefs.GetInt(heroes[i].name + "dexterity");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect = PlayerPrefs.GetInt(heroes[i].name + "intellect");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = PlayerPrefs.GetInt(heroes[i].name + "RequiredExp");
                heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina = PlayerPrefs.GetInt(heroes[i].name + "stamina");
            }

        }
    }
    public void saveData()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            Debug.Log(heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP);
            PlayerPrefs.SetInt(heroes[i].name + "agility", heroes[i].GetComponent<HeroStateMachine>().playerStats.agility);
            PlayerPrefs.SetFloat(heroes[i].name + "baseATK", heroes[i].GetComponent<HeroStateMachine>().playerStats.baseATK);
            PlayerPrefs.SetFloat(heroes[i].name + "baseDEF", heroes[i].GetComponent<HeroStateMachine>().playerStats.baseDEF);
            PlayerPrefs.SetFloat(heroes[i].name + "baseHP", heroes[i].GetComponent<HeroStateMachine>().playerStats.baseHP);
            PlayerPrefs.SetFloat(heroes[i].name + "baseMP", heroes[i].GetComponent<HeroStateMachine>().playerStats.baseMP);
            PlayerPrefs.SetInt(heroes[i].name + "CharacterLevel", heroes[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel);
            PlayerPrefs.SetFloat(heroes[i].name + "curATK", heroes[i].GetComponent<HeroStateMachine>().playerStats.curATK);
            PlayerPrefs.SetFloat(heroes[i].name + "curDEF", heroes[i].GetComponent<HeroStateMachine>().playerStats.curDEF);
            PlayerPrefs.SetInt(heroes[i].name + "CurExp", heroes[i].GetComponent<HeroStateMachine>().playerStats.CurExp);
            PlayerPrefs.SetFloat(heroes[i].name + "curHP", heroes[i].GetComponent<HeroStateMachine>().playerStats.curHP);
            PlayerPrefs.SetFloat(heroes[i].name + "curMP", heroes[i].GetComponent<HeroStateMachine>().playerStats.curMP);
            PlayerPrefs.SetInt(heroes[i].name + "dexterity", heroes[i].GetComponent<HeroStateMachine>().playerStats.dexterity);
            PlayerPrefs.SetInt(heroes[i].name + "intellect", heroes[i].GetComponent<HeroStateMachine>().playerStats.intellect);
            PlayerPrefs.SetInt(heroes[i].name + "RequiredExp", heroes[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp);
            PlayerPrefs.SetInt(heroes[i].name + "stamina", heroes[i].GetComponent<HeroStateMachine>().playerStats.stamina);
            /*PlayerPrefs.SetInt(heroes[i].name + "MagicCount", heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedMagic.Count);
            PlayerPrefs.SetInt(heroes[i].name + "SkillCount", heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedSkills.Count);
            for (int j = 0; j < heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedMagic.Count; j++)
                PlayerPrefs.SetString("magic" + j, heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedMagic[j].name);
            for (int j = 0; j < heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedSkills.Count; j++)
                PlayerPrefs.SetString("skill" + j, heroes[i].GetComponent<HeroStateMachine>().playerStats.UnlockedSkills[j].name);*/
            PlayerPrefs.Save();
        }
    }
    public void UpdateCoins(int coins)
    {

        Player.GetComponent<Player>().coins = coins;
    }

    public int GetCoins()
    {
        Player.GetComponent<Player>();

        return Player.GetComponent<Player>().coins;
    }

    public bool IsEnoughToBuy(int cost)
    {
        Player player = Player.GetComponent<Player>();
        if (player.coins < cost)
            return false;
        return true;
    }
    public void SubtractCoins(int toSubtract)
    {
        Player player = Player.GetComponent<Player>();
        player.coins -= toSubtract;
    }

    public void AddCoins(int toAdd)
    {
        Player player = Player.GetComponent<Player>();
        player.coins += toAdd;
    }

}
