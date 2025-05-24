using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Scriptable Objects/Battle/Encounter")]
public class EncounterScriptableObject : ScriptableObject
{
    public List<EncounterMonster> EnemyList;
}

[Serializable]
public struct EncounterMonster 
{
    public EnemyScriptableObject enemyData;
    public int amount;
}