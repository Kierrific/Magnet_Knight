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


    [Header("Stats Reference")]
    public StatsScript playerStats;
    public ShopReset spentCoins;

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
            // Subtract coins
            spentCoins.coinsSpent += cost;
            playerStats.CoinCount -= cost;
            Debug.Log("coins spent up");

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

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;
            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.CooldownReduction = .25f;
            isMaxLevel = true;
        }
    }

    public void MeleeDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MeleeDamageScaler = 2f;

            isMaxLevel = true;
        }
    }

    public void RangeDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.RangeDamageScaler = 2f;

            isMaxLevel = true;
        }
    }


    public void AbilityDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.AbilityDamageScaler = 2f;

            isMaxLevel = true;
        }
}

    public void MovementSpeed()
    {
        if (currentLevel != 4 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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


        }

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MoveSpeed = 7.5f;

            isMaxLevel = true;
        }
    }
        

    public void Defense()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.Defense = .1f;

            isMaxLevel = true;
        }
    }

    public void Health()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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
            playerStats.MaxHealth += (int)statIncrease;
            playerStats.Health += (int)statIncrease;
            currentStat += 10;



        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MaxHealth = 200;
            playerStats.Health = 200;

            isMaxLevel = true;
        }
    }

    /*public void Protection()
    { 
    idk what protection is might delete later or put it in.
    }
    */
    public void UniversalDamage()
    {
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

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
            playerStats.MaxScrap += (int)statIncrease;
            playerStats.Scrap += (int)statIncrease;
            currentStat += 10;



        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            playerStats.MaxScrap = 200;
            playerStats.Scrap = 200;

            isMaxLevel = true;
        }
    }

}
