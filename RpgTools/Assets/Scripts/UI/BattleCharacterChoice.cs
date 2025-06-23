using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterChoice : BaseCharacterMenuChoice
{
    
    [SerializeField]
    private Slider initiativeSlider;


    [SerializeField]
    private List<EffectImage> effectsImages;


    private Dictionary<DamageTypeEnum, GameObject> effects; 

    private float CurrentInitiativeValue = 0;

    public void UpdateInitiative(float newValue)
    {
        if (_characterData.Speed == 0 || CurrentHealth <= 0) return;
       CurrentInitiativeValue += newValue;
       if(CurrentInitiativeValue > _characterData.Speed)CurrentInitiativeValue = 0;
       initiativeSlider.value = CurrentInitiativeValue / _characterData.Speed;
    }

    public override void UpdateHealth(int newValue)
    {
        base.UpdateHealth(newValue);
        if (!effects.ContainsKey(DamageTypeEnum.Dead)) return;
        if (newValue > 0 && effects[DamageTypeEnum.Dead].activeSelf) { 
            effects[DamageTypeEnum.Dead].SetActive(false);
        }
    }
    public override void LoadData(CharacterChoiceData data)
    {
        base.LoadData(data);
        effects = new Dictionary<DamageTypeEnum, GameObject>();
        HideEffects();
        CurrentInitiativeValue = 0;
    }

    private void HideEffects()
    {
        foreach (EffectImage effectImage in effectsImages)
        {
            effectImage.image.SetActive(false);
            effects[effectImage.damageType] = effectImage.image;
        }
    }

    public void DisplayEffect(DamageTypeEnum effect)
    {
        if(effect == DamageTypeEnum.Dead)
        {
            HideEffects();
            CurrentInitiativeValue = 0;
        }
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
