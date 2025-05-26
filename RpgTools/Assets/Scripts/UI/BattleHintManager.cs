using TMPro;
using UnityEngine;

public class BattleHintManager : MonoBehaviour
{
    [SerializeField]
    StringEventScriptableObject showBattleHintEvent;

    [SerializeField]
    private GameObject hintObject;

    [SerializeField]
    private TextMeshProUGUI hintText;

    private void Start()
    {
        showBattleHintEvent.AddEvent(ShowHint);
    }

    private void ShowHint(string hint)
    {
        if (hint == "") hintObject.SetActive(false);
        else
        {
            hintObject.SetActive(true);
            hintText.text = hint;
        }
    }
}
