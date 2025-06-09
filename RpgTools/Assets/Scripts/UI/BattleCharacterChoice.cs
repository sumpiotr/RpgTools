using System;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterChoice : BaseCharacterMenuChoice
{
    
    [SerializeField]
    private Slider initiativeSlider;


    [SerializeField]
    private List<EffectImage> effectsImages;


    private Dictionary<DamageTypeEnum, GameObject> effects; 

    private float CurrentInitiativeValue;

    public void UpdateInitiative(float newValue)
    {
       CurrentInitiativeValue += newValue;
       if(CurrentInitiativeValue > _characterData.Speed)CurrentInitiativeValue -= _characterData.Speed;
       initiativeSlider.value = CurrentInitiativeValue / _characterData.Speed;
    }

    public override void LoadData(CharacterChoiceData data)
    {
        base.LoadData(data);

        effects = new Dictionary<DamageTypeEnum, GameObject>();

        foreach (EffectImage effectImage in effectsImages) 
        {
            effectImage.image.SetActive(false);
            effects[effectImage.damageType] = effectImage.image;
        }

    }

    public void DisplayEffect(DamageTypeEnum effect)
    {
        effects[effect].SetActive(true);
    }

    public void HideEffect(DamageTypeEnum effect)
    {
        effects[effect].SetActive(false);
    }

    public override CharacterChoiceData GetData()
    {
        return _characterData;
    }

    public override void UpdateData(CharacterChoiceData data)
    {
        base.UpdateData(data);
        if (data.Speed != CurrentInitiativeValue) UpdateInitiative(data.Speed);
    }

}

[Serializable]
struct EffectImage 
{
    public DamageTypeEnum damageType;
    public GameObject image;
}
