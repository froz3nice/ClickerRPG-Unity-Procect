using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing: BaseAttack {

    public HammerSwing()
    {
        attackName = "Hammer Swing";
        attackDescription = "Executes a powerful hammer swing";
        attackDamage = 15f;
        attackCost = 0;
        levelNeeded = 9;
    }

}
