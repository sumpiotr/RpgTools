using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject inventoryUI;

    [SerializeField]
    ChoiceMenuManager itemChoiceMenu;

    [SerializeField]
    private InputMapEnum _previousInputMap;

    private List<InventoryItemData> _items;

    private Action<bool> _onItemUsed;

    public static InventoryUIManager Instance = null;

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        itemChoiceMenu.SetOnHover(ShowDescription);
    }

    private void ShowDescription(int index)
    {
        if (index == -1) { 
            itemChoiceMenu.SetTitle("");
            return;
        }
        itemChoiceMenu.SetTitle(_items[index].Data.Description);
    }

    public void ShowInventory()
    {
        ShowInventory((bool a) => { });
    }

    public void ShowInventory(Action<bool> onItemUsed)
    {
        _onItemUsed = onItemUsed;
        _items = InventoryManager.Instance.GetNormalItems();
        inventoryUI.SetActive(true);
        itemChoiceMenu.LoadChoices(_items.Select(x => $"{x.Data.Name} x{x.Count}").ToList());
        itemChoiceMenu.Focus();
        _previousInputMap = InputManager.Instance.GetInputMap();
        InputManager.Instance.ChangeMapping(InputMapEnum.Inventory);
    }

    private void CloseInventory()
    {
        itemChoiceMenu.Unfocus();
        inventoryUI.SetActive(false);
        InputManager.Instance.ChangeMapping(_previousInputMap);
    }

    #region Input

    public void SelectionMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        itemChoiceMenu.SelectionMove(context);
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        BaseItemScriptableObject item = _items[itemChoiceMenu.GetSelectedIndex()].Data;
        CloseInventory();
        InventoryManager.Instance.UseItem(item, _onItemUsed);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
         if (!context.performed) return;
         CloseInventory();
        _onItemUsed(false);
    }




    #endregion
}

public struct InventoryItemData 
{
    public BaseItemScriptableObject Data;
    public int Count;

    public InventoryItemData(BaseItemScriptableObject data, int count)
    {
        Data = data;
        Count = count;
    }
}