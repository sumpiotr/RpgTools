using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCharacterDisplayManager : MonoBehaviour
{

    [SerializeField]
    private BattleCharacterChoiceMenu playerDisplay;

    [SerializeField]
    private BattleCharacterChoiceMenu enemyDisplay;

    private bool _chooseActive = false;
    private bool _playerChoose = false;

    private Action<int> _onChoosen;

    public void LoadEnemies(List<Enemy> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (Character character in data) 
        {
            choiceData.Add(new CharacterChoiceData(false, character.GetCharacterData().Sprite, character.GetCharacterData().Health, character.GetCharacterData().Speed, 1)); 
        }
        enemyDisplay.LoadChoices(choiceData);
    }

    public void LoadPlayers(List<PlayerCharacter> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (Character character in data)
        {
            choiceData.Add(new CharacterChoiceData(true, character.GetCharacterData().Sprite, character.GetCharacterData().Health, character.GetCharacterData().Speed, character.GetCharacterData().Energy));
        }
        playerDisplay.LoadChoices(choiceData);
        for (int i = 0; i < data.Count; i++) 
        {
            playerDisplay.UpdateChoiceHealth(i, data[i].GetCurrentStatValue(CharacterStatsEnum.Health));
            playerDisplay.UpdateChoiceEnergy(i, data[i].GetCurrentStatValue(CharacterStatsEnum.Energy));
        }
    }

    public void UpdateHealth(bool player, int index, int value)
    {
        if(player)playerDisplay.UpdateChoiceHealth(index, value);
        else enemyDisplay.UpdateChoiceHealth(index, value);
    }

    public void UpdatePlayerEnergy(int index, int value)
    {
        playerDisplay.UpdateChoiceEnergy(index, value);
    }

    public void UpdateInitiative(float value)
    {
        playerDisplay.UpdateChoicesInitiative(value);
        enemyDisplay.UpdateChoicesInitiative(value);
    }

    public void ChoiceCharacter(bool player, Action<int> onChoosen)
    {
        _chooseActive = true;
        _playerChoose = player;
        _onChoosen = onChoosen;
        if (player) playerDisplay.Focus();
        else enemyDisplay.Focus();
    }

    #region Input

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!_chooseActive) return;
        if (_playerChoose) playerDisplay.Unfocus();
        else enemyDisplay.Unfocus();
        _onChoosen.Invoke(-1);
        _chooseActive = false;
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        if (!_chooseActive) return;
        _onChoosen.Invoke(_playerChoose ? playerDisplay.GetSelectedIndex() : enemyDisplay.GetSelectedIndex());
        _chooseActive = false;
    }

    public void OnSelectionMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!_chooseActive) return;
        if (_playerChoose) playerDisplay.SelectionMove(context);
        else enemyDisplay.SelectionMove(context);
    }


    #endregion
}
