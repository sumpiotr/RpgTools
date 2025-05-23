using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private StringEventScriptableObject changeInputMapEvent;

    [SerializeField]
    private PlayerTurnMenuManager playerTurnManager;

    private List<Enemy> _enemyList;
    private List<Character> _characterList;

    private Queue<Character> _playerTurnQueue;
    private Queue<Enemy> _enemyTurnQueue;

    bool freeze = true;

    public void LoadBattle(List<Enemy> enemies, List<Character> characters)
    {
        _enemyList = enemies;
        _characterList = characters;
        _playerTurnQueue = new Queue<Character>();
        _enemyTurnQueue = new Queue<Enemy>();
        changeInputMapEvent.CallEvent("Battle");
    }

    public void Update()
    {
        if (!freeze) UpdateBattle();
    }

    public void UpdateBattle()
    {
        foreach (Character character in _characterList)
        {
            if (character.UpdateInitiative()) _playerTurnQueue.Enqueue(character);
        }

        foreach (Enemy enemy in _enemyList)
        {
            if (enemy.UpdateInitiative()) _enemyTurnQueue.Enqueue(enemy);
        }

        if (_playerTurnQueue.Count > 0)
        {
            freeze = true;
            ReselvePlayerTurnQueue();
        }
    }

    public void ReselvePlayerTurnQueue()
    {
        if (_playerTurnQueue.Count == 0)
        {
            freeze = false;
        }
        playerTurnManager.LoadMenus(_playerTurnQueue.Dequeue().GetCharacterData(), ResolvePlayerAction);
    }

    public void ResolvePlayerAction(ActionBaseScriptableObject action)
    {
        Debug.Log(action.Name);
    }
}
