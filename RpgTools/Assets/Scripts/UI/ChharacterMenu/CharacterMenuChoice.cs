using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuChoice : BaseCharacterMenuChoice
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI healthStateText;

    [SerializeField]
    private TextMeshProUGUI energyStateText;


    public override void LoadData(CharacterChoiceData data)
    {
        base.LoadData(data);
        nameText.SetText(data.Name);
    }

    public override void UpdateEnergy(int newValue)
    {
        energyStateText.text = $"{newValue} / {_characterData.MaxEnergy}";
        base.UpdateEnergy(newValue);
    }

    public override void UpdateHealth(int newValue)
    {
        healthStateText.text = $"{newValue} / {_characterData.MaxHealth}";
        base.UpdateHealth(newValue);
    }
}
