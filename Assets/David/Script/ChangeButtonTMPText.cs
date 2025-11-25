using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChangeButtonTMPText : MonoBehaviour
{
    public Button myButton;
    public TMP_Text buttonText;
    public KeyBinding KeyBindings;

    public void UpdateButtonText(Key key)
    {
        buttonText.text = key.ToString();
        Debug.Log("Button text updated to: " + buttonText.text);
    }
}