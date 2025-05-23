using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Scriptable Objects/Battle/Characters/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    public string Name;
    public int Health;
    public int Energy;
    public int Attack;
    public int Defense;
    public int Speed;
    public int CriticalRate;

    public List<ActionBaseScriptableObject> Skills;
}
