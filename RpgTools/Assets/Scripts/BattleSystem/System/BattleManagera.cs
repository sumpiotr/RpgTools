using System.Collections.Generic;

public class BattleManagera
{
    private List<Character> _enemyList;
    private List<Character> _characterList;

    private Queue<Character> turnQueue;

    bool freeze = false;

    public void LoadBattle(List<Character> enemies, List<Character> characters)
    {
        _enemyList = enemies;
        _characterList = characters;
        turnQueue = new Queue<Character>();
    }

    public void UpdateBattle()
    {
        if(freeze) return;
        foreach (Character character in _characterList) 
        {
            if(character.UpdateInitiative())turnQueue.Enqueue(character);
        }

        foreach(Character enemy in _enemyList)
        {
            if(enemy.UpdateInitiative())turnQueue.Enqueue(enemy);
        }

     
        ReselvePlayerTurnQueue();
        
    }

    public void ReselvePlayerTurnQueue()
    {
        if(turnQueue.Count == 0) 
        {
            freeze = false;
        }
        turnQueue.Dequeue().StartTurn();
    }

}
