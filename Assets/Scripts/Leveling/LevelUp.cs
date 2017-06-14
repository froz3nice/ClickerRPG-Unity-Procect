using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp {
    public int maxLvl = 50;
    //Level up character and determine his current CurExp to not lose any CurExp while leveling
    public void LevelUpCharacter(int i)
    {
        PlayerStats CharStats = BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats;
        //Tikrina ar CurExp viršija limita ar yra lygus reikiamam
        if (CharStats.CurExp > CharStats.RequiredExp)
            CharStats.CurExp -= CharStats.RequiredExp;
        else
            CharStats.CurExp = 0;
        //tikrina ar char max lvl
        if (CharStats.CharacterLevel < maxLvl)
            CharStats.CharacterLevel++;
        else
            CharStats.CharacterLevel = maxLvl;
        //Pakelti char'o stat'us
        IncreaseBaseStats(i);
        SetCurrentStats(i);
        
        //Išvesti kad chars pakilo lvl
        GameObject.Find("BattleCanvas").transform.FindChild("EndBattlePanel").transform.FindChild("Text").GetComponent<Text>().text
            += CharStats.theName + " leveled up to level " + CharStats.CharacterLevel + "\n";

        //Atrakinti skills/magijas
        UnlockSkills(i);
        UnlockMagic(i);

        //Nustatyti sekančio lvl CurExp
        DetermineRequiredCurExp(i);

        
    }
    private void DetermineRequiredCurExp(int i)
    {
        int temp = (BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.CharacterLevel * 100) + 25;
        BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats.RequiredExp = temp;
    }
    private void UnlockSkills(int i)
    {
        PlayerStats CharStats = BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats;
        foreach(BaseAttack atk in CharStats.attacks)
        {
            if (atk.levelNeeded <= CharStats.CharacterLevel && !CharStats.UnlockedSkills.Contains(atk))
            {
                CharStats.UnlockedSkills.Add(atk);
                GameObject.Find("BattleCanvas").transform.FindChild("EndBattlePanel").transform.FindChild("Text").GetComponent<Text>().text
            += CharStats.theName + " unlocked skill " + atk.attackName + "\n";
            }
        }
    }
    private void UnlockMagic(int i)
    {
        PlayerStats CharStats = BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats;
        foreach (BaseAttack atk in CharStats.MagicAttacks)
        {
            if (atk.levelNeeded <= CharStats.CharacterLevel && !CharStats.UnlockedMagic.Contains(atk))
            {
                CharStats.UnlockedMagic.Add(atk);
                GameObject.Find("BattleCanvas").transform.FindChild("EndBattlePanel").transform.FindChild("Text").GetComponent<Text>().text
            += CharStats.theName + " unlocked magic " + atk.attackName + "\n";
            }
                
        }
    }
    private void IncreaseBaseStats(int i)
    {
        PlayerStats CharStats = BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats;
        CharStats.agility += 2;
        CharStats.baseATK += 5;
        CharStats.baseDEF += 5;
        CharStats.baseHP += 10;
        CharStats.baseMP += 5;
        CharStats.dexterity += 2;
        CharStats.intellect += 2;
        CharStats.stamina += 2;
    }
    private void SetCurrentStats(int i)
    {
        PlayerStats CharStats = BattleStateMachine.HeroesManaging[i].GetComponent<HeroStateMachine>().playerStats;
        //HP
        CharStats.baseHP += CharStats.stamina * 5;
        CharStats.curHP = CharStats.baseHP;
        //MP
        CharStats.baseMP += CharStats.intellect * 2;
        CharStats.curMP = CharStats.baseMP;
        //Defense
        CharStats.baseDEF += CharStats.agility * 2;
        CharStats.curDEF = CharStats.baseDEF;
        //DMG
        CharStats.baseATK += CharStats.dexterity * 2;
        CharStats.curATK = CharStats.baseATK;

    }
}
