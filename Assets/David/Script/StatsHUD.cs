using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class StatsHUD : MonoBehaviour
{
    public StatsScript stats;

    public Image healthBar;
    public Image ScrapBar;
    


    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = stats.Health / stats.MaxHealth;
        ScrapBar.fillAmount = stats.Scrap / stats.MaxScrap;
        
    }
}
