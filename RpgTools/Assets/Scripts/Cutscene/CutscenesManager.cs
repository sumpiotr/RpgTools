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
    private SceneAsset _startScene;

    [SerializeField]
    private SceneAsset _debuggingScene;

    [SerializeField]
    private TextAsset _startDialog;

    [Header("Tutorial")]
    [SerializeField]
    private EncounterScriptableObject tutorialEncounter;
    [SerializeField]
    private CharacterScriptableObject tutorialAliceData;
    [SerializeField]
    private TextAsset tutorialDialog;



    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //StartCutscene();
        _changeSceneEvent.CallEvent(new SceneLoadData(new Vector2(-0.5f, -4), _debuggingScene.name, Vector2.zero));
    }

    private void StartCutscene()
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.Dialog);
        playerCollider.enabled = false;
        _changeSceneEvent.CallEvent(new SceneLoadData(new Vector2(4.35f, 1.16f), _startScene.name, Vector2.zero));
        DialogManager.Instance.StartDialog(_startDialog.text, () =>
        {
            playerCollider.gameObject.transform.position = new Vector3(3.15f, 1, 0);
            playerCollider.enabled = true;
            InputManager.Instance.ChangeMapping(InputMapEnum.Player);
        });
    }

    public void PlayCutscene(string name)
    {
        if(name == "GuardCutscene")
        {
            GuardCutscene();
        }
        else if(name == "Tutorial")
        {
            Tutorial();
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
        PlayerMenuManager.Instance.SetPlayerListeners(battlePlayers[0], 0);
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
            //BattleManager.Instance.DisableMainMenuOption("Attack");
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
    }


    #endregion
}
