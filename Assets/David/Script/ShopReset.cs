using UnityEngine;

public class ShopReset : MonoBehaviour
{
    [Header("All Coins Spent In The Shop")]
    public int coinsSpent
    {
        get { return SaveDataController.Instance.current.coinsSpent; } 
        set { SaveDataController.Instance.current.coinsSpent = value; }
    }

    [Header("Reference To Your Coin Value")]
    public StatsScript StatsScript;

    [Header("All Shop Items Reference")]

    public ShopButtons AttackSpeed;
    public ShopButtons MeleeDamage;
    public ShopButtons RangedDamage;
    public ShopButtons AbilityDamage;
    public ShopButtons MovementSpeed;
    public ShopButtons Defense;
    public ShopButtons Health;
    public ShopButtons Scrap;


    public void ResetShop()
    {
        SaveDataController.Instance.current.coins += coinsSpent;
        coinsSpent = 0;

        // Reset All Shop Item Costs To Default Values
        AttackSpeed.cost = 50;
        MeleeDamage.cost = 100;
        RangedDamage.cost = 100;
        AbilityDamage.cost = 100;
        MovementSpeed.cost = 50;
        Defense.cost = 50;
        Health.cost = 50;
        Scrap.cost = 100;


        // Reset All Shop Item Level Texts To 0
        AttackSpeed.levelText.text = "Unlock";
        AttackSpeed.level = "Unlock";

        MeleeDamage.levelText.text = "Unlock";
        MeleeDamage.level = "Unlock";

        RangedDamage.levelText.text = "Unlock";
        RangedDamage.level = "Unlock";

        AbilityDamage.levelText.text = "Unlock";
        AbilityDamage.level = "Unlock";

        MovementSpeed.levelText.text = "Unlock";
        MovementSpeed.level = "Unlock";

        Defense.levelText.text = "Unlock";
        Defense.level = "Unlock";

        Health.levelText.text = "Unlock";
        Health.level = "Unlock";

        Scrap.levelText.text = "Unlock";
        Scrap.level = "Unlock";


        //Reset Current Levels To 0
        AttackSpeed.currentLevel = 0;
        SaveDataController.Instance.current.atsLevel = 0;

        MeleeDamage.currentLevel = 0;
        SaveDataController.Instance.current.meleeLevel = 0;

        RangedDamage.currentLevel = 0;
        SaveDataController.Instance.current.rangedLevel = 0;

        AbilityDamage.currentLevel = 0;
        SaveDataController.Instance.current.abilityLevel = 0;

        MovementSpeed.currentLevel = 0;
        SaveDataController.Instance.current.msLevel = 0;

        Defense.currentLevel = 0;
        SaveDataController.Instance.current.defenseLevel = 0;

        Health.currentLevel = 0;
        SaveDataController.Instance.current.healthLevel = 0;

        Scrap.currentLevel = 0;
        SaveDataController.Instance.current.scrapLevel = 0;

        // Reset Stat Texts
        AttackSpeed.statUpgrade.text = "";
        MeleeDamage.statUpgrade.text = "";
        RangedDamage.statUpgrade.text = "";
        AbilityDamage.statUpgrade.text = "";
        MovementSpeed.statUpgrade.text = "";
        Defense.statUpgrade.text = "";
        Health.statUpgrade.text = "";
        Scrap.statUpgrade.text = "";

        // Reset Cost Texts
        AttackSpeed.costText.text = "Cost 50";   
        MeleeDamage.costText.text = "Cost 100";
        RangedDamage.costText.text = "Cost 100";
        AbilityDamage.costText.text = "Cost 100";
        MovementSpeed.costText.text = "Cost 50";
        Defense.costText.text = "Cost 50";
        Health.costText.text = "Cost 50";
        Scrap.costText.text = "Cost 100";


        // Reset Stat Values
        // AttackSpeed.currentStat = 5;
        // MeleeDamage.currentStat = 10;
        // RangedDamage.currentStat = 10;
        // AbilityDamage.currentStat = 10;
        // MovementSpeed.currentStat = 10;
        // Defense.currentStat = 1;
        // Health.currentStat = 10;
        // Scrap.currentStat = 10; 
        AttackSpeed.currentStat = 0;
        MeleeDamage.currentStat = 0;
        RangedDamage.currentStat = 0;
        AbilityDamage.currentStat = 0;
        MovementSpeed.currentStat = 0;
        Defense.currentStat = 0;
        Health.currentStat = 0;
        Scrap.currentStat = 0; 

        // // Reset Real Stat Values
        // StatsScript.CooldownReduction = 0f;
        // StatsScript.MeleeDamageScaler = 1;
        // StatsScript.RangeDamageScaler = 1;
        // StatsScript.AbilityDamageScaler = 1;
        // StatsScript.MoveSpeed = 5f;
        // StatsScript.Defense = 0;
        // StatsScript.MaxHealth = 100;
        // StatsScript.Health = 100;
        // StatsScript.Scrap = 100;
        // StatsScript.MaxScrap = 100;


    }
}
