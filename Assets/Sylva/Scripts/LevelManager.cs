using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    private void Awake()
    {
        main = this;
    }
}
