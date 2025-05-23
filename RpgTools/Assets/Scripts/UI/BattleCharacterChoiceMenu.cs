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

    public void UpdateChoiceInitiative(int index, int value)
    {
        ((BattleCharacterChoice)_choices[index]).UpdateInitiative(value);
    }
}
