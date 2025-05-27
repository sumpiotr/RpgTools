using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChoiceMenuManager : ScrollableBaseChoiceMenuManager<string>
{
    public void DisableOption(int index)
    {
        ((MenuChoice)_choices[index]).DisableData();
    }
}
