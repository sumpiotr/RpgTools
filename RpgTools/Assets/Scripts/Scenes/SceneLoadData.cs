using UnityEngine;

public struct SceneLoadData
{
    public Vector2 exitPosition;
    public Vector2 enterPosition;
    public string name;

    public SceneLoadData(Vector2 enterPosition, string name, Vector2 exitPosition)
    {
        this.enterPosition = enterPosition;
        this.name = name;
        this.exitPosition = exitPosition;
    }
}
