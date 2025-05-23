using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChoiceMenuManager : ScrollableBaseChoiceMenuManager<string>
{

    //[SerializeField]
    //private GameObject choicePrefab;

    //[SerializeField]
    //List<Transform> choicesContainers;

    //[SerializeField]
    //TextMeshProUGUI titleGui;

    //[SerializeField]
    //GameObject upArrow;

    //[SerializeField]
    //GameObject downArrow;

    //[SerializeField]
    //private int  prefabsPoolSize = 3;

    //List<MenuChoice> _choices;

    //private List<string> _dataChoices; 

    //private MenuChoice selected;

    //private int _startDisplayIndex = 0;

    //private int collumnIndex = 0;

    //private void Start()
    //{
        
    //}

    ////public void SetPoolSize(int size)
    ////{
    ////    for(int i = _dialogChoices.Count-size; i > 0; i--)
    ////    {
    ////        MenuChoice choice = _dialogChoices[_dialogChoices.Count-1];
    ////       _dialogChoices.RemoveAt(_dialogChoices.Count - 1);
    ////        Destroy(choice.gameObject);
    ////    }

    ////    for(int i = size - _dialogChoices.Count; i > 0; i--)
    ////    {
    ////        _dialogChoices.Add(Instantiate(choicePrefab, choicesContainer).GetComponent<MenuChoice>());
    ////        _dialogChoices[_dialogChoices.Count - 1].Index = _dialogChoices.Count - 1;
    ////    }
    ////}

    //public void GeneratePrefabs()
    //{
    //    _choices = new List<MenuChoice>();
    //    _startDisplayIndex = 0;
    //    collumnIndex = 0;
    //    for (int j = 0; j < choicesContainers.Count; j++) 
    //    {
    //        for (int i = 0; i < prefabsPoolSize; i++)
    //        {
    //            _choices.Add(Instantiate(choicePrefab, choicesContainers[j]).GetComponent<MenuChoice>());
    //            _choices[i].Index = i+(j*prefabsPoolSize);
    //        }
    //    }
       
    //}

    //public void LoadChoices(List<string> choices)
    //{
    //    if(_choices == null || _choices.Count == 0)GeneratePrefabs();
    //    _startDisplayIndex = 0;
    //    collumnIndex = 0;
    //    _dataChoices = choices;
    //    DisplayChoices();
    //}

    //private void DisplayChoices(int selectedIndex=0)
    //{
    //    for (int i = 0; i < _choices.Count; i++)
    //    {
    //        if (i + _startDisplayIndex >= _dataChoices.Count)
    //        {
    //            _choices[i].Selectable = false;
    //            _choices[i].LoadData("");
    //            continue;
    //        }
    //        else _choices[i].Selectable = true;
    //        _choices[i].Index = i;
    //        _choices[i].DeSelect();
    //        _choices[i].LoadData(_dataChoices[i+_startDisplayIndex]);
    //    }
    //    selected = _choices[selectedIndex];
    //    selected.Select();
    //    collumnIndex = selectedIndex/prefabsPoolSize;
        
        
    //    upArrow.SetActive(_startDisplayIndex != 0);
    //    downArrow.SetActive(_startDisplayIndex + _choices.Count < _dataChoices.Count);
        
    //}

    //public void SetTitle(string title)
    //{
    //    titleGui.text = title;
    //}

    //public int GetSelectedIndex()
    //{
    //    return selected.Index + _startDisplayIndex;
    //}

    //public string GetSelectedText()
    //{
    //    return  selected.GetData();
    //}

    //public MenuChoice GetSelectedChoice()
    //{
    //    return selected;
    //}

    //public void SelectNext()
    //{
    //    if (!gameObject.activeSelf) return;
    //    int newIndex = selected.Index + 1;
    //    if (newIndex < (collumnIndex+1)*prefabsPoolSize)
    //    {
    //        if (!_choices[newIndex].Selectable) return;
    //        selected.DeSelect();
    //        selected = _choices[newIndex];
    //        selected.Select();    
    //    }
    //   else if(_startDisplayIndex + prefabsPoolSize*choicesContainers.Count < _dataChoices.Count)
    //    {
    //        _startDisplayIndex += prefabsPoolSize * choicesContainers.Count;
    //        Debug.Log(_startDisplayIndex);
    //        Debug.Log(collumnIndex * prefabsPoolSize);
    //        selected.DeSelect();
    //        newIndex =  collumnIndex*prefabsPoolSize;
    //        DisplayChoices(_startDisplayIndex + newIndex < _dataChoices.Count ? newIndex : 0);
    //    }
    //}

    //public void SelectPrevious()
    //{
    //    if (!gameObject.activeSelf) return;
    //    int newIndex = selected.Index - 1;
    //    if (newIndex >= 0 + collumnIndex*prefabsPoolSize)
    //    {
    //        selected.DeSelect();
    //        selected = _choices[newIndex];
    //        selected.Select();
    //    }
    //    else if (_startDisplayIndex - prefabsPoolSize*choicesContainers.Count >= 0)
    //    {
    //        _startDisplayIndex -= prefabsPoolSize *choicesContainers.Count;
    //        DisplayChoices((prefabsPoolSize-1) + collumnIndex*prefabsPoolSize );
    //    }
    //}

    //public void NextCollumn()
    //{
    //    if (collumnIndex + 1 >= choicesContainers.Count) return;

    //    int newIndex = selected.Index + prefabsPoolSize;
    //    if (newIndex >= _choices.Count) newIndex = _choices.Count - 1;
    //    if (!_choices[newIndex].Selectable) return;
    //    collumnIndex++;
    //    selected.DeSelect();
    //    selected = _choices[newIndex];
    //    selected.Select();
    //}

    //public void PreviousCollumn()
    //{
    //    if (collumnIndex - 1 < 0) return;
    //    int newIndex = selected.Index - prefabsPoolSize;
    //    if (!_choices[newIndex].Selectable) return;
    //    collumnIndex--;
    //    selected.DeSelect();
       
    //    selected = _choices[newIndex];
    //    selected.Select();
    //}

}
