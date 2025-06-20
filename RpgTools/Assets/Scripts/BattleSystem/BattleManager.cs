using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    [SerializeField]
    private GameObject battleUI;

    [SerializeField]
    private Image fadeImage;        
    private const float fadeDuration = 1f;    
    

    [SerializeField]
    private PlayerTurnMenuManager playerTurnManager;

    [SerializeField]
    private AudioClip battleMusic;

    private BattleCharacterDisplayManager characterDisplayManager;

    private const float tickValue = 0.1f;
    private IEnumerator updateCoroutine;

    private List<Enemy> _enemyList;
    private List<PlayerCharacter> _playerList;

    private Queue<EffectInfo> _newEffectsInfo;
    private Queue<EffectInfo> _removedEffectsInfo;

    private Queue<PlayerCharacter> _playerTurnQueue;
    private Queue<Enemy> _enemyTurnQueue;

    private bool _inBattle = false;

    public Action<PlayerCharacter> OnPlayerTurnEnd { get; set; }
    public Action OnBattleWin { get; set; }


    //test data
    [SerializeField]
    private EncounterScriptableObject encouter;

    [SerializeField]
    private List<CharacterScriptableObject> playerlist;

    public static BattleManager Instance = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        OnPlayerTurnEnd = null;
    }

    private void Start()
    {
        characterDisplayManager = PlayerMenuManager.Instance.GetBattleMenuManger();
        //LoadBattle(encouter);
    }

    public bool InBattle()
    {
        return _inBattle;
    }

    public void EnableMenu()
    {
        playerTurnManager.EnableMenu();
    }

    public void DisableMenu()
    {
        playerTurnManager.DisableMenu();
    }

    private void DisplayBattleMessage(string message, Action onMessageEnded)
    {
        playerTurnManager.DisableMenu();

        Action onMessageEnd = () => {
            playerTurnManager.EnableMenu();
            onMessageEnded();
        };

        DialogManager.Instance.ShowSimpleMessage(message, onMessageEnd);
    }

    public void DisableMainMenuOption(string name)
    {
        playerTurnManager.DisableMainMenuOption(name);
    }

    public void EnableMainMenuOption(string name)
    {
        playerTurnManager.EnableMainMenuOption(name);
    }

    public void SetBattleMusic(AudioClip music)
    {
        battleMusic = music;
    }

    public void LoadBattle(EncounterScriptableObject encounter, List<PlayerCharacter> playerlist, Action onLoaded=null)
    {
        MusicManager.Instance.PlayMusic(battleMusic);
        PlayerMenuManager.Instance.SetInBattle(true);

        _enemyList = new List<Enemy>();
        _newEffectsInfo = new Queue<EffectInfo>();
        _removedEffectsInfo = new Queue<EffectInfo>();

        foreach (EncounterMonster encounterMonster in encounter.EnemyList)
        {
            for (int i = 0; i < encounterMonster.amount; i++)
            {
                Enemy enemy = new Enemy(encounterMonster.enemyData);
                if (encounterMonster.amount > 1) enemy.Name = $"{encounterMonster.enemyData.Name} {i + 1}";
                else enemy.Name = encounterMonster.enemyData.Name;
                _enemyList.Add(enemy);
            }
        }
        _playerList = playerlist;

        SetCharactersListeners();

        _playerTurnQueue = new Queue<PlayerCharacter>();
        _enemyTurnQueue = new Queue<Enemy>();

        InputManager.Instance.ChangeMapping(InputMapEnum.Battle);

        _inBattle = true;
        if(onLoaded == null)
        {
            onLoaded = StartBattle;
        }

        IEnumerator transition = Transition(onLoaded);
        StartCoroutine(transition);
    }

    public void LoadBattle(EncounterScriptableObject encounter, Action onLoaded=null)
    {
        LoadBattle(encounter, PlayerDataManager.Instance.GetPlayers(), onLoaded);
    }

    private void LoadBattleScreen()
    {
        battleUI.SetActive(true);
        characterDisplayManager.LoadPlayers(_playerList);
        characterDisplayManager.LoadEnemies(_enemyList);
    }


    private IEnumerator Transition(Action onLoaded)
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration); ;
            fadeImage.color = c; 
            yield return new  WaitForSeconds(Time.deltaTime);
        }
        c.a = 1;
        fadeImage.color = c;

        LoadBattleScreen();

        t = 0f;
        c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        c.a = 0;
        fadeImage.color = c;

        yield return new WaitForSeconds(fadeDuration);

        onLoaded();
    }



    public void StartBattle()
    {
        if (!_inBattle) return;
        updateCoroutine = UpdateBattle();
        StartCoroutine(updateCoroutine);
    }

    public void EndBattle()
    {
        PlayerMenuManager.Instance.SetInBattle(false);
        battleUI.SetActive(false);
        _playerTurnQueue.Clear();
        _enemyTurnQueue.Clear();
        _newEffectsInfo.Clear();
        _removedEffectsInfo.Clear();
        InputManager.Instance.ChangeMapping(InputMapEnum.Player);
        OnPlayerTurnEnd = null;
        MusicManager.Instance.PlayPreviousMusic();
        if (OnBattleWin != null) OnBattleWin.Invoke();
    }

    private void SetCharactersListeners()
    {
        SetEnemyHealthListeners();
        for (int i = 0; i < _enemyList.Count; i++)
        {
            int index = i;
            _enemyList[i].AddEffectGainListener((DamageTypeEnum effect) =>
            {
                _newEffectsInfo.Enqueue(new EffectInfo(false, index, effect));
            });

            _enemyList[i].AddEffectRemovedListener((DamageTypeEnum effect) =>
            {
                _removedEffectsInfo.Enqueue(new EffectInfo(false, index, effect));
            });
        }

        for (int i = 0; i < _playerList.Count; i++)
        {
            int index = i;

            _playerList[i].AddEffectGainListener((DamageTypeEnum effect) =>
            {
                _newEffectsInfo.Enqueue(new EffectInfo(true, index, effect));
            });

            _playerList[i].AddEffectRemovedListener((DamageTypeEnum effect) =>
            {
                _removedEffectsInfo.Enqueue(new EffectInfo(true, index, effect));
            });

            _playerList[i].SetChooseTarget(ChooseTarget);
        }
    }

    private void SetEnemyHealthListeners()
    {
        for(int i = 0; i < _enemyList.Count; i++)
        {
            int index = i;
            _enemyList[i].SetHealthListener(() =>
            {
                int health = _enemyList[index].GetCurrentStatValue(CharacterStatsEnum.Health);
                characterDisplayManager.UpdateHealth(false, index, health);
                if (health <= 0) { 
                    _enemyList.RemoveAt(index);
                    SetEnemyHealthListeners();
                    characterDisplayManager.LoadEnemies(_enemyList);
                }
            });
        }
    }

    private IEnumerator UpdateBattle()
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

    public void ResolveTurns()
    {
        if (_removedEffectsInfo.Count > 0)
        {
            ResolveRemovedEffectsInfo();
            return;
        }
        if (_newEffectsInfo.Count > 0)
        {
            ResolveNewEffectsInfo();
            return;
        }

        if(_enemyList.Count == 0)
        {
            EndBattle();
            return;
        }

        if (_playerTurnQueue.Count > 0)
        {
            ResolvePlayerTurnQueue();
        }
        else if (_enemyTurnQueue.Count > 0)
        {
            ResolveEnemyQueue();
        }
        else
        {
            updateCoroutine = UpdateBattle();
            StartCoroutine(updateCoroutine);
        }
    }

    private void ResolveNewEffectsInfo()
    {
        if (_newEffectsInfo.Count == 0)
        {
            ResolveTurns();
            return;
        }
        EffectInfo data = _newEffectsInfo.Dequeue();
        string info = "";
        CharacterScriptableObject characterData = data.Player ? _playerList[data.Index].GetCharacterData() : _enemyList[data.Index].GetCharacterData();
        if (data.Effect == DamageTypeEnum.Slash) info = $"Na {characterData.Name} zosta³o na³o¿one krwawienie";
        else if (data.Effect == DamageTypeEnum.Buldgeoning) info = $"{characterData.Name} zosta³ og³uszony";
        else if (data.Effect == DamageTypeEnum.Ice) info = $"{characterData.Name} zosta³ przymro¿ony";
        else if (data.Effect == DamageTypeEnum.Electricity) info = $"Na {characterData.Name} zosta³ na³o¿ony parali¿";
        else if (data.Effect == DamageTypeEnum.Fire) info = $"Na {characterData.Name} zosta³o na³o¿one przypalenie";
        else
        {
            ResolveNewEffectsInfo();
            return;
        }

        DisplayBattleMessage(info, ResolveNewEffectsInfo);
        characterDisplayManager.DisplayEffect(data.Player, data.Index, data.Effect);
    }

    private void ResolveRemovedEffectsInfo()
    {
        if (_removedEffectsInfo.Count == 0)
        {
            ResolveTurns();
            return;
        }
        EffectInfo data = _removedEffectsInfo.Dequeue();
        characterDisplayManager.HideEffect(data.Player, data.Index, data.Effect);
        ResolveRemovedEffectsInfo();
    }

    private void ResolvePlayerTurnQueue()
    {
        if (_playerTurnQueue.Count == 0) return;

        PlayerCharacter player = _playerTurnQueue.Dequeue();
        if (!player.CanStartTurn())
        {
            string infoMessage = $"{player.GetCharacterData().Name} odpoczywa";
            if (player.IsUnderEffect(DamageTypeEnum.Buldgeoning))
            {
                infoMessage = $"{player.GetCharacterData().Name} jest og³uszony";
            }
            else if (player.IsUnderEffect(DamageTypeEnum.Electricity))
            {
                infoMessage = $"{player.GetCharacterData().Name} jest sparali¿owany";
            }
            DisplayBattleMessage(infoMessage, () =>
            {
                EndPlayerTurn(player);
            });
            return;
        }
        LoadPlayerMenu(player);
    }

    private void ResolveEnemyQueue()
    {
        Enemy enemy = _enemyTurnQueue.Dequeue();
        if (!enemy.CanStartTurn())
        {
            string infoMessage = $"{enemy.GetCharacterData().Name} odpoczywa";
            if (enemy.IsUnderEffect(DamageTypeEnum.Buldgeoning))
            {
                infoMessage = $"{enemy.GetCharacterData().Name} jest og³uszony";
            }
            else if (enemy.IsUnderEffect(DamageTypeEnum.Electricity))
            {
                infoMessage = $"{enemy.GetCharacterData().Name} jest sparali¿owany";
            }
            DisplayBattleMessage(infoMessage, () =>
            {
                EndEnemyTurn(enemy);
            });
            return;
        }
        ActionBaseScriptableObject action = enemy.ChooseAction();
        if (action == null)
        {
            DisplayBattleMessage(enemy.Name + " odpoczywa", () => { EndEnemyTurn(enemy); });
            return;
        }

        DisplayBattleMessage($"{enemy.Name} u¿ywa {action.Name}", () =>
        {
            enemy.ChooseTargetAndResolveAction(action, _enemyList.Select(x => (Character)x).ToList(), _playerList.Select(x => (Character)x).ToList(), () => { EndEnemyTurn(enemy); });
        });
    }

    private void EndEnemyTurn(Character enemy)
    {
        if(enemy.GetInitiative() != 0)enemy.EndTurn();
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
        else if (actionType == PlayerActionTypeEnum.Item)
        {
            InventoryUIManager.Instance.ShowInventory((bool used)=>OnItemUsed(used, player));
        }
        else if (actionType == PlayerActionTypeEnum.Skill)
        {
            player.ChooseTargetAndResolveAction(action, _playerList.Select(x => (Character)x).ToList(), _enemyList.Select(x => (Character)x).ToList(), () => { EndPlayerTurn(player); });
        }
    }

    private void OnItemUsed(bool used, PlayerCharacter player)
    {
        if (used)
        {
            EndPlayerTurn(player);
        }
        else LoadPlayerMenu(player);
    }

    private void EndPlayerTurn(PlayerCharacter player) 
    {
        if(player.GetInitiative() != 0)player.EndTurn();
        if(OnPlayerTurnEnd != null)OnPlayerTurnEnd.Invoke(player);
        else ResolveTurns();
    }

    private void ChooseTarget(PlayerCharacter currentPlayer, bool player, Action<int> onChoosen) 
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.CharacterSelection);
        characterDisplayManager.ChoiceCharacter(player, (int index) =>
        {
            InputManager.Instance.ChangeMapping(InputMapEnum.Battle);
            if (index == -1) LoadPlayerMenu(currentPlayer);
            else
            {
                onChoosen(index);
            }
        });
    }


}

struct EffectInfo 
{
    public bool Player;
    public int Index;
    public DamageTypeEnum Effect;

    public EffectInfo(bool player, int index, DamageTypeEnum effect)
    {
        Player = player;
        Index = index;
        Effect = effect;
    }
}