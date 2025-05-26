using UnityEngine;

public abstract class ActionBaseScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
    public int Cost;
    public TargetEnum Target;
    public TargetTypeEnum TargetTypeEnum;
}
