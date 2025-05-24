using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterChoice : BaseChoiceMenu<CharacterChoiceData>
{
    private CharacterChoiceData _characterData;
    
    [SerializeField]
    private Image image;

    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider energySlider;
    [SerializeField]
    private Slider initiativeSlider;

    private int CurrentHealth;
    private int CurrentEnergy;
    private float CurrentInitiativeValue;

    [SerializeField]
    private GameObject images;

    public void UpdateHealth(int newValue)
    {
        CurrentHealth = newValue;
        healthSlider.value = CurrentHealth / _characterData.MaxHealth;
    }

    public void UpdateEnergy(int newValue)
    {
        CurrentEnergy = newValue;
        energySlider.value = CurrentEnergy / _characterData.MaxEnergy;
    }

    public void UpdateInitiative(float newValue)
    {
       CurrentInitiativeValue += newValue;
       initiativeSlider.value = CurrentInitiativeValue / _characterData.Speed;
    }

    public override void LoadData(CharacterChoiceData data)
    {
        _characterData = data;
        images.SetActive(true);
        image.sprite = data.Sprite;
        _characterData = data;
        energySlider.gameObject.SetActive(_characterData.Player);
        healthSlider.value = 1;
        energySlider.value = 1;
        initiativeSlider.value = 1;
    }

    public override CharacterChoiceData GetData()
    {
        return _characterData;
    }

    public override void UpdateData(CharacterChoiceData data)
    {
        energySlider.gameObject.SetActive(_characterData.Player);
        if(data.MaxHealth != CurrentHealth)UpdateHealth(data.MaxHealth);
        if(data.Speed != CurrentInitiativeValue)UpdateInitiative(data.Speed);
        if(data.MaxEnergy != CurrentEnergy)UpdateEnergy(data.MaxEnergy);
    }

    public override void ClearData()
    {
        images.SetActive(false);
    }
}

public struct CharacterChoiceData
{

    public bool Player;

    public Sprite Sprite;
    public int MaxHealth;
    public float Speed;
    public int MaxEnergy;

    public CharacterChoiceData(bool player, Sprite sprite, int maxHealth, float speed, int maxEnergy)
    {
        Player = player;
        Sprite = sprite;
        MaxHealth = maxHealth;
        Speed = speed;
        MaxEnergy = maxEnergy;
    }

    
}
