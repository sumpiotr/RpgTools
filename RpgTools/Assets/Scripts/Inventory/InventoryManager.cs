using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<BaseItemScriptableObject> _keyItems;
    private List<BaseItemScriptableObject> _items;

    public void AddItem(string item)
    {
        BaseItemScriptableObject itemData = Resources.Load<BaseItemScriptableObject>($"Items/{item}");
        if (itemData != null) 
        {
            if (itemData.Type == ItemType.Item) _items.Add(itemData);
            else _keyItems.Add(itemData);
        }
    }


    public void AddItem(BaseItemScriptableObject itemData)
    {
        if (itemData != null)
        {
            if (itemData.Type == ItemType.Item) _items.Add(itemData);
            else _keyItems.Add(itemData);
        }
    }
}
