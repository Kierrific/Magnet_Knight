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
        currentLevel = SaveDataController.Instance.current.atsLevel; 
        if (currentLevel != 4 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            spentCoins.coinsSpent += cost;
            playerStats.CoinCount -= cost;
            Debug.Log("coins spent up");

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.atsLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Attack Speed " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 5;



        }

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;
            SaveDataController.Instance.current.atsLevel++;
            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;
            isMaxLevel = true;
        }
    }

    public void MeleeDamage()
    {
        currentLevel = SaveDataController.Instance.current.meleeLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.meleeLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Melee Damage " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 10;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.meleeLevel++;


            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;

            isMaxLevel = true;
        }
    }

    public void RangeDamage()
    {
        currentLevel = SaveDataController.Instance.current.rangedLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.rangedLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Ranged Damage " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 10;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.rangedLevel++;


            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;

            isMaxLevel = true;
        }
    }


    public void AbilityDamage()
    {
        currentLevel = SaveDataController.Instance.current.abilityLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.abilityLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Ability Damage " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 10;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.abilityLevel++;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;

            isMaxLevel = true;
        }
}

    public void MovementSpeed()
    {
        currentLevel = SaveDataController.Instance.current.msLevel;
        if (currentLevel != 4 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.msLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Movement Speed " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 10;


        }

        else if (currentLevel == 4 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.msLevel++;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;

            isMaxLevel = true;
        }
    }
        

    public void Defense()
    {
        currentLevel = SaveDataController.Instance.current.defenseLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.defenseLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Defense " + currentStat + "%";
            statUpgrade.text = statText;
            currentStat += 1;


        }

        else if (currentLevel == 9 && playerStats.CoinCount >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                playerStats.CoinCount -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.defenseLevel++;

            }

            // max level reached
            level = "Max Level";
            levelText.text = level;
            costText.text = "";
            statUpgrade.text = maxText;

            isMaxLevel = true;
        }
    }

    public void Health()
    {
        currentLevel = SaveDataController.Instance.current.healthLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.healthLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Max Health " + currentStat + "%";
            statUpgrade.text = statText;
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
        currentLevel = SaveDataController.Instance.current.scrapLevel;
        if (currentLevel != 9 && playerStats.CoinCount >= cost)
        {
            // Subtract coins
            playerStats.CoinCount -= cost;
            spentCoins.coinsSpent += cost;

            // adds the level every buy
            currentLevel++;
            SaveDataController.Instance.current.scrapLevel++;
            level = "Level " + (currentLevel);
            levelText.text = level;

            // increases the cost every buy
            costText.text = "Cost " + Mathf.RoundToInt(cost * costMultiplier);
            cost = Mathf.RoundToInt(cost * costMultiplier);

            // upgrade the player stat text every buy
            statText = "Max Scrap " + currentStat + "%";
            statUpgrade.text = statText;
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

            isMaxLevel = true;
        }
    }

}
