
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField]
    private BattleCharacterDisplayManager battleCharacterDisplayManager;

    [SerializeField]
    private BattleCharacterChoiceMenu characterMenu;

    private bool battle = false;

    public static PlayerMenuManager Instance = null;

    private Action<int> _onChoose;

    private InputMapEnum _previous;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void Start()
    {
    }

    public BattleCharacterChoiceMenu GetMenu()
    {
        return battle ? battleCharacterDisplayManager.GetPlayerMenu() : characterMenu;
    }

    public BattleCharacterDisplayManager GetBattleMenuManger()
    {
        return battleCharacterDisplayManager;
    }

    public BattleCharacterChoiceMenu GetBasePlayerMenu()
    {
        return characterMenu;
    }

    public void SetInBattle(bool inBattle)
    {
        battle = inBattle;
    }

    public bool InBattle()
    {
        return battle;
    }

    public void UpdateHealthbar(int index, int value)
    {
        GetMenu().UpdateChoiceHealth(index, value);
    }

    public void UpdateEnergybar(int index, int value)
    {
        GetMenu().UpdateChoiceEnergy(index, value);
    }

    public void SetTitle(string title)
    {
        if(!battle)characterMenu.SetTitle(title);
    }

    public void ShowPlayerMenu(string title)
    {
        if (battle) return;
        characterMenu.gameObject.SetActive(true);
        characterMenu.SetTitle(title);
        characterMenu.Unfocus();
        List<PlayerCharacter> players = PlayerDataManager.Instance.GetPlayers();
        List<CharacterChoiceData> choices = new List<CharacterChoiceData>();
        foreach (PlayerCharacter player in players) 
        {
            CharacterScriptableObject data = player.GetCharacterData();
            choices.Add(new CharacterChoiceData(data.Name, data.Sprite, data.Health, data.Energy)); 
        }
        characterMenu.LoadChoices(choices);
        for (int i = 0; i < players.Count; i++) 
        {
            UpdateHealthbar(i, players[i].GetCurrentStatValue(CharacterStatsEnum.Health));
            UpdateEnergybar(i, players[i].GetCurrentStatValue(CharacterStatsEnum.Energy));
        }
        _previous = InputManager.Instance.GetInputMap();
        InputManager.Instance.ChangeMapping(InputMapEnum.CharacterSelection);
    }

    public void ShowPlayerMenu()
    {
        ShowPlayerMenu("");
    }

    public void CloseMenu()
    {
        characterMenu.gameObject.SetActive(false);
        InputManager.Instance.ChangeMapping(_previous);
    }

    public void StartSelection(Action<int> onChoose, string title)
    {
        if (battle) 
        {
            battleCharacterDisplayManager.ChoiceCharacter(true, onChoose);
            _previous = InputMapEnum.Battle;
        }
        else
        {
            _onChoose = onChoose;
            ShowPlayerMenu(title);
            characterMenu.Focus();
        }
    }

    public void StartSelection(Action<int> onChoose)
    {
        if (battle)
        {
            battleCharacterDisplayManager.ChoiceCharacter(true, onChoose);
            return;
        }
        _onChoose = onChoose;
        ShowPlayerMenu("");
        characterMenu.Focus();
    }

    #region input

    public void SelectionMove(InputAction.CallbackContext context)
    {
        if(!context.performed)return;
        if(battle)battleCharacterDisplayManager.OnSelectionMove(context);
        else characterMenu.SelectionMove(context);
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (battle) battleCharacterDisplayManager.OnConfirm(context);
        else
        {
            int index = characterMenu.GetSelectedIndex();
            if (index == -1) return;
            _onChoose.Invoke(index);
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if(!context.performed)return;
        if (battle) battleCharacterDisplayManager.OnCancel(context);
        else {
            CloseMenu();
            _onChoose.Invoke(-1);
        }
    }

    #endregion

}
