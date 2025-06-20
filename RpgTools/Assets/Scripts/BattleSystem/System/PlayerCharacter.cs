using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private const int restoreEnergyValue = 3;
    private bool _guarding = false;

    private Action<PlayerCharacter, bool, Action<int>> _chooseTarget;
    private Action onEnergyChange;


    public PlayerCharacter(CharacterScriptableObject characterData) : base(characterData)
    {
    }

    public void AddEnergyListener(Action action)
    {
        onEnergyChange = action;
    }
    public void SetChooseTarget(Action<PlayerCharacter, bool, Action<int>> chooseTarget)
    {
        _chooseTarget = chooseTarget;
    }

    public void Guard()
    {
        _guarding = true;
    }

    public override void TakeDamage(int attackDamage, DamageTypeEnum damageType, int effectChance)
    {
        if (_guarding) 
        {
            attackDamage /= 2;
            effectChance /= 2;
        }
        base.TakeDamage(attackDamage, damageType, effectChance);

      
    }

    public void BaseAttack(Character target)
    {
        if (_currentStats[CharacterStatsEnum.Energy] != GetCharacterData().Energy) 
        {
            int newEnergy = _currentStats[CharacterStatsEnum.Energy] + restoreEnergyValue;
            _currentStats[CharacterStatsEnum.Energy] = Math.Min(newEnergy, GetCharacterData().Energy);
            if (onEnergyChange != null) onEnergyChange();
        }
        target.TakeDamage(_characterData.Attack + _boofsValues[CharacterStatsEnum.Attack], _characterData.baseAttackType, 0);
    }

    public override void ResolveAction(ActionBaseScriptableObject action, List<Character> targets)
    {
        _currentStats[CharacterStatsEnum.Energy] -= action.Cost;
        if(onEnergyChange != null) onEnergyChange();
        base.ResolveAction(action, targets);
    }

    protected override void ChooseTargetAlly(ActionBaseScriptableObject action, List<Character> allies)
    {
        _chooseTarget(this, true, (int index) =>
        {
            if (index < 0)
            {
                return;
            }
            List<Character> list = new List<Character>();
            list.Add(allies[index]);
            ResolveAction(action, list);
        });
    }

    protected override void ChooseTargetEnemy(ActionBaseScriptableObject action, List<Character> enemies)
    {
        _chooseTarget(this, false, (int index) =>
        {
            if (index < 0)
            {
                return;
            }
            List<Character> list = new List<Character>();
            list.Add(enemies[index]);
            ResolveAction(action, list);
        });
    }

    public override void SetCurrentStatValue(CharacterStatsEnum stat, int value)
    {
        base.SetCurrentStatValue(stat, value);
        if (stat == CharacterStatsEnum.Energy && onEnergyChange != null) onEnergyChange();
    }
}
