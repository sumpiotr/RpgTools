using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private EnemyScriptableObject _enemyData;

    private const float weaknessDamageMultiplier = 1.25f;
    private const float weaknessEffectChanceMultiplier = 1.5f;

    private const float resistanceDamageMultiplier = 0.5f;
    private const float resistanceEffectChanceMultiplier = 1.5f;



    public Enemy(EnemyScriptableObject enemyData) : base(enemyData)
    {
        _enemyData = enemyData;
    }

    public override void TakeDamage(int attackDamage, DamageTypeEnum damageType, int effectChance)
    {
        if (_enemyData.weaknesses.Contains(damageType))
        {
            attackDamage = GetMultipliedValue(attackDamage, weaknessDamageMultiplier);
            effectChance = GetMultipliedValue(attackDamage, weaknessEffectChanceMultiplier);
        }
        else if (_enemyData.resistances.Contains(damageType)) 
        {
            attackDamage = GetMultipliedValue(attackDamage, resistanceDamageMultiplier);
            effectChance = GetMultipliedValue(attackDamage, resistanceEffectChanceMultiplier);
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
