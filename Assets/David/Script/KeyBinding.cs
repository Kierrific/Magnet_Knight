using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyBinding : MonoBehaviour
{
    public GameObject targetObject;
    public PlayerInput controls;
    public ChangeButtonTMPText changeButtonTMPText;

    public Key lastKeyPressed = Key.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
           targetObject.SetActive(false);
        }
        // Check all keys in the Keyboard class
        foreach (KeyControl keyControl in Keyboard.current.allKeys)
        {
            if (keyControl.wasPressedThisFrame)
            {
                lastKeyPressed = keyControl.keyCode;
                Debug.Log("Key Pressed: " + lastKeyPressed);
                changeButtonTMPText.UpdateButtonText(keyControl.keyCode);
                break;
            }
        }
    }
}
