using UnityEngine;

public class BaseItemScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
    public ItemType Type;
}

public enum ItemType
{
    Item,
    Key
}