using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private GameObject defaultButton;

    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void ShowGameOverScreen()
   {
        InputManager.Instance.ChangeMapping(InputMapEnum.UI);
        gameOverScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultButton);
   }

    public void HideGameOverScreen()
    {
        InputManager.Instance.ChangeMapping(InputMapEnum.Player);
        gameOverScreen.SetActive(false);
    }
}
