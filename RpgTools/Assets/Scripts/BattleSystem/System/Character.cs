using System;
using UnityEngine;

public class Character
{
    private CharacterScriptableObject _characterData;

    private int _currentHealth;
    private int _currentAttack;
    private int _currentDefense;
    private int _currentSpeed;
    private int _currentCriticalRate;

    private int _boostHealth;
    private int _boostAttack;
    private int _boostDefense;
    private int _boostSpeed;
    private int _boostCriticalRate;

    private int _initiative = 0;

    const int minDamage = 1;

    public Character(CharacterScriptableObject characterData)
    {
        LoadStats(characterData);
    }

    public void LoadStats(CharacterScriptableObject characterData)
    {
        _characterData = characterData;
        _characterData = characterData;
        _currentHealth = characterData.Health;
        _currentAttack = characterData.Attack;
        _currentDefense = characterData.Defense;
        _currentSpeed = characterData.Speed;
        _currentCriticalRate = characterData.CriticalRate;

        _boostHealth = 0;
        _boostAttack = 0;
        _boostDefense = 0;
        _boostSpeed = 0;
        _boostCriticalRate = 0;
    }

    public virtual void TakeDamage(int attackDamage, DamageTypeEnum damageType)
    {
        int totalDamae = attackDamage - _currentDefense - _boostDefense;
        attackDamage = Mathf.Max(minDamage, totalDamae);
        _currentHealth -= attackDamage;
    }

    public bool UpdateInitiative()
    {
        _initiative += 1;
        return _initiative > 0;
    }

    public virtual void StartTurn()
    {
        _initiative = _currentSpeed - _boostSpeed;
        _initiative = Mathf.Max(1, _initiative);
    }

    public CharacterScriptableObject GetCharacterData()
    {
        return _characterData;
    }

}
