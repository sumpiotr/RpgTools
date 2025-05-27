using System;
using TMPro;
using UnityEngine;

public class MenuChoice : BaseChoiceMenu<string>
{
    [SerializeField]
    private TextMeshProUGUI textUI;

    public void DisableData()
    {
        Debug.Log(textUI.text);
        textUI.color = Color.red;
    }

    public void EnableData()
    {
        textUI.color = Color.white;
    }


    public override void LoadData(string text)
    {
        textUI.text = text;
    }

    public override string GetData()
    {
        return textUI.text;
    }

    public override void ClearData()
    {
        textUI.text = "";
    }
}
