using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class KeyBinding : MonoBehaviour
{
    private const string ButtonToolTip = "Assign all control buttons here to be reactivated after assigning key binding";

    [Header("Target Object to Disable")]
    [Tooltip("Assign the Background to disable when a key is pressed")]
    public GameObject targetObject;
    [Header("Player Input")]
    [Tooltip("Assign the Player Input component here")]
    public PlayerInput controls;
    [Header("Change Button TMP Text Script")]
    [Tooltip("Assign the ChangeButtonTMPText script here to update button text when key binding")]
    public ChangeButtonTMPText changeButtonTMPText;

    [Header("All Control Buttons")]
    [Tooltip(ButtonToolTip)]
    public GameObject button1;
    [Tooltip(ButtonToolTip)]
    public GameObject button2;
    [Tooltip(ButtonToolTip)]
    public GameObject button3;
    [Tooltip(ButtonToolTip)]
    public GameObject button4;
    [Tooltip(ButtonToolTip)]
    public GameObject button5;
    [Tooltip(ButtonToolTip)]
    public GameObject button6;
    [Tooltip(ButtonToolTip)]
    public GameObject button7;
    [Tooltip(ButtonToolTip)]
    public GameObject button8;
    [Tooltip(ButtonToolTip)]
    public GameObject button9;
    [Tooltip(ButtonToolTip)]
    public GameObject button10;


    

    public Key lastKeyPressed = Key.None;

    private void Awake()
    {
    
    }
    // Update is called once per frame
    void Update()
    {
        
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

        if (Input.anyKey)
        {
            targetObject.SetActive(false);
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            button5.SetActive(true);
            button6.SetActive(true);
            button7.SetActive(true);
            button8.SetActive(true);
            button9.SetActive(true);
            button10.SetActive(true);
        }
    }


    public void KeyBind()
    { 
        
    }
}
