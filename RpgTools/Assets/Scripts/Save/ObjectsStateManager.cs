using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectsStateManager : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, bool>> objectsStates;

    public static ObjectsStateManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        objectsStates = new Dictionary<string, Dictionary<string, bool>>();
    }

    public void LoadData(Dictionary<string, bool> data)
    {
        string sceneName = SceneLoadManager.Instance.GetCurrentSceneName();
        if (!objectsStates.ContainsKey(sceneName))objectsStates.Add(sceneName, new Dictionary<string, bool>(data));
    }

    public void SaveState(string name, bool active)
    {
        string scene = SceneLoadManager.Instance.GetCurrentSceneName();
        if (!objectsStates.ContainsKey(scene))objectsStates.Add(scene, new Dictionary<string, bool>());
        objectsStates[scene][name] = active;
    }

    public void SaveStates(List<(string name, bool value)> values)
    {
        string scene = SceneLoadManager.Instance.GetCurrentSceneName();
        if (!objectsStates.ContainsKey(scene)) objectsStates.Add(scene, new Dictionary<string, bool>());
        foreach (var data in values)
        {
            objectsStates[scene][data.name] = data.value;
        }
    }

    public bool GetState(string scene, string name)
    {
        return objectsStates[scene][name];
    }

    public Dictionary<string, bool> GetStates(string scene)
    {
        if (!objectsStates.ContainsKey(scene)) return null;
        return objectsStates[scene];
    }


}
