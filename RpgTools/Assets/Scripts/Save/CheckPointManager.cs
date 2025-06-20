using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
   public static CheckPointManager Instance;

    private Vector2 playerPosition;
    private List<CheckPointPlayerData> players;
    private Dictionary<string, int> dialogVarStates;
    private Dictionary<string, bool> objectsStates;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void MakeCheckPoint()
    {
        players = new List<CheckPointPlayerData>();
        playerPosition = PlayerController.Instance.GetPosition();
        dialogVarStates = new Dictionary<string, int>();
        objectsStates = new Dictionary<string, bool>();
        foreach(var kv in DialogStatesManager.Instance.GetVariableStates())
        {
            dialogVarStates[kv.Key] = kv.Value;
        }

        Dictionary<string, bool> states = ObjectsStateManager.Instance.GetStates(SceneLoadManager.Instance.GetCurrentSceneName());
        if (states == null)
        {
            ObjectsStateManager.Instance.SaveStates(FindObjectsByType<StateObject>(FindObjectsSortMode.None).Select(x=> (x.gameObject.name, x.gameObject.activeSelf)).ToList());
            states = ObjectsStateManager.Instance.GetStates(SceneLoadManager.Instance.GetCurrentSceneName());
        }
        foreach (var kv in states)
        {
            objectsStates[kv.Key] = kv.Value;
        }
        foreach (PlayerCharacter player in PlayerDataManager.Instance.GetPlayers()) 
        {
            players.Add(new CheckPointPlayerData(player));
        }
    }

    public void LoadCheckPoint()
    {
        PlayerController.Instance.UpdatePosition(playerPosition);
        foreach (CheckPointPlayerData player in players) {
            player.LoadData(); 
        }
        DialogStatesManager.Instance.LoadVariablesStates(dialogVarStates);
        ObjectsStateManager.Instance.LoadData(objectsStates);
        foreach(StateObject stateObject in FindObjectsByType<StateObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Debug.Log("x");
            if(objectsStates.ContainsKey(stateObject.gameObject.name))stateObject.gameObject.SetActive(objectsStates[stateObject.gameObject.name]);
        }
    }
}

struct CheckPointPlayerData
{
    public PlayerCharacter playerCharacter;
    public int health;
    public int energy;

    public CheckPointPlayerData(PlayerCharacter playerCharacter)
    {
        this.playerCharacter = playerCharacter;
        this.health = playerCharacter.GetCurrentStatValue(CharacterStatsEnum.Health);
        this.energy = playerCharacter.GetCurrentStatValue(CharacterStatsEnum.Energy);
    }

    public void LoadData()
    {
        playerCharacter.SetCurrentStatValue(CharacterStatsEnum.Health, health);
        playerCharacter.SetCurrentStatValue(CharacterStatsEnum.Energy, energy);
    }
}