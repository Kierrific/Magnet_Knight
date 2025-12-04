using UnityEngine;

public class Fullscreen : MonoBehaviour
{
    public void Changed()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("FullScreen Changed");
    }
}
