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

    private bool isMaxLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initial UI setup
        level = "Unlock";
        levelText.text = level;
        costText.text = "Cost " + cost;
        statUpgrade.text = "";
        isMaxLevel = false;
    }


   public void AttackSpeed()
    {
        if (currentLevel != 4 && playerStats.CoinCount >= cost)
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

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.CooldownReduction = 2.5f;
            if (isMaxLevel == false)
            {
            // Subtract coins
            playerStats.CoinCount -= cost;
                
            }
            isMaxLevel = true;
        }
    }

    public void MeleeDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Melee Damage " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.MeleeDamageScaler += statIncrease;
            currentStat += 10;

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MeleeDamageScaler = 2f;

            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;

            }
            isMaxLevel = true;
        }
    }

    public void RangeDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Ranged Damage " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.RangeDamageScaler += statIncrease;
            currentStat += 10;

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.RangeDamageScaler = 2f;

            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;

            }
            isMaxLevel = true;
        }
    }


    public void AbilityDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Ranged Damage " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.AbilityDamageScaler += statIncrease;
            currentStat += 10;

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.AbilityDamageScaler = 2f;

            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;

            }
            isMaxLevel = true;
        }
}

    public void MovementSpeed()
    {
        if (currentLevel != 4 && playerStats.CoinCount >= cost)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Movement Speed " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.MoveSpeed += statIncrease;
            currentStat += 10;

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MoveSpeed = 7.5f;

            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;

            }
            isMaxLevel = true;
        }
    }
        

    public void Defense()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // adds the level every buy
            currentLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Ranged Damage " + currentStat + "%";
            statUpgrade.text = statText;
            playerStats.Defense += statIncrease;
            currentStat += 1;

            // Subtract coins
            playerStats.CoinCount -= cost;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.Defense = .1f;

            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;

            }
            isMaxLevel = true;
        }
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
