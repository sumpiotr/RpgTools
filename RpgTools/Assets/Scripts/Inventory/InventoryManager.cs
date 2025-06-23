using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<BaseItemScriptableObject> _keyItems;
    private Dictionary<BaseItemScriptableObject, int> _items;

    public static InventoryManager Instance = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        _keyItems = new List<BaseItemScriptableObject>();
        _items = new Dictionary<BaseItemScriptableObject, int>();
    }

    private void Start()
    {
    }

    public void AddItem(string item)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{item}");
        AddItem(itemData);
    }

    public void AddItems(string item, int count)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{item}");
        AddItems(itemData, count);
    }

    public void AddItems(BaseItemScriptableObject itemData, int count)
    {
        if (itemData != null)
        {
            if (itemData.Type == ItemType.Item)
            {
                if (_items.ContainsKey(itemData)) _items[itemData] += count;
                else _items[itemData] = count;
            }
            else { for(int i = 0; i < count; i++)_keyItems.Add(itemData); }
        }
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

    public void RemoveItem(string item)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{item}");
        RemoveItem(itemData);
    }

    public void RemoveItem(BaseItemScriptableObject itemData)
    {
        if (_items.ContainsKey(itemData))
        {
            _items.Remove(itemData);
        }
        else if (_keyItems.Contains(itemData))
        {
            _keyItems.Remove(itemData);
        }
    }

    public bool HasItem(string name)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{name}");
        if (_items.ContainsKey(itemData)) return _items[itemData] > 0;
        return _keyItems.Contains(itemData);
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
        PlayerCharacter choosen =  PlayerMenuManager.Instance.InBattle() ? PlayerDataManager.Instance.GetActivePlayers()[targetIndex] :  PlayerDataManager.Instance.GetPlayers()[targetIndex];
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
