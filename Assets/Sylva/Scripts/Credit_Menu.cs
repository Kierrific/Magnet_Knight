using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit_Menu : MonoBehaviour
{

    private int sceneNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneNum = 0;
    }

    public void NextCredit(int num)
    {
        sceneNum = num;

        if (sceneNum == 0)
        {
            SceneManager.LoadScene("Sylva_Credit");
        }
        else if (sceneNum == 1)
        {
            SceneManager.LoadScene("Alex_Credit");
        }
        else if (sceneNum == 2)
        {
            SceneManager.LoadScene("Eleazar_Credit");
        }
        else if (sceneNum == 3)
        {
            SceneManager.LoadScene("David_Credit");
        }
        else if (sceneNum == 4)
        {
            SceneManager.LoadScene("Dominic_Credit");
        }
        else if (sceneNum == 5)
        {
            SceneManager.LoadScene("Calvin_Credit");
        }
        else if (sceneNum == 6)
        {
            SceneManager.LoadScene("Dereck_Credit");
        }
    }

    public void MenuCredit()
    {
        SceneManager.LoadScene("Start menu");
    }
}
