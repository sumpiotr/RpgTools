using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterScriptableObject> startCharacterData;

    private List<PlayerCharacter> _players;

    public static PlayerDataManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _players = new List<PlayerCharacter>();
        foreach (CharacterScriptableObject character in startCharacterData)
        {
            _players.Add(new PlayerCharacter(character));
        }

        SetupPlayers();
    }

    void Start()
    {
        
    }

    private void SetupPlayers()
    {
        for(int i = 0; i < _players.Count; i++)
        {
            int index = i;
            _players[i].AddHealthListener(() =>
            {
                PlayerMenuManager.Instance.UpdateHealthbar(index, _players[index].GetCurrentStatValue(CharacterStatsEnum.Health));
            });

            _players[i].AddEnergyListener(() =>
            {
                PlayerMenuManager.Instance.UpdateEnergybar(index, _players[index].GetCurrentStatValue(CharacterStatsEnum.Energy));
            });
        }
    }

    public List<PlayerCharacter> GetPlayers() 
    { 
        return _players;
    }
   
}
