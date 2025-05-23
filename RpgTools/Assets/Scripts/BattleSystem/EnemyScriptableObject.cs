using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/Battle/Characters/Enemy")]
public class EnemyScriptableObject : CharacterScriptableObject
{
    public List<DamageTypeEnum> weaknesses;
    public List<DamageTypeEnum> resistances;
}
