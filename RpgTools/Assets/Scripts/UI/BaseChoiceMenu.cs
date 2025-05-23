using System;
using TMPro;
using UnityEngine;

public abstract class BaseChoiceMenu<T> : MonoBehaviour
{
    [SerializeField]
    private GameObject selection;

    public int Index = 0;

    public bool Selectable { get; set; }

    public void Select()
    {
        selection.SetActive(true);
    }

    public void DeSelect()
    {
        selection.SetActive(false);
    }

    public abstract void LoadData(T data);

    public abstract T GetData();

    public virtual void UpdateData(T data)
    {
        LoadData(data);
    }

    public abstract void ClearData();
}
