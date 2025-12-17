using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public void OpenDeathMenu()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToMM()
    {
        SceneManager.LoadScene(0);
    }
}