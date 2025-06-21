using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    private EnemyScriptableObject _enemyData;

    public string Name;

    private const float weaknessDamageMultiplier = 1.25f;
    private const float weaknessEffectChanceMultiplier = 1.5f;

    private const float resistanceDamageMultiplier = 0.5f;
    private const float resistanceEffectChanceMultiplier = 1.5f;

    private List<EnemyAction> _actions;

    private Action _onWeaknessHitted;
    private Action _onResistanceHitted;



    public Enemy(EnemyScriptableObject enemyData) : base(enemyData)
    {
        _enemyData = enemyData;
        _actions = new List<EnemyAction>();

        foreach(ActionBaseScriptableObject action in _enemyData.Skills) 
        {
          _actions.Add(new EnemyAction(action));
        }
    }

    public void SetOnWeaknessHitted(Action onWeaknessHitted)
    {
        _onWeaknessHitted = onWeaknessHitted;
    }

    public void SetOnResistanceHitted(Action onResistanceHitted)
    {
        _onResistanceHitted = onResistanceHitted;
    }

    public ActionBaseScriptableObject ChooseAction()
    {
        List<EnemyAction> actionPool = new List<EnemyAction>();
        foreach(EnemyAction action in _actions) 
        {
            if (action.cooldown == 0) actionPool.Add(action);
            else action.cooldown -= 1;
        }

        if (actionPool.Count == 0) return null;
        EnemyAction selected = actionPool[Random.Range(0, actionPool.Count)];
        selected.cooldown = selected.action.Cost;

        return selected.action;
    }

    public override void TakeDamage(int attackDamage, DamageTypeEnum damageType, int effectChance)
    {
        if (_enemyData.weaknesses.Contains(damageType))
        {
            attackDamage = GetMultipliedValue(attackDamage, weaknessDamageMultiplier);
            effectChance = GetMultipliedValue(effectChance, weaknessEffectChanceMultiplier);
            if (_onWeaknessHitted != null) _onWeaknessHitted();
        }
        else if (_enemyData.resistances.Contains(damageType)) 
        {
            attackDamage = GetMultipliedValue(attackDamage, resistanceDamageMultiplier);
            effectChance = GetMultipliedValue(effectChance, resistanceEffectChanceMultiplier);
            if (_onResistanceHitted != null) _onResistanceHitted();
        }
        else if (_enemyData.immunities.Contains(damageType))
        {
            attackDamage = 0;
            effectChance = 0;
        }
        base.TakeDamage(attackDamage, damageType, effectChance);
    }

    protected override void ChooseTargetAlly(ActionBaseScriptableObject action, List<Character> allies)
    {
        List<Character> list = new List<Character>();
        list.Add(allies[Random.Range(0, allies.Count - 1)]);
        ResolveAction(action, list);
    }

    protected override void ChooseTargetEnemy(ActionBaseScriptableObject action, List<Character> enemies)
    {
        List<Character> list = new List<Character>();
        list.Add(enemies[Random.Range(0, enemies.Count - 1)]);
        ResolveAction(action, list);
    }




}

class EnemyAction 
{
    public ActionBaseScriptableObject action;
    public int cooldown;

    public EnemyAction(ActionBaseScriptableObject action)
    {
        this.action = action;
        cooldown = 0;
    }
}