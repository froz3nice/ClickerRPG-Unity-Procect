using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass{

    public string theName;

    public float baseHP=100;
    public float curHP;

    public float baseMP=10;
    public float curMP;

    public float baseATK=10;
    public float curATK;

    public float baseDEF=10;
    public float curDEF;

    public List<BaseAttack> attacks = new List<BaseAttack> ();

    
}
