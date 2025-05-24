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

   
}
