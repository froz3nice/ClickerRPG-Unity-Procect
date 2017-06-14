using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour {
    public void back()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().MagicPanel.SetActive(false);
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().SkillPanel.SetActive(false);
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().ItemPanel.SetActive(false);
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().ActionPanel.SetActive(true);
    }
}
