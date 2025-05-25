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
        List<EnemyScriptableObject> enemies = new List<EnemyScriptableObject> ();
        List< PlayerCharacter > characters = new List<PlayerCharacter> ();
        foreach(EncounterMonster encounterMonster in encouter.EnemyList)
        {
            for (int i = 0; i < encounterMonster.amount; i++) enemies.Add(encounterMonster.enemyData);
        }
        foreach (CharacterScriptableObject data in playerlist) characters.Add(new PlayerCharacter(data));

        foreach(PlayerCharacter character in characters)
        {
            character.SetChooseTarget(ChooseTarget);
        }
        LoadBattle(enemies, characters);
    }

    public void LoadBattle(List<EnemyScriptableObject> enemies, List<PlayerCharacter> characters)
    {
        battleUI.SetActive(true);
        _enemyList = new List<Enemy>();
        foreach (EnemyScriptableObject enemy in enemies) 
        {
            Enemy e = new Enemy(enemy);
            _enemyList.Add(e);

        }
        _playerList = characters;

         characterDisplayManager.LoadPlayers(characters);
        characterDisplayManager.LoadEnemies(_enemyList);

        _playerTurnQueue = new Queue<PlayerCharacter>();
        _enemyTurnQueue = new Queue<Enemy>();

        changeInputMapEvent.CallEvent("Battle");

        updateCoroutine = UpdateBattle();
        StartCoroutine(updateCoroutine);
    }

    public void Update()
    {
    }

    public IEnumerator UpdateBattle()
    {
        while (true) 
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
            if (_playerTurnQueue.Count > 0 || _enemyTurnQueue.Count > 0)
            {
                ResolvePlayerTurnQueue();
                break;
            }
        }
        

    }


    private void ResolvePlayerTurnQueue()
    {
        if (_playerTurnQueue.Count == 0)
        {
            updateCoroutine = UpdateBattle();
            StartCoroutine(updateCoroutine);
            return;
        }
        PlayerCharacter currentCharacter = _playerTurnQueue.Dequeue();
        LoadPlayerMenu(currentCharacter);
    }

    private void LoadPlayerMenu(PlayerCharacter player)
    {
        playerTurnManager.LoadMenus(player.GetCharacterData(), (PlayerActionTypeEnum actionType, ActionBaseScriptableObject action) => ResolvePlayerAction(player, actionType, action));
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
           player.ChooseTargetAndResolveAction(action, _playerList.Select(x=>(Character)x).ToList(), _enemyList.Select(x => (Character)x).ToList(), ResolvePlayerTurnQueue);
        }
    }

    private void EndPlayerTurn(PlayerCharacter player) 
    {
        player.EndTurn();
        ResolvePlayerTurnQueue();
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
