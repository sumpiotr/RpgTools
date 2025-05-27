using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurnMenuManager : MonoBehaviour
{
    CharacterScriptableObject _currentCharacterData;

    [SerializeField]
    private StringEventScriptableObject showBattleHintEvent;

    [SerializeField]
    ChoiceMenuManager battleChoiceMenuManager;




    private Dictionary<string, Action> _mainMenu;
    private List<string> _mainMenuTips;
    private Menu _skillsMenu;

    private Stack<Menu> _menuStack;

    private Action<PlayerActionTypeEnum, ActionBaseScriptableObject> _onActionChoosen;

    private void Start()
    {
        _menuStack = new Stack<Menu>();
        _mainMenu = new Dictionary<string, Action>();
        _mainMenuTips = new List<string>();
        _mainMenu.Add("Attack", () =>
        {
            _onActionChoosen(PlayerActionTypeEnum.BaseAttack, null);
        });
        _mainMenuTips.Add("Podstawowy Atak. Odnawia troche energi");
        _mainMenu.Add("Skill", () => { SetupMenu(_skillsMenu); });
        _mainMenuTips.Add("U¿yj umiejêtnoœci");
        _mainMenu.Add("Use Item", ShowItemMenu);
        _mainMenuTips.Add("U¿yj przedmiotu");
        _mainMenu.Add("Guard", () =>
        {
            _onActionChoosen.Invoke(PlayerActionTypeEnum.Guard, null);
        });
        _mainMenuTips.Add("Zablokuj po³owe nadchodz¹cych obra¿eñ");


        battleChoiceMenuManager.SetOnHover(OnMenuHover);
    }

    #region Input Controls

    public void SelectionMove(InputAction.CallbackContext context)
    {
       battleChoiceMenuManager.SelectionMove(context);
    }

    public void OnConfirmButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if(_menuStack.Count == 0) return;
        if (!_menuStack.First().items[battleChoiceMenuManager.GetSelectedIndex()].enabled) return;
        _menuStack.First().onMenuItemConfirmed(battleChoiceMenuManager.GetSelectedChoice());
    }

    public void OnCancelButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_menuStack.Count <= 1) return;

        _menuStack.Pop();
        SetupMenu(_menuStack.First());
    }

    #endregion

    public void OnMenuHover(int index)
    {
        if (_menuStack.Count == 0) return;
        if (index == -1) showBattleHintEvent.CallEvent("");
        else showBattleHintEvent.CallEvent(_menuStack.First().items[index].tip);
    }

    public void LoadMenus(PlayerCharacter playerData, Action<PlayerActionTypeEnum, ActionBaseScriptableObject> onActionChoosen)
    {
        battleChoiceMenuManager.ShowChoices();
        _currentCharacterData = playerData.GetCharacterData();
        _onActionChoosen = (PlayerActionTypeEnum p, ActionBaseScriptableObject a) => {
            UnloadMenu();
        };
        _onActionChoosen += onActionChoosen;
        battleChoiceMenuManager.SetTitle(_currentCharacterData.Name);
        List<MenuItem> attackMenuItems = new List<MenuItem>();
        foreach (ActionBaseScriptableObject skill in _currentCharacterData.Skills)
        {
            attackMenuItems.Add(new MenuItem(skill.Name, $"{skill.Description} [{skill.Cost} energi]", skill.Cost <= playerData.GetCurrentStatValue(CharacterStatsEnum.Energy)));
        }
        _skillsMenu = new Menu(attackMenuItems, (BaseChoiceMenu<string> x) =>
        {
            _onActionChoosen(PlayerActionTypeEnum.Skill, _currentCharacterData.Skills[x.Index]);
        });


        SetupMenu(new Menu(_mainMenu.Keys.ToList(), _mainMenuTips, (BaseChoiceMenu<string> x) => { _mainMenu[x.GetData()].Invoke(); }));
    }

    private void SetupMenu(Menu menu)
    {
        _menuStack.Push(menu);
        battleChoiceMenuManager.LoadChoices(menu.items.Select(x=>x.text).ToList());

        for (int i = 0; i < menu.items.Count; i++){
            if (!menu.items[i].enabled)((MenuChoice)battleChoiceMenuManager.GetChoiceByIndex(i)).DisableData();
            else ((MenuChoice)battleChoiceMenuManager.GetChoiceByIndex(i)).EnableData();
        }
        
        battleChoiceMenuManager.Focus();
    }

    private void ShowItemMenu()
    {

    }

    public void DisableMenu()
    {
        battleChoiceMenuManager.gameObject.SetActive(false);
    }

    public void EnableMenu()
    {
        battleChoiceMenuManager.gameObject.SetActive(true);
    }

    private void UnloadMenu()
    {
        //battleChoiceMenuManager.gameObject.SetActive(false);
        battleChoiceMenuManager.Unfocus();
        battleChoiceMenuManager.HideChoices();
        battleChoiceMenuManager.SetTitle("");
        _menuStack.Clear();
    }

}

struct Menu 
{
    public List<MenuItem> items;
    public Action<BaseChoiceMenu<string>> onMenuItemConfirmed;

    public Menu(List<MenuItem> items, Action<BaseChoiceMenu<string>> onMenuItemConfirmed)
    {
        this.items = items;
        this.onMenuItemConfirmed = onMenuItemConfirmed;
    }

    public Menu(List<string> texts, List<string> tips, Action<BaseChoiceMenu<string>> onMenuItemConfirmed)
    {
        this.items = new List<MenuItem>();
        for (int i = 0; i < Math.Min(texts.Count, tips.Count); i++)
        {
            this.items.Add(new MenuItem(texts[i], tips[i], true));
        }
        this.onMenuItemConfirmed = onMenuItemConfirmed;
    }
}

struct MenuItem
{
    public string text;
    public string tip;
    public bool enabled;

    public MenuItem(string text, string tip, bool enabled)
    {
        this.text = text;
        this.tip = tip;
        this.enabled = enabled;
    }
}