using UnityEngine;

public class AddPlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptableObject character;

    public void AddPlayer()
    {
        PlayerDataManager.Instance.AddPlayer(character);
    }

    public void AddActivePlayer()
    {
        PlayerDataManager.Instance.AddActivePlayer(character);
    }
}
