using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    private bool _guarding = false;

    private Action<PlayerCharacter, bool, Action<int>> _chooseTarget;


    public PlayerCharacter(CharacterScriptableObject characterData) : base(characterData)
    {
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
        target.TakeDamage(_characterData.Attack + _boofsValues[CharacterStatsEnum.Attack], _characterData.baseAttackType, 0);
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

}
