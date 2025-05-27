using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private StringEventScriptableObject changeInputMapEvent;

    [SerializeField]
    private StringEventScriptableObject displayMessage;

    [SerializeField]
    private ActionEventScriptableObject setDialogCallbackEvent;

    [SerializeField]
    private GameObject battleUI;

    [SerializeField]
    private PlayerTurnMenuManager playerTurnManager;

    [SerializeField]
    private BattleCharacterDisplayManager characterDisplayManager;

    private const float tickValue = 0.1f;
    private IEnumerator updateCoroutine;

    private List<Enemy> _enemyList;
    private List<PlayerCharacter> _playerList;

    private Queue<PlayerCharacter> _playerTurnQueue;
    private Queue<Enemy> _enemyTurnQueue;


    //test data
    [SerializeField]
    private EncounterScriptableObject encouter;

    [SerializeField]
    private List<CharacterScriptableObject> playerlist;

    private void Start()
    {
        List< PlayerCharacter > characters = new List<PlayerCharacter> ();
     
        foreach (CharacterScriptableObject data in playerlist) characters.Add(new PlayerCharacter(data));

        foreach(PlayerCharacter character in characters)
        {
            character.SetChooseTarget(ChooseTarget);
        }
        LoadBattle(encouter, characters);
    }

    private void DisplayBattleMessage(string message, Action onMessageEnded)
    {
        playerTurnManager.DisableMenu();

        setDialogCallbackEvent.CallEvent(()=> {
            playerTurnManager.EnableMenu();
            changeInputMapEvent.CallEvent("battle");
            onMessageEnded();
            });

        displayMessage.CallEvent(message);    
    }

    public void LoadBattle(EncounterScriptableObject encounter, List<PlayerCharacter> characters)
    {
        battleUI.SetActive(true);
        _enemyList = new List<Enemy>();

        foreach (EncounterMonster encounterMonster in encouter.EnemyList)
        {
            for (int i = 0; i < encounterMonster.amount; i++)
            {
                Enemy enemy = new Enemy(encounterMonster.enemyData);
                if (encounterMonster.amount > 1) enemy.Name = $"{encounterMonster.enemyData.Name} {i+1}";
                else enemy.Name = encounterMonster.enemyData.Name;
                _enemyList.Add(enemy); 
            }
        }

        for (int i = 0; i < _enemyList.Count; i++) 
        {
            int index = i;
            _enemyList[i].AddHealthListener(() =>
            {
                characterDisplayManager.UpdateHealth(false, index, _enemyList[index].GetCurrentStatValue(CharacterStatsEnum.Health));
            });
        }
        _playerList = characters;

        for (int i = 0; i < _playerList.Count; i++) 
        {
            int index = i;
            _playerList[i].AddHealthListener(() =>
            {
                characterDisplayManager.UpdateHealth(true, index, _playerList[index].GetCurrentStatValue(CharacterStatsEnum.Health));
            });

            _playerList[i].AddEnergyListener(() =>
            {
                characterDisplayManager.UpdatePlayerEnergy(index, _playerList[index].GetCurrentStatValue(CharacterStatsEnum.Energy));
            });
        }

         characterDisplayManager.LoadPlayers(characters);
        characterDisplayManager.LoadEnemies(_enemyList);

        _playerTurnQueue = new Queue<PlayerCharacter>();
        _enemyTurnQueue = new Queue<Enemy>();

        changeInputMapEvent.CallEvent("Battle");

        updateCoroutine = UpdateBattle();
        StartCoroutine(updateCoroutine);
    }

    public IEnumerator UpdateBattle()
    {
        while (_playerTurnQueue.Count == 0 && _enemyTurnQueue.Count == 0) 
        {
            yield return new WaitForSeconds(tickValue);

            foreach (Enemy enemy in _enemyList)
            {
                if (enemy.UpdateInitiative(tickValue)) _enemyTurnQueue.Enqueue(enemy);
            }

            foreach (PlayerCharacter character in _playerList)
            {
                if (character.UpdateInitiative(tickValue)) _playerTurnQueue.Enqueue(character);
            }



            characterDisplayManager.UpdateInitiative(tickValue);
        }
        ResolveTurns();
    }

    private void ResolveTurns()
    {
        if(_playerTurnQueue.Count > 0) 
        {
            ResolvePlayerTurnQueue();
        }
        else if(_enemyTurnQueue.Count > 0)
        {
            ResolveEnemyQueue();
        }
        else
        {
            updateCoroutine = UpdateBattle();
            StartCoroutine(updateCoroutine);
        }
    }

    private void ResolvePlayerTurnQueue()
    {
        if (_playerTurnQueue.Count == 0)return;
        
        PlayerCharacter currentCharacter = _playerTurnQueue.Dequeue();
        LoadPlayerMenu(currentCharacter);
    }

    private void ResolveEnemyQueue()
    {
        Enemy enemy = _enemyTurnQueue.Dequeue();
        ActionBaseScriptableObject action = enemy.ChooseAction();
        if (action == null)
        {
            DisplayBattleMessage(enemy.Name + " odpoczywa", EndEnemyTurn);
            return;
        }

        DisplayBattleMessage($"{enemy.Name} u¿ywa {action.Name}", () =>
        {
            enemy.ChooseTargetAndResolveAction(action, _enemyList.Select(x => (Character)x).ToList(), _playerList.Select(x => (Character)x).ToList(), EndEnemyTurn);
        });
    }

    private void EndEnemyTurn()
    {
        ResolveTurns();
    }

    private void LoadPlayerMenu(PlayerCharacter player)
    {
        playerTurnManager.LoadMenus(player, (PlayerActionTypeEnum actionType, ActionBaseScriptableObject action) => ResolvePlayerAction(player, actionType, action));
    }

    public void ResolvePlayerAction(PlayerCharacter player, PlayerActionTypeEnum actionType, ActionBaseScriptableObject action)
    {
        if (actionType == PlayerActionTypeEnum.Guard)
        {
            player.Guard();
            EndPlayerTurn(player);
        }
        else if (actionType == PlayerActionTypeEnum.BaseAttack)
        {
            ChooseTarget(player, false, (int enemyIndex) =>
            {
                player.BaseAttack(_enemyList[enemyIndex]);
                EndPlayerTurn(player);
            });
        }
        else if (actionType == PlayerActionTypeEnum.Skill) 
        {
            player.ChooseTargetAndResolveAction(action, _playerList.Select(x => (Character)x).ToList(), _enemyList.Select(x => (Character)x).ToList(), ResolveTurns);
        }
    }

    private void EndPlayerTurn(PlayerCharacter player) 
    {
        player.EndTurn();
        ResolveTurns();
    }

    private void ChooseTarget(PlayerCharacter currentPlayer, bool player, Action<int> onChoosen) 
    {
        changeInputMapEvent.CallEvent("CharacterSelection");
        characterDisplayManager.ChoiceCharacter(player, (int index) =>
        {
            changeInputMapEvent.CallEvent("Battle");
            if (index == -1) LoadPlayerMenu(currentPlayer);
            else
            {
                onChoosen(index);
            }
        });
    }


}
