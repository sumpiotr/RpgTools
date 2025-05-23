using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Battle/Skills/Attack")]
public class AttackScriptableObject : ActionBaseScriptableObject
{
    public DamageTypeEnum DamageType;
    public float attackModifier = 1;
    public int critModifier = 1;
    public int effectChance = 20;
}

