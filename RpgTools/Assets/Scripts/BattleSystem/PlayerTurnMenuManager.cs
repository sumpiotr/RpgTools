using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurnMenuManager : MonoBehaviour
{
    CharacterScriptableObject _currentCharacterData;

    [SerializeField]
    ChoiceMenuManager battleChoiceMenuManager;




    private Dictionary<string, Action> _mainMenu;
    private Menu _skillsMenu;

    private Queue<Menu> _menuQueue;

    private Action<ActionBaseScriptableObject> _onActionChoosen;

    private void Start()
    {
        _mainMenu = new Dictionary<string, Action>();
        _mainMenu.Add("Attack", () =>
        {

        });
        _mainMenu.Add("Skill", () => { SetupMenu(_skillsMenu); });
        _mainMenu.Add("Use Item", ShowItemMenu);
        _mainMenu.Add("Defense", ChooseDefense);
    }

    #region Input Controls

    public void SelectionMove(InputAction.CallbackContext context)
    {
       battleChoiceMenuManager.SelectionMove(context);
    }

    public void OnConfirmButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _menuQueue.First().onMenuItemConfirmed(battleChoiceMenuManager.GetSelectedChoice());
    }

    public void OnCancelButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_menuQueue.Count <= 1) return;
        _menuQueue.Dequeue();
        SetupMenu(_menuQueue.First());
    }

    #endregion

    public void LoadMenus(CharacterScriptableObject characterData, Action<ActionBaseScriptableObject> onActionChoosen)
    {
        _currentCharacterData = characterData;
        _onActionChoosen = onActionChoosen;
        battleChoiceMenuManager.SetTitle(characterData.Name);
        List<string> attackNames = new List<string>();
        foreach (AttackScriptableObject attack in _currentCharacterData.Skills)
        {
            attackNames.Add(attack.Name);
        }
        _skillsMenu = new Menu(attackNames, (BaseChoiceMenu<string> x) =>
        {
            _onActionChoosen(_currentCharacterData.Skills[x.Index]);
        });

        SetupMenu(new Menu(_mainMenu.Keys.ToList(), (BaseChoiceMenu<string> x) => { _mainMenu[x.GetData()].Invoke(); }));
    }

    private void SetupMenu(Menu menu)
    {
        _menuQueue.Enqueue(menu);
        battleChoiceMenuManager.LoadChoices(menu.options);
        battleChoiceMenuManager.Focus();
    }

    private void ShowItemMenu()
    {

    }

    private void ChooseDefense()
    {

    }
}

struct Menu 
{
    public List<string> options;
    public Action<BaseChoiceMenu<string>> onMenuItemConfirmed;

    public Menu(List<string> options, Action<BaseChoiceMenu<string>> onMenuItemConfirmed)
    {
        this.options = options;
        this.onMenuItemConfirmed = onMenuItemConfirmed;
    }
}