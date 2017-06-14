using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash: BaseAttack
{
    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Slashes enemies with sword";
        attackDamage = 10f;
        attackCost = 0;
        levelNeeded = 5;
    }
}
