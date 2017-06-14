using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseExp{

    private static LevelUp LevelUpScript = new LevelUp();
    public static void AddExperience(int amount)
    {
        for (int i = 0; i < BattleStateMachine.HeroesManaging.Length; i++)
        {
            BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.CurExp += amount;
        }
        CheckIfPlayerLeveledUp();
            
    }
    private static void CheckIfPlayerLeveledUp()
    {
        for (int i = 0; i < BattleStateMachine.HeroesManaging.Length; i++)
        {
            if (BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.CurExp >=
               BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp)
                //for (int j = 0; j < BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.CurExp/ BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp; j++)
                    LevelUpScript.LevelUpCharacter(i);
        }
    }



}
