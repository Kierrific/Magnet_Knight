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
        SceneManager.LoadScene("StartMenu");
    }
}
