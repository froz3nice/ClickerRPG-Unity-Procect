using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE

    }
    public PerformAction battleStates;
    public bool expadd = false;
    public bool itemUsed = false;
    public List<HandleTurns> PerformList = new List<HandleTurns>();
    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    public static GameObject[] HeroesManaging = GameManager.instance.selectedHeroes; // naudojama keisti herojų info
    public GameObject En = GameManager.instance.selectedEnemies[0]; // naudojama sužinot kiek coins ar exp duoda enemy
    public int clicksCount = 0;
    public enum HeroGUI
	{
		ACTIVATE,
		WAITING,
		INPUT1,
        INPUT2,
		DONE
	}

	public HeroGUI HeroInput;


    public GameObject EndBattlePanel;
    

	public List<GameObject> HeroesToManage = new List<GameObject> ();
	public HandleTurns HeroChoise;

	public GameObject enemyButton;
    public GameObject enemyBtn;
    public Transform Spacer;

	public GameObject ActionPanel;
	public GameObject EnemySelectPanel;
    public GameObject MagicPanel;
    public GameObject SkillPanel;
    public GameObject ItemPanel;

    //magic attacks

    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject magicButton;
    public Transform skillSpacer;
    public Transform itemSpacer;
    public GameObject backButton;
    public GameObject itemButton;
    AudioControllerScript audioManager;
    private List<GameObject> atkBtns = new List<GameObject>();

    //private List<GameObject> enemyBtns = new List<GameObject>();
     
	public GameObject enemyPanel;
	public GameObject Hero1;
	public GameObject Hero2;
	public GameObject Enemy;
	public GameObject magicFire; 
	public GameObject magicPoison; 

	public int HeroInputLeft = 2;
    public int HeroDead = 0;
	public void turnOffActionPanel(){
		ActionPanel.SetActive (false);
		EnemySelectPanel.SetActive (true);
		Debug.Log ("attack pressed");
	}
	void Start () {
        UpdateHeroStats();
        audioManager = AudioControllerScript.instance;
		if (audioManager == null) {
			Debug.LogError ("someting went horribly wrong (audioController is not found)");
		}
		audioManager.Ambient ();
		magicFire = GameObject.Find ("Flash");
		magicFire.SetActive (false);
		magicPoison = GameObject.Find ("Poison");
		magicPoison.SetActive (false);

        Vector3 enemyPos = new Vector3(-4.5f, 1f, -1.25f);
        Enemy = Instantiate(GameManager.instance.selectedEnemies[0], enemyPos, Quaternion.identity) as GameObject;
        Enemy.transform.name = "Enemy";

        Vector3 hero1Pos = new Vector3(3, 1, -3);
        Hero1 = Instantiate(GameManager.instance.selectedHeroes[0], hero1Pos, Quaternion.identity) as GameObject;
		string name = GameManager.instance.selectedHeroes [0].GetComponent<HeroStateMachine> ().playerStats.theName;
		Hero1.transform.name = name;

        Vector3 hero2Pos = new Vector3(3, 1, 0);
        Hero2 = Instantiate(GameManager.instance.selectedHeroes[1], hero2Pos, Quaternion.identity) as GameObject;
		Hero1.transform.Rotate (0, -90, 0);
		Hero2.transform.Rotate (0, -90, 0);
		name = GameManager.instance.selectedHeroes [1].GetComponent<HeroStateMachine> ().playerStats.theName;
		Hero2.transform.name = name;
		Enemy.transform.Rotate(0,90,0);

        //_anim.Play("Run");
        battleStates = PerformAction.WAIT;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
		HeroInput = HeroGUI.ACTIVATE;
		ActionPanel.SetActive (false);
		EnemySelectPanel.SetActive (false);
        MagicPanel.SetActive(false);
		EnemyButtons ();

    }
	
	// Update is called once per frame
	void Update () {
		switch(battleStates)
        {
		case (PerformAction.WAIT):
                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                if(HerosInBattle.Count<1 || EnemysInBattle.Count < 1)
                {
                    battleStates = PerformAction.CHECKALIVE;
                }
                break;
		case (PerformAction.TAKEACTION):
			GameObject performer = GameObject.Find (PerformList [0].Attacker);

                if (PerformList[0].Type == "Hero")
                {
                    if (EnemysInBattle.Count != 0)
                    {
                        HeroStateMachine hsm = performer.GetComponent<HeroStateMachine>();
                        hsm.EnemyToAttack = PerformList[0].AttackersTarget;
                        hsm.currentState = HeroStateMachine.TurnState.ACTION;
                    }
                }
                if (PerformList [0].Type == "Enemy") {
                    Debug.Log(PerformList[0].Attacker);
				EnemyStateMachine esm = performer.GetComponent<EnemyStateMachine> ();
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                       
                        if (HerosInBattle.Count == 0 || esm.EnemyStats.curHP<=0)
                            break;
                        else if (PerformList[0].AttackersTarget == HerosInBattle[i])
                        {
                            esm.HeroToAttack = PerformList[0].AttackersTarget;
                            esm.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = HerosInBattle[Random.Range(0, HerosInBattle.Count)];
                            esm.HeroToAttack = PerformList[0].AttackersTarget;
                            esm.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }
			}
			battleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):

                break;
            case (PerformAction.CHECKALIVE):
                if (HerosInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    // lose game
                }
                else if (EnemysInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    // win game
                }
                else
                {
                    clearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                    //call function
                }
                break;
            case (PerformAction.LOSE):
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
                        clicksCount++;
                    Debug.Log("You lost the Battle");
                    //Defeat.SetActive(true);
                    if (clicksCount == 1)
                    {
						audioManager.GameOver ();
                        EndBattlePanel.transform.FindChild("Text").GetComponent<Text>().text = "You lost the Battle";
                        EndBattlePanel.SetActive(true);
                    }
				if (clicksCount == 2)
                        SceneManager.LoadScene(0);
                }

                break;
                
            case (PerformAction.WIN):
                {
                    
                    if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
                        clicksCount++;
                    Debug.Log("You won the Battle");
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                    }
                    if (clicksCount==1)
                    {
                        En.GetComponent<EnemyStateMachine>().EnemyStats.timesDefeated++;
						audioManager.GameWin ();
                        EndBattlePanel.transform.FindChild("Text").GetComponent<Text>().text = "You won the Battle\n" + "You earned:\n " + En.GetComponent<EnemyStateMachine>().EnemyStats.coinsToGive + " coins\n" + En.GetComponent<EnemyStateMachine>().EnemyStats.expToGive + " expierence";
                        EndBattlePanel.SetActive(true);
                    }
                    
                    if (!expadd && clicksCount == 2)
                    {
                        EndBattlePanel.transform.FindChild("Text").GetComponent<Text>().text = "Click again to end the battle\n";
                        GameManager.instance.Player.GetComponent<Player>().coins += En.GetComponent<EnemyStateMachine>().EnemyStats.coinsToGive;
                        IncreaseExp.AddExperience(En.GetComponent<EnemyStateMachine>().EnemyStats.expToGive);
                        expadd = true;
                        GameObject.Find("GameManager(Clone)").GetComponent<GameManager>().saveData();
                        //levelUpText(0);
                    }
                    if (clicksCount==3)
                    {
                        
                        SceneManager.LoadScene(0);
                    }
                }
                break;
        }

		switch (HeroInput) {
		case(HeroGUI.ACTIVATE):
			if (HeroesToManage.Count > 0) {
				
				HeroesToManage [0].transform.FindChild ("Selector").gameObject.SetActive (true);
				HeroChoise = new HandleTurns ();
				ActionPanel.SetActive (true);
                    CreateAttackButtons();
				HeroInput = HeroGUI.WAITING;
			}
			break;
		case(HeroGUI.WAITING):

			break;
		case(HeroGUI.DONE):
			HeroInputDone ();
			break;

		}
	}

    public void CollectActions(HandleTurns input)
    {
        PerformList.Add(input);
    }

	public void EnemyButtons(){

        //Create buttons 
		foreach (GameObject enemy in EnemysInBattle) {
		    GameObject newButton = Instantiate (enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

		    EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine> ();

		    Text buttonText = newButton.transform.FindChild ("Text").gameObject.GetComponent<Text>();
		    buttonText.text = cur_enemy.EnemyStats.theName;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer,false);
            enemyBtn = newButton;

		}
	}

	public void Input1(){//attack button
		
		HeroChoise.Attacker = HeroesToManage[0].name;
		HeroChoise.AttackersGameObject = HeroesToManage [0];
		HeroChoise.AttackersTarget = Enemy;
		HeroChoise.Type = "Hero";
		HeroChoise.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.baseatk;
		//CollectActions(HeroChoise);
		//Debug.Log ("Attack pressed");
		ActionPanel.SetActive (false);
		GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(Enemy); // Norint kad nereiktų select enemy šitą atkomentuoti
        //EnemySelectPanel.SetActive(true); // Norint kad nereiktų selectint enemy šitą užkomentuoti
		//HeroInputDone ();
		//Hero1.SetActive (false);
	}
    public void Input2(GameObject choosenEnemy)
    {//attack button

        HeroChoise.AttackersTarget = choosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    void HeroInputDone(){
        if (!itemUsed)
        {
            PerformList.Add(HeroChoise);
        }
        else
            HeroesToManage[0].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.ACTION;
        clearAttackPanel();


		HeroesToManage [0].transform.FindChild ("Selector").gameObject.SetActive (false);
		HeroesToManage.RemoveAt (0);
		HeroInput = HeroGUI.ACTIVATE;
	}

    void clearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        ActionPanel.SetActive(false);
        MagicPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
            Destroy(atkBtn);
        atkBtns.Clear();
    }

	public void DestroyHero(int whichHero){
		if (whichHero == 1) {
			Hero1.SetActive (false);
		} else if (whichHero == 2) {
			Hero2.SetActive (false);
		}
	}
	public void CreateBossPanel(float hp)
    {
        enemyPanel.GetComponent<Text>().text = EnemysInBattle[0].GetComponent<EnemyStateMachine>().EnemyStats.theName + " HP: " + hp;
    }	
	public void DisplayBossHp(float hp){
		if (hp <= 0) {
			enemyPanel.GetComponent<Text> ().text = "Boss dead m8";
			//Enemy.SetActive (false);
            Destroy(enemyBtn);
		} else {
			enemyPanel.GetComponent<Text> ().text = EnemysInBattle[0].GetComponent<EnemyStateMachine>().EnemyStats.theName + " HP: " + hp ;
		}
	}
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Basic";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AttackButton);

        GameObject Attack1Button = Instantiate(actionButton) as GameObject;
        Text Attack1ButtonText = Attack1Button.transform.FindChild("Text").gameObject.GetComponent<Text>();
        Attack1ButtonText.text = "Skill";
        Attack1Button.GetComponent<Button>().onClick.AddListener(() => Input5());
        Attack1Button.transform.SetParent(actionSpacer,false);
        atkBtns.Add(Attack1Button);

        if (HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.UnlockedSkills.Count > 0)
        {
            foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.attacks)
            {

                GameObject SkillButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = SkillButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName+" "+magicAtk.attackCost +"MP";
                AttackButton ATB = SkillButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                SkillButton.transform.SetParent(skillSpacer, false);
                atkBtns.Add(SkillButton);

            }
            GameObject BackButton = Instantiate(backButton) as GameObject;
            BackButton.transform.FindChild("Text").gameObject.GetComponent<Text>().text = "Back";
            BackButton.transform.SetParent(skillSpacer, false);
            atkBtns.Add(BackButton);
        }
        else
        {
            Attack1Button.GetComponent<Button>().interactable = false;
        }

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(MagicAttackButton);

        if(HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.UnlockedMagic.Count>0)
        {
            foreach(BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName + " " + magicAtk.attackCost + "MP";
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                atkBtns.Add(MagicButton);

            }
            GameObject BackButton = Instantiate(backButton) as GameObject;
            BackButton.transform.FindChild("Text").gameObject.GetComponent<Text>().text = "Back";
            BackButton.transform.SetParent(magicSpacer, false);
            atkBtns.Add(BackButton);
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }

        GameObject ItemButton = Instantiate(actionButton) as GameObject;
        Text ItemButtonText = ItemButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
        ItemButtonText.text = "Consumables";
        ItemButton.GetComponent<Button>().onClick.AddListener(() => Input6());
        ItemButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(ItemButton);

        if (HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.Consumables.Count > 0)
        {
            foreach (ItemStats item in HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.Consumables)
            {
                GameObject MagicButton = Instantiate(itemButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = item.name;
                Item ATB = MagicButton.GetComponent<Item>();
                ATB.itemToUse = item;
                MagicButton.transform.SetParent(itemSpacer, false);
                atkBtns.Add(MagicButton);

            }
            GameObject BackButton = Instantiate(backButton) as GameObject;
            BackButton.transform.FindChild("Text").gameObject.GetComponent<Text>().text = "Back";
            BackButton.transform.SetParent(itemSpacer, false);
            atkBtns.Add(BackButton);
        }
        else
        {
            ItemButton.GetComponent<Button>().interactable = false;
        }
    }
    public void Input7(ItemStats item)//choosen magic attack
    {
        float temp1 = HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curHP + item.amountOfHpToGive;
        float temp2 = HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curMP + item.amountOfMpToGive;
        if (temp1 > HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.baseHP)
            HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curHP = HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.baseHP;
        else
            HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curHP = temp1;
        if (temp2 > HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.baseMP)
            HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curMP = HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.baseMP;
        else
            HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curMP = temp2;
        ItemPanel.SetActive(false);
        itemUsed = true;
        HeroInput = HeroGUI.DONE;
        
    }
    public void Input4(BaseAttack choosenMagic)//choosen magic attack
    {
        if (choosenMagic.attackName.Equals("Poison 1"))
        {
            magicPoison.SetActive(true);
        }
        else if (choosenMagic.attackName.Equals("Fire 1"))
        {
            magicFire.SetActive(true);
        }
        HeroChoise.Attacker = HeroesToManage[0].name;
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.AttackersTarget = Enemy;
        HeroChoise.Type = "Hero";

        HeroChoise.choosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        SkillPanel.SetActive(false);
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(Enemy); // Norint kad nereiktų select enemy šitą atkomentuoti
        //EnemySelectPanel.SetActive(true); // Norint kad nereiktų selectint enemy šitą užkomentuoti
    }

    public void Input3()//switching to magic attacks
    {
        ActionPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }
    public void Input5()//switching to magic attacks
    {
        ActionPanel.SetActive(false);
        SkillPanel.SetActive(true);
    }
    public void Input6()//switching to magic attacks
    {
        ActionPanel.SetActive(false);
        ItemPanel.SetActive(true);
    }
    public void UpdateHeroStats()
    {
        for (int i = 0; i < HeroesManaging.Length; i++)
        {
            HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.curATK = HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.baseATK;
            HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.curDEF = HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.baseDEF;
            HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.curHP = HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.baseHP;
            HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.curMP = HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.baseMP;
        }
    }
}
