using TMPro;
using UnityEngine;

public class CoinShopUpdate : MonoBehaviour
{

    public TMP_Text coinText;
    public StatsScript playerStats;

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Coins: " + playerStats.CoinCount;
    }
}
