using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacterMenuChoice : BaseChoiceMenu<CharacterChoiceData>
    {
        protected CharacterChoiceData _characterData;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Slider healthSlider;
        [SerializeField]
        private Slider energySlider;

        [SerializeField]
        private GameObject choiceContainer;

        private int CurrentHealth;
        private int CurrentEnergy;

        public virtual void UpdateHealth(int newValue)
        {
            CurrentHealth = newValue;
            healthSlider.value = (float)CurrentHealth / (float)_characterData.MaxHealth;
        }

        public virtual void UpdateEnergy(int newValue)
        {
            CurrentEnergy = newValue;
            energySlider.value = (float)CurrentEnergy / (float)_characterData.MaxEnergy;
        }

        public override void LoadData(CharacterChoiceData data)
        {
            _characterData = data;
            choiceContainer.SetActive(true);
            image.sprite = data.Sprite;
            _characterData = data;
            energySlider.gameObject.SetActive(_characterData.Player);
            healthSlider.value = 1;
            energySlider.value = 1;
        }

        public override CharacterChoiceData GetData()
        {
            return _characterData;
        }

        public override void UpdateData(CharacterChoiceData data)
        {
            energySlider.gameObject.SetActive(_characterData.Player);
            if (data.MaxHealth != CurrentHealth) UpdateHealth(data.MaxHealth);
            if (data.MaxEnergy != CurrentEnergy) UpdateEnergy(data.MaxEnergy);
        }

        public override void ClearData()
        {
            choiceContainer.SetActive(false);
        }
    }

    public struct CharacterChoiceData
    {

        public bool Player;

        public string Name;
        public Sprite Sprite;
        public int MaxHealth;
        public float Speed;
        public int MaxEnergy;

        public CharacterChoiceData(bool player, string name, Sprite sprite, int maxHealth, float speed, int maxEnergy)
        {
            Player = player;
            Name = name;
            Sprite = sprite;
            MaxHealth = maxHealth;
            Speed = speed;
            MaxEnergy = maxEnergy;
        }

        public CharacterChoiceData(string name, Sprite sprite, int maxHealth, int maxEnergy)
        {
            Player = true;
            Name = name;
            Sprite = sprite;
            MaxHealth = maxHealth;
            Speed = 1;
            MaxEnergy = maxEnergy;
        }
    }

