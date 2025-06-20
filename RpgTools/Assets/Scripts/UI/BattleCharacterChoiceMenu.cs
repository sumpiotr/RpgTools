using System;
using UnityEngine;

public class BattleCharacterChoiceMenu : BaseChoiceMenuManager<CharacterChoiceData>
{
    public void UpdateChoiceHealth(int index, int value)
    {
        ((BaseCharacterMenuChoice)_choices[index]).UpdateHealth(value);
    }

    public void UpdateChoiceEnergy(int index, int value)
    {
        ((BaseCharacterMenuChoice)_choices[index]).UpdateEnergy(value);
    }

    public void UpdateChoicesInitiative(float value)
    {
        foreach (var choice in _choices) 
        {
            ((BattleCharacterChoice)choice).UpdateInitiative(value);
        }
      
    }

    public void UpdateChoiceInitiative(int index, float value)
    {
        ((BattleCharacterChoice)_choices[index]).UpdateInitiative(value);
    }

    public void DisplayChoiceEffect(int index, DamageTypeEnum effect)
    {
        ((BattleCharacterChoice)_choices[index]).DisplayEffect(effect);
    }

    public void HideChoiceEffect(int index, DamageTypeEnum effect)
    {
        ((BattleCharacterChoice)_choices[index]).HideEffect(effect);
    }
}
