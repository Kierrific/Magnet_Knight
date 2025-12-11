using UnityEngine;

public class ShopReset : MonoBehaviour
{
    [Header("All Coins Spent In The Shop")]
    public int coinsSpent;

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
        StatsScript.CoinCount += coinsSpent;
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

        MeleeDamage.currentLevel = 0;

        RangedDamage.currentLevel = 0;

        AbilityDamage.currentLevel = 0;

        MovementSpeed.currentLevel = 0;

        Defense.currentLevel = 0;

        Health.currentLevel = 0;

        Scrap.currentLevel = 0;

        // Reset Stat Texts
        AttackSpeed.statText = "";
        MeleeDamage.statText = "";
        RangedDamage.statText = "";
        AbilityDamage.statText = "";
        MovementSpeed.statText = "";
        Defense.statText = "";
        Health.statText = "";
        Scrap.statText = "";

        // Reset Stat Values
        AttackSpeed.currentStat = 0;
        MeleeDamage.currentStat = 0;
        RangedDamage.currentStat = 0;
        AbilityDamage.currentStat = 0;
        MovementSpeed.currentStat = 0;
        Defense.currentStat = 0;
        Health.currentStat = 0;
        Scrap.currentStat = 0;

        // Reset Real Stat Values
        StatsScript.CooldownReduction = 0f;
        StatsScript.MeleeDamageScaler = 1;
        StatsScript.RangeDamageScaler = 1;
        StatsScript.AbilityDamageScaler = 1;
        StatsScript.MoveSpeed = 5f;
        StatsScript.Defense = 0;
        StatsScript.MaxHealth = 100;
        StatsScript.Health = 100;
        StatsScript.Scrap = 100;
        StatsScript.MaxScrap = 100;


    }
}
