using System;
using UnityEngine;
using System.Collections;

public class EnemySelectButton : MonoBehaviour
{
	public GameObject EnemyPrefab;
	public void EnemySelect ()
	{
		GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ().Input2(EnemyPrefab);
	}
}

