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

    private Action<PlayerActionTypeEnum, ActionBaseScriptableObject> _onActionChoosen;

    private void Start()
    {
        _menuQueue = new Queue<Menu>();
        _mainMenu = new Dictionary<string, Action>();
        _mainMenu.Add("Attack", () =>
        {
            _onActionChoosen(PlayerActionTypeEnum.BaseAttack, null);
        });
        _mainMenu.Add("Skill", () => { SetupMenu(_skillsMenu); });
        _mainMenu.Add("Use Item", ShowItemMenu);
        _mainMenu.Add("Guard", () =>
        {
            _onActionChoosen.Invoke(PlayerActionTypeEnum.Guard, null);
        });
    }

    #region Input Controls

    public void SelectionMove(InputAction.CallbackContext context)
    {
       battleChoiceMenuManager.SelectionMove(context);
    }

    public void OnConfirmButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _menuQueue.Last().onMenuItemConfirmed(battleChoiceMenuManager.GetSelectedChoice());
    }

    public void OnCancelButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_menuQueue.Count <= 1) return;
        _menuQueue.Dequeue();
        SetupMenu(_menuQueue.First());
    }

    #endregion

    public void LoadMenus(CharacterScriptableObject characterData, Action<PlayerActionTypeEnum, ActionBaseScriptableObject> onActionChoosen)
    {
        battleChoiceMenuManager.gameObject.SetActive(true);
        _currentCharacterData = characterData;
        _onActionChoosen = onActionChoosen;
        _onActionChoosen += (PlayerActionTypeEnum p, ActionBaseScriptableObject a) => {
            UnloadMenu();
        };
        battleChoiceMenuManager.SetTitle(characterData.Name);
        List<string> attackNames = new List<string>();
        foreach (ActionBaseScriptableObject skill in _currentCharacterData.Skills)
        {
            attackNames.Add(skill.Name);
        }
        _skillsMenu = new Menu(attackNames, (BaseChoiceMenu<string> x) =>
        {
            _onActionChoosen(PlayerActionTypeEnum.Skill, _currentCharacterData.Skills[x.Index]);
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

    private void UnloadMenu()
    {
        //battleChoiceMenuManager.gameObject.SetActive(false);
        battleChoiceMenuManager.Unfocus();
        _menuQueue.Clear();
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