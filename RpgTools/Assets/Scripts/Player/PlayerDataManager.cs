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

    public void LoadPlayers(List<PlayerCharacter> players)
    {
        _players = players;
    }

    public void AddPlayer(CharacterScriptableObject player)
    {
        PlayerCharacter character = new PlayerCharacter(player);
        _players.Add(character);
        SetupPlayer(_players.Count - 1);
    }

    private void SetupPlayers()
    {
        for(int i = 0; i < _players.Count; i++)
        {
            int index = i;
            SetupPlayer(index);
        }
    }

    private void SetupPlayer(int index)
    {
        _activePlayers.Add(_players[index]);
        PlayerMenuManager.Instance.SetPlayerListeners(_players[index], index, _activePlayers.IndexOf(_players[index]));
    }

    public List<PlayerCharacter> GetActivePlayers() 
    { 
        return _activePlayers;
    }

    public List<PlayerCharacter> GetPlayers()
    {
        return _players;
    }

    public void RestorePlayers()
    {
        foreach(PlayerCharacter character in _players)
        {
            character.Restore();
        }
    }

    public void AddActivePlayer(CharacterScriptableObject playerData)
    {
        int index = 0;
        foreach (PlayerCharacter character in _players)
        {
            if(character.GetCharacterData() == playerData)
            {
                if (_activePlayers.Contains(character)) return;
                _activePlayers.Add(character);
                SetupPlayer(index);
                return;
            }
            index++;
        }
    }

    public void AddActivePlayer(string name)
    {
        int index = 0;
        foreach (PlayerCharacter character in _players)
        {
            if (character.GetCharacterData().Name == name)
            {
                if (_activePlayers.Contains(character)) return;
                _activePlayers.Add(character);
                SetupPlayer(index);
                return;
            }
                    index++;
        }
    }

}
