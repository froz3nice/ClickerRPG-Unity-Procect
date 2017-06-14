using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

    public BaseAttack magicAttackToPerform;

    public void CastMagicAttack()
    {
        if (GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().HeroesToManage[0].GetComponent<HeroStateMachine>().playerStats.curMP >= magicAttackToPerform.attackCost)
            GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input4(magicAttackToPerform);
    }
}
