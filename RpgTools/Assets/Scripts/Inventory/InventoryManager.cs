using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<BaseItemScriptableObject> _keyItems;
    private Dictionary<BaseItemScriptableObject, int> _items;

    public static InventoryManager Instance = null;

    [SerializeField]
    private List<BaseItemScriptableObject> _startItems;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        _keyItems = new List<BaseItemScriptableObject>();
        _items = new Dictionary<BaseItemScriptableObject, int>();
    }

    private void Start()
    {
        foreach(BaseItemScriptableObject item in _startItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(string item)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{item}");
        AddItem(itemData);
    }


    public void AddItem(BaseItemScriptableObject itemData)
    {
        if (itemData != null)
        {
            if (itemData.Type == ItemType.Item)
            {
                if (_items.ContainsKey(itemData)) _items[itemData] += 1;
                else _items[itemData] = 1;
            }
            else _keyItems.Add(itemData);
        }
    }

    public void UseItem(BaseItemScriptableObject item, Action<bool> onItemUsed)
    {
        if (_items.ContainsKey(item))
        {
            if (_items[item] <= 0) {
                onItemUsed(false);
                return; 
            }
        }
        if(item.GetType() == typeof(MessageItemScriptableObject))
        {
            DialogManager.Instance.ShowSimpleMessage(((MessageItemScriptableObject)item).Message);
        }
        else if(item.GetType() == typeof(ActionItemScriptableObject))
        {
            ActionItemScriptableObject actionItem = (ActionItemScriptableObject)item;
            PlayerMenuManager.Instance.StartSelection((int index) =>
            {
               OnItemTargetChoosen(actionItem, index, onItemUsed);
            }, $"{item.name} x{_items[item]}");
        }
    }

    private void OnItemTargetChoosen(ActionItemScriptableObject item, int targetIndex, Action<bool> onItemUsed)
    {
        if (targetIndex == -1)
        {
            onItemUsed(false);
            if (!PlayerMenuManager.Instance.InBattle()) InventoryUIManager.Instance.ShowInventory(onItemUsed);
            return;
        }
        PlayerCharacter choosen = PlayerDataManager.Instance.GetPlayers()[targetIndex];
        List<Character> list = new List<Character>();
        list.Add(choosen);
        choosen.ResolveAction(item.Action, list);
        UseUpItem(item);
        onItemUsed(true);
        if (PlayerMenuManager.Instance.InBattle())
        {
            PlayerMenuManager.Instance.CloseMenu();
        }
        else
        {
            if ((_items.ContainsKey(item)))
            {
                PlayerMenuManager.Instance.SetTitle($"{item.name} x{_items[item]}");
            }
            else
            {
                PlayerMenuManager.Instance.CloseMenu();
                PlayerMenuManager.Instance.ShowPlayerMenu();
            };
        }
    }

    private void UseUpItem(BaseItemScriptableObject item)
    {
        if (_items.ContainsKey(item))
        {
           _items[item] -= 1;
            if(_items[item] <= 0)_items.Remove(item);
        }
    }

    public List<InventoryItemData> GetKeyItems()
    {
        return _keyItems.Select(x => new InventoryItemData(x, 1)).ToList();
    }

    public List<InventoryItemData> GetNormalItems()
    {
        return _items.Select((x) => new InventoryItemData(x.Key, x.Value)).ToList();
    }
}
