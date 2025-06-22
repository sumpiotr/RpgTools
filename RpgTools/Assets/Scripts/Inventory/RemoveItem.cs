using UnityEngine;

public class RemoveItem : MonoBehaviour
{
    [SerializeField]
    private BaseItemScriptableObject item;

    public void Remove()
    {
        InventoryManager.Instance.RemoveItem(item);
    }
}
