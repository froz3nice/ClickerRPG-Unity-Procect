using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurns {

    public string Attacker;
	public string Type;
    public GameObject AttackersGameObject;
    public GameObject AttackersTarget;

    public BaseAttack choosenAttack;

}
