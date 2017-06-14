using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item: MonoBehaviour
{
    public ItemStats itemToUse;

    public void useItem()
    {
            GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input7(itemToUse);
    }
}
