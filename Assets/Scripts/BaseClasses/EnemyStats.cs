using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats: BaseClass  {

    public enum Type
    {
        FIRE,
        WATER,
        AIR,
        EARTH,
        LIGHT,
        DARK
    }
    public enum Rarity
    {
        MINION,
        COMMON,
        UNCOMMON,
        BOSS
    }
    public Type EnemyType;
    public Rarity EnemyRarity;
    public int coinsToGive;
    public int expToGive;
    public int timesDefeated;

}
