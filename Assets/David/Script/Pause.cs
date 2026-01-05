using UnityEngine;
using UnityEngine.InputSystem; 

public class Pause : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isPaused)
        //    {
        //        Time.timeScale = 1f;
        //        isPaused = false;
        //        pauseMenu.SetActive(false);

        //    }
        //    else
        //    {
        //        Time.timeScale = 0f;
        //        isPaused = true;
        //    }
        //}
    }

    public void PauseGame(InputAction.CallbackContext ctx)
    {

        if (ctx.started)
        {
            Debug.Log("gugyugugugugugug");
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                isPaused = true;
                pauseMenu.SetActive(true);

            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

    }

}
