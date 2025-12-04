using UnityEngine;
using TMPro;
public class QualityChange : MonoBehaviour
{
 

    public void handleInputData(int val)
    {
        if (val == 2)
        {
            low();
        }
        else if (val == 1)
        {
            medium();
        }
        else if (val == 0)
        {
            high();
        }
    }
         

    public void low()
    {
        QualitySettings.SetQualityLevel(2, true);
        Debug.Log("Quality set to  Low");
    }

    public void medium()
    {
        QualitySettings.SetQualityLevel(1, true);
        Debug.Log("Quality set to medium");
    }

    public void high()
    {
        QualitySettings.SetQualityLevel(0, true);
        Debug.Log("Quality set to high");
    }

}
