using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCharacterDisplayManager : MonoBehaviour
{
    [SerializeField]
    private StringEventScriptableObject showBattleHintEvent;

    [SerializeField]
    private BattleCharacterChoiceMenu playerDisplay;

    [SerializeField]
    private BattleCharacterChoiceMenu enemyDisplay;

    private bool _playerChoose = false;

    private Action<int> _onChoosen;

    private void Start()
    {
        enemyDisplay.SetOnHover(ShowEnemyInfo);
        playerDisplay.SetOnHover(ShowPlayerInfo);
    }

    private void ShowPlayerInfo(int index)
    {
        if (index == -1) showBattleHintEvent.CallEvent("");
        else showBattleHintEvent.CallEvent(playerDisplay.GetChoiceDataByIndex(index).Name);
    }

    private void ShowEnemyInfo(int index)
    {
        if (index == -1) showBattleHintEvent.CallEvent("");
        else showBattleHintEvent.CallEvent(enemyDisplay.GetChoiceDataByIndex(index).Name);
    }

    public void LoadEnemies(List<Enemy> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (Enemy enemy in data) 
        {
            choiceData.Add(new CharacterChoiceData(false, enemy.Name, enemy.GetCharacterData().Sprite, enemy.GetCharacterData().Health, enemy.GetCharacterData().Speed, 1)); 
        }
        enemyDisplay.LoadChoices(choiceData);

        for (int i = 0; i < data.Count; i++)
        {
            enemyDisplay.UpdateChoiceHealth(i, data[i].GetCurrentStatValue(CharacterStatsEnum.Health));
            enemyDisplay.UpdateChoiceInitiative(i, data[i].GetInitiative());
            foreach (DamageTypeEnum effect in data[i].GetEffects()) 
            {
                enemyDisplay.DisplayChoiceEffect(i, effect);
            }
        }
    }

    public void LoadPlayers(List<PlayerCharacter> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (Character character in data)
        {
            choiceData.Add(new CharacterChoiceData(true, character.GetCharacterData().Name, character.GetCharacterData().Sprite, character.GetCharacterData().Health, character.GetCharacterData().Speed, character.GetCharacterData().Energy));
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

    public void DisplayEffect(bool player, int index, DamageTypeEnum effect)
    {
        if(player)playerDisplay.DisplayChoiceEffect(index, effect);
        else enemyDisplay.DisplayChoiceEffect(index,effect);
    }

    public void HideEffect(bool player, int index, DamageTypeEnum effect)
    {
        if (player) playerDisplay.HideChoiceEffect(index, effect);
        else enemyDisplay.HideChoiceEffect(index, effect);
    }

    public void ChoiceCharacter(bool player, Action<int> onChoosen)
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.CharacterSelection);
        _playerChoose = player;
        _onChoosen = onChoosen;
        if (player) playerDisplay.Focus();
        else enemyDisplay.Focus();
    }

    #region Input

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_playerChoose) playerDisplay.Unfocus();
        else enemyDisplay.Unfocus();
        _onChoosen.Invoke(-1);
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        _onChoosen.Invoke(_playerChoose ? playerDisplay.GetSelectedIndex() : enemyDisplay.GetSelectedIndex());
        if (_playerChoose) playerDisplay.Unfocus();
        else enemyDisplay.Unfocus();
    }

    public void OnSelectionMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_playerChoose) playerDisplay.SelectionMove(context);
        else enemyDisplay.SelectionMove(context);
    }

    internal BattleCharacterChoiceMenu GetPlayerMenu()
    {
        return playerDisplay;
    }


    #endregion
}
