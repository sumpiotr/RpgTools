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
        BattleManager.Instance.LoadBattle(tutorialEncounter, () => {
            DialogManager.Instance.StartDialog(tutorialDialog.text, () => { BattleManager.Instance.StartBattle(); });
        });
    }

    private void TutorialEvents()
    {

    }

    #endregion
}
