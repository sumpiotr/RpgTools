using System.Collections.Generic;
using UnityEngine;

public abstract class ScrollableBaseChoiceMenuManager<T> : BaseChoiceMenuManager<T>
{
    private int _startDisplayIndex = 0;

    [SerializeField]
    private GameObject upArrow;

    [SerializeField]
    private GameObject downArrow;


    protected override void LoadData()
    {
        for (int i = 0; i < _choices.Count; i++)
        {
            if (i + _startDisplayIndex >= _dataChoices.Count)
            {
                _choices[i].Selectable = false;
                _choices[i].ClearData();
                continue;
            }
            else _choices[i].Selectable = true;
            _choices[i].Index = i;
            _choices[i].DeSelect();
            _choices[i].LoadData(_dataChoices[i + _startDisplayIndex]);
        }


        upArrow.SetActive(_startDisplayIndex != 0);
        downArrow.SetActive(_startDisplayIndex + _choices.Count < _dataChoices.Count);
    }

    public override void Focus(int selectedIndex = 0)
    {
        _startDisplayIndex = selectedIndex - (selectedIndex % (prefabsPoolSize * choicesContainers.Count));
        base.Focus(selectedIndex % (prefabsPoolSize * choicesContainers.Count));
    }

    public override void Unfocus()
    {
        _startDisplayIndex = 0;
        base.Unfocus();
    }

    public override void GeneratePrefabs()
    {
        _startDisplayIndex = 0;
        base.GeneratePrefabs();
    }

    public override void LoadChoices(List<T> choices)
    {
        _startDisplayIndex = 0;
        base.LoadChoices(choices);
    }

    public override int GetSelectedIndex()
    {
        if (selected == null) return -1;
        return selected.Index + _startDisplayIndex;
    }

    public override void SelectNext()
    {
        if (!gameObject.activeSelf || selected == null) return;
        int newIndex = selected.Index + 1;
        if (newIndex < (collumnIndex + 1) * prefabsPoolSize)
        {
            if (!_choices[newIndex].Selectable) return;
            selected.DeSelect();
            selected = _choices[newIndex];
            selected.Select();
        }
        else if (_startDisplayIndex + prefabsPoolSize * choicesContainers.Count < _dataChoices.Count)
        {
            _startDisplayIndex += prefabsPoolSize * choicesContainers.Count;
            selected.DeSelect();
            newIndex = collumnIndex * prefabsPoolSize + _startDisplayIndex;
            LoadData();
            Focus(newIndex < _dataChoices.Count ? newIndex : 0);
        }
    }

    public override void SelectPrevious()
    {
        if (!gameObject.activeSelf || selected == null) return;
        int newIndex = selected.Index - 1;
        if (newIndex >= 0 + collumnIndex * prefabsPoolSize)
        {
            selected.DeSelect();
            selected = _choices[newIndex];
            selected.Select();
        }
        else if (_startDisplayIndex - prefabsPoolSize * choicesContainers.Count >= 0)
        {
            _startDisplayIndex -= prefabsPoolSize * choicesContainers.Count;
            LoadData();
            Focus((prefabsPoolSize - 1) + collumnIndex * prefabsPoolSize);
        }
    }
}
