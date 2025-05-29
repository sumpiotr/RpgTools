using UnityEngine;

public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField]
    private BattleCharacterDisplayManager battleCharacterDisplayManager;

    [SerializeField]
    private BattleCharacterChoiceMenu characterMenu;

    private bool battle = false;

    public static PlayerMenuManager Instance = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public BattleCharacterChoiceMenu GetMenu()
    {
        return battle ? battleCharacterDisplayManager.GetPlayerMenu() : characterMenu;
    }

    public BattleCharacterDisplayManager GetBattleMenuManger()
    {
        return battleCharacterDisplayManager;
    }

    public BattleCharacterChoiceMenu GetBasePlayerMenu()
    {
        return characterMenu;
    }

    public void SetInBattle(bool inBattle)
    {
        battle = inBattle;
    }

    public void UpdateHealthbar(int index, int value)
    {
        GetMenu().UpdateChoiceHealth(index, value);
    }

    public void UpdateEnergybar(int index, int value)
    {
        GetMenu().UpdateChoiceEnergy(index, value);
    }
   
}
