using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    [TextArea] public string unitDescription;
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
    public bool crit;
    public bool miss;

    public List<PlayerMove> listOfUsablePlayerMoves;

    // load player data from save system
    public static void LoadPlayer(Unit unit)
    {
        Debug.Log("Loading player data in unit!");
        PlayerData data = SaveSystem.LoadPlayer();

        unit.unitName = data.unitName;
        unit.level = data.level;
        unit.damage = data.damage;
        unit.baseDamage = data.baseDamage;
        unit.defence = data.defence;
        unit.baseDefence = data.baseDefence;
        unit.agility = data.agility;
        unit.baseAgility = data.baseAgility;
        unit.luck = data.luck;
        unit.maxHP = data.maxHP;
        unit.currentHP = data.currentHP;
        unit.currentXP = data.currentXP;
        unit.currentSP = data.currentSP;
        unit.maxSP = data.maxSP;
        unit.toNextLevel = data.toNextLevel;
        unit.unitType = data.unitType;
    }

    // deal damage to unit when called, returning bool determines if the unit being attacked dies or not
    public bool TakeDamage(float dmgAmount, float defAmount, int agility, int luck, int enemyAgility, float atkStrength)
    {
        
        int critChance = Random.Range(1, 1000);
        int missChance = Random.Range(1, 1000);
        Debug.Log("missChance = " + missChance);
        Debug.Log("critChance = " + critChance);

        Debug.Log("atkStrength before division = " + atkStrength);
        // imagine having your damage multiplied by 100
        atkStrength /= 10;
        Debug.Log("atkStrength after division = " + atkStrength);

        float finalDamage = dmgAmount * atkStrength;
        Debug.Log("finalDamage = " + finalDamage);

        // check for missed attack
        if (enemyAgility > agility)
        {
            if (missChance == 1)
            {
                miss = true;
                FindObjectOfType<AudioManager>().Play("miss");
                Debug.Log("Unit missed!");
                finalDamage = 0;
            }
        }

        // check for crit
        if (critChance < luck)
        {
            if (critChance == 1)
            {
                crit = true;
                Debug.Log("Unit hit a crit!");
                FindObjectOfType<AudioManager>().Play("crirtical");
                finalDamage *= 4;
            }
        }

        // this is a formular i found on the internet and subsequently used in Sussy Dungeons (like most of the code here ¬-¬)
        // should incorporate input accuracy when that is made
        float thisAttack = finalDamage * (100 / (100 + defAmount));

        // make sure that if accuracy is 0 then the attack will be 0
        if (atkStrength <= 0)
        {
            Debug.Log("Attack is lower than 0");
            thisAttack = 0;
        }

        Debug.Log("thisAttack without fallback = " + thisAttack);

        currentHP -= (int)thisAttack;
        Debug.Log("Attack did " + thisAttack + " damage!");



        // return value based on if the unit targeted is dead
        if (currentHP < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        if (currentSP > maxSP)
        {
            currentSP = maxSP;
        }
    }

    // heals target unit, should be used for healing-type items
    public void Heal(float amount)
    {
        currentHP += (int)amount;
        // correct HP value
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void SavePlayer()
    {
        Debug.Log("Saving player data!");
        SaveSystem.SavePlayer(this);
    }

    // counts the amount of turns left until buff/debuff ends
    public bool UpdateStatCounter()
    {
        if (dmgCountdown >= 1)
        {
            dmgCountdown--;

            if (dmgCountdown == 0)
            {
                damage = baseDamage;

                return true;
            }

            return false;
        }

        if (defCountdown >= 1)
        {
            defCountdown--;

            if (defCountdown == 0)
            {
                defence = baseDefence;
                return true;
            }

            return false;
        }

        if (agiCountdown >= 1)
        {
            agiCountdown--;

            if (agiCountdown == 0)
            {
                agility = baseAgility;
                return true;
            }

            return false;
        }

        return false;
    }
}

public enum UnitType
{
    PLAYER,
    ENEMY,
    BOSS
}
