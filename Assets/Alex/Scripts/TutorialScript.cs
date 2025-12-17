using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour
{

    [Tooltip("The list of strings for the tutorial text")] [SerializeField] private List<string> _tutorialTexts;
    [Tooltip("Set this to the tutorial text game object.")] [SerializeField] private TMP_Text textGO;
    private int _textIndex = 0;
    void Start()
    {
        textGO.text = _tutorialTexts[0];
    }



    public void OpenTutorial()
    {
        gameObject.SetActive(true);
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
        
    }

    public void NextTutorial()
    {
        if (_textIndex + 2 < _tutorialTexts.Count)
        {
            _textIndex++;
            textGO.text = _tutorialTexts[_textIndex];
        }
    }

    public void PreviousTutorial()
    {
        if (_textIndex > 0)
        {
            _textIndex--;
            textGO.text = _tutorialTexts[_textIndex];
        }
    }

}
