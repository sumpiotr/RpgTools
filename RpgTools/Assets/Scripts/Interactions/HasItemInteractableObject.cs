using UnityEngine;
using UnityEngine.Events;

public class HasItemInteractableObject : InteractableObject
{
    [SerializeField]
    private BaseItemScriptableObject item;

    [SerializeField]
    private bool removeItem;

    [Header("Else")]
    [SerializeField]
    private UnityEvent HasntItem;

    public override void OnInteraction(GameObject caller)
    {
        if (InventoryManager.Instance.HasItem(item.name)) {
            if(removeItem)InventoryManager.Instance.RemoveItem(item);
            base.OnInteraction(caller); 
        }
        else HasntItem.Invoke();
    }
}
