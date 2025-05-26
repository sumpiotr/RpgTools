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

    private Queue<Menu> _menuQueue;

    private Action<PlayerActionTypeEnum, ActionBaseScriptableObject> _onActionChoosen;

    private void Start()
    {
        _menuQueue = new Queue<Menu>();
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

    public void OnMenuHover(int index)
    {
        if (_menuQueue.Count == 0) return;
        if (index == -1) showBattleHintEvent.CallEvent("");
        else showBattleHintEvent.CallEvent(_menuQueue.Last().tips[index]);
    }

    public void LoadMenus(CharacterScriptableObject characterData, Action<PlayerActionTypeEnum, ActionBaseScriptableObject> onActionChoosen)
    {
        battleChoiceMenuManager.ShowChoices();
        _currentCharacterData = characterData;
        _onActionChoosen = (PlayerActionTypeEnum p, ActionBaseScriptableObject a) => {
            UnloadMenu();
        };
        _onActionChoosen += onActionChoosen;
        battleChoiceMenuManager.SetTitle(characterData.Name);
        List<string> attackNames = new List<string>();
        List<string> attackDescriptions = new List<string>();
        foreach (ActionBaseScriptableObject skill in _currentCharacterData.Skills)
        {
            attackNames.Add(skill.Name);
            attackDescriptions.Add(skill.Description);
        }
        _skillsMenu = new Menu(attackNames, attackDescriptions, (BaseChoiceMenu<string> x) =>
        {
            _onActionChoosen(PlayerActionTypeEnum.Skill, _currentCharacterData.Skills[x.Index]);
        });

        SetupMenu(new Menu(_mainMenu.Keys.ToList(), _mainMenuTips, (BaseChoiceMenu<string> x) => { _mainMenu[x.GetData()].Invoke(); }));
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
        _menuQueue.Clear();
    }

}

struct Menu 
{
    public List<string> options;
    public List<string> tips;
    public Action<BaseChoiceMenu<string>> onMenuItemConfirmed;

    public Menu(List<string> options, List<string> tips, Action<BaseChoiceMenu<string>> onMenuItemConfirmed)
    {
        this.options = options;
        this.tips = tips;
        this.onMenuItemConfirmed = onMenuItemConfirmed;
    }
}