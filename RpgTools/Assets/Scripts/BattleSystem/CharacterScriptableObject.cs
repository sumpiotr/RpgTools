using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Scriptable Objects/Battle/Characters/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public int Health;
    public int Energy;
    public int Attack;
    public int Defense;
    public int Speed;
    public int CriticalRate;

    public DamageTypeEnum baseAttackType;

    public List<ActionBaseScriptableObject> Skills;
}
