using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

	//Sound
	AudioControllerScript audioManager;

	private BattleStateMachine BSM;
	public enum TurnState
	{
		PROCESSING,
		ADDTOLIST,
		WAITING,
		SELECTING,
		ACTION,
		DEAD,
		ATTACKING
	}
	public PlayerStats playerStats;
	public TurnState currentState;
	private float cur_cooldown = 0f;
	private float max_cooldown = 5f;
	private Image ProgressBar;
	public GameObject Selector;
    //UeByneratir
	private bool actionStarted = false;
	public GameObject EnemyToAttack;
	private Vector3 startPosition;
	private float animSpeed = 5f;
    //dead
    private bool alive = true;
    //heroPanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;
	private	EnemyStateMachine esm;
    private Animator _anim;
	public GameObject magicFlash;

	int jumpHash = Animator.StringToHash("Run");
	private bool isMummy = false;
	// Use this for initialization
	void Start () {
		audioManager = AudioControllerScript.instance;
		if (audioManager == null) {
			Debug.LogError ("someting went horribly wrong (audioController is not found)");
		}
        //find spacer 
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.FindChild("HeroPanel").transform.FindChild("HeroPanelSpacer");
        //create panel , fill in info
        CreateHeroPanel();
		GameObject performer = GameObject.Find ("Enemy");
		esm = performer.GetComponent<EnemyStateMachine> ();
		startPosition = transform.position;
		cur_cooldown = Random.Range (0, 2.5f);
		Selector.SetActive (false);
		BSM = GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ();
		currentState = TurnState.PROCESSING;
		Debug.Log ("Hero alive");
		foreach (GameObject hero in BattleStateMachine.HeroesManaging) {
			if (hero.name == "Mummy") {
				_anim = GetComponent<Animator> ();
				_anim.SetTrigger ("Idle");
			} else if (hero.name == "Fighter") {
				_anim = GetComponent<Animator> ();
				_anim.Play ("Idle_Sword_Shield");
			}else if (hero.name == "Goblin") {
				_anim = GetComponent<Animator> ();
				_anim.Play("idle_battle");
			}else if (hero.name == "Skeleton") {
				_anim = GetComponent<Animator> ();
				_anim.Play("Idle");
			}
		}
		SetMummyAnimation ("Idle");
		SetFighterAnimation ("Idle_Sword_Shield");
		//SetGoblinAnimation ("idle");
        //Animator _anim = GetComponent<Animator>();
		//StartCoroutine (TimeForAction ());
	}

	public void SetFighterAnimation(string name){
		foreach (GameObject hero in BattleStateMachine.HeroesManaging) {
			if (hero.name == "Fighter") {
				_anim.Play (name);
			}
		}
	}

	public void SetMummyAnimation(string name){
		foreach (GameObject hero in BattleStateMachine.HeroesManaging) {
			if (hero.name == "Mummy") {
					_anim.SetTrigger (name);
			}
		}
	}

	public void SetGoblinAnimation(string name){
		foreach (GameObject hero in BattleStateMachine.HeroesManaging) {
			if (hero.name == "Goblin") {
				_anim.Play(name);
			}
		}
	}
	public void SetSkeletonAnimation(string name){
		foreach (GameObject hero in BattleStateMachine.HeroesManaging) {
			if (hero.name == "Skeleton") {
				_anim.Play(name);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch(currentState)
        {
		case TurnState.ATTACKING:
			SetMummyAnimation ("Atack_1");
			SetGoblinAnimation ("attack1");
			SetFighterAnimation ("Atack_SpearShield");
			SetSkeletonAnimation ("Attack");
			break;
            case (TurnState.PROCESSING):
                UpgradeProcessBar();
                UpdateHeroPanel();
                break;

		case (TurnState.ADDTOLIST):
			BSM.HeroesToManage.Add (this.gameObject);
			currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
				//idle
			//SetGoblinAnimation ("idle");

                break;

            case (TurnState.SELECTING):

                break;

		case (TurnState.ACTION):
                if (!BSM.itemUsed)
                {
                    SetMummyAnimation("Run");
                    SetFighterAnimation("Run_Spear_Sheild");
                    SetGoblinAnimation("run");
                    SetSkeletonAnimation("Run");
                }
                    StartCoroutine(TimeForAction());
                
                break;

            case (TurnState.DEAD):
                if(!alive)
                {
                    return;
                }
                else
                {
					SetSkeletonAnimation ("Death");
					SetGoblinAnimation ("death1");
					SetFighterAnimation ("Die_SwordShield");
					SetMummyAnimation ("DieSpear");
					audioManager.PlayClip5 ();
                    // change tag
                    this.gameObject.tag = "DeadHero";
                    // not attackable by enemy
                    BSM.HerosInBattle.Remove(this.gameObject);
                    // not managable
                    BSM.HeroesToManage.Remove(this.gameObject);
                    // deactivate the selector
                    Selector.SetActive(false);
                    // reset gui
                    BSM.ActionPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    // remove item from performlist
                    if (BSM.HerosInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                            }
                        }
                    }
                    // change color / play animation
                    //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    // reset heroinput
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                    alive = false;
                    
                }
                break;

        }
    }

	private IEnumerator TimeForAction(){

		if (actionStarted) {
			yield break;
		}
		actionStarted = true;
		Debug.Log ("HERO In Action");
        if (!BSM.itemUsed)
        {
            //animate enemy
            Vector3 heroPosition = new Vector3
			( EnemyToAttack.transform.position.x+2f
				,EnemyToAttack.transform.position.y
				,EnemyToAttack.transform.position.z);
        
            while (MoveTowardsEnemy(heroPosition))
            {
                yield return null;
            }
            currentState = TurnState.ATTACKING;
            //wait
            DoDamage();
            audioManager.SwordHit();
            yield return new WaitForSeconds(0.25f);
            audioManager.PlayClip4();
            yield return new WaitForSeconds(2f);
            //SetMummyAnimation ("run");
            //move to start
            Vector3 firstPosition = startPosition;

            currentState = TurnState.WAITING;

            while (MoveTowardsStart(firstPosition))
            {
                yield return null;
            }
            //reomve this perf from the list in BSM
            BSM.PerformList.RemoveAt(0);
            //reset BSM
        }
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;


            cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;

            BSM.HeroInputLeft--;
            if (BSM.HeroInputLeft == 0)
            {// jei visi herojai atakavo per ejima
                //BSM.HeroInputLeft = 2 - BSM.HeroDead;
                esm.currentState = EnemyStateMachine.TurnState.ACTION;
                esm.currentState = EnemyStateMachine.TurnState.CHOOSEACTION;
            }
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        actionStarted = false;
        BSM.itemUsed = false;
        BSM.magicFire.SetActive (false);
		BSM.magicPoison.SetActive (false);

    }
	private bool MoveTowardsStart(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsEnemy(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}

	void UpgradeProcessBar(){
		cur_cooldown = cur_cooldown + Time.deltaTime;
		float calc_cooldown = cur_cooldown / max_cooldown;
		ProgressBar.transform.localScale = new Vector3 (Mathf.Clamp (calc_cooldown, 0, 1),
			ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
		if (cur_cooldown >= max_cooldown) {
			currentState = TurnState.ADDTOLIST;
		}
	}
    public void TakeDamage(float getDamageAmount)
    {
        if (getDamageAmount >= playerStats.curDEF)
        playerStats.curHP -= (getDamageAmount-playerStats.curDEF);
        if (playerStats.curHP <= 0)
        {
            playerStats.curHP = 0;
            /*int which = 0;
			if (playerStats.theName == "Hero 1") {
				which = 1;
			} else if (playerStats.theName == "Hero 2") {
				which = 2;
			}
			BSM.DestroyHero (which);*/
            BSM.HeroDead++;
            BSM.HeroInputLeft--;
            currentState = TurnState.DEAD;
        }
        UpdateHeroPanel();
    }
    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();
        stats.HeroName.text = playerStats.theName;
        stats.HeroHP.text = "HP: " + playerStats.curHP + "/" + playerStats.baseHP;//HP: 456
        stats.HeroMP.text = "MP: " + playerStats.curMP + "/" + playerStats.baseMP;//MP: 456

        ProgressBar = stats.ProgressBar;


        HeroPanel.transform.SetParent(HeroPanelSpacer, false);


    }
    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "HP: " + playerStats.curHP  + "/" + playerStats.baseHP;//HP: 456
        stats.HeroMP.text = "MP: " + playerStats.curMP + "/" + playerStats.baseMP;//MP: 456
        float hp = playerStats.curHP / playerStats.baseHP;
        ProgressBar.transform.localScale = new Vector3(hp, ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        //ProgressBar.fillAmount = playerStats.curHP/playerStats.baseHP;
    }

	void DoDamage ()
	{
		float calc_dmg = playerStats.curATK+BSM.PerformList[0].choosenAttack.attackDamage;

        playerStats.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
        UpdateHeroPanel();
		EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_dmg);
	}

}