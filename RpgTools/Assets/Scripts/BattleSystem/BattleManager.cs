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

    private const float tickValue = 0.2f;
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
        LoadBattle(enemies, characters);
    }

    public void LoadBattle(List<EnemyScriptableObject> enemies, List<PlayerCharacter> characters)
    {
        battleUI.SetActive(true);
        _enemyList = new List<Enemy>();
        foreach (EnemyScriptableObject enemy in enemies) 
        {
            _enemyList.Add(new Enemy(enemy));
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
            Action<bool, int> onTargetSelected = (bool a, int b) => { };
            if (action.GetType() == typeof(BuffScriptableObject)) 
            {
                BuffScriptableObject buff = (BuffScriptableObject)action;
                onTargetSelected = (bool player, int index) =>
                {
                    if (player) _playerList[index].Boost(buff.Amount, buff.Duration, buff.Stat);
                    else _enemyList[index].Boost(buff.Amount, buff.Duration, buff.Stat);
                };
            }
            else if(action.GetType() == typeof(AttackScriptableObject))
            {
                AttackScriptableObject attack = (AttackScriptableObject)action;
                onTargetSelected = (bool isPlayer, int index) =>
                {
                    if (isPlayer) player.Attack(_playerList[index], attack.DamageType, attack.effectChance);
                    else player.Attack(_enemyList[index], attack.DamageType, attack.effectChance);
                };
            }

            if (action.TargetTypeEnum == TargetTypeEnum.Single)
            {
                ChooseTarget(player, action.Target == TargetEnum.Allies, (int index) =>
                {
                    onTargetSelected(action.Target == TargetEnum.Allies, index);
                    EndPlayerTurn(player);
                });
            }
            else if (action.TargetTypeEnum == TargetTypeEnum.Multi)
            {
                if (action.Target == TargetEnum.Allies)
                {
                    for (int i = 0; i < _playerList.Count; i++) onTargetSelected(true, i);
                    EndPlayerTurn(player);
                }
                else
                {
                    for (int i = 0; i < _enemyList.Count; i++) onTargetSelected(false, i);
                    EndPlayerTurn(player);
                }
            }
            else
            {
                onTargetSelected(true, _playerList.IndexOf(player));
                EndPlayerTurn(player);
            }

        }
    }

    private void EndPlayerTurn(PlayerCharacter player) 
    {
        player.EndTurn();
        ResolvePlayerTurnQueue();
    }

    private void ChooseTarget(PlayerCharacter currentPlayer, bool player, Action<int> onChoosen) 
    {
        characterDisplayManager.ChoiceCharacter(player, (int index) =>
        {
            if (index == -1) LoadPlayerMenu(currentPlayer);
            else
            {
                onChoosen(index);
            }
        });
    }


}
