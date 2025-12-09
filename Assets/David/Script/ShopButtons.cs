using TMPro;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    [Header("UI Texts")]
    public TMP_Text costText;
    public TMP_Text levelText;
    public TMP_Text statUpgrade;

    [Header("Upgrade Info")]
    public int cost;
    public int currentLevel;
    public int currentStat;
    public float costMultiplier = 1.5f;
    public float statIncrease;


    [Header("Player Stats Reference")]
    public StatsScript playerStats;

    [Header("String Texts")]
    public string level;
    public string statText;
    public string maxText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initial UI setup
        level = "Unlock";
        levelText.text = level;
        costText.text = "Cost " + cost;
        statUpgrade.text = "";
    }


   public void AttackSpeed()
    {
        if (currentLevel != 4)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Attack Speed " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.CooldownReduction += statIncrease;
            currentStat += 5;


        }

        else
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.CooldownReduction = 2.5f;
        }
    }

    public void MeleeDamage()
    {
        if (currentLevel != 4)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Melee Damage +" + currentStat;
            statUpgrade.text = statText;
            playerStats.CooldownReduction += statIncrease;
            currentStat += 5;


        }

        else
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.CooldownReduction = 2.5f;
        }
    }

    public void RangeDamage()
    { 
    
    }

    public void AbilityDamage()
    { 
    
    }

    public void MovementSpeed()
    { 
    
    }

    public void Defense()
    { 
    
    }

    public void Health()
    {

    }

    public void Protection()
    { 
    
    }

    public void UniversalDamage()
    { 
    
    }
}
