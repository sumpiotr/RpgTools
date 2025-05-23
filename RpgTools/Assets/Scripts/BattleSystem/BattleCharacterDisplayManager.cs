using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterDisplayManager : MonoBehaviour
{
    [SerializeField]
    private BattleCharacterChoiceMenu playerDisplay;

    [SerializeField]
    private BattleCharacterChoiceMenu enemyDisplay;


    public void LoadEnemies(List<CharacterScriptableObject> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (CharacterScriptableObject character in data) 
        {
            choiceData.Add(new CharacterChoiceData(false, character.Health, character.Speed, character.Energy)); 
        }
        enemyDisplay.LoadChoices(choiceData);
    }

    public void LoadPlayers(List<CharacterScriptableObject> data)
    {
        List<CharacterChoiceData> choiceData = new List<CharacterChoiceData>();
        foreach (CharacterScriptableObject character in data)
        {
            choiceData.Add(new CharacterChoiceData(true, character.Health, character.Speed, character.Energy));
        }
        playerDisplay.LoadChoices(choiceData);
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

    public void UpdateInitiative(bool player, int index, int value)
    {
        if (player) playerDisplay.UpdateChoiceInitiative(index, value);
        else enemyDisplay.UpdateChoiceInitiative(index, value);
    }
}
