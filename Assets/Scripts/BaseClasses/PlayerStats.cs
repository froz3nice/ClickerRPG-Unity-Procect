using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats: BaseClass
{

    public int stamina=10;
    public int intellect=5;
    public int dexterity=10;
    public int agility=5;
    
    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();
    public List<BaseAttack> UnlockedSkills = new List<BaseAttack>();
    public List<BaseAttack> UnlockedMagic = new List<BaseAttack>();
    public List<ItemStats> Consumables = new List<ItemStats>();
    public BaseAttack baseatk;

    public int CurExp;
    public int RequiredExp;
    public int CharacterLevel;

}

