using UnityEngine;
using UnityEngine.Events;

public class StartEncounter : MonoBehaviour
{
    [SerializeField]
    private Sprite battleBackground;
    [SerializeField]
    private AudioClip battleMusic;

    [SerializeField]
    private EncounterScriptableObject encounter;

    [SerializeField]
    private UnityEvent onWin;

    public void StartBattle()
    {
        BattleManager.Instance.ChangeBackground(battleBackground);
        BattleManager.Instance.SetBattleMusic(battleMusic);
        BattleManager.Instance.LoadBattle(encounter);
        BattleManager.Instance.OnBattleWin = ()=> { onWin.Invoke(); };
    }
}
