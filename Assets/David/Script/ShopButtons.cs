using TMPro;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{

    public enum Upgrades
    {
        AttackSpeed = 0,
        MeleeDamage = 1,
        RangeDamage = 2,
        AbilityDamage = 3,
        MovementSpeed = 4,
        Defense = 5,
        Health = 6,
        MaxScrap = 7,
    }

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
    [Tooltip("Set this to the upgrade the button is set to.")] [SerializeField] private Upgrades _upgradeType = Upgrades.AttackSpeed;

    //I had to change the code a litle bit because the cost would reset everytime the scene is loaded so if u bought it then reopened the shop u can get all the levels for the cheapest price


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initial UI setup
        CalculateCost();
    }

    private void CalculateCost()
    {
        int upgradeLevel = 0;
        int maxLevel = 0;
        string statsText = "";
        switch (_upgradeType)
        {
            case Upgrades.AttackSpeed:
                upgradeLevel = SaveDataController.Instance.current.atsLevel; 
                maxLevel = 5;
                currentStat = upgradeLevel * 5;
                statsText = "Attack Speed " + currentStat + "%";
                break;
            case Upgrades.MeleeDamage:
                upgradeLevel = SaveDataController.Instance.current.meleeLevel; 
                maxLevel = 10;
                currentStat = upgradeLevel * 10;
                statsText = "Melee Damage " + currentStat + "%";
                break;  

            case Upgrades.RangeDamage:
                upgradeLevel = SaveDataController.Instance.current.rangedLevel; 
                maxLevel = 10;
                currentStat = upgradeLevel * 10;
                statsText = "Ranged Damage " + currentStat + "%";
                break;  
            case Upgrades.AbilityDamage:
                upgradeLevel = SaveDataController.Instance.current.abilityLevel; 
                maxLevel = 10;
                currentStat = upgradeLevel * 10;
                statsText = "Ability Damage " + currentStat + "%";
                break; 
            case Upgrades.MovementSpeed:
                upgradeLevel = SaveDataController.Instance.current.msLevel;  //ms 10% | defense 1% | health 10% | scrap 10%
                maxLevel = 5;
                currentStat = upgradeLevel * 10;
                statsText = "Movement Speed " + currentStat + "%";
                break;  
            case Upgrades.Defense:
                upgradeLevel = SaveDataController.Instance.current.defenseLevel;
                maxLevel = 10; 
                currentStat = upgradeLevel;
                statsText = "Defense " + currentStat + "%";
                break;  
            case Upgrades.Health:
                upgradeLevel = SaveDataController.Instance.current.healthLevel; 
                maxLevel = 10;
                currentStat = upgradeLevel * 10;
                statsText = "Health " + currentStat + "%";
                break;  
            case Upgrades.MaxScrap:
                upgradeLevel = SaveDataController.Instance.current.scrapLevel; 
                maxLevel = 10;
                currentStat = upgradeLevel * 10;
                statsText = "Max Scrap " + currentStat + "%";
                break;    

        }
        for (int i = 0; i < upgradeLevel; i += 1)
        {
            cost = Mathf.RoundToInt(cost * costMultiplier);
        }

        statUpgrade.text = "";
        if (upgradeLevel == 0)
        {
            levelText.text = "Unlock";
            costText.text = "Cost " + cost;
        }
        else if (upgradeLevel < maxLevel)
        {
            costText.text = "Cost " + cost;
            levelText.text = "Level " + (upgradeLevel);
            statUpgrade.text = statsText;
            
        }
        else
        {
            levelText.text = "Max Level";
            costText.text = "";
            statUpgrade.text = maxText;
            isMaxLevel = true;
        }

       
    
    }


   public void AttackSpeed()
    {
        currentLevel = SaveDataController.Instance.current.atsLevel; 
        if (currentLevel < 4 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            spentCoins.coinsSpent += cost;
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 5;
            statText = "Attack Speed " + currentStat + "%";
            statUpgrade.text = statText;



        }

        else if (currentLevel == 4 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Melee Damage " + currentStat + "%";
            statUpgrade.text = statText;


        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Ranged Damage " + currentStat + "%";
            statUpgrade.text = statText;


        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Ability Damage " + currentStat + "%";
            statUpgrade.text = statText;


        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 4 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Movement Speed " + currentStat + "%";
            statUpgrade.text = statText;


        }

        else if (currentLevel == 4 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 1;
            statText = "Defense " + currentStat + "%";
            statUpgrade.text = statText;


        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Max Health " + currentStat + "%";
            statUpgrade.text = statText;



        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.healthLevel++;
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
        if (currentLevel < 9 && SaveDataController.Instance.current.coins >= cost)
        {
            // Subtract coins
            SaveDataController.Instance.current.coins -= cost;
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
            currentStat += 10;
            statText = "Max Scrap " + currentStat + "%";
            statUpgrade.text = statText;



        }

        else if (currentLevel == 9 && SaveDataController.Instance.current.coins >= cost)
        {
            if (isMaxLevel == false)
            {
                // Subtract coins
                SaveDataController.Instance.current.coins -= cost;
                spentCoins.coinsSpent += cost;
                SaveDataController.Instance.current.scrapLevel++;
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
