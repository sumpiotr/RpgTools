using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character
{
    protected CharacterScriptableObject _characterData;

    protected Action _onActionResolved;

    private Action _onHealthChange;


    private Dictionary<CharacterStatsEnum, int> _currentStats;
    protected Dictionary<CharacterStatsEnum, int> _boofsValues;

    private Dictionary<DamageTypeEnum, int> _effectsCounter;
    private Dictionary<CharacterStatsEnum, int> _boofsCounter;

    private HashSet<CharacterStatsEnum> _boofs;
    private HashSet<DamageTypeEnum> _effects;


    private float _initiative = 0;

    const int minDamage = 1;

    public Character(CharacterScriptableObject characterData)
    {
        _currentStats = new Dictionary<CharacterStatsEnum, int>();
        _boofsValues = new Dictionary<CharacterStatsEnum, int>();
        _effectsCounter = new Dictionary<DamageTypeEnum, int>();
        _boofsCounter = new Dictionary<CharacterStatsEnum, int>();
        _boofs = new HashSet<CharacterStatsEnum>();
        _effects = new HashSet<DamageTypeEnum>();
        LoadStats(characterData);
    }

    public void LoadStats(CharacterScriptableObject characterData)
    {
        _characterData = characterData;
        _currentStats[CharacterStatsEnum.Health] = characterData.Health;
        _currentStats[CharacterStatsEnum.Energy] = characterData.Energy;
        _currentStats[CharacterStatsEnum.Attack] = characterData.Attack;
        _currentStats[CharacterStatsEnum.Defense] = characterData.Defense;
        _currentStats[CharacterStatsEnum.Speed] = characterData.Speed;
        _currentStats[CharacterStatsEnum.CriticalRate] = characterData.CriticalRate;


        foreach (DamageTypeEnum value in Enum.GetValues(typeof(DamageTypeEnum))) 
        {
            _effectsCounter[value] = 0;
        }

        foreach (CharacterStatsEnum value in Enum.GetValues(typeof(CharacterStatsEnum)))
        {
            _boofsCounter[value] = 0;
        }


        _boofsValues[CharacterStatsEnum.Attack] = 0;
        _boofsValues[CharacterStatsEnum.Defense] = 0;
        _boofsValues[CharacterStatsEnum.Speed] = 0;
        _boofsValues[CharacterStatsEnum.CriticalRate] = 0;
    }

    public void AddHealthListener(Action onHealthChange)
    {
        _onHealthChange += onHealthChange;
    }

    public virtual void TakeDamage(int attackDamage, DamageTypeEnum damageType, int effectChance)
    {
        int totalDamae = attackDamage - _currentStats[CharacterStatsEnum.Defense] - _boofsValues[CharacterStatsEnum.Defense];
        attackDamage = Mathf.Max(minDamage, totalDamae);
        _currentStats[CharacterStatsEnum.Health] -= attackDamage;
        _onHealthChange.Invoke();
    }


    public void Boost(int amount, int duration, CharacterStatsEnum type)
    {
        if(type == CharacterStatsEnum.Health || type == CharacterStatsEnum.Energy)
        {
            _currentStats[type] += amount;
            _onHealthChange();
        }
        else
        {
            if (duration < 0) return;
            _boofsValues[type] = amount+1;
            _boofs.Add(type);
        }
    }

    public void Attack(Character target, DamageTypeEnum type, int effectChance)
    {
        target.TakeDamage(_currentStats[CharacterStatsEnum.Attack] + _boofsValues[CharacterStatsEnum.Attack], type, effectChance);
    }

    public bool UpdateInitiative(float val)
    {
        _initiative += val;
        return _initiative >= _currentStats[CharacterStatsEnum.Speed];
    }

    public virtual void StartTurn()
    {
        _initiative = _currentStats[CharacterStatsEnum.Speed] - _boofsValues[CharacterStatsEnum.Speed];
        _initiative = Mathf.Max(1, _initiative);
    }

    public void EndTurn()
    {
        _initiative = 0;
        CheckBoofs();
        CheckEffects();
    }

    private void CheckBoofs()
    {
        foreach (CharacterStatsEnum type in _boofs) 
        {
            _boofsCounter[type] -= 1;
            if (_boofsCounter[type] == 0)
            {
                _boofs.Remove(type);
                _boofsValues[type] = 0;
            }
        }
    }

    private void CheckEffects()
    {
        if (_effects.Contains(DamageTypeEnum.Fire))
        {
            _currentStats[CharacterStatsEnum.Health] -= GetMultipliedValue(_characterData.Health, 0.1f);
        }
        
        foreach(DamageTypeEnum effect in _effects)
        {
            _effectsCounter[effect] -= 1;
            if(_effectsCounter[effect] == 0)
            {
                _effects.Remove(effect);
            }
        }
    }

    public CharacterScriptableObject GetCharacterData()
    {
        return _characterData;
    }

    public int GetCurrentStatValue(CharacterStatsEnum stat)
    {
        return _currentStats[stat];
    }

    protected int GetMultipliedValue(int value, float multiplier)
    {
        return (int)Mathf.Ceil(multiplier * ((float)value));
    }

    public void ChooseTargetAndResolveAction(ActionBaseScriptableObject action, List<Character> allies, List<Character> enemies, Action onResolved)
    {
        _onActionResolved = onResolved;
        List<Character> targets = new List<Character>();
        if (action.TargetTypeEnum == TargetTypeEnum.Self)
        {
            targets.Add(this);
            ResolveAction(action, targets);
        }
        else if (action.TargetTypeEnum == TargetTypeEnum.Multi)
        {
            if (action.Target == TargetEnum.Enemies)ResolveAction(action, enemies);
            else ResolveAction(action, allies);
        }
        else
        {
            if (action.Target == TargetEnum.Enemies) ChooseTargetEnemy(action, enemies);
            else ChooseTargetAlly(action, allies);
        }
    }

    protected virtual void ChooseTargetEnemy(ActionBaseScriptableObject action, List<Character> enemies)
    {

    }

    protected virtual void ChooseTargetAlly(ActionBaseScriptableObject action, List<Character> allies)
    {

    }

    public void ResolveAction(ActionBaseScriptableObject action, List<Character> targets)
    {
        if (action.GetType() == typeof(BuffScriptableObject))
        {
            BuffScriptableObject buff = (BuffScriptableObject)action;
            foreach (Character target in targets)
            {
                target.Boost(buff.Amount, buff.Duration, buff.Stat);
            }
        }
        else if (action.GetType() == typeof(AttackScriptableObject))
        {
            AttackScriptableObject attack = (AttackScriptableObject)action;
            foreach(Character target in targets) 
            {
                Attack(target, attack.DamageType, attack.effectChance);
            }
        }

        EndTurn();
        _onActionResolved.Invoke();
    }


}
