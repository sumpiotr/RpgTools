using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterScriptableObject> startCharacterData;

    private List<PlayerCharacter> _players;

    private List<PlayerCharacter> _activePlayers;

    public static PlayerDataManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _players = new List<PlayerCharacter>();
        _activePlayers = new List<PlayerCharacter>();
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
            PlayerMenuManager.Instance.SetPlayerListeners(_players[i], i);
            if (_players[i].GetCharacterData().name == "Fex")_activePlayers.Add(_players[i]);
        }
    }

    public List<PlayerCharacter> GetPlayers() 
    { 
        return _activePlayers;
    }

    public void AddActivePlayer(CharacterScriptableObject playerData)
    {
        foreach (PlayerCharacter character in _players)
        {
            if(character.GetCharacterData() == playerData)
            {
                _activePlayers.Add(character);
                return;
            }
        }
    }
   
}
