using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartButton()
    { 
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void QuitToStart()
        {
        SceneManager.LoadScene("Start menu");
    }

    public void ShopButton()
    { 
        SceneManager.LoadScene("Shop");
    }

    public void SettingsButton()
    {
        SceneManager.LoadScene("Settings");
    }
}
