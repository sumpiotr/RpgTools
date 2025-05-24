using UnityEngine;

public class PlayerCharacter : Character
{

    private bool _guarding = false;

    public PlayerCharacter(CharacterScriptableObject characterData) : base(characterData)
    {
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

    

}
