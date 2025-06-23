using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{

    [SerializeField]
    private LoadSceneEventScriptableObject _changeSceneEvent;

    private int _eventIndex;

    public static CutscenesManager Instance;

    [Header("Start Scene")]

    [SerializeField]
    private BoxCollider2D playerCollider;

    [SerializeField]
    private string _startScene;

    [SerializeField]
    private TextAsset _startDialog;

    [Header("Tutorial")]
    [SerializeField]
    private EncounterScriptableObject tutorialEncounter;
    [SerializeField]
    private CharacterScriptableObject tutorialAliceData;
    [SerializeField]
    private CharacterScriptableObject AliceData;
    [SerializeField]
    private TextAsset tutorialDialog;


    [Header("MetalDog")]
    [SerializeField]
    private EncounterScriptableObject metalDogEncounterCutscene;
    [SerializeField]
    private EncounterScriptableObject metalDogEncounterBattle;
    [SerializeField]
    private TextAsset metalDogEncounterDialog;

    [Header("BeforeMinesTalk")]
    [SerializeField]
    private TextAsset beforeMinesTalkDialog;
    [SerializeField]
    private string bossHouseBeforeMines;
    [SerializeField]
    private string mines;

    [Header("Elevator")]
    [SerializeField]
    AudioClip endMusic;
    [SerializeField]
    GameObject endScreen;

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //StartCutscene();
        //new Vector2(89, 28)
        //_changeSceneEvent.CallEvent(new SceneLoadData(new Vector2(0, 0), _debuggingScene.name, Vector2.zero));
    }

    private void StartCutscene()
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.Dialog);
        playerCollider.enabled = false;
        SceneLoadManager.Instance.LoadScene(new SceneLoadData(new Vector2(4.35f, 1.16f), _startScene, Vector2.zero));
        DialogManager.Instance.StartDialog(_startDialog.text, () =>
        {
            playerCollider.gameObject.transform.position = new Vector3(3.15f, 1, 0);
            playerCollider.enabled = true;
            InputManager.Instance.ChangeMapping(InputMapEnum.Player);
        });
    }

    public void PlayCutscene(string name)
    {
        if(name == "Start")
        {
            StartCutscene();
        }
        else if(name == "GuardCutscene")
        {
            GuardCutscene();
        }
        else if(name == "Tutorial")
        {
            Tutorial();
        }
        else if(name == "Lunchbox")
        {
            PlayerController.Instance.UpdatePosition(new Vector2(15.5f, 16));
        }
        else if(name == "Artifact")
        {
            Artifact();
        }
        else if(name == "MetalDogEncounter")
        {
            MetalDogEncounter();
        }
        else if(name == "MetalDogTame")
        {
            MetalDogTame();
        }
        else if(name == "Elevator1")
        {
            ElevatorCutscene();
        }
    }

    private void GuardCutscene()
    {
        GameObject guard = GameObject.Find("Guard");
        if (guard != null) 
        {
            guard.transform.position = new Vector3(2.7f, 9.9f, 0);
        }
    }

    #region Tutorial

    private void Tutorial()
    {
        _eventIndex = 0;
        List<PlayerCharacter> battlePlayers = new List<PlayerCharacter>();
        battlePlayers.Add(new PlayerCharacter(tutorialAliceData));
        PlayerMenuManager.Instance.SetPlayerListeners(battlePlayers[0], 0, 0);
        BattleManager.Instance.LoadBattle(tutorialEncounter, battlePlayers, () => {
            battlePlayers[0].SetCurrentStatValue(CharacterStatsEnum.Energy, 1);
            BattleManager.Instance.DisableMainMenuOption("Use Item");
            BattleManager.Instance.OnPlayerTurnEnd = (PlayerCharacter player)=> { TutorialEvents(); };
            BattleManager.Instance.OnBattleWin = EndTutorial;
            DialogManager.Instance.StartDialog(tutorialDialog.text, () => { BattleManager.Instance.StartBattle(); });
        });
    }

    private void TutorialEvents()
    {
        if (_eventIndex == 0)
        {
            _eventIndex++;
            BattleManager.Instance.DisableMainMenuOption("Attack");
            BattleManager.Instance.DisableMainMenuOption("Guard");
            DialogManager.Instance.StartDialog(tutorialDialog.text, () => { BattleManager.Instance.ResolveTurns(); });
        }
        else if (_eventIndex == 1) 
        {
            _eventIndex++;
            BattleManager.Instance.EnableMainMenuOption("Attack");
            BattleManager.Instance.EnableMainMenuOption("Guard");
            BattleManager.Instance.EnableMainMenuOption("Use Item");
            DialogManager.Instance.StartDialog(tutorialDialog.text, () => { BattleManager.Instance.ResolveTurns(); });
        }
        else
        {
            BattleManager.Instance.ResolveTurns();
        }

    }

    private void EndTutorial() 
    {
        DialogManager.Instance.StartDialog(tutorialDialog.text);
        PlayerDataManager.Instance.AddActivePlayer(AliceData);
    }


    #endregion

    private void Artifact()
    {
        GameObject artifact = GameObject.Find("Artifact");
        if (artifact == null) return;
        artifact.GetComponent<StateObject>().SetState(false);
    }

    #region MetalDogEncounter

    private void MetalDogEncounter()
    {
        PlayerDataManager.Instance.AddActivePlayer(AliceData);
        _eventIndex = 0;
        PlayerDataManager.Instance.RestorePlayers();
        BattleManager.Instance.SetBattleMusic("Sonic2Boss");
        BattleManager.Instance.ChangeBackground("FieldBattle");
        BattleManager.Instance.OnPlayerTurnEnd = (PlayerCharacter player) =>
        {
            MetalDogEvents(player);
        };
        BattleManager.Instance.LoadBattle(metalDogEncounterCutscene);

    }

    private void MetalDogEvents(PlayerCharacter player)
    {
       if(_eventIndex == 0)
       {
            if (player.GetCurrentAction() != null)
            {
                if (player.GetCurrentAction().GetType() == typeof(AttackScriptableObject))
                {
                    _eventIndex++;
                    DialogManager.Instance.StartDialog(metalDogEncounterDialog.text, () =>
                    {
                        BattleManager.Instance.ResolveTurns();
                    });
                }
                else
                {
                    BattleManager.Instance.ResolveTurns();
                }
            }
            else
            {
                BattleManager.Instance.ResolveTurns();
            }
       }
       else if(_eventIndex > 0 && _eventIndex <= 2)
       {
            _eventIndex++;
            BattleManager.Instance.ResolveTurns();
       }
       else if(_eventIndex == 3)
       {
            _eventIndex++;
            DialogManager.Instance.StartDialog(metalDogEncounterDialog.text, () =>
            {
                PlayerDataManager.Instance.RestorePlayers();
                BattleManager.Instance.SetBattleMusic("FaceDown");
                PlayerDataManager.Instance.AddActivePlayer("Fex");
                BattleManager.Instance.OnPlayerTurnEnd = null;
                MusicManager.Instance.PlayPreviousMusic();
                BattleManager.Instance.LoadBattle(metalDogEncounterBattle);
                BattleManager.Instance.OnBattleWin = () =>
                {
                    MetalDogEncounterEnd();
                };
            });
        }
    }

    private void MetalDogEncounterEnd()
    {
        MusicManager.Instance.PlayMusic("MinesEntrance");
        DialogManager.Instance.StartDialog(metalDogEncounterDialog.text, () =>
        {
            IEnumerator fade = BackgroundPanelManager.Instance.FadeIn(() =>
            {
                SceneLoadManager.Instance.LoadScene(new SceneLoadData(new Vector2(0.7f, -0.7f), bossHouseBeforeMines, Vector2.up));
                DialogManager.Instance.ShowSimpleMessage("Jakiœ czas póŸniej...", BeforeMinesTalk);
            });
            StartCoroutine(fade);
        });
    }



    #endregion

    private void BeforeMinesTalk()
    {
        IEnumerator fadeOut = BackgroundPanelManager.Instance.FadeOut(() => {
            DialogManager.Instance.StartDialog(beforeMinesTalkDialog.text, () => {
                SceneLoadManager.Instance.LoadSceneTransition(new SceneLoadData(Vector2.zero, mines, Vector2.zero));
            });
        });
        StartCoroutine(fadeOut);


    }

    private void MetalDogTame()
    {
        GameObject evil = GameObject.Find("evilWilkur");
        if (evil == null) return;
        evil.SetActive(false);
    }

    private void ElevatorCutscene()
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.Block);
        endScreen.SetActive(true);
        MusicManager.Instance.PlayMusic(endMusic);
    }
   
}
