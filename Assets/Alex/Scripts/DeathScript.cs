using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{

    public void OpenDeathMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    [ContextMenu("Swap Scenes")]
    public void ReturnToMM()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Start menu");
    }
}