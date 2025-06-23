using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    [SerializeField]
    private GameObject battleUI;

    [SerializeField]
    private Image backgroundImage;
    

    [SerializeField]
    private PlayerTurnMenuManager playerTurnManager;

    [SerializeField]
    private AudioClip battleMusic;

    private BattleCharacterDisplayManager characterDisplayManager;

    private const float tickValue = 0.1f;
    private IEnumerator updateCoroutine;

    private List<Enemy> _activeEnemyList;
    private List<Enemy> _enemyList;
    private List<PlayerCharacter> _playerList;

    private Queue<EffectInfo> _newEffectsInfo;
    private Queue<EffectInfo> _removedEffectsInfo;

    bool _weaknesHitted = false;
    bool _resistanceHitted = false;

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

    public void SetBattleMusic(string name)
    {
        battleMusic = Resources.Load<AudioClip>("Music/" + name); ;
    }

    public void ChangeBackground(string name)
    {
        backgroundImage.sprite = Resources.Load<Sprite>($"Images/{name}");
    }

    public void ChangeBackground(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public List<PlayerCharacter> GetPlayers()
    {
        return _playerList;
    }

    public void LoadBattle(EncounterScriptableObject encounter, List<PlayerCharacter> playerlist, Action onLoaded=null)
    {
        MusicManager.Instance.PlayMusic(battleMusic);
        PlayerMenuManager.Instance.SetInBattle(true);

        _activeEnemyList = new List<Enemy>();
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
                _activeEnemyList.Add(enemy);
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

        IEnumerator transition = BackgroundPanelManager.Instance.Transition(LoadBattleScreen, onLoaded);
        StartCoroutine(transition);
    }

    public void LoadBattle(EncounterScriptableObject encounter, Action onLoaded=null)
    {
        LoadBattle(encounter, PlayerDataManager.Instance.GetActivePlayers(), onLoaded);
    }

    private void LoadBattleScreen()
    {
        battleUI.SetActive(true);
        characterDisplayManager.LoadPlayers(_playerList);
        characterDisplayManager.LoadEnemies(_activeEnemyList);
    }


    



    public void StartBattle()
    {
        if (!_inBattle) return;
        updateCoroutine = UpdateBattle();
        StartCoroutine(updateCoroutine);
    }

    public void EndBattle(bool win)
    {
        PlayerMenuManager.Instance.SetInBattle(false);
        battleUI.SetActive(false);
        foreach (var player in _playerList) 
        {
            player.ClearBattle();
            player.Revive();
        }
        _playerTurnQueue.Clear();
        _enemyTurnQueue.Clear();
        _newEffectsInfo.Clear();
        _removedEffectsInfo.Clear();
        InputManager.Instance.ChangeMapping(InputMapEnum.Player);
        OnPlayerTurnEnd = null;
        MusicManager.Instance.PlayPreviousMusic();
        if (win)
        {
            if (OnBattleWin != null) OnBattleWin.Invoke();
        }
        else
        {
            Debug.Log("Game Over");
            GameOverManager.Instance.ShowGameOverScreen();
        }
        OnBattleWin = null;
    }

    private void SetCharactersListeners()
    {
        SetEnemyListeners();

        for (int i = 0; i < _playerList.Count; i++)
        {
            int index = i;

            _playerList[i].SetEffectGainListener((DamageTypeEnum effect) =>
            {
                _newEffectsInfo.Enqueue(new EffectInfo(true, index, effect));
            });

            _playerList[i].SetEffectRemovedListener((DamageTypeEnum effect) =>
            {
                _removedEffectsInfo.Enqueue(new EffectInfo(true, index, effect));
            });

            _playerList[i].SetOnDeathListener(() =>
            {
                characterDisplayManager.DisplayEffect(true, index, DamageTypeEnum.Dead);
            });

            _playerList[i].SetChooseTarget(ChooseTarget);
        }
    }

    private void SetEnemyListeners()
    {
        for(int i = 0; i < _activeEnemyList.Count; i++)
        {
            int index = i;
            _activeEnemyList[i].SetHealthListener(() =>
            {
                int health = _activeEnemyList[index].GetCurrentStatValue(CharacterStatsEnum.Health);
                characterDisplayManager.UpdateHealth(false, index, health);
            });

            _activeEnemyList[i].SetEffectGainListener((DamageTypeEnum effect) =>
            {
                _newEffectsInfo.Enqueue(new EffectInfo(false, index, effect));
            });

            _activeEnemyList[i].SetEffectRemovedListener((DamageTypeEnum effect) =>
            {
                _removedEffectsInfo.Enqueue(new EffectInfo(false, index, effect));
            });

            _activeEnemyList[i].SetOnDeathListener(() =>
            {
                _activeEnemyList.RemoveAt(index);
                SetEnemyListeners();
                characterDisplayManager.LoadEnemies(_activeEnemyList);
            });

            _activeEnemyList[i].SetOnWeaknessHitted(() =>
            {
                _weaknesHitted = true;
            });

            _activeEnemyList[i].SetOnResistanceHitted(() =>
            {
                _resistanceHitted = true;
            });
        }
    }

    private IEnumerator UpdateBattle()
    {
        while (_playerTurnQueue.Count == 0 && _enemyTurnQueue.Count == 0)
        {
            yield return new WaitForSeconds(tickValue);

            foreach (Enemy enemy in _activeEnemyList)
            {
                if (enemy.UpdateInitiative(tickValue)) _enemyTurnQueue.Enqueue(enemy);
            }

            foreach (PlayerCharacter character in _playerList)
            {
                if (character.IsDead()) continue;
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

        if(_playerList.Where(x=>x.IsDead()).Count() == _playerList.Count)
        {
            DisplayBattleMessage("Ostatni z sojuszników poleg³!", () =>
            {
                EndBattle(false);
            });
            return;
        }

        if(_activeEnemyList.Count == 0)
        {
            EndBattle(true);
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

        if (!data.Player && _activeEnemyList.Contains(_enemyList[data.Index])) characterDisplayManager.HideEffect(data.Player, _activeEnemyList.IndexOf(_enemyList[data.Index]), data.Effect);
        else characterDisplayManager.HideEffect(data.Player, data.Index, data.Effect);
        ResolveRemovedEffectsInfo();
    }

    private void ResolvePlayerTurnQueue()
    {
        if (_playerTurnQueue.Count == 0) return;

        PlayerCharacter player = _playerTurnQueue.Dequeue();
        player.SetCurrentAction(null);
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
            enemy.ChooseTargetAndResolveAction(action, _activeEnemyList.Select(x => (Character)x).ToList(), _playerList.Where(x=>!x.IsDead()).Select(x => (Character)x).ToList(), () => { EndEnemyTurn(enemy); });
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
                player.BaseAttack(_activeEnemyList[enemyIndex]);
                EndPlayerTurn(player);
            });
        }
        else if (actionType == PlayerActionTypeEnum.Item)
        {
            InventoryUIManager.Instance.ShowInventory((bool used)=>OnItemUsed(used, player));
        }
        else if (actionType == PlayerActionTypeEnum.Skill)
        {
            player.ChooseTargetAndResolveAction(action, _playerList.Select(x => (Character)x).ToList(), _activeEnemyList.Select(x => (Character)x).ToList(), () => { EndPlayerTurn(player); });
        }
    }

    private void OnItemUsed(bool used, PlayerCharacter player)
    {
        if (used)
        {
            InputManager.Instance.ChangeMapping(InputMapEnum.Battle);
            EndPlayerTurn(player);
        }
        else
        {
            InputManager.Instance.ChangeMapping(InputMapEnum.Battle);
            LoadPlayerMenu(player);
        }
    }

    private void EndPlayerTurn(PlayerCharacter player) 
    {
        if(_resistanceHitted && _weaknesHitted)
        {
            _resistanceHitted = false;
            _weaknesHitted = false;
        }

        if (_weaknesHitted || _resistanceHitted) 
        {
            string message = _weaknesHitted ? "Ten atak jest super efektywny!" : "Ten atak nie jest zbyt efektywny :(";
            _weaknesHitted = false;
            _resistanceHitted = false;
            DisplayBattleMessage(message, () =>
            {
                EndPlayerTurn(player);
            });
            return;
        }

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