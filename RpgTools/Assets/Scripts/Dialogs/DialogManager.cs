using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Rendering;
using System;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset simpleMessage;

    private Action onDialogEnded;

    private InputMapEnum _previousInput = InputMapEnum.Player;

    #region UI variables
    [Header("Dialog UI")]
    [SerializeField]
    GameObject dialogUI;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Image portrait;

    [Header("Choices")]
    [SerializeField]
    private ChoiceMenuManager dialogChoicesManager;

    #endregion 

    #region letter animation variables
    private IEnumerator _animationCoroutine;
    private bool _animating = false;
    private const float letterDelay = 0.05f;
    #endregion

    private Story _currentStory;

    public static DialogManager Instance = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogText.gameObject.SetActive(true);
        dialogUI.SetActive(false);
        dialogChoicesManager.gameObject.SetActive(false);
    }

    public void StartDialog(string data, Action onEnded)
    {
        StartDialog(new Story(data), onEnded);
    }

    public void StartDialog(string data)
    {
        StartDialog(new Story(data), null);
    }

    public void ShowSimpleMessage(string message, Action onEnded)
    {
        Story story = new Story(simpleMessage.text);
        story.variablesState["message"] = message;
        StartDialog(story, onEnded);
    }

    public void ShowSimpleMessage(string message)
    {
       ShowSimpleMessage(message, null);
    }

    private void StartDialog(Story story, Action onEnded)
    {
        if (_currentStory != null) return;

        _previousInput = InputManager.Instance.GetInputMap();
        InputManager.Instance.ChangeMapping(InputMapEnum.Dialog);

        dialogUI.SetActive(true);

        _currentStory = story;
        onDialogEnded = onEnded;
        SetFunctions(_currentStory.globalTags);
        ContinueDialog();
    }

    private void SetFunctions(List<string> tags)
    {
        if (tags == null) return;
        foreach (string tag in tags) 
        {
            if (tag.Length <= 5) continue;
            if(tag.Substring(0, 5) == "using")
            {
                string functionName = tag.Substring(5);
                functionName = functionName.Trim();
                if(functionName == "SetPortrait")
                {
                    _currentStory.BindExternalFunction(functionName, (string name) => {
                        SetPortrait(name);
                    });
                }
            }
        }
    }

    private void ContinueDialog()
    {
        if (_currentStory == null) return;
        if (!_currentStory.canContinue)
        {
            EndDialog();
            return;
        }

        string text = _currentStory.Continue();

        if(_currentStory.currentChoices.Count > 0) 
        {
            dialogChoicesManager.gameObject.SetActive(true);
            dialogText.gameObject.SetActive(false);

            dialogChoicesManager.SetTitle(text);

            List<string> choices = new List<string>();
            foreach (Choice choice in _currentStory.currentChoices) 
            {
                choices.Add(choice.text);
            }
            dialogChoicesManager.LoadChoices(choices);
            dialogChoicesManager.Focus();
        }
        else
        {
            if (text.Trim() == "")
            {
                ContinueDialog();
                return;
            }
            _animationCoroutine = TextAnimationCoroutine(text);
            StartCoroutine(_animationCoroutine);
        }
    }

    private void EndDialog()
    {
        _currentStory = null;
        dialogUI.SetActive(false);
        InputManager.Instance.ChangeMapping(_previousInput);
        if(onDialogEnded != null)
        {
            onDialogEnded();
            //onDialogEnded = null;
        }
    }


    public void ConfirmButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_currentStory == null) return;
        if (_animating)
        {
            StopCoroutine(_animationCoroutine);
            _animating = false;
            dialogText.maxVisibleCharacters = _currentStory.currentText.Length;
            return;
        }
        if(_currentStory.currentChoices.Count > 0)
        {
            int choiceIndex = dialogChoicesManager.GetSelectedIndex();
            dialogChoicesManager.gameObject.SetActive(false);
            dialogText.gameObject.SetActive(true);
            _currentStory.ChooseChoiceIndex(choiceIndex);
        }
        ContinueDialog();
    }

    public void SelectionMove(InputAction.CallbackContext context)
    {
        if(_currentStory == null) return;
        dialogChoicesManager.SelectionMove(context);
    }

    private void SetPortrait(string name)
    {
        portrait.sprite = Resources.Load<Sprite>($"Images/{name}");
    }

    private IEnumerator TextAnimationCoroutine(string text)
    {
        _animating = true;
        dialogText.text = text.Replace("@", "");
        dialogText.maxVisibleCharacters = 0;
        foreach(char c in text)
        {
            if(c != '@')dialogText.maxVisibleCharacters += 1;
            yield return new WaitForSeconds(letterDelay);
        }

        yield return new WaitForSeconds(0.1f);
        _animating = false;
    }


}
