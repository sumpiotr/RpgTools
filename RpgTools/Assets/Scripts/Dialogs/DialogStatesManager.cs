using UnityEngine;
using System.Collections.Generic;

public class DialogStatesManager : MonoBehaviour
{
    public static DialogStatesManager Instance { get; private set; }

    private Dictionary<string, int> variableStates = new Dictionary<string, int>();

    private Dictionary<string, string> states = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public void SetState(string name, string state)
    {
        states[name] = state;
    }

    public string GetState(string name)
    {
        return states[name];
    }


    public void SetVariable(string name, int value)
    {
        variableStates[name] = value;
    }

    public int GetVariable(string name)
    {
        if (variableStates.ContainsKey(name))
        {
            return variableStates[name];
        }
        return -1;
    }
}

