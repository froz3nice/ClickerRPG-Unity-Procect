using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD,
		ATTACKING
    }

	//sound variables
	public string BossDeathSound1 = "die";

	//caching
	AudioControllerScript audioManager;

    public EnemyStats EnemyStats;
    public TurnState currentState;
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
	private float animSpeed = 5f;

    private bool alive = true;
    private Vector3 startposition;
	//time for action
	private bool actionStarted = false;
	private Animation anim;
	private Animator animator;
	public GameObject HeroToAttack;
    void Start () {
		audioManager = AudioControllerScript.instance;
		if (audioManager == null) {
			Debug.LogError ("someting went horribly wrong (audioController is not found)");
		}

        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        BSM.CreateBossPanel(EnemyStats.curHP);
		if (BSM.Enemy.GetComponent<Animation> () != null) {
			anim = BSM.Enemy.GetComponent<Animation> ();
		} else {
			animator = BSM.Enemy.GetComponent<Animator> ();
		}
        startposition = transform.position;
		currentState = TurnState.WAITING;
    }

    // Update is called once per frame
    void Update()
    {
		switch (currentState) {
		case TurnState.ATTACKING:
			if (anim != null) {
				anim.Play ("Attack");
				//voras
			} else {
				animator.Play ("creature1Attack2");
				//monstras
			}
			break;

		case (TurnState.PROCESSING):
			UpgradeProcessBar ();
			break;

		case (TurnState.CHOOSEACTION):
			if (BSM.HerosInBattle.Count != 0 && BSM.HeroInputLeft == 0) {
				ChooseAction ();
				BSM.HeroInputLeft = 2 - BSM.HeroDead;
				currentState = TurnState.WAITING;
			}

			break;

		case (TurnState.WAITING):
			if (anim != null) {
				anim.Play ("Idle");
			} else {
				animator.Play ("creature1Idle");
			}
			break;

		case (TurnState.ACTION):
			if (anim != null) {
				anim.Play ("Run");
			} else {
				animator.Play ("creature1run");
			}
			if (alive) {
				StartCoroutine (TimeForAction ());
				//anim.Stop ();
				//anim.Play("Run");
			}
            //currentState = TurnState.WAITING;
			break;

		case (TurnState.DEAD):
			if (!alive) {
				return;
			} else {
				//change tag
				this.gameObject.tag = "DeadEnemy";
				if (anim != null) {
					anim.Play ("Death");
					audioManager.PlayClip3 ();
				} else {
					animator.Play ("creature1Die");
					audioManager.PlayClip3 ();
				}
				BSM.EnemysInBattle.Remove (this.gameObject);
				if (BSM.EnemysInBattle.Count > 0) {
					for (int i = 0; i < BSM.PerformList.Count; i++) {
						if (BSM.PerformList [i].AttackersGameObject == this.gameObject) {
							BSM.PerformList.Remove (BSM.PerformList [i]);
						}
						if (BSM.PerformList [i].AttackersTarget == this.gameObject) {
							BSM.PerformList [i].AttackersTarget = BSM.HerosInBattle [Random.Range (0, BSM.EnemysInBattle.Count)];
						}
					}
				}
				//color/death animation
				//this.gameObject.GetComponent<Animation>().Play("Death");
				//this.gameObject.SetActive (false);
				// alive =false
				alive = false;
				//BSM.EnemyButtons();

				BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
			}
			break;  

		}
    }
    void UpgradeProcessBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }
    void ChooseAction()
    {
        HandleTurns myAttack = new HandleTurns();
        myAttack.Attacker = "Enemy"; // vietoj "Enemy" buvo  EnemyStats.theName; ti jei kokių problemų kils bandykit atkeist
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0,BSM.HerosInBattle.Count)];

        int num = Random.Range(0, EnemyStats.attacks.Count);
        myAttack.choosenAttack = EnemyStats.attacks[num];
        Debug.Log(this.gameObject.name + " has choosen" + myAttack.choosenAttack.attackName + " and do " + myAttack.choosenAttack.attackDamage + " damage!");
        BSM.CollectActions(myAttack);
    }
	private IEnumerator TimeForAction(){

		if (actionStarted) {
			yield break;
		}
		actionStarted = true;

		//animate enemy
		Vector3 heroPosition = new Vector3
			( HeroToAttack.transform.position.x-3.5f
				,HeroToAttack.transform.position.y
				,HeroToAttack.transform.position.z);
		//BSM.Enemy.GetComponent<Animation>().Play("Run");
		while (MoveTowardsEnemy(heroPosition)) {
			yield return null;
		}

		//wait
		//BSM.Enemy.GetComponent<Animation>().Play("Attack");
		currentState = TurnState.ATTACKING;
		yield return new WaitForSeconds(1f);

        //do damage
        DoDamage();
		//BSM.Enemy.GetComponent<Animation>().Play("Run");
		//move to start
		Vector3 firstPosition = startposition;
		while (MoveTowardsStart(firstPosition)) {
			yield return null;
		}
		//reomve this perf from the list in bsm
		BSM.PerformList.RemoveAt(0);
		//reset bsm
		BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

		actionStarted =false;
		cur_cooldown = 0f;
		currentState = TurnState.PROCESSING;
		currentState = TurnState.WAITING;

	}
	private bool MoveTowardsStart(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsEnemy(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
    void DoDamage ()
    {
        float calc_dmg = EnemyStats.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_dmg);
    }

	public void TakeDamage(float getDamageAmount)
	{
        if (getDamageAmount >= EnemyStats.curDEF)
            EnemyStats.curHP -= (getDamageAmount - EnemyStats.curDEF);
		if (EnemyStats.curHP <= 0)
		{
			EnemyStats.curHP = 0;
            
        
            currentState = TurnState.DEAD;
		}
		BSM.DisplayBossHp (EnemyStats.curHP);
	}


}
