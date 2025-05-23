using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Objects/Battle/Skills/Buff")]
public class BuffScriptableObject : ActionBaseScriptableObject
{
    public CharacterStatsEnum Stat;
    public int Amount = 0;
    public int Duration = 1;
}
