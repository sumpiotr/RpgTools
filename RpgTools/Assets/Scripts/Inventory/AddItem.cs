using UnityEngine;

public class AddItem : MonoBehaviour
{
    [SerializeField]
    private BaseItemScriptableObject item;

    public void Add()
    {
        InventoryManager.Instance.AddItem(item);
    }
}
