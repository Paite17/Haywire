using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string unitName;
    public string unitDescription;
    public int level;
    public float damage;
    public float baseDamage;
    public float defence;
    public float baseDefence;
    public int agility;
    public int baseAgility;
    public int luck;
    public int maxHP;
    public int currentHP;
    public int currentXP;
    public int currentSP;
    public int maxSP;
    public int toNextLevel;
    public UnitType unitType;
    public int dmgCountdown;
    public int defCountdown;
    public int agiCountdown;
    public bool dmgBoostActive;
    public bool defBoostActive;
    public int agiBoostActive;

    // constructor for the data
    public PlayerData(Unit player)
    {
        damage = player.damage;
        level = player.level;
        maxHP = player.maxHP;
        currentHP = player.currentHP;
        currentXP = player.currentXP;
        toNextLevel = player.toNextLevel;
        unitName = player.unitName;
        currentSP = player.currentSP;
        maxSP = player.maxSP;
        defence = player.defence;
        agility = player.agility;
        luck = player.luck;
        baseDamage = player.baseDamage;
        baseDefence = player.baseDefence;
        baseAgility = player.baseAgility;
        unitType = player.unitType;

    }
}
