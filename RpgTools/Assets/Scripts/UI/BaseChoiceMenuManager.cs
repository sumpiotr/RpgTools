using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseChoiceMenuManager<T> : MonoBehaviour
{
    [SerializeField]
    private GameObject choicePrefab;

    [SerializeField]
    protected List<Transform> choicesContainers;

    [SerializeField]
    private TextMeshProUGUI titleGui;

    [SerializeField]
    protected int prefabsPoolSize = 3;

    protected List<BaseChoiceMenu<T>> _choices;

    protected List<T> _dataChoices;

    protected BaseChoiceMenu<T> selected;

    protected int collumnIndex = 0;

    private void Start()
    {

    }

    //public void SetPoolSize(int size)
    //{
    //    for(int i = _dialogChoices.Count-size; i > 0; i--)
    //    {
    //        MenuChoice choice = _dialogChoices[_dialogChoices.Count-1];
    //       _dialogChoices.RemoveAt(_dialogChoices.Count - 1);
    //        Destroy(choice.gameObject);
    //    }

    //    for(int i = size - _dialogChoices.Count; i > 0; i--)
    //    {
    //        _dialogChoices.Add(Instantiate(choicePrefab, choicesContainer).GetComponent<MenuChoice>());
    //        _dialogChoices[_dialogChoices.Count - 1].Index = _dialogChoices.Count - 1;
    //    }
    //}

    public virtual void GeneratePrefabs()
    {
        _choices = new List<BaseChoiceMenu<T>>();
        collumnIndex = 0;
        for (int j = 0; j < choicesContainers.Count; j++)
        {
            for (int i = 0; i < prefabsPoolSize; i++)
            {
                _choices.Add(Instantiate(choicePrefab, choicesContainers[j]).GetComponent<BaseChoiceMenu<T>>());
                _choices[i].Index = i + (j * prefabsPoolSize);
            }
        }

    }

    public virtual void LoadChoices(List<T> choices)
    {
        if (_choices == null || _choices.Count == 0) GeneratePrefabs();
        collumnIndex = 0;
        _dataChoices = choices;
        LoadData();
    }

    protected virtual void LoadData()
    {
        for (int i = 0; i < _choices.Count; i++)
        {
            if (i  >= _dataChoices.Count)
            {
                _choices[i].Selectable = false;
                _choices[i].ClearData();
                continue;
            }
            else _choices[i].Selectable = true;
            _choices[i].Index = i;
            _choices[i].DeSelect();
            _choices[i].LoadData(_dataChoices[i]);
        }
    }

    public virtual void Focus(int selectedIndex = 0)
    {
        selected = _choices[selectedIndex];
        selected.Select();
        collumnIndex = selectedIndex / prefabsPoolSize;
    }

    public virtual void Unfocus()
    {
        selected.DeSelect();
        selected = null;
        collumnIndex = 0;
    }

    public void SetTitle(string title)
    {
        titleGui.text = title;
    }

    public virtual int GetSelectedIndex()
    {
        return selected.Index;
    }

    public T GetSelectedData()
    {
        return selected.GetData();
    }

    public BaseChoiceMenu<T> GetSelectedChoice()
    {
        return selected;
    }

    public void UpdateChoice(int index, T data)
    {
        _choices[index].UpdateData(data);
    }

    public virtual void SelectNext()
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
    }

    public virtual void SelectPrevious()
    {
        if (!gameObject.activeSelf || selected == null) return;
        int newIndex = selected.Index - 1;
        if (newIndex >= 0 + collumnIndex * prefabsPoolSize)
        {
            selected.DeSelect();
            selected = _choices[newIndex];
            selected.Select();
        }
    }

    public  void NextCollumn()
    {
        if (collumnIndex + 1 >= choicesContainers.Count || selected == null) return;

        int newIndex = selected.Index + prefabsPoolSize;
        if (newIndex >= _choices.Count) newIndex = _choices.Count - 1;
        if (!_choices[newIndex].Selectable) return;
        collumnIndex++;
        selected.DeSelect();
        selected = _choices[newIndex];
        selected.Select();
    }

    public  void PreviousCollumn()
    {
        if (collumnIndex - 1 < 0 || selected == null) return;
        int newIndex = selected.Index - prefabsPoolSize;
        if (!_choices[newIndex].Selectable) return;
        collumnIndex--;
        selected.DeSelect();

        selected = _choices[newIndex];
        selected.Select();
    }

    public void SelectionMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!gameObject.activeSelf) return;
        Vector2 reading = context.ReadValue<Vector2>();
        if (reading.y == 0 && reading.x == 0) return;
        if (Mathf.Abs(reading.y) > Mathf.Abs(reading.x))
        {
            if (reading.y > 0) SelectPrevious();
            else SelectNext();
        }
        else
        {
            if (reading.x > 0) NextCollumn();
            else PreviousCollumn();
        }
    }

}
