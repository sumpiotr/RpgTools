using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    UnityEvent<Vector2> interactionEvent;

    public virtual void OnInteraction(GameObject caller)
    {
        interactionEvent.Invoke(caller.transform.position);
    }
}
