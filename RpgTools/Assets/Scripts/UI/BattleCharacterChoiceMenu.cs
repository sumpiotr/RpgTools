using UnityEngine;

public class BattleCharacterChoiceMenu : BaseChoiceMenuManager<CharacterChoiceData>
{
    public void UpdateChoiceHealth(int index, int value)
    {
        ((BattleCharacterChoice)_choices[index]).UpdateHealth(value);
    }

    public void UpdateChoiceEnergy(int index, int value)
    {
        ((BattleCharacterChoice)_choices[index]).UpdateEnergy(value);
    }

    public void UpdateChoicesInitiative(float value)
    {
        foreach (var choice in _choices) 
        {
            ((BattleCharacterChoice)choice).UpdateInitiative(value);
        }
      
    }
}
